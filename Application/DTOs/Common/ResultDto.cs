namespace RealState.Core.Application.DTOs.Common
{
    public class ResultDto<T> where T : class
    {
        public bool IsError { get; set; } = false;
        public List<string> Message { get; set; } = [];
        public T? Data { get; set; }
    }
}
