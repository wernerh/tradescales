using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TradeScales.Data.Extensions;
using TradeScales.Data.Infrastructure;
using TradeScales.Data.Repositories;
using TradeScales.Entities;
using TradeScales.Web.Infrastructure.Core;
using TradeScales.Web.Infrastructure.Extensions;
using TradeScales.Web.Models;

namespace TradeScales.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/drivers")]
    public class DriversController : ApiControllerBase
    {
        #region Fields

        private readonly IEntityBaseRepository<Driver> _driversRepository;

        #endregion

        #region Constructor

        public DriversController(IEntityBaseRepository<Driver> driversRepository, IEntityBaseRepository<Error> _errorsRepository, IUnitOfWork _unitOfWork)
            : base(_errorsRepository, _unitOfWork)
        {
            _driversRepository = driversRepository;
        }

        #endregion

        #region Public Methods

        [AllowAnonymous]
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                var drivers = _driversRepository.GetAll();
                return request.CreateResponse(HttpStatusCode.OK, Mapper.Map<IEnumerable<Driver>, IEnumerable<DriverViewModel>>(drivers));
            });
        }

        public HttpResponseMessage Get(HttpRequestMessage request, string filter)
        {
            filter = filter.ToLower().Trim();

            return CreateHttpResponse(request, () =>
            {
                var drivers = _driversRepository.GetAll()
                    .Where(c => c.Email.ToLower().Contains(filter) ||
                    c.FirstName.ToLower().Contains(filter) ||
                    c.LastName.ToLower().Contains(filter)).ToList();

                return request.CreateResponse(HttpStatusCode.OK, Mapper.Map<IEnumerable<Driver>, IEnumerable<DriverViewModel>>(drivers));
            });
        }

        [Route("details/{id:int}")]
        public HttpResponseMessage Get(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                var driver = _driversRepository.GetSingle(id);

                return request.CreateResponse(HttpStatusCode.OK, Mapper.Map<Driver, DriverViewModel>(driver));
            });
        }

        [HttpPost]
        [Route("add")]
        public HttpResponseMessage Add(HttpRequestMessage request, DriverViewModel driver)
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
                    if (_driversRepository.DriverExists(driver.Email, driver.IdentityCard))
                    {
                        ModelState.AddModelError("Invalid driver", "Email or Identity Card number already exists");
                        response = request.CreateResponse(HttpStatusCode.BadRequest,
                        ModelState.Keys.SelectMany(k => ModelState[k].Errors)
                              .Select(m => m.ErrorMessage).ToArray());
                    }
                    else
                    {
                        Driver newDriver = new Driver();
                        newDriver.UpdateDriver(driver);

                        _driversRepository.Add(newDriver);
                        _unitOfWork.Commit();

                        // Update view model
                        driver = Mapper.Map<Driver, DriverViewModel>(newDriver);
                        response = request.CreateResponse(HttpStatusCode.Created, driver);
                    }
                }

                return response;
            });
        }

        [HttpPost]
        [Route("update")]
        public HttpResponseMessage Update(HttpRequestMessage request, DriverViewModel driver)
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
                    Driver newDriver = _driversRepository.GetSingle(driver.ID);
                    newDriver.UpdateDriver(driver);

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
                List<Driver> drivers = null;
                int totalDrivers = new int();

                if (!string.IsNullOrEmpty(filter))
                {
                    filter = filter.Trim().ToLower();

                    drivers = _driversRepository.FindBy(c => c.LastName.ToLower().Contains(filter) ||
                            c.IdentityCard.ToLower().Contains(filter) ||
                            c.FirstName.ToLower().Contains(filter))
                        .OrderBy(c => c.ID)
                        .Skip(currentPage * currentPageSize)
                        .Take(currentPageSize)
                        .ToList();

                    totalDrivers = _driversRepository.GetAll()
                        .Where(c => c.LastName.ToLower().Contains(filter) ||
                            c.IdentityCard.ToLower().Contains(filter) ||
                            c.FirstName.ToLower().Contains(filter))
                        .Count();
                }
                else
                {
                    drivers = _driversRepository.GetAll()
                        .OrderBy(c => c.ID)
                        .Skip(currentPage * currentPageSize)
                        .Take(currentPageSize)
                    .ToList();

                    totalDrivers = _driversRepository.GetAll().Count();
                }

                IEnumerable<DriverViewModel> driversViewModel = Mapper.Map<IEnumerable<Driver>, IEnumerable<DriverViewModel>>(drivers);

                PaginationSet<DriverViewModel> pagedSet = new PaginationSet<DriverViewModel>()
                {
                    Page = currentPage,
                    TotalCount = totalDrivers,
                    TotalPages = (int)Math.Ceiling((decimal)totalDrivers / currentPageSize),
                    Items = driversViewModel
                };

                response = request.CreateResponse(HttpStatusCode.OK, pagedSet);
                return response;
            });
        }

        #endregion
    }
}