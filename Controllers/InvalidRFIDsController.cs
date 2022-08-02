using ExitGateReportPanel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ExitGateReportPanel.Controllers
{
    public class InvalidRFIDsController : Controller
    {
        private readonly IConfiguration _configuration;
        public InvalidRFIDsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // GET: InvalidRFIDs
        public ActionResult Index()
        {
            InvalidRFID invalidRFID = new InvalidRFID(_configuration);
            var model = invalidRFID.Get();
            return View(model);
        }

        // GET: InvalidRFIDs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            InvalidRFID model = new InvalidRFID(_configuration);
            InvalidRFID invalidRFID = model.Find((int)id);
            if (invalidRFID == null)
            {
                return NotFound();
            }
            return View(invalidRFID);
        }

        // GET: InvalidRFIDs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: InvalidRFIDs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(InvalidRFID invalidRFID)
        {
            if (ModelState.IsValid)
            {
                InvalidRFID model = new InvalidRFID(_configuration);
                model.Insert(invalidRFID);
                await UpSertAsync(invalidRFID);
                return RedirectToAction("Index");
            }

            return View(invalidRFID);
        }

        // GET: InvalidRFIDs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            InvalidRFID model = new InvalidRFID(_configuration);
            InvalidRFID invalidRFID = model.Find((int)id);
            if (invalidRFID == null)
            {
                return NotFound();
            }
            return View(invalidRFID);
        }

        // POST: InvalidRFIDs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(InvalidRFID invalidRFID)
        {
            if (ModelState.IsValid)
            {
                InvalidRFID model = new InvalidRFID(_configuration);
                model.Update(invalidRFID);

                await UpSertAsync(invalidRFID);

                return RedirectToAction("Index");
            }
            return View(invalidRFID);
        }

        // GET: InvalidRFIDs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            InvalidRFID model = new InvalidRFID(_configuration);
            InvalidRFID invalidRFID = model.Find((int)id);
            if (invalidRFID == null)
            {
                return NotFound();
            }
            return View(invalidRFID);
        }

        // POST: InvalidRFIDs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            InvalidRFID model = new InvalidRFID(_configuration);
            InvalidRFID invalidRFID = model.Find((int)id);
            model.Delete(invalidRFID);

            invalidRFID.IsEnable = false;
            await UpSertAsync(invalidRFID);

            return RedirectToAction("Index");
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
