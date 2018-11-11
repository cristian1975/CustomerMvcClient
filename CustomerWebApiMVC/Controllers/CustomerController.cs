using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CustomerWebApiMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace CustomerWebApiMVC.Controllers
{
    public class CustomerController : Controller
    {
        public ActionResult Index()
        {
            IEnumerable<Customer> custList;
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Customers").Result;
            custList = response.Content.ReadAsAsync<IEnumerable<Customer>>().Result;
            return View(custList);
        }

        public ActionResult AddOrEdit(string id = "")
        {
            if (string.IsNullOrEmpty(id))
                return View(new Customer());
            else
            {
                HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("Customers/" + id.ToString()).Result;
                return View(response.Content.ReadAsAsync<Customer>().Result);
            }
        }
        [HttpPost]
        public ActionResult AddOrEdit(Customer cust)
        {
            if (string.IsNullOrEmpty(cust.Id))
            {
                HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("Customers", cust).Result;
                TempData["SuccessMessage"] = "Saved Successfully";
            }
            else
            {
                HttpResponseMessage response =  GlobalVariables.WebApiClient.PutAsJsonAsync("Customers/" + cust.Id, cust).Result;
                TempData["SuccessMessage"] = "Updated Successfully";
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(string id)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.DeleteAsync("Customers/" + id).Result;
            TempData["SuccessMessage"] = "Deleted Successfully";
            return RedirectToAction("Index");
        }
    }
}