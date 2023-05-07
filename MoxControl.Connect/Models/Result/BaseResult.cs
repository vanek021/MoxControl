using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Connect.Models.Result
{
    public class BaseResult
    {
        public BaseResult(bool success, string? errorMessage = null)
        {
            Success = success;
            ErrorMessage = errorMessage;
        }

        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
