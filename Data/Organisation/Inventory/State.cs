namespace Tela.Data.Organisation.Inventory;
//TODO:a 79. Add State abstract class for State Pattern application to Book
public abstract class State
{
    public enum Types { Available, Unavailable }
    public abstract Transaction Loan(Func<Transaction> f);
    public abstract Transaction Return(Func<Transaction> f);
    public abstract Transaction Hold(Func<Transaction> f);
    public abstract Transaction Release(Func<Transaction> f);
    public abstract Types Type { get; }
}