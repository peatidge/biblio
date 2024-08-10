create procedure [dbo].[GetTranceActionsStatim] 
@libraryId int, @start datetime
as
begin
declare @holdCount decimal = 
(
	select count(h.Id) from [trance.actions].Holds h
	inner join [inventory].[Books] b on b.[UID] = h.BookUID
	inner join [inventory].[References] r on r.Id = b.ReferenceId 
	where r.LibraryId = @libraryId and h.[Start] > @start
); 
declare @loanCount decimal = 
(
	select count(l.Id) from [trance.actions].Loans l
	inner join [inventory].[Books] b on b.[UID] = l.BookUID
	inner join [inventory].[References] r on r.Id = b.ReferenceId 
	where r.LibraryId = @libraryId and l.[Start] > @start
);
declare @restorationCount decimal = 
(
	select count(rs.Id) from [inventory].Restorations rs
	inner join [inventory].[Books] b on b.[UID] = rs.BookUID
	inner join [inventory].[References] r on r.Id = b.ReferenceId 
	where r.LibraryId = @libraryId and rs.[Start] > @start
);
select 
r.ISBN,r.Title,
count(b.UID) Books,
(
	select count(*) from [trance.actions].[Holds] h
	inner join [inventory].Books b on b.ReferenceId = r.Id 
	where h.BookUID = b.UID and h.[Start] > @start
) Holds,
(
	select coalesce(nullif(count(*),0) / @holdCount * 100,0)
	from [trance.actions].[Holds] h
	inner join [inventory].Books b on b.ReferenceId = r.Id 
	where h.BookUID = b.UID and h.[Start] > @start
) HoldPercentage,
(
	select count(*) from [trance.actions].[Loans] l
	inner join [inventory].Books b on b.ReferenceId = r.Id 
	where l.BookUID = b.UID and l.[Start] > @start
) Loans,
(
	select coalesce(nullif(count(*),0) / @loanCount * 100,0)
	from [trance.actions].[Loans] l
	inner join [inventory].Books b on b.ReferenceId = r.Id 
	where l.BookUID = b.UID and l.[Start] > @start
) LoanPercentage,
(
	select count(*) from [inventory].[Restorations] rs
	inner join [inventory].Books b on b.ReferenceId = r.Id 
	where rs.BookUID = b.UID and rs.[Start] > @start
) Restorations,
(
	select coalesce(nullif(count(*),0) / @restorationCount * 100,0)
	from [inventory].[Restorations] rs
	inner join [inventory].Books b on b.ReferenceId = r.Id 
	where rs.BookUID = b.UID and rs.[Start] > @start
) RestorationPercentage
from [inventory].[References] r
left outer join [inventory].[Books] b on b.ReferenceId = r.Id
where r.LibraryId = @libraryId
group by r.Id,r.ISBN,r.Title
end