using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaAPI.Shared.Utils
{
    public class ServiceResponse<T>
    {
        public T? Data { get; set; }
        public bool Success { get; set; } = true;
        public string Message { get; set; } = "Operação realizada com sucesso.";
        public List<string>? Errors { get; set; }
        public ServiceResponse() { }
        public ServiceResponse(T data)
        {
            Data = data;
        }
        public ServiceResponse(string message, bool success, List<string>? errors = null)
        {
            Message = message;
            Success = success;
            Errors = errors;
        }
        public ServiceResponse(T data, string message, bool success = true, List<string>? errors = null)
        {
            Data = data;
            Message = message;
            Success = success;
            Errors = errors;
        }
    }

}
