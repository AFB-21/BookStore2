using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Infrastructure.BaseResponses
{
    public class LoginResponse
    {
        public string Token { get; set; } = default!;
        public DateTime ExpiresAt { get; set; }
        public string RefreshToken { get; set; } = default!;
        //public UserInfoDto User { get; set; } = default!;
    }
}
