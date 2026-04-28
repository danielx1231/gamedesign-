using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YuLongFSM
{
    public  abstract class FSMIState<T>
    {
       public  T fSMData;
       public FSMManager<T> fSMManager;
        public virtual void OnEnter()
        {

        }
        public virtual void OnExit()
        {
            
        }
        public virtual void OnUpdate()
        {
            
        }

    }
}
