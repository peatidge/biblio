using Microsoft.AspNetCore.Identity;
//TODO:a 51. Add using Tela.Data.Organisation.Inventory to LibraryUser.cs
using Tela.Data.Organisation.Inventory;
namespace Tela.Data.Organisation;
//TODO: 3. Add LibraryUser (extends IdentityUser)
public abstract class LibraryUser : IdentityUser
{
    protected LibraryUser() { }
    public LibraryUser(int libraryId, string username, string firstName, string lastName, string email)
    {
        UserName = username;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        LibraryId = libraryId;
    }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public DateTime Registered { get; set; } = default!;
    public virtual Roles Role { get; private set; }
    //TODO: 9. Add LibraryId Foreign Key
    public int LibraryId { get; set; }
    //TODO:a 10. Add Library Navigation Property (implement bidirectional navigability)
    public Library Library { get; set; } = default!;
    //TODO:a 52. Add Transactions Collection Navigation Property to LibraryUser
    public List<Transaction> Transactions { get; set; } = new();

}
//TODO: 4. Add Librarian (extends LibraryUser)
public class Librarian : LibraryUser
{
    protected Librarian() { }
    public Librarian(int libraryId, string username, string firstName, string lastName, string email) 
        : base(libraryId, username, firstName, lastName, email)
    {
    }
    public override Roles Role => Roles.LIBRARIAN;
    //TODO:a 54. Add Restoration Collection Navigation Property to Librarian
    public List<Restoration> Restorations { get; set; } = new();
}
//TODO: 5. Add Member (extends LibraryUser)
public class Member : LibraryUser
{
    protected Member() { }
    public Member(int libraryId, string username, string firstName, string lastName, string email) 
        : base(libraryId, username, firstName, lastName, email)
    {
    }
    public override Roles Role { get => Roles.MEMBER; } 
}