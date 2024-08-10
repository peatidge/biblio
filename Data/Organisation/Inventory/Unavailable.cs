//TODO:a 83. Add using Tela.Data.Core to Unavailable
using Tela.Data.Core;
namespace Tela.Data.Organisation.Inventory;
//TODO:a 82. Add Unavailable State for Book
public class Unavailable : State
{
    public override Types Type => Types.Unavailable;
    public override Transaction Hold(Func<Transaction> f) => throw new DomainRuleException("I'm a busy book at the moment");
    public override Transaction Release(Func<Transaction> f) => f();
    public override Transaction Loan(Func<Transaction> f) => throw new DomainRuleException("I'm a busy book at the moment");
    public override Transaction Return(Func<Transaction> f) => f();
}