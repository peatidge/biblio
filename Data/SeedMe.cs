using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Tela.Data.Organisation;
using Tela.Data.Organisation.Inventory;
using Tela.Models;
namespace Tela.Data;
//TODO:b 445. Add SeedMe class to Data
public interface ISeedMe
{
    void Seed(ModelBuilder mb);
}
public class SeedMe :ISeedMe
{  
    public void Seed(ModelBuilder mb) 
    {
        foreach (string r in new[] { "LIBRARIAN", "MEMBER" })
        {
            mb.Entity<IdentityRole>().HasData(new { Id = $"{r[0]}0112358-1321-3455-8900-000000000042", Name = r, NormalizedName = r });
        }
        foreach (var l in Genus.Libraries)
        {
            SeedLibrary(mb,l);
        }
    }
    //TODO:b 471. Update SeedLibrary to include seed data for Holds, Loans & Restorations
    private int _referenceId = 1, _bc = 1, _transactionId = 1, _restorationId = 1;
    private void SeedLibrary(ModelBuilder mb,(int Id, string Name, string Domain, string Blurbosity) library)
    {
        mb.Entity<Library>().HasData(new { library.Id, library.Name,library.Blurbosity});
        var users = new List<LibraryUser>();
        foreach (var u in Genus.Users)
        {        
            string username = $"{u.FirstName}.{u.LastName}@{library.Domain}";
            LibraryUser user = u.Role == "L"
             ? new Librarian(library.Id, username, u.FirstName, u.LastName, username)
             : new Member(library.Id,username,u.FirstName,u.LastName, username);
            user.Id = Guid.NewGuid().ToString();
            user.PasswordHash = "AQAAAAIAAYagAAAAEEuuNou4A6Qtew94aXLElxSU+e9FH7gOqxg/v5GYkXM3WO7cLNSq/uaV/LIuoqavEw==";
            user.SecurityStamp = "VBJMLIX57VXKL2RYG5PM775LZLLJSOEG";
            user.Registered = DateTime.Now;
            user.EmailConfirmed = true;    
            user.NormalizedEmail = user.NormalizedUserName = username.ToUpper();
            switch (u.Role)
            {
                case "L": mb.Entity<Librarian>().HasData(user); break;
                case "M": mb.Entity<Member>().HasData(user); break;
            }
            users.Add(user); 
            mb.Entity<IdentityUserRole<string>>().HasData(new { UserId = user.Id, RoleId = $"{u.Role}0112358-1321-3455-8900-000000000042" });          
            //TODO:b 447. NOTE: Add Claim to User
            mb.Entity<IdentityUserClaim<string>>()
                .HasData(new{ Id = _bc++,UserId = user.Id,ClaimType="bibliotheca",ClaimValue=library.Id.ToString()}); 

        }  
        foreach ((string ISBN,string Title,string Author) r in Genus.References)
        {
            mb.Entity<Reference>().HasData(new{Id=_referenceId,r.ISBN,r.Title,r.Author,LibraryId=library.Id});
            for(int i = 0; i < Q.Int(1,11); i++)
            {
                var uid = Guid.NewGuid();
                mb.Entity<Book>().HasData(new { UID = uid, ReferenceId = _referenceId });               
                for(var start = DateTime.Now.AddYears(-2); start < DateTime.Now; start = start.AddDays(Q.Int(1,30)))
                {
                    var member = users.OfType<Member>().ElementAt(Q.Int(0,users.OfType<Member>().Count() -1));
                    var librarian = users.OfType<Librarian>().ElementAt(Q.Int(0,users.OfType<Librarian>().Count() - 1));
                    var end = start.AddDays(Q.Int(1,14));
                    if (Q.CoinFlip(50))
                    {
                        mb.Entity<Hold>().HasData(new { Id = _transactionId++, MemberId = member.Id, LibrarianId = librarian.Id, Start = start, End = end, Version = Guid.NewGuid(), BookUID = uid, IsDeleted = false });
                    }
                    else if (Q.CoinFlip(50))
                    {
                        mb.Entity<Loan>().HasData(new { Id = _transactionId++, MemberId = member.Id, LibrarianId = librarian.Id, Start = start, End = end, Version = Guid.NewGuid(), BookUID = uid, IsDeleted = false });
                    }
                    else
                    {
                        mb.Entity<Restoration>().HasData(new { Id = _restorationId++,  LibrarianId = librarian.Id, Start = start, End = end, BookUID = uid });
                    }                 
                    start = end;          
                }
            }
            _referenceId++; 
        }
    }
}
//TODO:b 446. Add Genus class to Data
public static class Genus
{
    public static (int Id, string Name, string Domain, string Blurbosity)[] Libraries
     => [(1, "Dobbs' Technology Library", "dobbs", "Chickety-Boo"), (2, "Anagignōskō", "subgenius", "Infinity & Yonder")]; 
        
    public static (string FirstName, string LastName, string Role)[] Users =>
    [
        ("Lib","Rarian","L"),
        ("Mem","Ber","M"),
        ("Page","Turner","M"),
        ("Read","Mindopenheimer","M"),
        ("Booker","Pages","M"),
        ("Libby","Arian","M"),
        ("Wise","Wordsworth","M"),
        ("Shelfa","Lotabooks","M"),
        ("Studious","Learnwell","M"),
        ("Quill","Penwise","M"),
        ("Biblios","Scriptor","M"),
        ("Quest","Knowmore","M"),
        ("Papyrus","Scrollio","M"),
        ("Atlas","Mapwright","M"),
        ("Nova","Enlight","M"),
        ("Thesaurus","Wordfinder","M"),
        ("Codex","Anciento","M"),
        ("Leaf","Through","M"),
        ("Clever","Readly","M"),
        ("Serif","Typography","M"),
        ("Mystery","Finder","M"),
        ("Tome","Raider","M"),
        ("Folio","Binder","M"),
        ("Lorem","Ipsum","M"),
        ("Echo","Primer","M"),
        ("Archive","Keeper","M"),
        ("Grimoire","Spellbound","M"),
        ("Legend","Tales","M"),
        ("Pixel","Display","M"),
        ("Cyber","Text","M"),
        ("Lexicon","Compendium","M"),
        ("Doc","Umenta","M"),
        ("Ink","Well","M"),
        ("Dusty","Shelves","M"),
        ("Scroll","Ancient","M"),
        ("Index","Lookup","M"),
        ("Cite","Source","M"),
        ("Glossa","Ridex","M"),
        ("Narrative","Saga","M"),
        ("Edition","Print","M"),
        ("Quote","Citeable","M"),
        ("Cliff","Hangar","M"),
        ("Blip","Cosmonomicon","M"),
        ("Xylo","Bibliotron","M"),
        ("Zap","Scriptoid","M"),
        ("Klix","Pageflip","M"),
        ("Snarf","Textoid","M"),
        ("Bloop","Datastar","M"),
        ("Zork","Scriptzor","M"),
        ("Vex","Narratex","M"),
        ("Glim","Scriptog","M"),
        ("Nook","Readar","M"),
        ("Flap","Journy","M"),
        ("Scribble","Plottwist","M"),
        ("Quirk","Novellia","M")
    ];
    public static (string ISBN, string Title, string Author)[] References =>
    [
        ("978-0201835953","The Mythical Byte","D.Igital Promethius"),
        ("978-0136042594","Artificial Intelligence","A.L. Lofus" ),
        ("978-0262510875","Byte Club","0011001100"),
        ("978-0201633610","Design Patterns & Reusable Software","Chuck Gravitons" ),
        ("978-0132350884","Char Wars","A. Letter"),
        ("978-0262033848","Introduction to Algorithms","Stooks Net"),
        ("978-0131103627","Stops the Motor of the World","John Galt" ),
        ("978-0520150380","Lord of the Strings","karnt parse"),
        ("000-0000000000","Lateralus~Nibble V0","Fibonacci"),
        ("001-0000000000","Lateralus~Nibble V1","Fibonacci"),
        ("001-0000000001","Lateralus~Nibble V1","Fibonacci"),
        ("002-0000000000","Lateralus~Nibble V2","Fibonacci"),
        ("003-0000000000","Lateralus~Nibble V3","Fibonacci"),
        ("005-0000000000","Lateralus~Nibble V5","Fibonacci"),
        ("878-0201835953","Halt & Catch Fire","Nero D. Idit"),
        ("878-0136042594","Timing & Race Conditions","Cronos"),
        ("878-0262510875","'Software Engineering'","Margaret hamilton"),
        ("878-0201633610","I.T. Unprocessed","E.A. Torganics"),
        ("878-0132350884","Viruses & Worm(hole)s","V.O. Idrus"),
        ("878-0262033848","T.O.M.A", "Caliente Brothers"),
        ("878-0220133811","Vapourware (etheral edition)","Green Tony & Jacky Blue Note")
    ];
}