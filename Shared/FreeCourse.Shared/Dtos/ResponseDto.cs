using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FreeCourse.Shared.Dtos
{
    public class ResponseDto<T>
    {
        public T Data { get; set; }

        [JsonIgnore]
        public int StatusCode { get; private set; }

        [JsonIgnore]
        public bool IsSuccessful { get; private set; }

        public List<String> Errors { get; set; }

        // static factory method
        public static ResponseDto<T> Success(T data, int statusCode) => new() { Data = data, StatusCode = statusCode, IsSuccessful = true };
        public static ResponseDto<T> Success(int statusCode) => new() { Data = default, StatusCode = statusCode, IsSuccessful = true };
        public static ResponseDto<T> Fail(List<string> errors, int statusCode) => new() { StatusCode = statusCode, Errors = errors, IsSuccessful = false };
        public static ResponseDto<T> Fail(string error, int statusCode) => new() { Errors = new List<string> { error }, StatusCode = statusCode, IsSuccessful = false };
    }
}
