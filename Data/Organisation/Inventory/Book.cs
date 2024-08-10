using Tela.Data.Core;
namespace Tela.Data.Organisation.Inventory;
//TODO:a 40. Add Book Entity
public class Book
{
    //TODO:a 84. Add private State _state field to Book
    private State _state = default!;
    private Book(int referenceId, Guid uID)
    {
        UID = uID; 
        ReferenceId = referenceId;
        //TODO:b 101. Invoke StateCheck method in Book constructor overload (called by dbcontext)
        StateCheck(); 
    }
    public Book(Reference reference)
    {
        Reference = reference;
        //TODO:b 102. Invoke StateCheck method in Book constructor overload (new local instance)
        StateCheck();
    }
    public Guid UID { get; private set; }
    public int ReferenceId { get; private set; } = default!;
    public Reference Reference { get; private set; } = default!;
    //TODO:a 45. Add Transactions Collection Navigation Property
    public List<Transaction> Transactions { get; private set; } = new();
    //TODO:b 460. Add Indexer for Transactions in Book
    public Transaction this[int transactionId] 
        => Transactions.FirstOrDefault(t => t.Id == transactionId) ?? throw new DomainRuleException("Transaction Absentium");
    //TODO:a 85. Add Filtered Transactions collection for only current transactions (end date is max value used to flag active transaction)
    public IReadOnlyList<Transaction> ActiveTransactions => Transactions.Where(t => t.End == DateTime.MaxValue).ToList();
    //TODO:a 86. Add Filtered Transactions collection for only Transactions of type Loan
    //and derived IsOnLoan property that checks current transactions for Loans
    public IReadOnlyList<Loan> Loans => Transactions.OfType<Loan>().ToList();
    public bool IsOnLoan => Loans.Any(l => l.End == DateTime.MaxValue);
    //TODO:a 87. Add Filtered Transactions collection for only Transactions of type Hold
    //and derived IsOnHold property that checks current transactions for Holds
    public IReadOnlyList<Hold> Holds => Transactions.OfType<Hold>().ToList();
    public bool IsOnHold => Holds.Any(h => h.End == DateTime.MaxValue);
    //TODO:a 49. Add Restorations Collection Navigation Property
    public List<Restoration> Restorations { get; private set; } = new();
    //TODO:a 89. Add Filtered Restorations collection for only active restorations (start and end overlaps now...tau)
    public IReadOnlyList<Restoration> ActiveRestorations => Restorations.Where(t => t.Start <= DateTime.Now && t.End >= DateTime.Now).ToList();
    //TODO:a 90. Add derived IsBeingRestored property that checks active restorations
    public bool IsBeingRestored => ActiveRestorations.Any();
    //TODO:a 91. Add State Property derived from _state field
    public State.Types State => _state?.Type ?? Inventory.State.Types.Unavailable;
    //TODO:a 92. Add StateCheck method to update State based on current transactions and restorations
    public void StateCheck()
    {
        var type = IsOnLoan || IsOnHold || IsBeingRestored ? Inventory.State.Types.Unavailable : Inventory.State.Types.Available;
        _state = _state?.Type != type ? type switch
        {
            Inventory.State.Types.Available => new Available(),
            Inventory.State.Types.Unavailable => new Unavailable(),
            _ => throw new DomainRuleException("Invalid Book State: (superposition aperiodically unavailable)")
        } : _state;
    }
    //TODO:a 93. Add IsAvailable derived property that checks State
    public bool IsAvailable
    {
        get
        {
            StateCheck();
            return State == Inventory.State.Types.Available;
        }
    }
    //TODO:a 94. Add Hold method to create a new Hold transaction
    public Transaction Hold(Librarian librarian, Member member)
    {
        StateCheck();
        return _state.Hold(() =>
        {
            var t = new Hold(librarian, member, this);
            Transactions.Add(t);
            StateCheck();
            return t!;
        });
    }
    //TODO:a 96. Add Release method to finalise a Hold transaction
    public Transaction Release(int transactionId)
    {
        StateCheck();
        return _state.Release(() =>
        {
            var t = Transactions.FirstOrDefault(l => l.Id == transactionId) 
                ?? throw new DomainRuleException("No Hold found for the provided transaction identifier");          
            t.Finalise(this); 
            StateCheck();
            return t;
        });
    }
    //TODO:a 97. Add Loan method to create a new Loan transaction
    public Transaction Loan(Librarian librarian, Member member)
    {
        StateCheck();
        //auto release if existing hold for member i.e. the member is upgrading to a loan
        var transaction = Holds.LastOrDefault(t => t.MemberId == member.Id && t.LibrarianId == librarian.Id && t.End > DateTime.Now);
        if (transaction != null)
        {
            Release(transaction.Id);
        }
        return _state.Loan(() =>
        {
            var loan = new Loan(librarian, member, this);
            Transactions.Add(loan);
            StateCheck();
            return loan;
        });
    }
    //TODO:a 98. Add Return method to finalise a Loan transaction
    public Transaction Return(int transactionId)
    {
        StateCheck();
        return _state.Return(() =>
        {
            var t = Transactions.FirstOrDefault(l => l.Id == transactionId)
                ?? throw new DomainRuleException("No Loan found for the provided transaction identifier");
            t.Finalise(this);
            StateCheck();
            return t;
        });
    }
    //TODO:a 99. Add Restoration method to create a new Restoration
    public Restoration ScheduleRestoration(Librarian librarian, DateTime start, DateTime end)
    {
        StateCheck();
        //TODO:b 100. Check if the Book is already being Restored, Held or is on Loan 
        if (Transactions.Any(t => t.Start < end && t.End > start) || Restorations.Any(r => r.Start < end && r.End > start))
        {
            throw new DomainRuleException("Book is already being restored, held or on-loan for the provided time period");
        }
        var restoration = new Restoration(librarian, this, start, end);
        Restorations.Add(restoration);
        StateCheck();
        return restoration;
    }

  
}