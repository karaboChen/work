namespace DemoApi.Tools
{
    public class ApiResult<T>
    {
        public T? Data { get; set; }

        public string Message { get; set; } = string.Empty;

        public int StatusCode { get; set; }

        private ApiResult(T? data, string message, int statusCode)
        {
            Data = data;
            Message = message;
            StatusCode = statusCode;
        }

        // ✅ 成功：200 OK
        public static ApiResult<T> Ok(T data, string message = "成功")
            => new ApiResult<T>(data, message, StatusCodes.Status200OK);

        // ✅ 成功：201 Created
        public static ApiResult<T> Created(T data, string message = "已建立")
            => new ApiResult<T>(data, message, StatusCodes.Status201Created);

        // ⚠️ 錯誤：400 Bad Request
        public static ApiResult<T> Fail(string message = "失敗")
            => new ApiResult<T>(default, message, StatusCodes.Status400BadRequest);

        // ❌ 找不到：404 Not Found
        public static ApiResult<T> NotFound(string message = "找不到資料")
            => new ApiResult<T>(default, message, StatusCodes.Status404NotFound);

        // 🚫 未授權：401 Unauthorized
        public static ApiResult<T> Unauthorized(string message = "未授權")
            => new ApiResult<T>(default, message, StatusCodes.Status401Unauthorized);

        // 💥 伺服器錯誤：500
        public static ApiResult<T> ServerError(string message = "伺服器錯誤")
            => new ApiResult<T>(default, message, StatusCodes.Status500InternalServerError);
    }

}
