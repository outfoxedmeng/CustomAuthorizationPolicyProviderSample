using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PolicyProviderSample.Authorization
{
    /// <summary>
    /// 在Startup中的 endpoints.MapDefaultControllerRoute()时，实例化
    /// </summary>
    public class MinimumAuthorizeAttribute : AuthorizeAttribute
    {
        const string POLICY_PREFIX = "MinimumAge";
        public MinimumAuthorizeAttribute(int age)
        {
            Age = age;
        }

        public int Age
        {
            get
            {
                if (int.TryParse(Policy.Substring(POLICY_PREFIX.Length), out var age))
                {
                    return age;
                }
                return default(int);
            }
            set
            {
                Policy = $"{POLICY_PREFIX}{value.ToString()}";
            }
        }
    }
}
