using ExitGateReportPanel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;

namespace ExitGateReportPanel.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ProductController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly string strConString = "";
        public ProductController(IConfiguration IConfig)
        {
            configuration = IConfig;
            strConString = configuration.GetConnectionString("Default");
        }


        // GET: api/Produt
        [HttpGet]
        public List<Product> GetProducts()
        {
            Product model = new Product(configuration);
            List<Product> dt = model.Get();
            return dt;
        }


        // POST: api/Product
        [HttpPost]
        public bool InsertProduct([FromBody] Product pro)
        {
            Product model = new Product(configuration);

            //List<Product> dt= model.post(pro);
            var dt = AddProduct(pro);

            return dt;



        }

        
        private bool AddProduct(Product pro)
        {
            string query = "";
            Product _model = new Product(configuration);
            //@"datasource=localhost;port=3306;username=root;password=password";
            DataTable dt = new DataTable();
            using (MySqlConnection con = new MySqlConnection(strConString))
            {
                if (_model.Get(pro.tagID).Count > 0)
                    query = $"Update mcon set facilityCode= @facilityCode, thingTypeCode= @thingTypeCode,Timestamp = @Timestamp,itemcode = @itemcode, supplier = @supplier, productDescrip = @productDescrip, sGroup=@sGroup,Department =  @Department, StockType =@StockType, DocumentNum=  @DocumentNum, brand =@brand, serialNUM = @serialNUM , status = @status where tagID = '{pro.tagID}' ";
                else
                    query = "Insert into mcon (facilityCode, thingTypeCode, Timestamp, tagID, itemcode, supplier, productDescrip, sGroup, Department, StockType, DocumentNum, brand, serialNUM, status) values(@facilityCode, @thingTypeCode, @Timestamp, @tagID, @itemcode, @supplier, @productDescrip, @sGroup, @Department, @StockType, @DocumentNum, @brand, @serialNUM, @status)";

                con.Open();
                MySqlDataReader myreader;

                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {

                    cmd.Parameters.AddWithValue("@facilityCode", pro.facilityCode);
                    cmd.Parameters.AddWithValue("@thingTypeCode", pro.thingTypeCode);
                    cmd.Parameters.AddWithValue("@Timestamp", pro.Timestamp);
                    cmd.Parameters.AddWithValue("@tagID", pro.tagID);
                    cmd.Parameters.AddWithValue("@itemcode", string.IsNullOrEmpty(pro.itemcode) ? "0" : pro.itemcode);
                    cmd.Parameters.AddWithValue("@supplier", pro.supplier);
                    cmd.Parameters.AddWithValue("@productDescrip", pro.productDescrip);
                    cmd.Parameters.AddWithValue("@sGroup", pro.sGroup);
                    cmd.Parameters.AddWithValue("@Department", pro.Department);
                    cmd.Parameters.AddWithValue("@StockType", pro.StockType);
                    cmd.Parameters.AddWithValue("@DocumentNum", pro.DocumentNum);
                    cmd.Parameters.AddWithValue("@brand", pro.brand);
                    cmd.Parameters.AddWithValue("@serialNUM", pro.serialNUM);
                    cmd.Parameters.AddWithValue("@status", pro.status);

                    myreader = cmd.ExecuteReader();
                    dt.Load(myreader);
                    myreader.Close();
                    con.Close();
                }
            }
            return true;
        }

        [HttpPut]
        // PUT: api/Friend/5
        public Product UpdateProduct(int id, [FromBody] Product pro)
        {
            Product model = new Product(configuration);


            Product dt = model.put(pro);

            return dt;
        }

        [HttpDelete]
        // DELETE: api/Friend/5
        public List<Product> DeleteProduct(int id)
        {
            /* Friend friend = friends.Find(f => f.id == id);
             friends.Remove(friend);*/
            return null;
        }
    }
}
