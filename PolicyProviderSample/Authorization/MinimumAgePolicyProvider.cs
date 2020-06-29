using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PolicyProviderSample.Authorization
{
    public class MinimumAgePolicyProvider : IAuthorizationPolicyProvider
    {
        const string POLICY_PREFIX = "MinimumAge";
        public DefaultAuthorizationPolicyProvider FallBackPolicyProvider { get; }

        public MinimumAgePolicyProvider(IOptions<AuthorizationOptions> options)
        {
            FallBackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => FallBackPolicyProvider.GetDefaultPolicyAsync();

        public Task<AuthorizationPolicy> GetFallbackPolicyAsync() => FallBackPolicyProvider.GetFallbackPolicyAsync();

        /// <summary>
        /// 授权时调用
        /// </summary>
        /// <param name="policyName">授权Attribute实例中的Policy属性值</param>
        /// <returns></returns>
        public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            //解析授权Attribute中的Policy
            if (policyName.StartsWith(POLICY_PREFIX, StringComparison.OrdinalIgnoreCase) && int.TryParse(policyName.Substring(POLICY_PREFIX.Length), out var age))
            {
                var builder = new AuthorizationPolicyBuilder();

                //实例化 IAuthorizationRequirement对象
                builder.AddRequirements(new MinimumAgeRequirement(age));

                //注意，这里可以添加多个条件，而且Handler会验证所有的条件
                //builder.AddRequirements(new MinimumAgeRequirement(33));

                return Task.FromResult(builder.Build());
            }


            return FallBackPolicyProvider.GetPolicyAsync(policyName);
        }
    }
}
