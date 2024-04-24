using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Util.Observer
{
    internal interface IObserver<T>
    {
        void Handle(T observed);
    }
}
