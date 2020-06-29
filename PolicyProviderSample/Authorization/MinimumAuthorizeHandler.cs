using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace PolicyProviderSample.Authorization
{
    /// <summary>
    /// 执行完IAuthorizationPolicyProvider.GetPolicyAsync方法后，实例化此类
    /// </summary>
    public class MinimumAuthorizeHandler : AuthorizationHandler<MinimumAgeRequirement>
    {
        private readonly ILogger<MinimumAuthorizeHandler> _logger;

        public MinimumAuthorizeHandler(ILogger<MinimumAuthorizeHandler> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 执行完IAuthorizationPolicyProvider.GetPolicyAsync方法后，执行该方法
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requirement">IAuthorizationPolicyProvider.GetPolicyAsync方法中，builder.AddRequirements(new MinimumAgeRequirement(age))。注意：前面AddRequirements添加了多少个，此方法就会执行多少次</param>
        /// <returns></returns>
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirement requirement)
        {
            _logger.LogWarning("Evaluating for age >={age}", requirement.Age);

            var dateOfBirthClaim = context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.DateOfBirth);
            if (dateOfBirthClaim != null)
            {
                var age = DateTime.Now.Year - Convert.ToDateTime(dateOfBirthClaim.Value).Year;

                if (age >= requirement.Age)
                {
                    _logger.LogInformation("Minimum age authorization {age} satisfied", requirement.Age);
                    context.Succeed(requirement);
                }
                else
                {
                    _logger.LogInformation("Current user's age {age} does not satisfied the minimum requirement {require}", age, requirement.Age);
                }
            }
            else
            {
                _logger.LogInformation("No DateOfBirth claim present");
            }

            return Task.CompletedTask;
        }
    }
}
