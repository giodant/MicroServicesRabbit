using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MicroRabbit.Authentication.Api.Helpers
{
    public sealed class JwtTokenBuilder
    {
        private SecurityKey _securityKey = null;
        private string _subject = "";
        private string _issuer = "";
        private string _audience = "";
        private Dictionary<string, string> _claims = new Dictionary<string, string>();
        private int _expieryInMinutes;

        public JwtTokenBuilder AddSecurityKey(SecurityKey securityKey)
        {
            this._securityKey = securityKey;
            return this;
        }

        public JwtTokenBuilder AddSubject(string subject)
        {
            this._subject = subject;
            return this;
        }

        public JwtTokenBuilder AddIssuer(string issuer)
        {
            this._issuer = issuer;
            return this;
        }

        public JwtTokenBuilder AddAudience(string audience)
        {
            this._audience = audience;
            return this;
        }

        public JwtTokenBuilder AddClaim(string type, string value)
        {
            this._claims.Add(type, value);
            return this;
        }

        public JwtTokenBuilder AddRole(string value)
        {
            this._claims.Add(ClaimTypes.Role, value);
            return this;
        }

        public JwtTokenBuilder AddRoles(IEnumerable<string> groups)
        {
            if (groups.Any())
            {
                groups.ToList().ForEach(g => AddRole(g));
            }
            return this;
        }

        public JwtTokenBuilder AddClaims(Dictionary<string, string> claims)
        {
            this._claims.Union(claims);
            return this;
        }

        public JwtTokenBuilder AddExpiry(int expieryInMinutes)
        {
            this._expieryInMinutes = expieryInMinutes;
            return this;
        }

        public JwtSecurityToken Build()
        {
            EnsureArguments();

            var claims = new List<Claim>
            {
              new Claim(JwtRegisteredClaimNames.Sub, this._subject),
              new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            }
            .Union(this._claims.Select(item => new Claim(item.Key, item.Value)));

            var token = new JwtSecurityToken(
                              issuer: this._issuer,
                              audience: this._audience,
                              claims: claims,
                              notBefore: DateTime.Now,
                              expires: DateTime.Now.AddMinutes(_expieryInMinutes),
                              signingCredentials: new SigningCredentials(
                                                        this._securityKey,
                                                        SecurityAlgorithms.HmacSha256));

            return token;
        }

        #region  Private Methods

        private void EnsureArguments()
        {
            if (this._securityKey == null)
                throw new ArgumentNullException("Security Key");

            if (string.IsNullOrEmpty(this._issuer))
                throw new ArgumentNullException("Issuer");

            if (string.IsNullOrEmpty(this._audience))
                throw new ArgumentNullException("Audience");
        }

        #endregion
    }
}
