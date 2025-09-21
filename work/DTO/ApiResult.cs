namespace work.DTO
{
    public class ApiResult<T>
    {
        public T? Data { get; set; }

        public string  Message { get; set; } = string.Empty;

        private ApiResult( T? data, string message ="成功")
        {
            Data = data;
            Message = message;
        }

        public static ApiResult<T> Ok(T data) =>
            new ApiResult<T>(data);

        public static ApiResult<T> Fail(string message)
        {
            object? defaultValue = default;
            defaultValue = Activator.CreateInstance(typeof(T));

            return new ApiResult<T>((T?)defaultValue, message);
        }
    }
}
