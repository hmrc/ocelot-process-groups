using System.Threading.Tasks;

namespace ProductGrouping.Interfaces
{
    public interface IAuthRepository
    {
        Task<bool> IsAuthedRole(string pid);
    }
}
