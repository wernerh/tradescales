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
    [RoutePrefix("api/hauliers")]
    public class HauliersController : ApiControllerBase
    {
        #region Fields

        private readonly IEntityBaseRepository<Haulier> _hauliersRepository;

        #endregion

        #region Constructor

        public HauliersController(IEntityBaseRepository<Haulier> hauliersRepository, IEntityBaseRepository<Error> _errorsRepository, IUnitOfWork _unitOfWork)
           : base(_errorsRepository, _unitOfWork)
        {
            _hauliersRepository = hauliersRepository;
        }

        #endregion

        #region Public Methods

        [AllowAnonymous]
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                var hauliers = _hauliersRepository.GetAll();
                return request.CreateResponse(HttpStatusCode.OK, Mapper.Map<IEnumerable<Haulier>, IEnumerable<HaulierViewModel>>(hauliers));
            });
        }

        public HttpResponseMessage Get(HttpRequestMessage request, string filter)
        {
            filter = filter.ToLower().Trim();

            return CreateHttpResponse(request, () =>
            {
                var hauliers = _hauliersRepository.GetAll()
                    .Where(p => p.Code.ToLower().Contains(filter) || p.Name.ToLower().Contains(filter));

                return request.CreateResponse(HttpStatusCode.OK, Mapper.Map<IEnumerable<Haulier>, IEnumerable<HaulierViewModel>>(hauliers));
            });
        }

        [Route("details/{id:int}")]
        public HttpResponseMessage Get(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                var haulier = _hauliersRepository.GetSingle(id);
                return request.CreateResponse(HttpStatusCode.OK, Mapper.Map<Haulier, HaulierViewModel>(haulier));
            });
        }

        [HttpPost]
        [Route("add")]
        public HttpResponseMessage Add(HttpRequestMessage request, HaulierViewModel haulier)
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
                    if (_hauliersRepository.HaulierExists(haulier.Code, haulier.Name))
                    {
                        ModelState.AddModelError("Invalid haulier", "Haulier already exists");
                        response = request.CreateResponse(HttpStatusCode.BadRequest,
                        ModelState.Keys.SelectMany(k => ModelState[k].Errors)
                              .Select(m => m.ErrorMessage).ToArray());
                    }
                    else
                    {
                        Haulier newHaulier = new Haulier();
                        newHaulier.UpdateHaulier(haulier);

                        _hauliersRepository.Add(newHaulier);
                        _unitOfWork.Commit();

                        // Update view model
                        haulier = Mapper.Map<Haulier, HaulierViewModel>(newHaulier);
                        response = request.CreateResponse(HttpStatusCode.Created, haulier);
                    }
                }

                return response;
            });
        }

        [HttpPost]
        [Route("update")]
        public HttpResponseMessage Update(HttpRequestMessage request, HaulierViewModel haulier)
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
                    Haulier newHaulier = _hauliersRepository.GetSingle(haulier.ID);
                    newHaulier.UpdateHaulier(haulier);
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
                List<Haulier> hauliers = null;
                int totalHauliers = new int();

                if (!string.IsNullOrEmpty(filter))
                {
                    filter = filter.Trim().ToLower();

                    hauliers = _hauliersRepository.FindBy(c => c.Code.ToLower().Contains(filter) || c.Name.ToLower().Contains(filter))
                        .OrderBy(c => c.ID)
                        .Skip(currentPage * currentPageSize)
                        .Take(currentPageSize)
                        .ToList();

                    totalHauliers = _hauliersRepository.GetAll()
                        .Where(c => c.Code.ToLower().Contains(filter) || c.Name.ToLower().Contains(filter))
                        .Count();
                }
                else
                {
                    hauliers = _hauliersRepository.GetAll()
                        .OrderBy(c => c.ID)
                        .Skip(currentPage * currentPageSize)
                        .Take(currentPageSize)
                    .ToList();

                    totalHauliers = _hauliersRepository.GetAll().Count();
                }

                IEnumerable<HaulierViewModel> hauliersViewModels = Mapper.Map<IEnumerable<Haulier>, IEnumerable<HaulierViewModel>>(hauliers);

                PaginationSet<HaulierViewModel> pagedSet = new PaginationSet<HaulierViewModel>()
                {
                    Page = currentPage,
                    TotalCount = totalHauliers,
                    TotalPages = (int)Math.Ceiling((decimal)totalHauliers / currentPageSize),
                    Items = hauliersViewModels
                };

                response = request.CreateResponse(HttpStatusCode.OK, pagedSet);
                return response;
            });
        }
    }

    #endregion
}