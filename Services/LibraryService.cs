//TODO:b 185. Add using statements to LibraryService.cs
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using System.Security.Claims;
using Tela.Data.Core;
using Tela.Data.Organisation;
//TODO:b 209. Add Tela.Models using statements to LibraryService.cs
using Tela.Models;
namespace Tela.Services;
//TODO:b 184. Add LibraryService
public class LibraryService
{
    protected readonly ApplicationDbContext _context;
    protected readonly UserManager<LibraryUser> _userManager;
    public LibraryService(ApplicationDbContext context, UserManager<LibraryUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    //TODO:b 186. Add GetQuery method to LibraryService 
    public IQueryable<Library> GetQuery(int libraryId, string librarianId, TemporalisLibri temporal, string? memberId = null,DateTime start = default)
        => _context.GetLibraryQuery(libraryId, librarianId, temporal, memberId,start);//TODO:b 478. Update GetQuery to include start date filter
    //TODO:b 210. Add RegisterLibrarianAsync method to LibraryService
    public async Task<LibrarianDTO> RegisterLibrarianAsync(int libraryId,string email,string firstname, string lastName,string password = "1")
    {
        //TODO:b 215. Implement RegisterLibrarianAsync
        var librarian = new Librarian(libraryId, email, firstname, lastName, email) { Registered = DateTime.Now };
        var result = await _userManager.CreateAsync(librarian, password);
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(librarian, Roles.LIBRARIAN.ToString());
            return librarian.Adapt<LibrarianDTO>();
        }
        throw new ApplicationException(result.Errors.First().Description); 
    }
    //TODO:b 211. Add RegisterMemberAsync method to LibraryService
    public async Task<MemberDTO> RegisterMemberAsync(int libraryId, string email, string firstname, string lastName, string password = "1")
    {
        //TODO:b 216. Implement RegisterMemberAsync
        var member = new Member(libraryId, email, firstname, lastName, email) { Registered = DateTime.Now };
        var result = await _userManager.CreateAsync(member, password);
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(member,Roles.MEMBER.ToString());
            //TODO:b 448. Add Claim to Member when being registered
            await _userManager.AddClaimAsync(member, new Claim("bibliotheca",libraryId.ToString()));
            return member.Adapt<MemberDTO>();
        }
        throw new ApplicationException(result.Errors.First().Description);
    }
    //TODO:b 302. Add GetMembersAsync & GetMemberCountAsync methods to LibraryService 
    public async Task<ICollection<T>> GetMembersAsync<T>(int libraryId, int skip = 0, int take = int.MaxValue)
        => await _context.Users.Where(u => u.LibraryId == libraryId && u.Role == Roles.MEMBER)
            .OrderBy(u=>u.UserName).Skip(skip).Take(take).ProjectToType<T>().ToListAsync();
    public async Task<int> GetMemberCountAsync(int libraryId) 
        => await _context.Users.CountAsync(u => u.LibraryId == libraryId && u.Role == Roles.MEMBER);
    //TODO:b 391 Add GetMemberAsync (with search term) to LibraryService
    public async Task<ICollection<T>> GetMembersAsync<T>(int libraryId, string searchTerm)
    => await _context.Users
         .Where(u => u.LibraryId == libraryId && u.Role == Roles.MEMBER)
         .Where(u => u.FirstName.Contains(searchTerm) || u.LastName.Contains(searchTerm) || (u.Email ?? "").Contains(searchTerm))
         .ProjectToType<T>().ToListAsync();
    //TODO:b 395. Add GetMemberAsync (libraryId, id) to LibraryService
    public async Task<T> GetMemberAsync<T>(int libraryId, string id)
     => (await _context.Users.Where(u => u.LibraryId == libraryId && u.Role == Roles.MEMBER && u.Id == id)
        .ProjectToType<T>().FirstOrDefaultAsync())!;
}