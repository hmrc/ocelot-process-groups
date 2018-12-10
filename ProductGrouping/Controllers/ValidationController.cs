using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProductGrouping.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace ProductGrouping.Controllers
{
    public class ValidationController : Controller
    {
        private readonly ILogger<ValidationController> _logger;
        private readonly IProductGroupRepository _productGroupRepository;

        public ValidationController(ILogger<ValidationController> logger,
                                    IProductGroupRepository productGroupRepository)
        {
            _logger = logger;
            _productGroupRepository = productGroupRepository;
        }

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