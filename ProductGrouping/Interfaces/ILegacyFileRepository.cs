using System.Threading.Tasks;

namespace ProductGrouping.Interfaces
{
    /// <summary>
    /// Legacy file repository interface
    /// </summary>
    public interface ILegacyFileRepository
    {
        /// <summary>
        /// Required Publish function
        /// </summary>
        /// <returns>Task</returns>
        Task Publish();
    }
}
