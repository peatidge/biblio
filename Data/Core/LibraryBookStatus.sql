CREATE VIEW [dbo].[LibraryBookStatus]
AS
select
l.Id,
l.Name,
r.ISBN,
r.Title,
count(b.UID) Count,
SUM(CASE WHEN lo.[End] is not null THEN 1 ELSE 0 End) AS OnLoan,
SUM(CASE WHEN h.[End] is not null THEN 1 ELSE 0 End) AS OnHold,
SUM(CASE WHEN re.[End] is not null THEN 1 ELSE 0 End) AS BeingRestored,
SUM(CASE WHEN lo.[End] is null and h.[End] is null and re.[End] is null THEN 1 ELSE 0 End) AS Available
from organisation.Libraries l
inner join [inventory].[References] r on r.LibraryId = l.Id
inner join inventory.Books b on b.ReferenceId = r.Id
left outer join [trance.actions].Loans lo on lo.BookUID = b.UID and lo.IsDeleted = 0 and lo.[End] > GETDATE()
left outer join [trance.actions].Holds h on h.BookUID = b.UID and h.IsDeleted = 0 and h.[Start] < GETDATE()and h.[End] > GETDATE()
left outer join inventory.Restorations re on re.BookUID = b.UID and re.[Start] < GETDATE()and re.[End] > GETDATE()
group by l.Id, l.Name, r.ISBN, r.Title