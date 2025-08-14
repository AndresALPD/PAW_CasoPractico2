using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAWCP2.Core.Architecture
{
    public class OperationResult<T>
    {
        public bool Success { get; private set; }
        public string? Error { get; private set; }
        public T? Data { get; private set; }

        public static OperationResult<T> Ok(T data) =>
            new OperationResult<T> { Success = true, Data = data };

        public static OperationResult<T> Fail(string error) =>
            new OperationResult<T> { Success = false, Error = error };
    }
}
