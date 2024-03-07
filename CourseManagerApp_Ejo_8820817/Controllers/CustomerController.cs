using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Customers.Entities;
using Customers.Services;
using CustomerManagerApp_Ejo_8820817.Models;
using CustomerManagerApp_Ejo_8820817.DataAccess;
using System.Text.RegularExpressions;

namespace CustomerManagerApp_Ejo_8820817.Controllers
{
    public class CustomerController : Controller
    {

        private readonly IProcessDataService _processDataService;

        /// <summary>
        /// The DI container hands us this DB context object when the controller
        /// </summary>
        public CustomerController(CustomerDbContext coursesDbContext, IProcessDataService processDataService)
        {
          
            _customerDbContext = coursesDbContext;//  Save the database context for later use
           _processDataService = processDataService;// Save the data service for later use
        }

        //Retrieves and displays a list of girst group
        public IActionResult GetInitCustomers()
        {
            string group = "A-E";
            var (lowerBound, upperBound) = _processDataService.GetBoundsForGroup(group);
            // Query the database for customers associated the group
            var customers = _customerDbContext.Customers
                .Where(c => c.Name.ToLower().Substring(0, 1).CompareTo(lowerBound) >= 0
                        && c.Name.ToLower().Substring(0, 1).CompareTo(upperBound) <= 0)
                .Include(c => c.Invoices)
                .OrderBy(m => m.Name)
                .ToList();

            CustomersViewModel customerToManageModel = new CustomersViewModel()
            {
                Customers = customers,
                Group = group
            };

            // Render the view with the list of customers
            return View("List", customerToManageModel);

        }

        [HttpGet("/customer/group/{group}")]
        public IActionResult GetCustomerByGroup(string group)
        {
            var (lowerBound, upperBound) = _processDataService.GetBoundsForGroup(group);

            // Query the database for customers associated the group
            var customers = _customerDbContext.Customers
                .Where(c=>c.Name.ToLower().Substring(0,1).CompareTo(lowerBound)>=0
                        && c.Name.ToLower().Substring(0,1).CompareTo(upperBound)<=0)
                .Include(c => c.Invoices)
                .OrderBy(m => m.Name)
                .ToList();

            foreach (var c in customers)
            {
                if (c != null && c.IsDeleted == true)
                {
                    // delete a customer
                    _customerDbContext.Customers.Remove(c);
                    _customerDbContext.SaveChanges();
                }
            }

            CustomersViewModel customerToManageModel = new CustomersViewModel()
            {
                Customers = customers,
                Group = group
            };

            // Render the view with the list of customers
            return View("List", customerToManageModel);
        }

        //Retrieves and displays details for customer by ID
        [HttpGet("/customer/{id}")]
        public IActionResult GetManageFormById(int id)
        {

            Customer? customer = _customerDbContext.Customers
                    .Include(c => c.Invoices)
                    .Where(m => m.CustomerId == id)
                    .FirstOrDefault();

            // Prepare the view model for add view
            CustomerToManageModel customerToManageModel = new CustomerToManageModel()
            {
                ActiveCustomer = customer,
            };

            // Render the Add view
            return View("Add", customerToManageModel);
        }


        // For adding new invoices for a customer with id
        [HttpPost("/customer/{id}/invoices-form")]
        public IActionResult AddInvoiceToCustomerById(bool isInvoiceForm, int id, int selectedInvoiceId, UpdateInvoiceModel manageModel)
        {
            if(ModelState.IsValid)
            { 
                if(isInvoiceForm)
                {
                    Invoice invoice = manageModel.NewInvoice;
                    invoice.CustomerId = id;
                    _customerDbContext.Invoices.Add(invoice);
                }
                else
                {
                    InvoiceLineItem item = new InvoiceLineItem();
                    item = manageModel.NewLineItem;
                    item.InvoiceId = selectedInvoiceId;
                    _customerDbContext.InvoiceLineItems.Add(item);

                }

                _customerDbContext.SaveChanges();
                return RedirectToAction("GetCustomerInvoicesById", "Customer", new { id = id, selectedInvoiceId = selectedInvoiceId });


            }
            else
            {
                Customer? customer = _customerDbContext.Customers
                .Include(m => m.Invoices).ThenInclude(i => i.InvoiceLineItems)
                .Where(m => m.CustomerId == id)
                .FirstOrDefault();

                var invoices = customer.Invoices;


                var paymentTerms = _customerDbContext.PaymentTerms.OrderBy(p => p.PaymentTermsId).ToList();

                var selectedInvoice = _customerDbContext.Invoices
                                 .Include(m => m.InvoiceLineItems)
                                 .Where(m => m.InvoiceId == selectedInvoiceId)
                                 .FirstOrDefault();

                var firstInvoice = invoices.Count == 0 ? null : invoices.First();
                int firstInvoiceId = invoices.Count == 0 ? -1 : invoices.First().InvoiceId;


                manageModel.Customer = customer;
                    manageModel.Group = manageModel.Group;
                manageModel.Invoices = invoices;
                manageModel.SelectedInvoice = (selectedInvoice != null) ? selectedInvoice : firstInvoice;
                manageModel.SelectedInvoiceNumber = (selectedInvoice != null) ? selectedInvoice.InvoiceId : firstInvoiceId;
                manageModel.PaymentTermsList = paymentTerms;
                

                return View("Invoice", manageModel);

            }

        }

        // For adding new invoices and items for a customer with id
        [HttpGet("/customer/{id}/invoices")]
        public IActionResult GetCustomerInvoicesById(int id, string group, int selectedInvoiceId)
        {

            Customer? customer = _customerDbContext.Customers
                   .Include(m => m.Invoices).ThenInclude(i => i.InvoiceLineItems)  
                   .Where(m => m.CustomerId == id)
                   .FirstOrDefault();

            var invoices = customer.Invoices;

   
            var paymentTerms = _customerDbContext.PaymentTerms.OrderBy(p => p.PaymentTermsId).ToList();

            var selectedInvoice = _customerDbContext.Invoices
                             .Include(m => m.InvoiceLineItems)
                             .Where(m => m.InvoiceId == selectedInvoiceId)
                             .FirstOrDefault();

            var firstInvoice = invoices.Count == 0 ? null : invoices.First();
            int firstInvoiceId = invoices.Count == 0 ? -1 : invoices.First().InvoiceId;
           
            UpdateInvoiceModel customerDetailsViewModel = new UpdateInvoiceModel()
            {
                Customer = customer,
                Group = group,
                Invoices = invoices,
                SelectedInvoice = (selectedInvoice != null) ? selectedInvoice : firstInvoice,
                SelectedInvoiceNumber = (selectedInvoice != null) ? selectedInvoice.InvoiceId : firstInvoiceId,
                NewInvoice = new Invoice(),
                NewLineItem = new InvoiceLineItem(),
                PaymentTermsList = paymentTerms
            };

            return View("Invoice", customerDetailsViewModel);
        }

        // A GET handler that returns the blank form to add a new customer
        [HttpGet("/customer/add-form")]
        public IActionResult GetAddNewCustomerForm(string group)
        {
            ViewData["CurrentGroup"] = group;

            CustomerToManageModel updateViewModel = new CustomerToManageModel()
            {
                ActiveCustomer = new Customer()
            };

            // and then pass it to the view:
            return View("Add", updateViewModel);
        }

        // A POST handler that gets the Customer dat in the HTTP POST body passed
        // as a param and is then added to the DB:
        [HttpPost("/customer/add")]
        public IActionResult ProcessAddRequest(CustomerToManageModel updateCustomerViewModel, string group)
        {
            ViewData["CurrentGroup"] = group;
            if (ModelState.IsValid)
            {
                Customer customer = updateCustomerViewModel.ActiveCustomer;
                // because it's valid add it to the DB & save changes:
                _customerDbContext.Customers.Add(customer);
                _customerDbContext.SaveChanges();

                // Add a message to the TempData dictionary that the customer was added:
                TempData["LastActionMessage"] = $"The customer {updateCustomerViewModel.ActiveCustomer.Name}) was added successfully!";

             
                // We redirect back to the all customer view:
                return RedirectToAction("GetCustomerByGroup", new { group = group });
            }
            else
            {

                // and then pass it to the view:
                return View("Add", updateCustomerViewModel);
            }
        }

        // A GET handler that returns the form filled with the customer with id
        [HttpGet("/customer/{id}/edit-form")]
        public IActionResult GetEditFormById(int id, string group)
        {

            ViewData["CurrentGroup"] = group;

            Customer? customer = _customerDbContext.Customers
                   .Include(c => c.Invoices)
                   .Where(m => m.CustomerId == id)
                   .FirstOrDefault();

            CustomerToManageModel updateViewModel = new CustomerToManageModel()
            {
                ActiveCustomer = customer
            };

            // and then pass it to the view:
            return View("Edit", updateViewModel);
        }

        // A POST handler that edits the customer using the data in the HTTP POST body passed
        // as a param and is then added to the DB:00
        [HttpPost("/customer/edit")]
        public IActionResult ProcessEditRequest(CustomerToManageModel updateCustomerViewModel, string group)
        {
            ViewData["CurrentGroup"] = group;
            if (ModelState.IsValid)
            {
                // because it's valid update it to the DB & save changes:
                _customerDbContext.Customers.Update(updateCustomerViewModel.ActiveCustomer);
                _customerDbContext.SaveChanges();

                TempData["LastActionMessage"] = $"The customer {updateCustomerViewModel.ActiveCustomer.Name} was updated successfully!";
               
                return RedirectToAction("GetCustomerByGroup", new { group = group });
            }
            else
            {

                // and then pass it to the view:
                return View("Edit", updateCustomerViewModel);
            }
        }

        [HttpGet("/customer/{id}/delete")]
        public async Task<IActionResult> GetDeleteCustomerById(int id, string group)
        {
            var (lowerBound, upperBound) = _processDataService.GetBoundsForGroup(group);
            var customers = _customerDbContext.Customers
               .Where(c => c.Name.ToLower().Substring(0, 1).CompareTo(lowerBound) >= 0
                       && c.Name.ToLower().Substring(0, 1).CompareTo(upperBound) <= 0)
               .Include(c => c.Invoices)
               .OrderBy(m => m.Name)
               .ToList();

            foreach (var c in customers)
            {
                if (c != null && c.IsDeleted == true)
                {
                    // delete a customer
                    _customerDbContext.Customers.Remove(c);
                    _customerDbContext.SaveChanges();
                }
            }


            var customer = _customerDbContext.Customers.Find(id);
            customer.IsDeleted = true;

            TempData["UndoCustomerId"] = id;

            _customerDbContext.Customers.Update(customer);
            _customerDbContext.SaveChanges();

            // Query the database for courses, including associated students
            customers = _customerDbContext.Customers
                .Where(c => c.Name.ToLower().Substring(0, 1).CompareTo(lowerBound) >= 0
                        && c.Name.ToLower().Substring(0, 1).CompareTo(upperBound) <= 0)
                .Include(c => c.Invoices)
                .OrderBy(m => m.Name)
                .ToList();
             

            CustomersViewModel customerToManageModel = new CustomersViewModel()
            {
                Customers = customers,
                Group = lowerBound + "-" + upperBound
            };

            // Render the view with the list of courses
            return View("List", customerToManageModel);

        }


        [HttpGet("/customer/{id}/undo")]
        public async Task<IActionResult> UndoDelete(int id, string group)
        {

            var customer = _customerDbContext.Customers.Find(id);
            customer.IsDeleted = false;

            TempData["UndoCustomerId"] = null;

            _customerDbContext.Customers.Update(customer);
            _customerDbContext.SaveChanges();

            var (lowerBound, upperBound) = _processDataService.GetBoundsForGroup(group);

            // Query the database for courses, including associated students
            var customers = _customerDbContext.Customers
                .Where(c => c.Name.ToLower().Substring(0, 1).CompareTo(lowerBound) >= 0
                        && c.Name.ToLower().Substring(0, 1).CompareTo(upperBound) <= 0)
                .Include(c => c.Invoices)
                .OrderBy(m => m.Name)
                .ToList();


            CustomersViewModel customerToManageModel = new CustomersViewModel()
            {
                Customers = customers,
                Group = lowerBound + "-" + upperBound
            };

            // Render the view with the list of courses
            return View("List", customerToManageModel);
        }


        private readonly CustomerDbContext _customerDbContext;
    }
}
