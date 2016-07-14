namespace Products.WebApi.Contracts
{
    /// <summary>
    /// A Custom logger interface is preferred, in order not to depend on current platform and logger framework
    /// </summary>
    public interface IProductsLogger
    {
        void Debug(string message);
        void Info(string message);
        void Error(string message);
        void Fatal(string message);
        void Warn(string message);
    }
}
