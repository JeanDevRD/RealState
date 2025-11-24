namespace RealState.Core.Domain.Common
{
    public class CommonEntity<TKey>
    {
        public required TKey Id { get; set; }
    }
}
