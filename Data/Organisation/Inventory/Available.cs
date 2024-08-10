//TODO:a 81. Add using Tela.Data.Core to Available
using Tela.Data.Core;
namespace Tela.Data.Organisation.Inventory;
//TODO:a 80. Add Available State for Book
public class Available : State
{
    public override Types Type => Types.Available;
    public override Transaction Hold(Func<Transaction> f) => f();
    public override Transaction Release(Func<Transaction> f) => throw new DomainRuleException("Book is currently not on hold");
    public override Transaction Loan(Func<Transaction> f) => f();
    public override Transaction Return(Func<Transaction> f) => throw new DomainRuleException("Book is currently not on loan");
}