using Microsoft.Extensions.Logging;
using ProductGrouping.Interfaces;
using ProductGrouping.Models;
using System;
using System.Collections.Generic;
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

        public LegacyFileRepository(ILogger<LegacyFileRepository> logger)
        {
            _logger = logger;
        }

        public async Task Publish(IEnumerable<ProductGroup> productGroups)
        {
            var publishFile = $"{Environment.GetEnvironmentVariable("LegacyProductGroupingLocation", EnvironmentVariableTarget.Machine)}ProductGrouping.xml";
            
            XElement export = new XElement("dataroot",
                                        productGroups.Select(p => new XElement("ProductGrouping",
                                                                             new XElement("ref", p.ProductReference),
                                                                             new XElement("area", p.Group))
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
