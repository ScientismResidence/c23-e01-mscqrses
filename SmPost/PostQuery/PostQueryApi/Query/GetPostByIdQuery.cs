using CqrsCore.Query;

namespace PostQueryApi.Query;

public class GetPostByIdQuery : QueryBase
{
    public Guid Id { get; set; }
}