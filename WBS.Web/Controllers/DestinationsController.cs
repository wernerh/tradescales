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
    [RoutePrefix("api/destinations")]
    public class DestinationsController : ApiControllerBase
    {
        #region Fields

        private readonly IEntityBaseRepository<Destination> _destinationsRepository;

        #endregion

        #region Constructor

        public DestinationsController(IEntityBaseRepository<Destination> destinationsRepository, IEntityBaseRepository<Error> _errorsRepository, IUnitOfWork _unitOfWork)
           : base(_errorsRepository, _unitOfWork)
        {
            _destinationsRepository = destinationsRepository;
        }

        #endregion

        #region Public Methods

        [AllowAnonymous]
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                var destinations = _destinationsRepository.GetAll();
                return request.CreateResponse(HttpStatusCode.OK, Mapper.Map<IEnumerable<Destination>, IEnumerable<DestinationViewModel>>(destinations));
            });
        }

        public HttpResponseMessage Get(HttpRequestMessage request, string filter)
        {
            filter = filter.ToLower().Trim();

            return CreateHttpResponse(request, () =>
            {
                var destinations = _destinationsRepository.GetAll()
                    .Where(p => p.Code.ToLower().Contains(filter) || p.Name.ToLower().Contains(filter));

                return request.CreateResponse(HttpStatusCode.OK, Mapper.Map<IEnumerable<Destination>, IEnumerable<DestinationViewModel>>(destinations));
            });
        }

        [Route("details/{id:int}")]
        public HttpResponseMessage Get(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                var destination = _destinationsRepository.GetSingle(id);
                return request.CreateResponse(HttpStatusCode.OK, Mapper.Map<Destination, DestinationViewModel>(destination));
            });
        }

        [HttpPost]
        [Route("add")]
        public HttpResponseMessage Add(HttpRequestMessage request, DestinationViewModel destination)
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
                    if (_destinationsRepository.DestinationExists(destination.Code, destination.Name))
                    {
                        ModelState.AddModelError("Invalid destination", "Destination already exists");
                        response = request.CreateResponse(HttpStatusCode.BadRequest,
                        ModelState.Keys.SelectMany(k => ModelState[k].Errors)
                              .Select(m => m.ErrorMessage).ToArray());
                    }
                    else
                    {
                        Destination newDestination = new Destination();
                        newDestination.UpdateDestination(destination);

                        _destinationsRepository.Add(newDestination);
                        _unitOfWork.Commit();

                        // Update view model
                        destination = Mapper.Map<Destination, DestinationViewModel>(newDestination);
                        response = request.CreateResponse(HttpStatusCode.Created, destination);
                    }
                }

                return response;
            });
        }

        [HttpPost]
        [Route("update")]
        public HttpResponseMessage Update(HttpRequestMessage request, DestinationViewModel destination)
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
                    Destination newDestination = _destinationsRepository.GetSingle(destination.ID);
                    newDestination.UpdateDestination(destination);
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
                List<Destination> destinations = null;
                int totalDestinations = new int();

                if (!string.IsNullOrEmpty(filter))
                {
                    filter = filter.Trim().ToLower();

                    destinations = _destinationsRepository.FindBy(c => c.Code.ToLower().Contains(filter) || c.Name.ToLower().Contains(filter))
                        .OrderBy(c => c.ID)
                        .Skip(currentPage * currentPageSize)
                        .Take(currentPageSize)
                        .ToList();

                    totalDestinations = _destinationsRepository.GetAll()
                        .Where(c => c.Code.ToLower().Contains(filter) || c.Name.ToLower().Contains(filter))
                        .Count();
                }
                else
                {
                    destinations = _destinationsRepository.GetAll()
                        .OrderBy(c => c.ID)
                        .Skip(currentPage * currentPageSize)
                        .Take(currentPageSize)
                    .ToList();

                    totalDestinations = _destinationsRepository.GetAll().Count();
                }

                IEnumerable<DestinationViewModel> destinationsViewModels = Mapper.Map<IEnumerable<Destination>, IEnumerable<DestinationViewModel>>(destinations);

                PaginationSet<DestinationViewModel> pagedSet = new PaginationSet<DestinationViewModel>()
                {
                    Page = currentPage,
                    TotalCount = totalDestinations,
                    TotalPages = (int)Math.Ceiling((decimal)totalDestinations / currentPageSize),
                    Items = destinationsViewModels
                };

                response = request.CreateResponse(HttpStatusCode.OK, pagedSet);
                return response;
            });
        }
    }

    #endregion
}