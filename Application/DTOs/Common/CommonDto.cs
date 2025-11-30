namespace RealState.Core.Application.DTOs.Common
{
    public class CommonDto<TKey>
    {
        public required TKey Id { get; set; }
    }
}
