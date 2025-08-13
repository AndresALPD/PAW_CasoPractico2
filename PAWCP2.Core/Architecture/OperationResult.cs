using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAWCP2.Core.Architecture
{
    internal class OperationResult<T>
    {
        public bool Success { get; init; }
        public string? Error { get; init; }
        public T? Data { get; init; }

        public static OperationResult<T> Ok(T data) => new() { Success = true, Data = data };
        public static OperationResult<T> Fail(string error) => new() { Success = false, Error = error };
    }
}
