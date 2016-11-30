using SnackProject.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SnackProject.Automatics
{
    public interface ExecutionTask
    {
        void Execute(ApplicationDbContext context);
        string GetKey();
    }
}
