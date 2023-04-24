using PostCommon.Dto;
using PostQueryDomain.Entity;

namespace PostQueryApi.Dto;

public class PostResponse : ResponseBase
{
    public List<PostEntity> Posts { get; set; }
}