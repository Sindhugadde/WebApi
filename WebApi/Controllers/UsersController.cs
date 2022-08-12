using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using WebApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Cors;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _configuration;
    
        [JsonConstructor]

        public UsersController( IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet]
        [EnableCors]
        public Array Get()
        {
           string query = @"
select firstname,lastname,telephone,email from webappDB.dbo.submitusers";
     
            DataTable table = new DataTable();
            string sqldtsource = _configuration.GetConnectionString("usersAppCon");
            SqlDataReader myreader;

            using (SqlConnection myconn = new SqlConnection(sqldtsource))
            {
                myconn.Open();
                using (SqlCommand mycommand = new SqlCommand(query,myconn))
                {
                    myreader = mycommand.ExecuteReader();
                    table.Load(myreader);
                    
                    myreader.Close();
                    myconn.Close();
                    
                }

            }
            string JSONresult;
            JSONresult = JsonConvert.SerializeObject(table);
            submitusers[] users = JsonConvert.DeserializeObject<submitusers[]>(JSONresult);
            return users;
        }

              [HttpPost]
        [Route("AddUser")]
        [EnableCors]

        public JsonResult Post( submitusers user )
        {
            string query = @"insert into dbo.submitusers(firstname,lastname,telephone,email) 
      values(
          '" + user.Firstname +@"'
          ,'"  + user.Lastname+@"'
          ,'" +user.Telephone+@"' 
          ,'" +user.Email+@"'
           )";
            DataTable table = new DataTable();
            string sqldtsource = _configuration.GetConnectionString("usersAppCon");
            SqlDataReader myreader;
            using (SqlConnection myconn = new SqlConnection(sqldtsource))
            {
                myconn.Open();
                using (SqlCommand mycommand = new SqlCommand(query, myconn))
                {
                    myreader = mycommand.ExecuteReader();
                    table.Load(myreader);
                    myreader.Close();
                    myconn.Close();

                }
            }
            return new JsonResult("Added the user successfully");
        }
    }
}
