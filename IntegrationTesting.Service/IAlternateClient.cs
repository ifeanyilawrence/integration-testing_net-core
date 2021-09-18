using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationTesting.Service
{
    public interface IAlternateClient
    {
        Task<string> GetStringAsync(string address);
    }
}
