using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoxControl.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
    public class InjectableAttribute : Attribute
    {
        public readonly Type ServiceType;

        public InjectableAttribute()
        {
        }

        public InjectableAttribute(Type serviceType)
        {
            ServiceType = serviceType;
        }
    }
}
