namespace FumicertiApi.Interface
{
    // Service Interface
    public interface IConsumptionReportService
    {
        Task<object> GetConsumptionReportAsync(DateTime from, DateTime to, string type, int companyId);
    }

}
