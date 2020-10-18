using System;
using System.Collections.Generic;
using System.Text;

namespace CSE.BL.Interfaces
{
    public interface IDatabaseConnection
    {
        void SetConnection(object dataPath);
        void Dispose();
    }
}
