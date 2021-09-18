namespace IntegrationTesting.Api.Controllers
{
    using System;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;
    using System.Web.Http.Description;
    using IntegrationTesting.Service;
    using IntegrationTesting.Shared;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly IFetchData _fetchData;
        private readonly IAlternateClient _httpClient;

        public DataController(IFetchData fetchData,
            IAlternateClient httpClient)
        {
            _fetchData = fetchData;
            _httpClient = httpClient;
        }

        [HttpGet]
        [Route("")]
        [ResponseType(typeof(DataResult))]
        public async Task<IActionResult> GetData()
        {
            var result = await _fetchData.GetRecords();

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet]
        [Route("/get-from-API")]
        [ResponseType(typeof(DataResult))]
        public async Task<IActionResult> GetDataFromAPI()
        {
            var result = await _httpClient.GetStringAsync("/todogs/1");

            var resultObject = JsonSerializer.Deserialize<DataResult>(result);

            if (resultObject == null)
                return NotFound();

            return Ok(resultObject);
        }
    }
}
