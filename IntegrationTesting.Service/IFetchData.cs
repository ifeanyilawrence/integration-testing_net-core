namespace IntegrationTesting.Service
{
    using System.Threading.Tasks;
    using IntegrationTesting.Shared;

    public interface IFetchData
    {
        Task<DataResult> GetRecords();
        Task SaveRecord(DataViewModel viewModel);
    }
}
