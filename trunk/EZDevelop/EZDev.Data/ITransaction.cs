using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EZDev.Data
{
    public interface ITransaction : IDisposable
    {
        void Commit();
        void Rollback();
    }

}
