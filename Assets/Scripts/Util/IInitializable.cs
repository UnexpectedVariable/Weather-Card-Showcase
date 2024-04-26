using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Util
{
    internal interface IInitializable
    {
        void Initialize();
        bool IsInitialized { get; }
    }
}
