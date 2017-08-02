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
    public class PeopleApiController : ApiController
    {
        // GET api/<controller>
        [HttpGet]
        public HttpResponseMessage Get()
        {
            ItemsResponse<Person> response = new ItemsResponse<Person>();
            try
            {
                PeopleService svc = new PeopleService();
                List<Person> people = svc.SelectAll();
                response.Items = people;
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
        public HttpResponseMessage Post(Person model)
        {
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);

            ItemResponse<int> response = new ItemResponse<int>();
            try
            {
                PeopleService svc = new PeopleService();
                response.Item = svc.Insert(model);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
            return Request.CreateResponse(HttpStatusCode.OK, response);
        }


        [HttpPut]
        public HttpResponseMessage Put(Person model)
        {
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ModelState);

            try
            {
                PeopleService svc = new PeopleService();
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
                PeopleService svc = new PeopleService();
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