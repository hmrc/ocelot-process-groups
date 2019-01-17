using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductGrouping.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace ProductGrouping.Controllers
{
    /// <summary>
    /// Validation Controller
    /// </summary>
    public class ValidationController : Controller
    {
        private readonly ILogger<ValidationController> _logger;
        private readonly IProductGroupRepository _productGroupRepository;

        /// <summary>
        /// Vailidation Controller constructor
        /// </summary>
        /// <param name="logger">Logger dependency injected</param>
        /// <param name="productGroupRepository">Product Group repository dependency injected</param>
        public ValidationController(ILogger<ValidationController> logger,
                                    IProductGroupRepository productGroupRepository)
        {
            _logger = logger;
            _productGroupRepository = productGroupRepository;
        }

        /// <summary>
        /// CheckProductReferenceExists
        /// </summary>
        /// <param name="productReference">ProductReference</param>
        /// <returns>Task IAction result (true/false)</returns>
        public async Task<IActionResult> CheckProductReferenceExist(string productReference)
        {
            var result = await _productGroupRepository.GetMany(p => p.ProductReference == productReference);

            if (result.Any())
            {
                return Content("false");
            }
            else
            {
                return Content("true");
            }
        }       
    }
}