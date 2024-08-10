using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Tela.Data.Core;
using Tela.Data.Organisation;
using Tela.Data.Organisation.Inventory;

namespace Tela.Services; 
//TODO:b 339. Add Reference Service
public class ReferenceService : LibraryService
{
    public ReferenceService(ApplicationDbContext context, UserManager<LibraryUser> userManager)
        : base(context, userManager) { }
    //TODO:b 342. Add AddReferenceAsync and IsUniqueISBN methods to ReferenceService
    public async Task<Reference> AddReferenceAsync(int libraryId, string librarianId, string title, string isbn, string author, int books = 0)
    {
        Reference reference = null!;
        //TODO:b 345. Implement logic for AddReferenceAsync
        reference = (await GetQuery(libraryId, librarianId, TemporalisLibri.Forgetful).FirstAsync())
                            .AddReference(isbn, title, author, books);
        await _context.SaveChangesAsync();
        return reference;
    }
    //This is used to check if the ISBN is unique as per domain rules
    public async Task<bool> IsUniqueISBN(int libraryId, string isbn)
   => await _context.References.AnyAsync(r => r.LibraryId == libraryId && r.ISBN == isbn);
    //TODO:b 351. Add GetReferencesAsync and GetReferenceCountAsync to ReferenceService
    public async Task<ICollection<T>> GetReferencesAsync<T>(int libraryId, string librarianId,TemporalisLibri temporal,int skip = 0,int take = int.MaxValue)
    => (await GetQuery(libraryId,librarianId,temporal).FirstAsync())
            .References.OrderBy(r => r.Title).ThenBy(r => r.ISBN).Skip(skip).Take(take).Adapt<ICollection<T>>();
    public async Task<int> GetReferenceCountAsync(int libraryId)
       => await _context.References.CountAsync(u => u.LibraryId == libraryId);
    //TODO:b 355. Add GetReferenceAsync to ReferenceService
    public async Task<T> GetReferenceAsync<T>(int libraryId, string librarianId,int id,TemporalisLibri temporal)
        => (await GetQuery(libraryId, librarianId, temporal).FirstAsync())
            .References.FirstOrDefault(r => r.Id == id).Adapt<T>();
    //TODO:b 357. Add UpdateReferenceAsync to ReferenceService
    public async Task<Reference> UpdateReferenceAsync(int libraryId, string librarianId, string title, string isbn, string author, int books = 0)
    {
        var library = await GetQuery(libraryId, librarianId,TemporalisLibri.Forgetful).FirstAsync(); 
        var reference = library.UpdateReference(isbn, title, author);
        await _context.SaveChangesAsync();
        return reference;
    }
    //TODO:b 368. Add GetBooksAsync, GetBookAsync and GetBookCountAsync to ReferenceService
    public async Task<ICollection<T>> GetBooksAsync<T>(int libraryId,string librarianId, int referenceId, TemporalisLibri temporal, int skip = 0, int take = int.MaxValue)
      => (await GetQuery(libraryId,librarianId, temporal).FirstAsync()).References.First(r=>r.Id==referenceId).Books.OrderBy(b => b.UID)
                    .Skip(skip).Take(take).Adapt<ICollection<T>>();
    public async Task<T> GetBookAsync<T>(int libraryId, string librarianId,Guid uid, TemporalisLibri temporal)
    => (await GetQuery(libraryId, librarianId, temporal).FirstAsync())[uid].Adapt<T>();
    public async Task<int> GetBookCountAsync(int libraryId, int referenceId)
      => await _context.Books.CountAsync(b => b.Reference.LibraryId == libraryId && b.ReferenceId == referenceId);
    //TODO:b 392. Add GetBooksAsync (with search term) to ReferenceService (bit lazy, should add sproc)
    public async Task<ICollection<T>> GetBooksAsync<T>(int libraryId, string searchTerm)
    => (await _context.Books
        .Include(b => b.Reference)
        .Include(b => b.Transactions)
        .Where(b => b.Reference.LibraryId == libraryId)
        .Where(
            b => b.Reference.Title.Contains(searchTerm)
            || b.Reference.Author.Contains(searchTerm)
            || b.UID.ToString().Contains(searchTerm)
            || b.Reference.ISBN.Contains(searchTerm)
        ).ToListAsync()).Adapt<ICollection<T>>();
    //TODO:b 396. Add GetBookAsync (libraryId, uid) to ReferenceService
    public async Task<T> GetBookAsync<T>(int libraryId, Guid uid)
   => (await _context.Books.Include(b => b.Reference).Include(b => b.Transactions).FirstAsync(r => r.Reference.LibraryId == libraryId && r.UID == uid))
       .Adapt<T>();
}