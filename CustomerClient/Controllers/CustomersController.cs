using Common;
using CustomerClient.Models;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Cors;
using System.Web.Mvc;
using CustomerClient.ViewModels;

namespace CustomerClient.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CustomersController : Controller
    {
        public MoneyOrderRequest moRequest;
        
        CustomerRepository customerRepo = new CustomerRepository();


        // GET: Customers
        public ActionResult Index()
        {
            var moVM = new MoneyOrderVM();
            moVM.Requests = customerRepo.GetMoneyOrderRequests();
            moVM.MoneyOrders = customerRepo.GetMoneyOrders();

            return View(moVM);
        }


        /// <summary>
        /// creates Money order request
        /// </summary>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public JsonResult CreateMoneyOrderRequest(decimal amount)
        {
            customerRepo.CreateMoneyOrderRequest(amount);

            return Json("MO Request Created" , JsonRequestBehavior.AllowGet);
        }

        [EnableCors(origins: "*", headers: "*", methods: "*")]
        public JsonResult GetMoneyOrderRequest(int id)
        {
            var moReq = customerRepo.GetMoneyOrderByID(id);
            return Json(moReq, JsonRequestBehavior.AllowGet);
        }

        
        public JsonResult RequestCash(int id)
        {
            var result = customerRepo.RequestCash(id);
            return Json("Cash Requested!!", JsonRequestBehavior.AllowGet);
        }



    }
}