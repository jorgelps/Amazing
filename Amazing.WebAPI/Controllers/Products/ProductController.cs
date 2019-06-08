using Amazing.DT.General;
using Amazing.DT.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Amazing.WebAPI.Controllers.Products
{
    public class ProductController : ApiController
    {
       
        public DTServiceResponse<DTProduct> GetListproducts(int id)
        {
            DTServiceResponse<DTProduct> response = new DTServiceResponse<DTProduct>();
            DTProduct product = new DTProduct();
            response.Result = product;
            return response;
        }       
    }
}