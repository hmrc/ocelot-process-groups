using Microsoft.Extensions.Logging;
using ProductGrouping.Interfaces;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Xml;

namespace ProductGrouping.Repositories
{
    /// <summary>
    /// Auth repository
    /// </summary>
    public class AuthRepository: IAuthRepository
    {
        private readonly ILogger<AuthRepository> _logger;

        /// <summary>
        /// Auth repository constructor
        /// </summary>
        /// <param name="logger">Logger dependency injected</param>
        public AuthRepository(ILogger<AuthRepository> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Is user authed
        /// </summary>
        /// <param name="pid">Users PID</param>
        /// <returns>Task of bool</returns>
        public Task<bool> IsAuthedRole(string pid) => Task.Run(() => IsAuthedRoleAsync(pid));

        private bool IsAuthedRoleAsync(string pid)
        {
#if DEBUG
            return true;
#else
            try
            {
                var file = Environment.GetEnvironmentVariable("StaffList", EnvironmentVariableTarget.Machine);
                XmlDocument xml = new XmlDocument();
                string textFromPage;

                WebClient web = new WebClient
                {
                    Credentials = CredentialCache.DefaultCredentials
                };

                Stream stream = web.OpenRead(file);

                using (StreamReader reader = new StreamReader(stream))
                {
                    textFromPage = reader.ReadToEnd();
                }

                xml.LoadXml(textFromPage);

                var nodelocation = $"dataroot/Entry[PID='{pid}']";
                var entry = xml.SelectSingleNode(nodelocation);

                if (entry == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch(Exception ex)
            {
                _logger.LogCritical(500, ex.Message, ex);

                return false;
            }   
#endif
        }
    }
}
