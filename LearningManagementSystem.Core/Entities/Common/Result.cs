using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningManagementSystem.Core.Entities.Common
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public string Message { get; }
        public ErrorType? ErrorType { get; set; }
        public T Data { get; }

        private Result(bool isSuccess, string message, T data)
        {
            IsSuccess = isSuccess;
            Message = message;
            Data = data;
        }

        public static Result<T> Success(T data) => new(true, null, data);
        public static Result<T> Failure(string message) => new(false, message, default);
    }
    public enum ErrorType
    {
        ValidationError,
        BusinessLogicError,
        NotFoundError,
        UnauthorizedError,
        SystemError

    }
}
