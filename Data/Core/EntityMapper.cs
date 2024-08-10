using Microsoft.EntityFrameworkCore;
using Tela.Data.Organisation;
//TODO:a 57. Add using Tela.Data.Organisation.Inventory in EntityMapper
using Tela.Data.Organisation.Inventory;
using Tela.Data.Records;
namespace Tela.Data.Core;
//TODO:a 11. Add EntityMapper
public class EntityMapper
{
    public EntityMapper(ModelBuilder mb)
    {
        //TODO:a 12. Add Initial Library Entity Mapping
        mb.Entity<Library>(l =>
        {
            l.Property(l => l.Name).IsRequired()
                .HasMaxLength(100);
            l.Property(l => l.Blurbosity).HasDefaultValue("Blurb");
            l.HasMany(l => l.Users)
                .WithOne(p => p.Library)
                .HasForeignKey(p => p.LibraryId)
                .OnDelete(DeleteBehavior.Restrict);
            //TODO:a 50. Map Library to Reference Relationship
            l.HasMany(c => c.References)
                .WithOne(r => r.Library)
                .HasForeignKey(r => r.LibraryId)
                .OnDelete(DeleteBehavior.Restrict);
            //TODO:b 454. Update Library entity to map to table in organisation schema
            l.ToTable("Libraries", schema: "organisation"); 
        });
        //TODO:a 13. Add Initial LibraryUser Entity Mapping
        mb.Entity<LibraryUser>(u =>
        {
            u.Property(u => u.Role)
             .HasConversion<int>();
            u.HasDiscriminator(u => u.Role)
               .HasValue<Librarian>(Roles.LIBRARIAN)
               .HasValue<Member>(Roles.MEMBER);
        });
        //TODO:a 14. Add Initial Librarian Entity Mapping
        mb.Entity<Librarian>(l =>
        {
            //TODO:a 53. Map Librarian to Transaction Relationship
            l.HasMany(u => u.Transactions)
              .WithOne(t => t.Librarian)
              .HasForeignKey(t => t.LibrarianId)
              .OnDelete(DeleteBehavior.Restrict);
            //TODO:a 55. Map Librarian to Restoration Relationship
            l.HasMany(u => u.Restorations)
                .WithOne(r => r.Librarian)
                .HasPrincipalKey(r => r.Id)
                .HasForeignKey(r => r.LibrarianId)
                .OnDelete(DeleteBehavior.Restrict);
        });
        //TODO:a 15. Add Initial Member Entity Mapping
        mb.Entity<Member>(m =>
        {
            //TODO:a 56. Map Member to Transaction Relationship
            m.HasMany(u => u.Transactions)
             .WithOne(t => t.Member)
             .HasForeignKey(t => t.MemberId)
             .OnDelete(DeleteBehavior.Restrict);
        });
        //TODO:a 58. Add Reference Entity Mapping
        mb.Entity<Reference>(r =>
        {
            r.Property(r => r.ISBN)
                .IsRequired().HasMaxLength(20);
            r.Property(r => r.Title)
                .IsRequired().HasMaxLength(250);
            r.Property(r => r.Author)
                .IsRequired().HasMaxLength(250);
            r.HasMany(r => r.Books)
            .WithOne(b => b.Reference)
            .HasForeignKey(b => b.ReferenceId)
            .OnDelete(DeleteBehavior.Restrict);
            //TODO:a 59. Implement Surrogate Key for Reference Entity
            r.HasKey(r => r.Id);
            r.HasIndex(r => new { r.ISBN, r.LibraryId }).IsUnique();
            //TODO:b 124. Update Reference Entity Mapping to ignore derived properties
            r.Ignore(r => r.Available)
           .Ignore(r => r.Unavailable)
           .Ignore(r => r.IsAvailable)
           .Ignore(r => r.AvailableCount)
           .Ignore(r => r.UnavailableCount)
           .Ignore(r => r.OnLoan)
           .Ignore(r => r.OnLoanCount)
           .Ignore(r => r.OnHold)
           .Ignore(r => r.OnHoldCount)
           .Ignore(r => r.BeingRestored)
           .Ignore(r => r.BeingRestoredCount);
            //TODO:b 455. Update Reference entity to map to table in inventory schema
            r.ToTable("References",schema:"inventory");
        });
        //TODO:a 60. Add Book Entity Mapping
        mb.Entity<Book>(b =>
        {
            b.HasKey(b => b.UID);
            b.Property(b => b.UID).HasDefaultValueSql("NEWID()");
            b.HasMany(b => b.Restorations)
              .WithOne(r => r.Book)
              .HasForeignKey(r => r.BookUID)
              .OnDelete(DeleteBehavior.Restrict);
            b.HasMany(b => b.Transactions)
            .WithOne(t => t.Book)
            .HasForeignKey(t => t.BookUID)
            .OnDelete(DeleteBehavior.Restrict);
            //TODO:b 125. Update Book Entity Mapping to ignore derived properties
            b.Ignore(b => b.State)
            .Ignore(b => b.ActiveTransactions)
            .Ignore(b => b.Loans)
            .Ignore(b => b.Holds)
            .Ignore(b => b.IsOnLoan)
            .Ignore(b => b.IsOnHold)
            .Ignore(b => b.ActiveRestorations)
            .Ignore(b => b.IsBeingRestored);
            //TODO:b 456. Update Book entity to map to table in inventory schema
            b.ToTable("Books", schema: "inventory");
        });
        //TODO:a 61. Add Transaction Entity Mapping
        mb.Entity<Transaction>(t =>
        {
            //TODO:a 62. Implement Table Per Concrete Type Mapping Strategy for Transaction Entity
            t.UseTpcMappingStrategy();
            t.Ignore(t => t.Type);
            //TODO:b 476. Add IX_Transactions_Start Index (then nuke > genus)
            t.HasIndex(t => t.Start,"IX_Transactions_Start").IncludeProperties(t=>t.BookUID);
            t.Property<DateTime>("Timestamp")
                .HasDefaultValueSql("GETDATE()")
                .ValueGeneratedOnAddOrUpdate();
            //TODO:a 63. Implement Concurrent Control Mapping for Transaction Entity 
            t.Property(t => t.Version).IsConcurrencyToken();
            //TODO:b 457. Update Transaction entities (Loan/Hold) to map to table in inventory schema
            mb.Entity<Loan>().ToTable("Loans",schema:"trance.actions");
            mb.Entity<Hold>().ToTable("Holds",schema:"trance.actions");
        });
        //TODO:a 64. Add Restoration Entity Mapping
        mb.Entity<Restoration>(r => {
            //TODO:b 458. Update Restoration entity to map to table in inventory schema
            r.ToTable("Restorations",schema: "inventory");
        });
        //TODO:b 428a. Add LibraryBookStatus Entity Mapping (Keyless Entity)
        mb.Entity<LibraryBookStatus>(lbs => lbs.HasNoKey().ToView("LibraryBookStatus",schema:"tau"));
        //TODO:b 436. Add Graviton Entity Mapping (add migration CalibrateGraviton and update database)
        //TODO:b 437. Run provided script InsertAsciiData.sql against the database
        mb.Entity<Graviton>(g =>
        {
            g.Property(g => g.Wave).IsUnicode(true).HasMaxLength(10000);
            g.Property(g => g.Duration).HasDefaultValue(250);
        });   
    }
    //TODO:b 424. Add static method EnsureExistDance to EntityMapper 
    //This is an idempotent method that ensures the existence of SQL view(s) and/or records in the database
    public static void EnsureExistDance(IConfiguration configuration)
    {
        using var _context = new ApplicationDbContext(
            new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(configuration.GetConnectionString("DefaultConnection")).Options,
            new SeedMe()
        );
        if (_context.Database.CanConnect())
        {
            //TODO:b 459. Update Create LibraryBookStatus View to now be schema qualified
            _context.Database.ExecuteSqlRaw(
                @"IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'tau')
                    EXEC('CREATE SCHEMA tau');
                IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_SCHEMA = 'tau' AND TABLE_NAME = 'LibraryBookStatus')
                    BEGIN
                    EXEC('CREATE VIEW [tau].[LibraryBookStatus]
                    AS
                        SELECT l.Id, l.Name, r.ISBN, r.Title, COUNT(b.UID) AS Count,
                        SUM(CASE WHEN lo.[End] IS NOT NULL THEN 1 ELSE 0 END) AS OnLoan,
                        SUM(CASE WHEN h.[End] IS NOT NULL THEN 1 ELSE 0 END) AS OnHold,
                        SUM(CASE WHEN re.[End] IS NOT NULL THEN 1 ELSE 0 END) AS BeingRestored,
                        SUM(CASE WHEN lo.[End] IS NULL AND h.[End] IS NULL AND re.[End] IS NULL THEN 1 ELSE 0 END) AS Available
                        FROM [organisation].[Libraries] l
                        INNER JOIN [inventory].[References] r ON r.LibraryId = l.Id
                        INNER JOIN [inventory].[Books] b ON b.ReferenceId = r.Id
                        LEFT OUTER JOIN [trance.actions].[Loans] lo ON lo.BookUID = b.UID AND lo.IsDeleted = 0 AND lo.[End] > GETDATE()
                        LEFT OUTER JOIN [trance.actions].[Holds] h ON h.BookUID = b.UID AND h.IsDeleted = 0 AND h.[Start] < GETDATE() AND h.[End] > GETDATE()
                        LEFT OUTER JOIN [inventory].[Restorations] re ON re.BookUID = b.UID AND re.[Start] < GETDATE() AND re.[End] > GETDATE()
                        GROUP BY l.Id, l.Name, r.ISBN, r.Title')
                    END;"
            );
        }
    }
}