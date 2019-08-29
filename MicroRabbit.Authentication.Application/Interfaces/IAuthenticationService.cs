using MicroRabbit.Authentication.Application.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MicroRabbit.Authentication.Application.Interfaces
{
    public interface IAuthenticationService
    {
        CreateSessionResult CreateSession(CreateSessionRequest userLoginRequest);
        ServiceResult SessionLogout(string sessionId);
        List<SessionLogItem> GetSessions(System.Linq.Expressions.Expression<Func<SessionLog, bool>> predicate);
        ServiceResult ForceSessionsLogout(System.Linq.Expressions.Expression<Func<SessionLog, bool>> predicate);
        SessionLogItem GetSession(string sessionId);
        SessionLogItem GetSession(long id);
        Task<ServiceResult> SessionLogoutAsync(string sessionId);
        Task<SessionLogItem> GetSessionAsync(string sessionId);
        Task<SessionLogItem> GetSessionAsync(long id);
    }
}
