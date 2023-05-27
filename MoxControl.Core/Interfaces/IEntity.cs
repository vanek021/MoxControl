using IDKEY = System.Int64;

namespace MoxControl.Core.Interfaces
{
    public interface IEntity
    {
        IDKEY Id { get; set; }
    }
}
