using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WBS.Data.Extensions;
using WBS.Data.Infrastructure;
using WBS.Data.Repositories;
using WBS.Entities;
using WBS.Web.Infrastructure.Core;
using WBS.Web.Infrastructure.Extensions;
using WBS.Web.Models;

namespace WBS.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/customers")]
    public class CustomersController : ApiControllerBase
    {
        #region Fields

        private readonly IEntityBaseRepository<Customer> _customersRepository;

        #endregion

        #region Constructor

        public CustomersController(IEntityBaseRepository<Customer> customersRepository, IEntityBaseRepository<Error> _errorsRepository, IUnitOfWork _unitOfWork)
          : base(_errorsRepository, _unitOfWork)
        {
            _customersRepository = customersRepository;
        }

        #endregion

        #region Public Methods

        [AllowAnonymous]
        public HttpResponseMessage Get(HttpRequestMessage request)
        {          
            return CreateHttpResponse(request, () =>
            {
                var customers = _customersRepository.GetAll();               
                return request.CreateResponse(HttpStatusCode.OK, Mapper.Map<IEnumerable<Customer>, IEnumerable<CustomerViewModel>>(customers));
            });
        }

        public HttpResponseMessage Get(HttpRequestMessage request, string filter)
        {
            filter = filter.ToLower().Trim();

            return CreateHttpResponse(request, () =>
            {
                var customers = _customersRepository.GetAll()
                    .Where(c => c.Code.ToLower().Contains(filter) ||
                    c.Name.ToLower().Contains(filter)).ToList();

                return request.CreateResponse(HttpStatusCode.OK, Mapper.Map<IEnumerable<Customer>, IEnumerable<CustomerViewModel>>(customers));
            });
        }

        [Route("details/{id:int}")]
        public HttpResponseMessage Get(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                var customer = _customersRepository.GetSingle(id);

                response = request.CreateResponse(HttpStatusCode.OK, Mapper.Map<Customer, CustomerViewModel>(customer));

                return response;
            });
        }

        [HttpPost]
        [Route("add")]
        public HttpResponseMessage Add(HttpRequestMessage request, CustomerViewModel customer)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest,
                        ModelState.Keys.SelectMany(k => ModelState[k].Errors)
                              .Select(m => m.ErrorMessage).ToArray());
                }
                else
                {
                    if (_customersRepository.CustomerExists(customer.Code))
                    {
                        ModelState.AddModelError("Invalid customer", "Customer code already exists");
                        response = request.CreateResponse(HttpStatusCode.BadRequest,
                        ModelState.Keys.SelectMany(k => ModelState[k].Errors)
                              .Select(m => m.ErrorMessage).ToArray());
                    }
                    else
                    {
                        Customer newCustomer = new Customer();
                        newCustomer.UpdateCustomer(customer);

                        _customersRepository.Add(newCustomer);
                        _unitOfWork.Commit();

                        // Update view model
                        response = request.CreateResponse(HttpStatusCode.Created, Mapper.Map<Customer, CustomerViewModel>(newCustomer));
                    }
                }

                return response;
            });
        }

        [HttpPost]
        [Route("update")]
        public HttpResponseMessage Update(HttpRequestMessage request, CustomerViewModel customer)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest,
                        ModelState.Keys.SelectMany(k => ModelState[k].Errors)
                              .Select(m => m.ErrorMessage).ToArray());
                }
                else
                {
                    Customer _customer = _customersRepository.GetSingle(customer.ID);
                    _customer.UpdateCustomer(customer);
                    _unitOfWork.Commit();

                    response = request.CreateResponse(HttpStatusCode.OK);
                }

                return response;
            });
        }

        [HttpGet]
        [Route("search/{page:int=0}/{pageSize=4}/{filter?}")]
        public HttpResponseMessage Search(HttpRequestMessage request, int? page, int? pageSize, string filter = null)
        {
            int currentPage = page.Value;
            int currentPageSize = pageSize.Value;

            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;
                List<Customer> customers = null;
                int totalCustomers = new int();

                if (!string.IsNullOrEmpty(filter))
                {
                    filter = filter.Trim().ToLower();

                    customers = _customersRepository.FindBy(c => c.Code.ToLower().Contains(filter) || c.Name.ToLower().Contains(filter))
                        .OrderBy(c => c.ID)
                        .Skip(currentPage * currentPageSize)
                        .Take(currentPageSize)
                        .ToList();

                    totalCustomers = _customersRepository.GetAll()
                        .Where(c => c.Code.ToLower().Contains(filter) || c.Name.ToLower().Contains(filter))
                        .Count();
                }
                else
                {
                    customers = _customersRepository.GetAll()
                        .OrderBy(c => c.ID)
                        .Skip(currentPage * currentPageSize)
                        .Take(currentPageSize)
                    .ToList();

                    totalCustomers = _customersRepository.GetAll().Count();
                }

                IEnumerable<CustomerViewModel> customerViewModel = Mapper.Map<IEnumerable<Customer>, IEnumerable<CustomerViewModel>>(customers);

                PaginationSet<CustomerViewModel> pagedSet = new PaginationSet<CustomerViewModel>()
                {
                    Page = currentPage,
                    TotalCount = totalCustomers,
                    TotalPages = (int)Math.Ceiling((decimal)totalCustomers / currentPageSize),
                    Items = customerViewModel
                };

                response = request.CreateResponse(HttpStatusCode.OK, pagedSet);

                return response;
            });
        }


        #endregion
    }
}