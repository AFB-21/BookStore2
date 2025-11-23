using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Infrastructure.BaseResponses
{
    public class ErrorResponse
    {
        public string Message { get; set; } = default!;
        public Dictionary<string, string[]>? Errors { get; set; }
    }

}
