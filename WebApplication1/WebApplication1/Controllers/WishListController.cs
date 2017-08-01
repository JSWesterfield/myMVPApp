using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    [RoutePrefix("WishList")]
    public class WishListController : Controller
    {
        // GET: WishList
        //public ActionResult Index()
        //{
        //    return View();
        //}

        //WishList display page prior to create wish page
        [Route("~/index")]
        public ActionResult Index()
        {
            return View();
        }

        //CREATE PAGE(create form for posting to the lists page)
        [Route("~/create")]
        public ActionResult WishListCreate(int locationId)
        {
            WishListCreateViewModel model = new WishListCreateViewModel();
            model.Location = locationId;
            return View("AdminCreate", model);
        }

        //WishList display(display list of users bucket list items)
        [Route("~/wishlist")]
        public ActionResult AdminIndex(int locationId)
        {
            ItemViewModel<int> response = new ItemViewModel<int>();
            response.Item = locationId;
            return View(response);
        }

        
}