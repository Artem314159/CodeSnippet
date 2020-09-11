using CodeSnippet.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeSnippet.Services.Base
{
    public class ServiceBase
    {
        protected DataContext db;

        public ServiceBase(DataContext dataContext)
        {
            db = dataContext;
        }
    }
}
