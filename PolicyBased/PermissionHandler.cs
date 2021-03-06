﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PolicyBased
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        public List<UserPermission> UserPermissions { get; set; }
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PermissionHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            //赋值用户权限
            UserPermissions = requirement.UserPermissions;

            var t = context.Resource.GetType();

            if (context.Resource is Endpoint endpoint)
            {
                var actionDescriptor = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();
                var pageDescriptor = endpoint.Metadata.GetMetadata<PageActionDescriptor>();
            }
            //获取httpcontext
            var httpContext = _httpContextAccessor.HttpContext;

            //获取请求url
            var requestUrl = httpContext.Request.Path.Value.ToLower();
            //是否经过验证
            var isAuthenticated = httpContext.User.Identity.IsAuthenticated;
            if (isAuthenticated)
            {
                //如果UserPermissions中包含请求的url
                if (UserPermissions.GroupBy(g => g.Url).Where(w => w.Key.ToLower() == requestUrl).Any())
                {
                    var userName = httpContext.User.Claims.SingleOrDefault(s => s.Type == ClaimTypes.Sid).Value;
                    //如果UserPermissions中包含用户+url的组合
                    if (UserPermissions.Where(w => w.UserName == userName && w.Url.ToLower() == requestUrl).Any())
                    {
                        context.Succeed(requirement);
                    }
                    else
                    {
                        //无权访问
                        httpContext.Response.Redirect(requirement.DeniedAction);

                    }
                }
                else
                {
                    //如果请求的是UserPermissions之外的Url，则允许
                    context.Succeed(requirement);
                }
            }
            return Task.CompletedTask;
        }
    }
}
