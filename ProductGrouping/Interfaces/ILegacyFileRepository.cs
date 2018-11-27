using ProductGrouping.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductGrouping.Interfaces
{
    public interface ILegacyFileRepository
    {
        Task Publish(IEnumerable<ProductGroup> productGroups);
    }
}
