using Microsoft.Extensions.Logging;

namespace SettlementBookingSystem.Common.BaseClasses
{
    public class BaseService
    {
        public ILogger Logger { get; set; }

        public BaseService(ILogger logger)
        {
            Logger = logger;
        }
    }
}
