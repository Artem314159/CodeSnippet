using Autofac;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeSnippet.Controllers
{
    public class ApiControllerBase : ControllerBase
    {
        protected readonly ILifetimeScope Scope;

        public ApiControllerBase(ILifetimeScope scope)
        {
            Scope = scope;
        }
    }
}
