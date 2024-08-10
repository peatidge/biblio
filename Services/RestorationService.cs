using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Tela.Data.Core;
using Tela.Data.Organisation;
using Tela.Data.Organisation.Inventory;
namespace Tela.Services;
//TODO:b 412a. Add RestorationService class that inherits from ReferenceService
public class RestorationService : ReferenceService
{
    public RestorationService(ApplicationDbContext context, UserManager<LibraryUser> userManager) : base(context, userManager)
    {
    }
    //TODO:b 413. Add GetRestorationsAsync & GetRestorationCountAsync methods to RestorationService (challenge write tests)
    public async Task<ICollection<T>> GetRestorationsAsync<T>(int libraryId, string librarianId, TemporalisLibri temporal, int skip = 0, int take = int.MaxValue)
    => (await GetQuery(libraryId,librarianId,temporal).FirstAsync())
        .References.SelectMany(r => r.Books).SelectMany(b => b.Restorations)
            .OrderByDescending(t => t.Start).ThenByDescending(t => t.End)
               .Skip(skip).Take(take).ToArray().Adapt<ICollection<T>>();
    public async Task<int> GetRestorationCountAsync(int libraryId, string librarianId, TemporalisLibri temporal)
    => (await GetQuery(libraryId,librarianId,temporal).FirstAsync())
        .References.SelectMany(r => r.Books).SelectMany(b => b.Restorations).Count();
    //TODO:b 418. Add ScheduleRestorationAsync method to RestorationService (challenge write tests)
    public async Task<Restoration> ScheduleRestorationAsync(int libraryId,Librarian librarian, Guid uid,DateTime start, DateTime end)
    {
        var library = await GetQuery(libraryId,librarian.Id,TemporalisLibri.ActiveTransactionsAndRestorations).FirstAsync();
        var t = library.ScheduleRestoration(librarian, uid, start, end);
        await _context.SaveChangesAsync();
        return t;
    }
}