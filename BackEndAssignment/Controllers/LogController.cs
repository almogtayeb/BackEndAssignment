using BackEndAssignment.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace BackEndAssignment.Controllers
{
    public class LogController : ApiController
    {
        [Route("Log/{Priority}/{Message}")]
        [HttpGet]
        public void Log(string Priority, string Message)
        {
            int priorityInt;
            if (!Int32.TryParse(Priority, out priorityInt))
            {
                throw new ArgumentException("Priority must be an integer");
            }

            Logger.Write(priorityInt, Message);

        }
    }
}
