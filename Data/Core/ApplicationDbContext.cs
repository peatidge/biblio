using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Tela.Data.Organisation;
using Tela.Data.Organisation.Inventory;
using Tela.Data.Records;
namespace Tela.Data.Core;
//TODO: 6. Update ApplicationDbContext (extends IdentityDbContext<LibraryUser>)
public class ApplicationDbContext : IdentityDbContext<LibraryUser>
{
    private readonly ISeedMe _seedMe;
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ISeedMe seedMe)
        : base(options)
    {
        _seedMe = seedMe;
    }
    //TODO:a 65. Add Library DbSet (Libraries)
    public DbSet<Library> Libraries { get; set; }
    //TODO:a 66. Add Reference DbSet (References)
    public DbSet<Reference> References { get; set; }
    //TODO:a 67. Add Book DbSet (Books)
    public DbSet<Book> Books { get; set; }
    //TODO:a 68. Add Transaction DbSet (Transactions)
    public DbSet<Transaction> Transactions { get; set; }
    //TODO:a 69. Add Loan DbSet (Loans)
    public DbSet<Loan> Loans { get; set; }
    //TODO:a 70. Add Hold DbSet (Holds)
    public DbSet<Hold> Holds { get; set; }
    //TODO:a 71. Add Restoration DbSet (Restorations)
    public DbSet<Restoration> Restorations { get; set; }
    //TODO:b 427. Add LibraryBookStatus DbSet (LibraryBookStatuses) for SQL View
    public DbSet<LibraryBookStatus> LibraryBookStatuses { get; set; }
    //TODO:b 435. Add Graviton DbSet (Gravitons)
    public DbSet<Graviton> Gravitons { get; set; }
    //TODO:b 183. Add GetLibraryQuery method to ApplicationDbContext
    public IQueryable<Library> GetLibraryQuery(int libraryId, string librarianId, TemporalisLibri temporal, string? memberId = null, DateTime start = default)
    {
        //TODO:b 341. Validate Librarian :Ensure any caller of this method has a valid librarianId and Librarian associated enitity
        var librarian = Users.FirstOrDefault(u => u.Id == librarianId && u.Role == Organisation.Roles.LIBRARIAN) 
            ?? throw new DomainRuleException("Librarian: FourZeroFour");
        //TODO:b 477. Update Query to include start date filter
        IQueryable<Library> query = Libraries.Where(l => l.Id == libraryId); 

        IIncludableQueryable<Library,IList<Book>> includes 
            = query.Include(l => l.Users).Include(l => l.References).ThenInclude(r => r.Books);
        
        switch (temporal)
        {
            //TODO:b 208. Implement GetLibraryQuery Logic
            case TemporalisLibri.ActiveTransactionsOnly:
                query = includes.ThenInclude(b => b.Transactions.Where(t => t.End == DateTime.MaxValue)).AsQueryable();
                break;
            case TemporalisLibri.ActiveRestorationsOnly:
                query = includes.ThenInclude(b => b.Restorations.Where(t => t.Start <= DateTime.Now && t.End >= DateTime.Now)).AsQueryable();
                break;
            case TemporalisLibri.ActiveTransactionsAndRestorations:
                query = includes.ThenInclude(b => b.Transactions.Where(t => t.End == DateTime.MaxValue))
                                .Include(r => r.References).ThenInclude(r => r.Books)
                                    .ThenInclude(b => b.Restorations.Where(t => t.Start <= DateTime.Now && t.End >= DateTime.Now))
                                    .AsQueryable();
                break; 
            case TemporalisLibri.AllTransactions:
                query = includes.ThenInclude(b => b.Transactions.Where(t=>t.Start > start)).AsQueryable();
                break; 
            case TemporalisLibri.AllRestorations:
                query = includes.ThenInclude(b => b.Restorations.Where(rs => rs.Start > start)).AsQueryable();
                break; 
            case TemporalisLibri.AllTransactionsAndRestorations:
                query = includes.ThenInclude(b => b.Transactions.Where(t => t.Start > start))
                                .Include(r => r.References).ThenInclude(r => r.Books)
                                .ThenInclude(b => b.Restorations.Where(rs => rs.Start > start))
                                .AsQueryable();
                break; 
            case TemporalisLibri.Forgetful:
                query = includes.AsQueryable();
                break;
            default:
                throw new DomainRuleException("Invalid Temporalis (Stabilization Failure)"); 
        };

        return query; 
    }
    //TODO:b 378. Add Tau Method to ApplicationDbContext
    public BookTau Tau(Guid uid, DateTime start, DateTime end) => new BookTau(this, uid, start, end);
    //TODO:a 19. Override OnModelCreating
    override protected void OnModelCreating(ModelBuilder mb)
    {
        //TODO:a 20. Call base.OnModelCreating (this is required for mapping the Identity tables e.g. Users to Roles N-N joining table etc...)
        base.OnModelCreating(mb);
        //TODO:a 21. Add EntityMapper for custom domain entity mappings
        new EntityMapper(mb);
        //TODO:b 450. Invoke SeedMe() from OnModelCreating
        //dotnet ef migrations add SeedMe
        //delete database
        //dotnet ef database update :)
        //remember Graviton Ascii Art
        _seedMe.Seed(mb);
        //TODO:b 475. Nuke Db & Migrations (delete database and migrations folder)
        //Add new migration: dotnet ef migrations add Genus
        //Update database: dotnet ef database update
        //Execute scripts InsertReferenceSprocStat.sql and InsertAsciiData.sql against new database
    }
}           