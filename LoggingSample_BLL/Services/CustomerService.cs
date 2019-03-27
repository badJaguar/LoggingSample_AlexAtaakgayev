﻿using System;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using LoggingSample_BLL.Helpers;
using LoggingSample_BLL.Models;
using LoggingSample_DAL.Context;

namespace LoggingSample_BLL.Services
{
    public class CustomerService : IDisposable
    {
        private readonly AppDbContext _context = new AppDbContext();

        public Task<CustomerModel> GetCustomerAsync(int customerId)
        {
            if (customerId == 56)
            {
                throw new CustomerServiceException("Wrong id has been requested",
                    CustomerServiceException.ErrorType.WrongCustomerId);
            }

            return _context.Customers.SingleOrDefaultAsync(item => item.Id == customerId).ContinueWith(task =>
            {
                var customer = task.Result;

                return customer?.Map();
            });
        }

        public async Task<CustomerModel> CreateCustomerAsync(CustomerModel model)
        {
            var dbModels = await _context.Customers
                .Where(customer => customer.Id == model.Id).ToListAsync();
            var equitable =await _context.Customers.Select(customer => customer.Id).ToListAsync();
            foreach (var customerId in equitable)
            {
                if (model.Id == customerId)
                {
                    throw new CustomerServiceException($"Model {customerId} is exists in database", CustomerServiceException.ErrorType.ModelIsExists);
                }
            }
            
            
            _context.Customers.AddRange(dbModels);
            await _context.SaveChangesAsync();
            return model;
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }

    public class CustomerServiceException : Exception
    {

        public CustomerServiceException(string message, ErrorType errorType) : base(message)
        {
            Type = errorType;
        }
        public enum ErrorType
        {
            WrongCustomerId,
            ModelIsExists
        }

        public ErrorType Type { get; set; }
    }
}