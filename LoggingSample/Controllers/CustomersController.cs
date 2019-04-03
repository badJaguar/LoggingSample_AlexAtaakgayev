using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using LoggingSample_BLL.Helpers;
using LoggingSample_BLL.Models;
using LoggingSample_BLL.Services;
using NLog;
using UrlHelper = System.Web.Http.Routing.UrlHelper;

namespace LoggingSample.Controllers
{
    [System.Web.Http.RoutePrefix("api/customers")]
    public class CustomersController : ApiController
    {
        private readonly CustomerService _customerService = new CustomerService();
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        [System.Web.Http.Route("")]
        public async Task<IHttpActionResult> Get()
        {
            try
            {
                var customers =(await _customerService.GetAllAsync())
                    .Select(InitCustomer).ToList();

                if (customers.Count <= 0)
                {
                    Logger.Warn($"{nameof(customers)} is empty.");
                }

                Logger.Info($"All customers was returned successfully.");
                return Ok(customers);
            }
            catch (CustomerServiceException e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        [System.Web.Http.Route("{customerId}", Name = "Customer")]
        public async Task<IHttpActionResult> Get(int customerId)
        {
            Logger.Info($"Start getting customer with id {customerId}.");

            try
            {
                var customer = await _customerService.GetCustomerAsync(customerId);

                if (customer == null)
                {
                    Logger.Info($"No customer with id {customerId} was found.");
                    return NotFound();
                }

                Logger.Info($"Retrieving customer with id {customerId} to response.");

                return Ok(InitCustomer(customer));
            }
            catch (CustomerServiceException ex)
            {
                if (ex.Type == CustomerServiceException.ErrorType.WrongCustomerId)
                {
                    Logger.Warn($"Wrong customerId has been request: {customerId}", ex);
                    return BadRequest($"Wrong customerId has been request: {customerId}");
                }
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Some error occured while getting customerId {customerId}");
                throw;
            }
        }

        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("")]
        public async Task<IHttpActionResult> PostCustomerAsync([FromBody] CustomerModel model)
        {
            Logger.Info($"Starting try to add a customer {model}.");
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Select(pair => pair.Value.Errors)
                    .Where(collection => collection.Count > 0).ToList();

                foreach (var error in errors)
                {
                    Logger.Error(error);
                }
                return BadRequest(ModelState);
            }

            await _customerService.CreateCustomerAsync(model);
            return Ok(InitCustomer(model));
        }


        private object InitCustomer(CustomerModel model)
        {
            return new
            {
                _self = new UrlHelper(Request).Link("Customer", new
                {
                    customerId = model.Id
                }),
                orders = new UrlHelper(Request).Link("Orders", new
                {
                    customerId = model.Id
                }),
                data = model
            };
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _customerService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}