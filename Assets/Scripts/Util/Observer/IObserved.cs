using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Util.Observer
{
    internal interface IObserved<T>
    {
        public void Attach(IObserver<T> observer);
        public void Attach(ICollection<IObserver<T>> observers);

        public void Detach(IObserver<T> observer);
        public void Detach(ICollection<IObserver<T>> observers);

        public void Notify();

        public bool Contains(IObserver<T> observer);
    }
}
