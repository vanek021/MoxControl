namespace MoxControl.Core.Attributes
{
#pragma warning disable CS8618
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
#pragma warning restore CS8618
}
