﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MotionsberegnerRestService.Model;

namespace MotionsberegnerRestService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StepController : ControllerBase
    {

        private const string conn = "Server=tcp:motionsberegner.database.windows.net,1433;Initial Catalog=profileDB;Persist Security Info=False;User ID=shabz;Password=Password123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        // GET: api/Step
        [HttpGet]
        public List<StepData> GetAllData()
        {
            List<StepData> result = new List<StepData>();

            string sql = "SELECT id, profileId, steps, logDate FROM StepData "; //SQL Command

            using (SqlConnection databaseConnection = new SqlConnection(conn))
            {
                databaseConnection.Open();

                using (SqlCommand selectCommand = new SqlCommand(sql, databaseConnection))
                {
                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                int id = reader.GetInt32(0);
                                int fId = reader.GetInt32(1);
                                int steps = reader.GetInt32(2);
                                DateTime logDate = reader.GetDateTime(3);

                                StepData sd = new StepData(id, fId, steps, logDate);

                                result.Add(sd);
                            }

                        }
                    }
                }
            }

            return result;
        }

        // GET: api/Step/5
        [HttpGet("{id}", Name = "GetStep")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Step
        [HttpPost]
        public int Post([FromBody] StepData stepData)
        {
            int insId = 0;

            string sql = "INSERT INTO stepData(profileId, steps, logDate) OUTPUT INSERTED.id VALUES(@profileId, @steps, @logDate);"; //SQL Command - OUTPUT INSERTED.id udskriver den valgte kolonne 'id', fra den indsatte række.

            using (SqlConnection databaseConnection = new SqlConnection(conn))
            {
                databaseConnection.Open();

                using (SqlCommand cmd = new SqlCommand(sql, databaseConnection))
                {
                    cmd.Parameters.AddWithValue("@profileId", stepData.FId); //@profileId er placeholder og bliver erstattet af 'stepData.FId'
                    cmd.Parameters.AddWithValue("@steps", stepData.Steps);
                    cmd.Parameters.AddWithValue("@logDate", stepData.LogDate);

                    insId = (int)cmd.ExecuteScalar(); //Tager kolonnen som bliver returneret i SQL Command (OUTPUT INSERTED.id) og indsætter som 'insId'
                }
                
            }

            return insId;
        }

        //// PUT: api/Step/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
