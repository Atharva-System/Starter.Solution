namespace Starter.Blazor.Modules.User.Models;

public class GridResponse<T>
{
    public List<T> Data { get; set; }
    public List<Dictionary<string, string>> HeaderInfo { get; set; }

    public GridResponse()
    {
        Data = new List<T>();
        HeaderInfo = new List<Dictionary<string, string>>();
    }
}
