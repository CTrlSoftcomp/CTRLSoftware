using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CTrlSoft.Core.Api.Models
{
    public class ValidationError
    {
        public string Message { get; set; }
        public string PropertyName { get; set; }
    }
}
