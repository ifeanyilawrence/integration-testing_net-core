namespace IntegrationTesting.Service
{
    using System;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;
    using IntegrationTesting.Shared;

    public class FetchData : IFetchData
    {
        public async Task<DataResult> GetRecords()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com");
            var result = await client.GetStringAsync("/todos/1");

            var resultObject = JsonSerializer.Deserialize<DataResult>(result);

            return resultObject;
        }

        public Task SaveRecord(DataViewModel viewModel)
        {
            throw new NotImplementedException();
        }
    }
}
