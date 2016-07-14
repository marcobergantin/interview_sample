using NLog;
using Products.WebApi.Contracts;

namespace Products.WebApi.Tracing
{
    /// <summary>
    /// Wrapper around NLog's Logger class
    /// </summary>
    public class LogManager : 
        Logger,
        IProductsLogger
    {
    }
}