using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Util.JSON
{
    internal interface ISerializable<T> where T : ISerialization
    {
        T Serialize();
    }
}
