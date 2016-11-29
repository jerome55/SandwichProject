using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientProject.Data
{
    public class DbInitializer
    {
        public static void Initialize(ClientContext context)
        {
            context.Database.EnsureCreated();
        }
    }
}