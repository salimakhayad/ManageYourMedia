using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyMedia.Core.User
{
    public class MediaUserClaimsPrincipalFactory:UserClaimsPrincipalFactory<MediaUser>
    {
        public MediaUserClaimsPrincipalFactory(UserManager<MediaUser> userManager, IOptions<IdentityOptions> optionsAccessor) : base(userManager, optionsAccessor)
        {

        }
        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(MediaUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim("Rol", user.Role));
            return identity;
        }
    }
}
