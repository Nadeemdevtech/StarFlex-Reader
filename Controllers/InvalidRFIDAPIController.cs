using ExitGateReportPanel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ExitGateReportPanel.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class InvalidRFIDAPIController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public InvalidRFIDAPIController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // GET: InvalidRFIDs
        [HttpGet]
        public List<InvalidRFID> GetAll()
        {
            InvalidRFID invalidRFID = new InvalidRFID(_configuration);
            var model = invalidRFID.Get();
            return model;
        }

        [HttpGet]
        public InvalidRFID GetByID(int? id)
        {
            InvalidRFID model = new InvalidRFID(_configuration);
            InvalidRFID invalidRFID = model.Find((int)id);

            return invalidRFID;
        }


        [HttpPost]
        public async Task<InvalidRFID> CreateInvalidRFIDAsync(InvalidRFID invalidRFID)
        {
            InvalidRFID model = new InvalidRFID(_configuration);
            model.Insert(invalidRFID);

            await UpSertAsync(invalidRFID);

            return invalidRFID;
        }


        [HttpPut]
        public async Task<InvalidRFID> EditAsync(InvalidRFID invalidRFID)
        {
            InvalidRFID model = new InvalidRFID(_configuration);
            model.Update(invalidRFID);

            await UpSertAsync(invalidRFID);

            return invalidRFID;
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            InvalidRFID model = new InvalidRFID(_configuration);
            InvalidRFID invalidRFID = model.Find((int)id);
            model.Delete(invalidRFID);

            invalidRFID.IsEnable = false;
            await UpSertAsync(invalidRFID);

            return Ok(true);
        }

        private async Task UpSertAsync(InvalidRFID invalidRFID)
        {
            ExternalApiModel externalApiModel = new ExternalApiModel();
            externalApiModel.tagID = invalidRFID.RFID;
            if (invalidRFID.IsEnable)
                externalApiModel.status = "Whitelisted";
            else
                externalApiModel.status = "GRN";

            await UpSertApiAsync(externalApiModel);
        }

        private async Task UpSertApiAsync(ExternalApiModel model)
        {
            var IpAddress = _configuration.GetValue<string>("WhiteListIP");
            var ApiKey = _configuration.GetValue<string>("WhiteListAPIKey");
            HttpClient httpClient = new HttpClient();
            var stringContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            HttpRequestMessage request = new HttpRequestMessage();
            
            request.RequestUri = new Uri(IpAddress);
            request.Method = HttpMethod.Post;
            request.Headers.Add("api_key", ApiKey);
            request.Content = stringContent;
            HttpResponseMessage responses = await httpClient.SendAsync(request);
            var responseString = await responses.Content.ReadAsStringAsync();
            var statusCode = responses.StatusCode;
        }
    }
}
