//TODO:b 300. Add Pager class to Models for pagination
using cloudscribe.Pagination.Models;
namespace Tela.Models;
//TODO:b 481. Update Pager to include Params (also update GetPagerModel below and _Pager.cshtml partial view in Shared folder)
public interface IPager
{
    string Controller { get; set; }
    string Action { get; set; }
    int PageSize { get; set; }
    long PageNumber { get; set; }
    long TotalItems { get; set; }
    public Dictionary<string,string> Params { get; set; }
}
public class Pager : IPager
{
    public Pager(string controller, string action, dynamic args)
    {
        Controller = controller;
        Action = action;
        PageNumber = args.PageNumber;
        PageSize = args.PageSize;
        TotalItems = args.TotalItems;
        Params = args.Params != null ? (args.Params as Dictionary<string,string>)!:new();
    }
    public string Controller { get; set; } = default!;
    public string Action { get; set; } = default!;
    public int PageSize { get; set; }
    public long PageNumber { get; set; }
    public long TotalItems { get; set; }
    public Dictionary<string, string> Params { get; set; }
}
public static class PagerExtensions
{
    public static IPager GetPagerModel<T>(this PagedResult<T> t, string controller, string action, Dictionary<string,string> qParams = null!) where T : class
        => new Pager(controller, action, new { t.PageNumber,t.PageSize,t.TotalItems, Params = qParams});
}