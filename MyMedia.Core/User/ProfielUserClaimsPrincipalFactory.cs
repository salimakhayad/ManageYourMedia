using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyMedia.Core.User
{
    public class ProfielUserClaimsPrincipalFactory:UserClaimsPrincipalFactory<Profiel>
    {
        public ProfielUserClaimsPrincipalFactory(UserManager<Profiel> userManager, IOptions<IdentityOptions> optionsAccessor) : base(userManager, optionsAccessor)
        {

        }
        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(Profiel user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim("FavorieteKleur", user.FavorieteKleur));
            return identity;
        }
    }
}
