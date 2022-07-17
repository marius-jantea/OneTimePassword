using OneTimePasswordBusinessLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneTimePasswordBusinessLogic.DefaultImplementations
{
    public class OneTimePasswordApplicationFactory : IOneTimePasswordApplicationFactory
    {
        public OneTimePasswordApplication Create()
        {
            var generatorImplementation = new OneTimePasswordGenerator();
            var repositoryImplementation = new OneTimePasswordRepository();

            var communicatorImplementation = new OneTimePasswordCommunicator();
            var configurationImplementation = new OneTimePasswordConfiguration();

            return new OneTimePasswordApplication(configurationImplementation, generatorImplementation,
                repositoryImplementation, communicatorImplementation);
        }
    }
}
