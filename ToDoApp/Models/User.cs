using System.Security.Claims;

namespace ToDoApp.Models
{
    public class User : Entity
    {
        public string ? Firstname { get; set; }
        public string ? Lastname { get; set; }
        public string? EMail { get; set; }
        public string ? PasswordHash { get; set; }
        public bool ? IsAdmin { get; set; }
        public bool MailAddressConfirmed { get; set; }
        public string ? PasswordResetToken { get; set; }

        public List<Claim> ToClaims()
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, EMail),
            new Claim("ID", ID.ToString())
        };

            if ((bool)IsAdmin)
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));

            return claims;
        }
    }
    }
