using Mapster;
using Microsoft.AspNetCore.Identity;
//TODO:b 126. Add using Microsoft.EntityFrameworkCore to Ludus
using Microsoft.EntityFrameworkCore;
using Tela.Data.Organisation;
//TODO:a 73. Add using Tela.Data.Organisation.Inventory to Ludus
using Tela.Data.Organisation.Inventory;
using Tela.Models;
namespace Tela.Data.Core;
//TODO:a 22. Add (play area) Ludus for quick ad-hoc testing
public class Ludus
{
    //TODO:a 23. Ludus: Add ApplicationDbContext, UserManager<LibraryUser>, RoleManager<IdentityRole> DI
    private readonly ApplicationDbContext _context;
    private readonly UserManager<LibraryUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;    
    //TODO:a 24. Ludus: Add Constructor with ApplicationDbContext, UserManager<LibraryUser>, RoleManager<IdentityRole> DI
    public Ludus(ApplicationDbContext context, UserManager<LibraryUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }
    //TODO:a 29. Ludus: Add Evolvere to orchestrate the creation and persistence of Library, Roles, Librarian and Member
    public async Task<(Library Library, IdentityRole[] Roles, Librarian Librarian, Member Member)> EvolvereAsync()
    {
        var library = await BibliothecaFormularumAsync();
        var roles = await RollOutRolesAsync();
        var librarian = await MaterializeLibrarianAsync(library);
        var member = await ImplicateMemberAsync(library);
        //TODO:a 76. Ludus: Invoke IngestKnowledgeAsync to create and persist Reference
        var reference = await IngestKnowledgeAsync(library);
        //TODO:a 77. Ludus: Invoke HoldLoanAndRestoreAsync to create and persist Hold, Loan and Restoration
        await HoldLoanAndRestoreAsync(librarian, member, reference);
        return (library, roles, librarian, member); 
    }
    //TODO:a 25. Ludus: Add BibliothecaFormularumAsync to create and persist Library
    public async Task <Library> BibliothecaFormularumAsync()
    {
        var library = new Library("Dobbs' Technology Library") { Blurbosity= "Chickety-Boo" };
        _context.Add(library);
        await _context.SaveChangesAsync();
        return library;
    }
    //TODO:a 26. Ludus: Add RollOutRolesAsync to create and persist IdentityRoles
    public async Task<IdentityRole[]> RollOutRolesAsync()
    {
        var result = new List<IdentityRole>();
        foreach (var role in Enum.GetValues<Roles>())
        {
            var identityRole = new IdentityRole(role.ToString()) { Id = role.ToString(), NormalizedName = role.ToString() };                 
            await _roleManager.CreateAsync(identityRole);
            result.Add(identityRole);
        }
        return result.ToArray();
    }
    //TODO:a 27. Ludus: Add MaterializeLibrarianAsync to create and persist Librarian
    public async Task<Librarian> MaterializeLibrarianAsync(Library library)
    {
        var librarian = new Librarian(library.Id,"Lib.Rarian@dobbs","Lib","Rarian","Lib.Rarian@dobbs");
        librarian.Registered = DateTime.Now;
        await _userManager.CreateAsync(librarian, "1");
        await _userManager.AddToRoleAsync(librarian, Roles.LIBRARIAN.ToString());
        return librarian; 
    }
    //TODO:a 28. Ludus: Add ImplicateMemberAsync to create and persist Member
    public async Task<Member> ImplicateMemberAsync(Library library)
    {
        var member = new Member(library.Id,"Mem.Ber@dobbs", "Mem", "Ber", "Mem.Ber@dobbs");
        member.Registered = DateTime.Now;
        await _userManager.CreateAsync(member, "1");
        await _userManager.AddToRoleAsync(member, Roles.MEMBER.ToString());
        return member;
    }
    //TODO:a 74. Ludus: Add IngestKnowledgeAsync to create and persist Reference
    public async Task<Reference> IngestKnowledgeAsync(Library library)
    {
        var reference = new Reference(library, "978-0201835953", "The Mythical Byte", "D.Igital Promethius",books:10);
        library.References.Add(reference);
        await _context.SaveChangesAsync();
        return reference;
    }
    //TODO:a 75. Ludus: Add HoldLoanAndRestoreAsync to create and persist Hold, Loan and Restoration
    public async Task HoldLoanAndRestoreAsync(Librarian librarian, Member member, Reference reference)
    {
        reference.Books[0].Transactions.Add(new Hold(librarian, member, reference.Books[0]));
        reference.Books[1].Transactions.Add(new Loan(librarian, member, reference.Books[1]));
        reference.Books[2].Restorations.Add(new Restoration(librarian, reference.Books[2], DateTime.Now, DateTime.Now.AddDays(42)));
        await _context.SaveChangesAsync();
    }
    //TODO:b 127. Add MutareDominium to run ad-hoc actions against Book state machine
    public async Task<(Book Book, string[] Output, string[] Errors)> MutareDominium()
    {
        //TODO:b 128. Make MutareDominium Library, User, Reference, Book, Transaction and Restoration instantiation idempotent
        //i.e. it will not change the state of the system by creating a new Library if one exists
        if (!_context.Libraries.Any())
        {
            await EvolvereAsync(); 
        }     
        //TODO:b 129. Retrieve the Library with all related entities
        var library = await _context.Libraries
            .Include(l => l.Users)
            .Include(l => l.References)
                .ThenInclude(r => r.Books)
                .ThenInclude(b => b.Transactions)
             .Include(l => l.References)
                .ThenInclude(r => r.Books)
                .ThenInclude(b => b.Restorations)
        .FirstAsync();
        //TODO:b 130. Idempontent creation of second Member (ReMem.Ber@dobbs) for testing
        var remember = library.Users.OfType<Member>().FirstOrDefault(u => u.UserName == "ReMem.Ber@dobbs"); 
        if (remember == null)
        {
            remember = new Member(library.Id, "ReMem.Ber@dobbs", "ReMem", "Ber", "ReMem.Ber@dobbs");
            remember.Registered = DateTime.Now;
            await _userManager.CreateAsync(remember, "1");
            await _userManager.AddToRoleAsync(remember,Roles.MEMBER.ToString());
        }
        //TODO:b 131. Declare variables (pointers) for the last book (specimen selected for testing), librarian and member
        var book = library.References.SelectMany(r => r.Books).Last();    
        var librarian = library.Users.OfType<Librarian>().First();
        var member = library.Users.OfType<Member>().First();
        //TODO:b 132. Declare lists for output and error messages
        var output = new List<string>(); 
        var errors = new List<string>();        
        var tri = (Action asyncAction) => 
        { 
            try 
            {
                  asyncAction(); 
            } 
            catch (DomainRuleException ex) 
            { 
                errors.Add($"{DateTime.Now.Ticks} {ex.Message}"); 
            } 
        };
        try
        {
            Transaction transaction = null!;
            //TODO:b 133. Hold the book (Mem.Ber@dobbs) 
            tri(() => transaction = library.Hold(librarian, member, book.UID));
            await _context.SaveChangesAsync();
            output.Add($"{DateTime.Now.Ticks} On Hold - {member.UserName} ({book.State})");
            //TODO:b 134. Try hold the book again, this time for a different member (should fail)
            errors.Add($"{DateTime.Now.Ticks} Try hold the book again, this time for a different member");
            tri(() => library.Hold(librarian, remember, book.UID));
            //TODO:b 135. Try loan the book to the different member (should fail)
            errors.Add($"{DateTime.Now.Ticks} Try loan the book to the different member (should fail)");
            tri(() => library.Loan(librarian, remember, book.UID));           
            //TODO:b 136. Loan the book the original member (should auto release hold and succeed)
            tri(() => transaction = library.Loan(librarian, member, book.UID));
            await _context.SaveChangesAsync(); 
            output.Add($"{DateTime.Now.Ticks} On Loan - {member.UserName} ({book.State})");
            //TODO:b 137. Test another member trying to hold or loan the book that is currently on loan (should fail)
            errors.Add($"{DateTime.Now.Ticks} 2xTests: Another member trying to hold or loan the book that is currently on loan (should fail)");
            tri(() => library.Hold(librarian, remember, book.UID));
            tri(() => library.Loan(librarian, remember, book.UID));
            //TODO:b 138. Return the book (should succeed)
            tri(() => transaction = library.Return(book.UID, transaction.Id));
            await _context.SaveChangesAsync();
            output.Add($"{DateTime.Now.Ticks} Returned - {member.UserName} ({book.State})");
            //TODO:b 139. Test another member trying to hold and then loan the book which has now been returned (should succeed)
            tri(() => transaction = library.Hold(librarian, remember, book.UID));
            await _context.SaveChangesAsync();
            output.Add($"{DateTime.Now.Ticks} On Hold - {remember.UserName} ({book.State})");
            tri(() => transaction = library.Loan(librarian, remember, book.UID));
            await _context.SaveChangesAsync();
            output.Add($"{DateTime.Now.Ticks} On Loan - {remember.UserName} ({book.State})");
            //TODO:b 140. Return the book (should succeed)
            tri(() => transaction = library.Return(book.UID,transaction.Id)); 
            await _context.SaveChangesAsync();
            output.Add($"{DateTime.Now.Ticks} Returned - {remember.UserName} ({book.State})");
            //TODO:b 141. Schedule a 7 day restoration for the book (should succeed)
            tri(() => library.ScheduleRestoration(librarian, book.UID, DateTime.Now.AddMilliseconds(1), DateTime.Now.AddDays(42%35)));
            await _context.SaveChangesAsync();
            output.Add($"{DateTime.Now.Ticks} Restoration Scheduled (notice provided to restorer ~1ms) ({book.State})");
            //TODO:b 142. Test trying to hold whilst being restored  (should fail)
            errors.Add($"{DateTime.Now.Ticks} Try hold whilst being restored (should fail)");
            tri(() => transaction = library.Hold(librarian,member, book.UID));
            //TODO:b 143. Test trying to loan whilst being restored  (should fail)
            errors.Add($"{DateTime.Now.Ticks} Try loan whilst being restored (should fail)");
            tri(() => transaction = library.Loan(librarian,member, book.UID));
        }
        catch (Exception ex)
        {
            errors.Add(ex.Message);
        }
        return (book,output.ToArray(),errors.ToArray());
    }
    //TODO:b 176. Add MapToAdaptResult record
    public record MapToAdaptResult
        (LibrarianDTO Librarian,MemberDTO Member,ReferenceDTO Reference, BookDTO[] Books,TransactionDTO[] Transactions,RestorationDTO[] Restorations);
    //TODO:b 177. Implement MapToAdapt to adapt the Book, Librarian, Member, Reference, Restoration and Transaction Entities to mapped DTO
    public async Task<MapToAdaptResult> MapToAdapt()
    {
        var library = await _context.Libraries
            .Include(l => l.Users)
                .ThenInclude(u => (u as Librarian)!.Restorations.Where(r => r.Start < DateTime.Now && r.End > DateTime.Now))
            .Include(l => l.References)
                .ThenInclude(r => r.Books)
                   .ThenInclude(b => b.Transactions)
            .FirstAsync();
        //TODO:b 180. Return the record with the adaptations 
        return new(       
            library.Users.OfType<Librarian>().First().Adapt<LibrarianDTO>(),
            library.Users.OfType<Member>().First().Adapt<MemberDTO>(),
            library.References.First().Adapt<ReferenceDTO>(),
            library.References.SelectMany(r => r.Books).Adapt<BookDTO[]>(),          
            library.References.SelectMany(r => r.Books).SelectMany(b=>b.Transactions).Adapt<TransactionDTO[]>(),
            library.References.SelectMany(r => r.Books).SelectMany(b => b.Restorations).Adapt<RestorationDTO[]>()
          );
    }
    //TODO:b 181. Add BarrelRoll to create and persist a Library with 10 Librarians, 10 Members, 3 References with 3 Books each
    public async Task<Library> BarrelRoll()
    {
        var library = new Library("Library 1");
        _context.Libraries.Add(library);
        await RollOutRolesAsync();
        for (int i = 1; i < 11; i++)
        {
            var l = new Librarian(library.Id, $"L{i}@e", $"L{i}", $"{i}L", $"L{i}@e");
            await _userManager.CreateAsync(l, "1");
            await _userManager.AddToRoleAsync(l, Roles.LIBRARIAN.ToString());
            var m = new Member(library.Id, $"M{i}e", $"M{i}", $"{i}M", $"M{i}@e");
            await _userManager.CreateAsync(m, "1");
            await _userManager.AddToRoleAsync(m, Roles.MEMBER.ToString());
        }
        var ls = library.Users.OfType<Librarian>().ToArray();
        var ms = library.Users.OfType<Member>().ToArray();
        for (int i = 1; i < 4; i++)
        {
            var r = new Reference(library, $"000-000000000{i}", $"Generics v{i}", "Gene.XYZ.Rics", books: 3);
            library.References.Add(r);

        }
        await _context.SaveChangesAsync();
        //Volume 1 Book Specimens
        var bs = library.References[0].Books;
        //First Book is held by the first Librarian and Member
        library.Hold(ls[0], ms[0], bs[0].UID);
        //Second Book is loaned by the first Librarian and second Member
        library.Loan(ls[0], ms[1], bs[1].UID);
        //Third Book is scheduled for restoration by the first Librarian from now for 13 days
        library.ScheduleRestoration(ls[0], bs[2].UID, DateTime.Now, DateTime.Now.AddDays(13));
        await _context.SaveChangesAsync();

        //Volume 2 Book Specimens
        //**Note: domain business rules should not allow transactions and restorations to be created in the past.
        //The historical data could be provided from data seeding or such.
        //This project provides examples of implementing business rules on many occasions
        //but also evdeavours to not overly complicate the code with too many.
        bs = library.References[1].Books;
        //First Book is held by the first Librarian and Member
        var t = library.Hold(ls[0], ms[0], bs[0].UID);
        //timewarp the hold to 6 weeks ago
        t.Start = DateTime.Now.AddDays(-(7 * 6));
        //set it free (it should not be included when only active transactions are loaded)
        library.Release(bs[0].UID, t.Id);
        //Second Book is loaned by the first Librarian and second Member
        t = library.Loan(ls[0], ms[1], bs[1].UID);
        //timewarp the loan to 6 weeks ago
        t.Start = DateTime.Now.AddDays(-(7 * 6));
        //return it to bibliotheca (it should not be included when only active transactions are loaded)
        library.Return(bs[1].UID, t.Id);
        //Third Book is scheduled for restoration by the first Librarian from 6 weeks ago for 2 weeks       
        library.ScheduleRestoration(ls[0], bs[2].UID, DateTime.Now.AddDays(-(7 * 6)), DateTime.Now.AddDays(-(7 * 4)));
        await _context.SaveChangesAsync();
        return library;
    }

}