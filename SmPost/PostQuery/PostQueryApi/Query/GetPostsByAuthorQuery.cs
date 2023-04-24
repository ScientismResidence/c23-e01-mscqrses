using CqrsCore.Query;

namespace PostQueryApi.Query;

public class GetPostsByAuthorQuery : QueryBase
{
    public string Author { get; set; }
}