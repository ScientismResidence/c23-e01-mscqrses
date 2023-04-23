using PostCommon.Dto;

namespace PostCmdApi.Dto;

public class PostAddedResponse : ResponseBase
{
    public Guid Id { get; set; }
}