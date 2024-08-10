using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Tela.Data.Core;
using Tela.Data.Organisation;
using Tela.Data.Organisation.Inventory;
using Tela.Models;
namespace Tela.Services;
//TODO:b 382. Add TransactionService to Services
public class TransactionService : ReferenceService
{
    public TransactionService(ApplicationDbContext context, UserManager<LibraryUser> userManager, IMapper mapper) 
        : base(context, userManager) {}
    //TODO:b 384. Add GetTransactionsAsync to TransactionService
    //TODO:b 479. Update GetTransactionsAsync and GetTransactionCountAsync to include start date
    public async Task<ICollection<T>> GetTransactionsAsync<T>(int libraryId,string librarianId,TemporalisLibri temporal, int skip = 0, int take = int.MaxValue,DateTime start = default)
    =>  (await GetQuery(libraryId,librarianId,temporal,start:start).FirstAsync())
            .References.SelectMany(r => r.Books.SelectMany(b => b.Transactions))
                .OrderByDescending(t => t.Start).ThenByDescending(t=>t.End)
                    .Skip(skip).Take(take).ToArray().Adapt<ICollection<T>>();
    public async Task<int> GetTransactionCountAsync(int libraryId,DateTime start = default)
      => await _context.Transactions.CountAsync(t => t.Librarian.LibraryId == libraryId && t.Start > start);
    //TODO:b 407. Add GetTransactionAsync to TransactionService
    new public async Task<T> GetTransactionAsync<T>(int libraryId, string librarianId, int id, TemporalisLibri temporal)
    => (await GetQuery(libraryId, librarianId, temporal).FirstAsync())
            .References.SelectMany(r => r.Books.SelectMany(b => b.Transactions)).First(t => t.Id == id).Adapt<T>();
    //TODO:b 389. Add HoldBookAsync to TransactionService (challenge write tests)
    public async Task<Transaction> HoldBookAsync(int libraryId, string librarianId, string memberId, Guid uid)
    {
        var library = await GetQuery(libraryId, librarianId,TemporalisLibri.ActiveTransactionsAndRestorations, memberId).FirstAsync();
        var member = library.Users.OfType<Member>().First(u => u.Id == memberId);
        var librarian = library.Users.OfType<Librarian>().First(u => u.Id == librarianId);
        var t = library.Hold(librarian, member, uid);
        await _context.SaveChangesAsync();
        return t;
    }
    //TODO:b 403. Add ReleaseBookAsync to TransactionService (challenge write tests)
    public async Task<Transaction> ReleaseBookAsync(int libraryId,string librarianId,Guid uid,int transactionId)
    {
        var library = await GetQuery(libraryId, librarianId, TemporalisLibri.ActiveTransactionsAndRestorations).FirstAsync();
        //TODO:b 462. Add cheeky Book Tau (time) Loxa (bug) to simulate concurrency issue
        if (!Q.CoinFlip(74)) {
            _context.Database.ExecuteSqlRaw(
                $"UPDATE [trance.actions].Holds SET [End] = @end,[Version] = @version WHERE Id = @id",
                [
                    new SqlParameter("end",library[uid][transactionId].Start.AddMinutes(Q.Int(1, 42))),
                    new SqlParameter("version",Guid.NewGuid()),
                    new SqlParameter("id",transactionId)
                ]
            );
        }
        var t = library.Release(uid,transactionId);
        //TODO:b 463. Add Error Handling for Concurrency Issue
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            var msg = new StringBuilder();
            foreach (var entry in ex.Entries)
            {
                var ov = (DateTime) entry.OriginalValues[nameof(Hold.End)]!;
                var cv = (DateTime) entry.CurrentValues[nameof(Hold.End)]!;
                var dv = (DateTime) entry.GetDatabaseValues()![nameof(Hold.End)]!;             
                msg.AppendLine($"BOO BOO: BOOK TAU"); 
                msg.AppendLine($"Original: {ov} Current: {cv} Database: {dv}");
                msg.AppendLine($"Texalotl Unit Discrepancy: {(dv - cv).TotalMilliseconds / Math.PI - 42}");
            }
            throw new DomainRuleException(msg.ToString());
        }
        return t;
    }
    //TODO:b 405. Add LoanBookAsync to TransactionService (challenge write tests)
    public async Task<Transaction> LoanBookAsync(int libraryId, string librarianId, string memberId, Guid uid)
    {
        var library = await GetQuery(libraryId, librarianId, TemporalisLibri.ActiveTransactionsAndRestorations, memberId).FirstAsync();      
        var librarian = library.Users.OfType<Librarian>().First(u => u.Id == librarianId);
        var member = library.Users.OfType<Member>().First(u => u.Id == memberId);
        //... following the footsteps ... spellbound
        var trance = library.Loan(librarian, member, uid); 
        await _context.SaveChangesAsync();
        return trance;
    }
    //TODO:b 409. Add ReturnBookAsync to TransactionService (challenge write tests)
    public async Task<Transaction> ReturnBookAsync(int libraryId, string librarianId, string memberId, Guid uid, int transactionId)
    {
        var library = await GetQuery(libraryId, librarianId,TemporalisLibri.ActiveTransactionsAndRestorations, memberId).FirstAsync();
        //TODO:b 467. Add cheeky Book Tau (time) Loxa (bug) to simulate concurrency issue
        if (!Q.CoinFlip(88))
        {
            _context.Database.ExecuteSqlRaw(
                $"UPDATE [trance.actions].Loans SET [End] = @end,[Version] = @version WHERE Id = @id",
                [
                    new SqlParameter("end",library[uid][transactionId].Start.AddMinutes(Q.Int(1, 42))),
                    new SqlParameter("version",Guid.NewGuid()),
                    new SqlParameter("id",transactionId)
                ]
            );
        }
        var t = library.Return(uid, transactionId);
        //TODO:b 468. Add Error Handling for Concurrency Issue
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            var msg = new StringBuilder();
            foreach (var entry in ex.Entries)
            {
                var ov = (DateTime)entry.OriginalValues[nameof(Loan.End)]!;
                var cv = (DateTime)entry.CurrentValues[nameof(Loan.End)]!;
                var dv = (DateTime)entry.GetDatabaseValues()![nameof(Loan.End)]!;
                msg.AppendLine($"BOO BOO: BOOK TAU");
                msg.AppendLine($"Original: {ov} Current: {cv} Database: {dv}");
                msg.AppendLine($"Texalotl Unit Discrepancy: {(dv - cv).TotalMilliseconds / Math.PI - 42}");
            }
            throw new DomainRuleException(msg.ToString());
        }
        return t;
        //TODO:b 469. [Exercise] Code Smell (DRY) - Refactor the concurrency handling code in ReleaseBookAsync and ReturnBookAsync
    }
}