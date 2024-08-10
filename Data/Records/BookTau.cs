using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Tela.Data.Core;
namespace Tela.Data.Records;
//TODO:b 376. Add BookTau  
public class BookTau
{
    public BookTau(ApplicationDbContext context,Guid uid, DateTime start, DateTime end)
        => Records = context.Database.SqlQueryRaw<Record>(Query,
            new SqlParameter("@uid",uid),
			new SqlParameter("@start",start),
			new SqlParameter("@end",end)
        ).ToArray(); 

    public IReadOnlyList<Record> Records { get; private set; } 
    public class Record
    {
        public int Id { get; set; }
        public Guid UID { get; set; } = default!;
        public Guid MemberId { get; set; } = default!;
        public Guid LibrarianId { get; set; } = default!;
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Type { get; set; } = default!;
    }
	//TODO:b 461. Update BookTau Query to now be schema qualified
    private string Query = 
    $@"WITH Book_CTE (Id, [UID],MemberId,LibrarianId,[Start],[End],[Type])
    AS
    (
        select
	    cast(h.Id as Int) Id, 
	    cast(h.BookUID as uniqueidentifier) [UID], 
	    cast(h.MemberId as uniqueidentifier) MemberId, 
	    cast(h.LibrarianId as uniqueidentifier) LibrarianId,
	    h.[Start], h.[End],
	    'HOLD' [Type] from [trance.actions].Holds h
	    union
	    select cast(l.Id as Int) Id, 
	    cast(l.BookUID as uniqueidentifier) [UID],
	    cast(l.MemberId as uniqueidentifier) MemberId,
	    cast(l.LibrarianId  as uniqueidentifier) LibrarianId, 
	    l.[Start],l.[End],
	    'LOAN' [Type]  from [trance.actions].Loans l
	    union
	    select cast(r.Id as Int) Id, 
	    cast(r.BookUID as uniqueidentifier) [UID],
	    cast(0x0 as uniqueidentifier) MemberId,
	    cast(r.LibrarianId as uniqueidentifier) LibrarianId,
	    r.[Start],r.[End],
	    'RESTORATION' [Type] from [inventory].Restorations r
    )
    select * from Book_CTE where Book_CTE.[UID] = @uid 
	and 
	(
		(Book_CTE.[Start] >= @start or Book_CTE.[Start] < @end)
		or 
		(Book_CTE.[End] > @start or Book_CTE.[End] <= @end)
	)";
}