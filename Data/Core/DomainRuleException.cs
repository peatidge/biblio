namespace Tela.Data.Core;
//TODO:a 78. Add Custom DomainRuleException class
public class DomainRuleException : ApplicationException
{
    public DomainRuleException(string message)
        : base(message)
    {
        
    }
}
