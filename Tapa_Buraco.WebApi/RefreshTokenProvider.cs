using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Tapa_Buraco.WebApi
{
    public class RefreshTokenProvider : AuthenticationTokenProvider
    {
        public async override Task CreateAsync(AuthenticationTokenCreateContext context)
        {            
            context.SetToken(context.SerializeTicket());
        }

        public async override Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            context.DeserializeTicket(context.Token);
        }
    }
}