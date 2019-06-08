using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Amazing.Support.Log;

namespace Amazing.WebAPI.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        [Authorize]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            int numero = 1;
            int cero = 0;
            try
            {

                var tota = numero / cero;
            }
            catch (Exception ex)
            {

                LogManager.RegistrarLogExcepcion(ex);
                LogManager.RegistrarLogExcepcion(ex, numero);
            }

            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
