using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApplication1.Controllers.Api
{
    [RoutePrefix("api/list")]
    public class ListApiController : ApiController
    {
        private IListsService _ListsService;

        public ListsApiController(IListsService ListsService)
        {
            _ListsService = ListsService;
        }
    }
}
