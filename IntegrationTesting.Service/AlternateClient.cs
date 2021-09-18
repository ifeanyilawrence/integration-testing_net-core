using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationTesting.Service
{
    public class AlternateClient : IAlternateClient
    {
        public async Task<string> GetStringAsync(string address)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://jsonplaceholder.typicode.com");

            return await client.GetStringAsync(address); 
        }
    }
}
