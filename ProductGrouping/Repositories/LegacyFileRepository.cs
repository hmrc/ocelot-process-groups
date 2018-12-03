using Microsoft.Extensions.Logging;
using ProductGrouping.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ProductGrouping.Repositories
{
    public class LegacyFileRepository: ILegacyFileRepository
    {
        private readonly ILogger<LegacyFileRepository> _logger;
        private readonly IProductGroupRepository _productGroupRepository;

        public LegacyFileRepository(ILogger<LegacyFileRepository> logger,
                                    IProductGroupRepository productGroupRepository)
        {
            _logger = logger;
            _productGroupRepository = productGroupRepository;
        }

        public async Task Publish()
        {
            var publishFile = $"{Environment.GetEnvironmentVariable("LegacyProductGroupingLocation", EnvironmentVariableTarget.Machine)}";
            var productGroups = await _productGroupRepository.GetMany();

            XElement export = new XElement("dataroot",
                                        productGroups.Select(p => new XElement("ProductGrouping",
                                                                             new XElement("ref", p.ProductReference),
                                                                             new XElement("area", p.Parent?.ProductReference))
                                                                             )                                                 
                                                                            );

            await CreateFile(publishFile, export.ToString());

            return;
        }

        private Task CreateFile(string publishFile, string data) => Task.Run(() => CreateFileAsync(publishFile, data));

        private void CreateFileAsync(string publishFile, string data)
        {
            if (File.Exists(publishFile))
            {
                File.Delete(publishFile);
            }

            using (FileStream fs = File.Create(publishFile))
            {
                Byte[] info = new UTF8Encoding(true).GetBytes(data);
                fs.Write(info, 0, info.Length);
            }

            return;
        }
    }
}
