using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1
{
    public abstract class MiddlewareBase
    {
        private readonly RequestDelegate _next;

        public MiddlewareBase(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            await context.Response.WriteAsync($"{this.GetType().Name}=>");
            await _next(context);
        }
    }

    public class Foo : MiddlewareBase
    {
        public Foo(RequestDelegate next) : base(next)
        {
        }
    }
    public class Bar : MiddlewareBase
    {
        public Bar(RequestDelegate next) : base(next)
        {
        }
    }
    public class Baz : MiddlewareBase
    {
        public Baz(RequestDelegate next) : base(next)
        {
        }
    }
    public class Gux : MiddlewareBase
    {
        public Gux(RequestDelegate next) : base(next)
        {
        }
    }

}
