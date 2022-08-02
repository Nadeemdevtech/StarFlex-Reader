using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace ExitGateReportPanel.Models
{
    public class Product
    {
        public int ID { get; set; }
        public string facilityCode { get; set; }
        public string thingTypeCode { get; set; }
        public DateTime Timestamp { get; set; }
        public string tagID { get; set; }
        public string itemcode { get; set; }
        public string supplier { get; set; }
        public string productDescrip { get; set; }
        public string sGroup { get; set; }
        public string Department { get; set; }
        public string StockType { get; set; }
        public string DocumentNum { get; set; }
        public string brand { get; set; }
        public string serialNUM { get; set; }
        public string status { get; set; }
        public string exitgatename { get; set; }

        private readonly IConfiguration configuration;
        private readonly string connectionString = "";
        public Product(IConfiguration IConfig)
        {
            configuration = IConfig;
            connectionString = configuration.GetConnectionString("Default");
        }

        public Product()
        {
        }

        public List<Product> Get()
        {
            List<Product> pro = new List<Product>();

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("select * from mcon  order by ID desc", con);

                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Product person = new Product();
                    person.ID = Convert.ToInt32(reader["ID"]);
                    person.facilityCode = reader["facilityCode"].ToString();
                    person.thingTypeCode = reader["thingTypeCode"].ToString();
                    person.Timestamp = (DateTime)reader["Timestamp"];
                    person.tagID = reader["tagID"].ToString();
                    person.itemcode = reader["itemcode"].ToString();// reader["itemcode"] is DBNull ? 0 : Convert.ToInt32(reader["itemcode"]);
                    person.supplier = reader["supplier"].ToString();
                    person.productDescrip = reader["productDescrip"].ToString();
                    person.sGroup = reader["sGroup"].ToString();
                    person.Department = reader["Department"].ToString();
                    person.StockType = reader["StockType"].ToString();
                    person.DocumentNum = reader["DocumentNum"].ToString();
                    person.brand = reader["brand"].ToString();
                    person.serialNUM = reader["serialNUM"].ToString();
                    person.status = reader["status"].ToString();
                    person.exitgatename = reader["exitgatename"].ToString();


                    pro.Add(person);
                }
                reader.Close();
            }
            return pro;
        }

        public List<Product> Get(string tagID)
        {
            List<Product> pro = new List<Product>();

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand($"select * from mcon  where tagID = '{tagID}'", con);

                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Product exitGate = new Product();
                    exitGate.ID = Convert.ToInt32(reader["ID"]);
                    exitGate.facilityCode = reader["facilityCode"].ToString();
                    exitGate.thingTypeCode = reader["thingTypeCode"].ToString();
                    exitGate.Timestamp = (DateTime)reader["Timestamp"];
                    exitGate.tagID = reader["tagID"].ToString();
                    exitGate.itemcode = reader["itemcode"].ToString();
                    exitGate.supplier = reader["supplier"].ToString();
                    exitGate.productDescrip = reader["productDescrip"].ToString();
                    exitGate.sGroup = reader["sGroup"].ToString();
                    exitGate.Department = reader["Department"].ToString();
                    exitGate.StockType = reader["StockType"].ToString();
                    exitGate.DocumentNum = reader["DocumentNum"].ToString();
                    exitGate.brand = reader["brand"].ToString();
                    exitGate.serialNUM = reader["serialNUM"].ToString();
                    exitGate.status = reader["status"].ToString();
                    exitGate.exitgatename = reader["exitgatename"].ToString();

                    pro.Add(exitGate);
                }
                reader.Close();
            }
            return pro;
        }

        public async Task<List<Product>> GetReportFromExitGateAsync()
        {
            List<Product> pro = new List<Product>();

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("select * from mcon  order by Timestamp desc", con);

                MySqlDataReader reader = (MySqlDataReader)await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    Product person = new Product();
                    person.ID = Convert.ToInt32(reader["ID"]);
                    person.facilityCode = reader["facilityCode"].ToString();
                    person.thingTypeCode = reader["thingTypeCode"].ToString();
                    person.Timestamp = (DateTime)reader["Timestamp"];
                    person.tagID = reader["tagID"].ToString();
                    person.itemcode = reader["itemcode"].ToString();//reader["itemcode"] is DBNull ? 0 : Convert.ToInt32(reader["itemcode"]);
                    person.supplier = reader["supplier"].ToString();
                    person.productDescrip = reader["productDescrip"].ToString();
                    person.sGroup = reader["sGroup"].ToString();
                    person.Department = reader["Department"].ToString();
                    person.StockType = reader["StockType"].ToString();
                    person.DocumentNum = reader["DocumentNum"].ToString();
                    person.brand = reader["brand"].ToString();
                    person.serialNUM = reader["serialNUM"].ToString();
                    person.status = reader["status"].ToString();
                    person.exitgatename = reader["exitgatename"].ToString();

                    pro.Add(person);
                }
                reader.Close();
            }
            return pro;
        }

        //public List<Product> post(Product pro)
        //{
        //   string strConString = @"datasource=localhost;port=3306;username=root;password=password";
        //    DataTable dt = new DataTable();
        //    using (MySqlConnection con = new MySqlConnection(strConString))
        //    {
        //        con.Open();


        //        string query = "Insert into innoventgate.mcon (facilityCode, thingTypeCode, Timestamp, tagID, itemcode, supplier, productDescrip, sGroup, Department, StockType, DocumentNum, brand, serialNUM, status) values(@facilityCode, @thingTypeCode, @Timestamp, @tagID, @itemcode, @supplier, @productDescrip, @sGroup, @Department, @StockType, @DocumentNum, @brand, @serialNUM, @status)";
        //        MySqlDataReader myreader;

        //        using (MySqlCommand cmd = new MySqlCommand(query, con))
        //        {

        //            cmd.Parameters.AddWithValue("@facilityCode", pro.facilityCode);
        //            cmd.Parameters.AddWithValue("@thingTypeCode", pro.thingTypeCode);
        //            cmd.Parameters.AddWithValue("@Timestamp", pro.Timestamp);
        //            cmd.Parameters.AddWithValue("@tagID", pro.tagID);
        //            cmd.Parameters.AddWithValue("@itemcode", pro.itemcode);
        //            cmd.Parameters.AddWithValue("@supplier", pro.supplier);
        //            cmd.Parameters.AddWithValue("@productDescrip", pro.productDescrip);
        //            cmd.Parameters.AddWithValue("@sGroup", pro.sGroup);
        //            cmd.Parameters.AddWithValue("@Department", pro.Department);
        //            cmd.Parameters.AddWithValue("@StockType", pro.StockType);
        //            cmd.Parameters.AddWithValue("@DocumentNum", pro.DocumentNum);
        //            cmd.Parameters.AddWithValue("@brand", pro.brand);
        //            cmd.Parameters.AddWithValue("@serialNUM", pro.serialNUM);
        //            cmd.Parameters.AddWithValue("@status", pro.status);
        //          //  cmd.Parameters.AddWithValue("@udfs" :{ })



        //            myreader = cmd.ExecuteReader();
        //            dt.Load(myreader);
        //            myreader.Close();
        //            con.Close();
        //        }
        //    }
        //    return new System.Web.Http.Results.JsonResult("Record Added");
        //}

        public Product put(Product pro)
        {
            string strConString = connectionString;//@"datasource=localhost;username=root;password=password";
            DataTable dt = new DataTable();
            using (MySqlConnection con = new MySqlConnection(strConString))
            {
                con.Open();


                string query = " update mcon set @thingTypeCode, @Timestamp, @tagID, @itemcode, @supplier, @productDescrip, @sGroup, @Department, @brand, @serialNUM, @status  where facilityCode = @facilityCode)";
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

                    // myreader = cmd.ExecuteReader();
                    //dt.Load(myreader);
                    //myreader.Close();
                    con.Close();
                }
            }
            return pro;
        }

    }
}
