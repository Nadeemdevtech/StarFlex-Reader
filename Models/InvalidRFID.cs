using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace ExitGateReportPanel.Models
{
    public class InvalidRFID
    {
        public int ID { get; set; }
        public string RFID { get; set; }
        public string Reason { get; set; }
        public bool IsEnable { get; set; }
        public InvalidRFID()
        {

        }

        private readonly IConfiguration configuration;
        private readonly string connectionString = "";
        public InvalidRFID(IConfiguration IConfig)
        {
            configuration = IConfig;
            connectionString = configuration.GetConnectionString("Default");
        }

        public List<InvalidRFID> Get()
        {
            List<InvalidRFID> _list = new List<InvalidRFID>();

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("select * from invalidrfid  order by ID desc", con);

                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    InvalidRFID model = new InvalidRFID();
                    model.ID = Convert.ToInt32(reader["ID"]);
                    model.RFID = reader["RFID"].ToString();
                    model.IsEnable = (bool)reader["IsEnable"];
                    model.Reason = reader["Reason"].ToString();

                    _list.Add(model);
                }
                reader.Close();
            }
            return _list;
        }

        public InvalidRFID Find(int id)
        {
            InvalidRFID model = new InvalidRFID();
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand("select * from invalidrfid  where Id = " + id, con);

                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {

                    model.ID = Convert.ToInt32(reader["ID"]);
                    model.RFID = reader["RFID"].ToString();
                    model.IsEnable = (bool)reader["IsEnable"];
                    model.Reason = reader["Reason"].ToString();

                }
                reader.Close();
            }
            return model;
        }


        public void Update(InvalidRFID model)
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand($"Update invalidrfid  set IsEnable = {model.IsEnable} , RFID = '{model.RFID}',  Reason = '{model.Reason}' where ID = {model.ID}", con);

                cmd.ExecuteNonQuery();
            }
        }

        //public void Insert(InvalidRFID model)
        //{
        //    using (MySqlConnection con = new MySqlConnection(connectionString))
        //    {
        //        con.Open();
        //        MySqlCommand cmd = new MySqlCommand($"Insert into  invalidrfid (rfid,isenable)  values('{model.RFID}',{model.IsEnable})", con);

        //        cmd.ExecuteNonQuery();
        //    }
        //}

        public void Insert(InvalidRFID model)
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();


                string query = "Insert into invalidrfid (rfid, isenable, Reason) values(@rfID, @IsEnable, @Reason)";
                MySqlDataReader myreader;

                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {

                    cmd.Parameters.AddWithValue("@rfID", model.RFID);
                    cmd.Parameters.AddWithValue("@IsEnable", model.IsEnable);
                    cmd.Parameters.AddWithValue("@Reason", model.Reason);

                    myreader = cmd.ExecuteReader();
                    myreader.Close();
                    con.Close();
                }
            }

        }


        public void Delete(InvalidRFID model)
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                MySqlCommand cmd = new MySqlCommand($"Delete from  invalidrfid where ID = {model.ID}", con);

                cmd.ExecuteNonQuery();
            }
        }
    }
}
