using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Configuration;

namespace ManxBirdLifeAppWebApi.Controllers
{
    
    public class LoginController : ApiController
    {        
        [HttpPost]
        [ResponseType(typeof(LoginResponse))]
        public IHttpActionResult Post(LoginCredentials postData)
        {
            var response = new LoginResponse();
            response.success = false;

            if (!ModelState.IsValid)
            {
                response.message = "Bad request";
                return BadRequest();
            }

            string adminPassword = ConfigurationManager.AppSettings["AdminLoginPassword"];
            if (postData.UserName.Equals("Admin", StringComparison.OrdinalIgnoreCase) && postData.Password.Equals(adminPassword))
            {
                response.success = true;                
            }   
            else
            {
                response.success = false;
                response.message = "Invalid username and password combination.";                 
            }
            return Ok(response);           

        }
    }

    public class LoginResponse
    {
        public bool success { get; set; }
        public string message { get; set; }
    }

    public class LoginCredentials
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
