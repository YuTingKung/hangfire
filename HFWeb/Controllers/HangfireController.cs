using Hangfire;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HFWeb.Controllers
{
    public class HangfireController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            // delay: 設定時間間隔，每隔時間間隔執行一次
            BackgroundJob.Schedule(() => FunctionInsert(), TimeSpan.FromSeconds(10) );
            return new string[] { "value1", "value2" };
        }

        public void FunctionInsert()
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["HangfireConnection"].ConnectionString))
            {
                con.Open();
                var time = DateTime.Now;
                SqlCommand cmd = new SqlCommand("INSERT INTO dbo.Users (UserId,UserName,Password,RoleId) values(@UserId,@UserName,@Password,@RoleId)", con);
                cmd.Parameters.Add(new SqlParameter("@UserId", "100"));
                cmd.Parameters.Add(new SqlParameter("@UserName", time.ToString()));
                cmd.Parameters.Add(new SqlParameter("@Password", "200"));
                cmd.Parameters.Add(new SqlParameter("@RoleId", "300"));
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            RecurringJob.AddOrUpdate(() => FunctionInsert(), "*/2 * * * *");
            return id.ToString();
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}