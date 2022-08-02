using ExitGateReportPanel.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using ReflectionIT.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ExitGateReportPanel.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly string strConString = "";
        private readonly string StoreName = "";

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IConfiguration IConfig)
        {
            _logger = logger;
            configuration = IConfig;
            strConString = configuration.GetConnectionString("Default");
            StoreName = configuration.GetValue<string>("StoreName");
        }

        public ActionResult Login()
        {
            return View();
        }
        public ActionResult LogOut()
        {
            return View("Login");
        }

        [HttpPost]
        public ActionResult Login(string uname, string pswd)
        {
            try
            {
                if ((uname.ToLower().Trim() == "admin" && pswd.Trim() == "4dm!n@sDG$") || (uname.ToLower().Trim() == "user" && pswd.Trim() == "56789"))
                {
                    HttpContext.Session.SetString("UserName", uname.ToLower().Trim());
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                HttpContext.Session.SetString("error", ex.Message);
                HttpContext.Session.SetString("errorInner", ex.InnerException.ToString());
                throw ex;
            }

        }

        public List<Product> Get()
        {
            try
            {
                Product model = new Product(configuration);
                List<Product> dt = model.Get();
                return dt;
            }
            catch (Exception ex)
            {
                HttpContext.Session.SetString("error", ex.Message);
                HttpContext.Session.SetString("errorInner", ex.InnerException.ToString());
                throw ex;
            }
        }

        public async Task<List<Product>> GetReportAsync()
        {
            try
            {
                Product model = new Product(configuration);
                List<Product> dt = await model.GetReportFromExitGateAsync();
                return dt.OrderByDescending(x => x.Timestamp).ToList();
            }
            catch (Exception ex)
            {
                HttpContext.Session.SetString("error", ex.Message);
                HttpContext.Session.SetString("errorInner", ex.InnerException.ToString());
                throw ex;
            }
        }


        // POST: api/Product

        public Product Post(Product pro)
        {
            Product model = new Product(configuration);

            //List<Product> dt= model.post(pro);
            Product dt = AddProduct(pro);

            return dt;



        }

        private Product AddProduct(Product pro)
        {
            DataTable dt = new DataTable();
            using (MySqlConnection con = new MySqlConnection(strConString))
            {
                con.Open();

                string query = "Insert into mcon (facilityCode, thingTypeCode, Timestamp, tagID, itemcode, supplier, productDescrip, sGroup, Department, StockType, DocumentNum, brand, serialNUM, status) values(@facilityCode, @thingTypeCode, @Timestamp, @tagID, @itemcode, @supplier, @productDescrip, @sGroup, @Department, @StockType, @DocumentNum, @brand, @serialNUM, @status)";
                MySqlDataReader myreader;

                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {

                    cmd.Parameters.AddWithValue("@facilityCode", pro.facilityCode);
                    cmd.Parameters.AddWithValue("@thingTypeCode", pro.thingTypeCode);
                    cmd.Parameters.AddWithValue("@Timestamp", pro.Timestamp);
                    cmd.Parameters.AddWithValue("@tagID", pro.tagID);
                    cmd.Parameters.AddWithValue("@itemcode", pro.itemcode);
                    cmd.Parameters.AddWithValue("@supplier", pro.supplier);
                    cmd.Parameters.AddWithValue("@productDescrip", pro.productDescrip);
                    cmd.Parameters.AddWithValue("@sGroup", pro.sGroup);
                    cmd.Parameters.AddWithValue("@Department", pro.Department);
                    cmd.Parameters.AddWithValue("@StockType", pro.StockType);
                    cmd.Parameters.AddWithValue("@DocumentNum", pro.DocumentNum);
                    cmd.Parameters.AddWithValue("@brand", pro.brand);
                    cmd.Parameters.AddWithValue("@serialNUM", pro.serialNUM);
                    cmd.Parameters.AddWithValue("@status", pro.status);
                    //  cmd.Parameters.AddWithValue("@udfs" :{ })



                    myreader = cmd.ExecuteReader();
                    dt.Load(myreader);
                    myreader.Close();
                    con.Close();
                }
            }
            return pro;
        }


        // PUT: api/Friend/5
        public Product Put(int id, Product pro)
        {
            Product model = new Product(configuration);


            Product dt = model.put(pro);

            return dt;
        }

        // DELETE: api/Friend/5
        public List<Product> Delete(int id)
        {
            /* Friend friend = friends.Find(f => f.id == id);
             friends.Remove(friend);*/
            return null;
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        public async Task<IActionResult> Index(int page = 1, int selectedInterval = 0, bool isNotSold = false, bool isRefresh = false)
        {
            
            var products = await GetReportAsync();
            if (isNotSold)
                products = products.Where(x => x.status.ToLower() == "not sold").ToList();

            var dataPage = products.GetPaged(page, 20);
            dataPage.switchValue        = isNotSold;
            dataPage.isRefresh          = isRefresh;
            dataPage.selectedInterval   = selectedInterval;
            dataPage.StoreName          = StoreName;

            return View(dataPage);
        }

        [HttpPost]
        public async Task<IActionResult> ExportCSV()
        {
            #region Get list of Students from Database
            var products = await GetReportAsync();
            List<object> lstProducts = (from s in products.OrderByDescending(x => x.Timestamp).ToList()
                                        select new[] {
                                                                s.facilityCode,
                                                                s.thingTypeCode,
                                                                s.Timestamp.ToString(),
                                                                s.tagID,
                                                                s.itemcode,
                                                                s.supplier,
                                                                s.productDescrip,
                                                                s.sGroup,
                                                                s.Department,
                                                                s.StockType,
                                                                s.DocumentNum,
                                                                s.serialNUM,
                                                                s.brand,
                                                                s.status
                                  }).ToList<object>();

            #endregion

            #region Create Name of Columns

            var names = typeof(Product).GetProperties()
                        .Select(property => property.Name)
                        .ToArray();

            lstProducts.Insert(0, names.Where(x => x != names[0]).ToArray());

            #endregion

            #region Generate CSV

            StringBuilder sb = new StringBuilder();

            foreach (var item in lstProducts)
            {
                string[] arrStudents = (string[])item;
                foreach (var data in arrStudents)
                {
                    //Append data with comma(,) separator.
                    sb.Append(data + ',');
                }
                //Append new line character.
                sb.Append("\r\n");
            }

            #endregion

            #region Download CSV

            return File(Encoding.ASCII.GetBytes(sb.ToString()), "text/csv", "ExitGateReportData.csv");

            #endregion
        }
    }

}
