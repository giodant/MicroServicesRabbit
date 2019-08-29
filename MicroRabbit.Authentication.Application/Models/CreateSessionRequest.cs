using FluentValidation;
using FluentValidation.Results;
using Newtonsoft.Json;
using System.Text;

namespace MicroRabbit.Authentication.Application.Models
{
    public enum GrantType
    {
        Password,
        RefreshToken,
        Badge
    }
    /// <summary>
    /// Validation class for UserLoginRequest
    /// </summary>
    public class CreateSessionRequestValidator : AbstractValidator<CreateSessionRequest>
    {
        public CreateSessionRequestValidator()
        {
            RuleFor(x => x.UserName).NotEmpty().Length(3, 25).When(x => x.GrantType == GrantType.Password);
            RuleFor(x => x.UserName).Matches("^[a-zA-Z0-9]+(?:[_ -.]?[a-zA-Z0-9])*$").When(x => x.GrantType == GrantType.Password);
            RuleFor(x => x.Password).NotEmpty().Length(3, 15).When(x => x.GrantType == GrantType.Password);
            RuleFor(x => x.Culture).NotEmpty().When(x => x.GrantType == GrantType.Password);
            RuleFor(x => x.RefreshToken).NotEmpty().When(x => x.GrantType == GrantType.RefreshToken);
        }
    }

    public class CreateSessionRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Culture { get; set; }
        public string Ip { get; set; }
        public string RefreshToken { get; set; }
        public GrantType GrantType { get; set; }

        [JsonConstructor]
        public CreateSessionRequest()
        {
            GrantType = GrantType.Password;
        }

        public CreateSessionRequest(string username, string password, string ip, string culture = "it-IT")
        {
            UserName = username;
            Password = password;
            Culture = culture;
            Ip = ip;
            GrantType = GrantType.Password;
        }

        public CreateSessionRequest(string refreshToken)
        {
            RefreshToken = refreshToken;
            GrantType = GrantType.RefreshToken;
        }


        public bool IsValid()
        {
            ValidationResult validationResult = new CreateSessionRequestValidator().Validate(this);
            if (!validationResult.IsValid)
            {
                StringBuilder sb = new StringBuilder();
                validationResult.Errors.ForEach(z => sb.AppendLine($"{z.ErrorCode} - {z.ErrorMessage}"));
                throw new System.Exception(sb.ToString());
            }
            return true;
        }
    }
}
