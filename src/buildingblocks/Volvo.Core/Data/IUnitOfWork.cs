using System.Threading.Tasks;

namespace Volvo.Core.Data
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}
