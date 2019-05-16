using DemoDatatables.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Linq.Dynamic;
using System.Data.Entity;

namespace DemoDatatables.Controllers
{
    public class DemoController : Controller
    {
        // GET: Demo
        public ActionResult ShowGrid()
        {
            return View();
        }

        public ActionResult LoadData()
        {
            try
            {
                //Creating instance of DatabaseContext class
                using (DatabaseContext _context = new DatabaseContext())
                {
                    var draw = Request.Form.GetValues("draw").FirstOrDefault();
                    var start = Request.Form.GetValues("start").FirstOrDefault();
                    var length = Request.Form.GetValues("length").FirstOrDefault();
                    var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                    var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                    var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();


                    //Paging Size (10,20,50,100)  
                    int pageSize = length != null ? Convert.ToInt32(length) : 0;
                    int skip = start != null ? Convert.ToInt32(start) : 0;
                    int recordsTotal = 0;

                    // Getting all Customer data  
                    var customerData = (from tempcustomer in _context.Customers
                                        select tempcustomer);

                    //Sorting  
                    if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                    {
                        customerData = customerData.OrderBy(sortColumn + " " + sortColumnDir);
                    }

                    //Search  
                    if (!string.IsNullOrEmpty(searchValue))
                    {
                        customerData = customerData.Where(m => m.CompanyName == searchValue
                        || m.ContactName == searchValue || m.Country == searchValue);
                    }

                    //total number of rows count   
                    recordsTotal = customerData.Count();
                    //Paging   
                    var data = customerData.Skip(skip).Take(pageSize).ToList();
                    //Returning Json Data  
                    return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        [HttpGet]
        public ActionResult Edit(int? ID)
        {
            try
            {
                using (DatabaseContext _context = new DatabaseContext())
                {
                    var Customer = (from customer in _context.Customers
                                    where customer.CustomerID == ID
                                    select customer).FirstOrDefault();

                    return View(Customer);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpPost]
        public JsonResult DeleteCustomer(int? ID)
        {
            using (DatabaseContext _context = new DatabaseContext())
            {
                var customer = _context.Customers.Find(ID);
                if (ID == null)
                    return Json(data: "Not Deleted", behavior: JsonRequestBehavior.AllowGet);
                _context.Customers.Remove(customer);
                _context.SaveChanges();

                return Json(data: "Deleted", behavior: JsonRequestBehavior.AllowGet);
            }
        }

    }
}