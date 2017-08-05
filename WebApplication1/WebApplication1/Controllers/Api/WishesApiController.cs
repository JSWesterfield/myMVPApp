using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WikiWebStarter.Example.Models;
using WikiWebStarter.Example.Services;
using WikiWebStarter.Web.Models.Responses;

namespace WikiWebStarter.Example.Controllers.Api
{
    [Route("api/wish")]
    public class WishesApiController : ApiController
    {
        // GET api/<controller>
        [HttpGet]
        public HttpResponseMessage Get()
        {
            ItemsResponse<Wish> response = new ItemsResponse<Wish>();
            try
            {
                WishesService svc = new WishesService();
                List<Wish> wishes = svc.SelectAll();
                response.Items = wishes;
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }
        

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        public HttpResponseMessage Post(Wish model)
        {
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);

            ItemResponse<int> response = new ItemResponse<int>();
            try
            {
                WishesService svc = new WishesService();
                response.Item = svc.Insert(model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }


        [HttpPut]
        public HttpResponseMessage Put(Wish model)
        {
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ModelState);

            try
            {
                WishesService svc = new WishesService();
                svc.Update(model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
            return Request.CreateResponse(HttpStatusCode.OK, new SuccessResponse());
        }

        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            try
            {
                WishesService svc = new WishesService();
                svc.Delete(id);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
            return Request.CreateResponse(HttpStatusCode.OK, new SuccessResponse());
        }
    }
}