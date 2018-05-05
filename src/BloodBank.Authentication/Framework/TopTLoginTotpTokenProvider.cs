using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BloodBank.Authentication.Framework
{
    public class TopTLoginTotpTokenProvider : TotpSecurityStampBasedTokenProvider<User>
    {
        public override Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<User> manager, User user)
        {
            return Task.FromResult(false);
        }
        public override async Task<string> GetUserModifierAsync(string purpose, UserManager<User> manager, User user)
        {
            var email = await manager.GetEmailAsync(user);
            return "PasswordlessLogin:" + purpose + ":" + email;
        }
    }
}
