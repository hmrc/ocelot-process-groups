using System.Threading.Tasks;

namespace ProductGrouping.Interfaces
{
    /// <summary>
    /// Auth Repository Interface
    /// </summary>
    public interface IAuthRepository
    {
        /// <summary>
        /// Required IsAuthedRole function
        /// </summary>
        /// <param name="pid">PID</param>
        /// <returns>Task bool</returns>
        Task<bool> IsAuthedRole(string pid);
    }
}
