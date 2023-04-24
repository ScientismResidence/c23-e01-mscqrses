using CqrsCore.Query;

namespace PostQueryApi.Query;

public class GetPostsWithLikesQuery : QueryBase
{
    public int Likes { get; set; }
}