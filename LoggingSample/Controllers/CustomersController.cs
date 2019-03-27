using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Routing;
using LoggingSample_BLL.Helpers;
using LoggingSample_BLL.Models;
using LoggingSample_BLL.Services;
using LoggingSample_DAL.Context;
using NLog;

namespace LoggingSample.Controllers
{
    [RoutePrefix("api/customers")]
    public class CustomersController : ApiController
    {
        private readonly AppDbContext _context = new AppDbContext();
        private readonly CustomerService _customerService = new CustomerService();
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        [Route("")]
        public async Task<IHttpActionResult> Get()
        {
            var customers = (await _context.Customers.ToListAsync()).Select(item => item.Map()).Select(InitCustomer);

            return Ok(customers);
        }

        [Route("{customerId}", Name = "Customer")]
        public async Task<IHttpActionResult> Get(int customerId)
        {
            Logger.Info($"Start getting customer with id {customerId}.");

            try
            {
                var customer = await _customerService.GetCustomer(customerId);

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


        private object InitCustomer(CustomerModel model)
        {
            return new
            {
                _self = new UrlHelper(Request).Link("Customer", new {customerId = model.Id}),
                orders = new UrlHelper(Request).Link("Orders", new { customerId = model.Id }),
                data = model
            };
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
                _customerService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}