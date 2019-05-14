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
    [RoutePrefix("api/products")]
    public class ProductsController : ApiControllerBase
    {
        #region Fields

        private readonly IEntityBaseRepository<Product> _productsRepository;

        #endregion

        #region Constructor

        public ProductsController(IEntityBaseRepository<Product> productsRepository, IEntityBaseRepository<Error> _errorsRepository, IUnitOfWork _unitOfWork)
           : base(_errorsRepository, _unitOfWork)
        {
            _productsRepository = productsRepository;
        }

        #endregion

        #region Public Methods

        [AllowAnonymous]
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                var products = _productsRepository.GetAll();
                return request.CreateResponse(HttpStatusCode.OK, Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(products));
            });
        }

        public HttpResponseMessage Get(HttpRequestMessage request, string filter)
        {
            filter = filter.ToLower().Trim();

            return CreateHttpResponse(request, () =>
            {
                var products = _productsRepository.GetAll()
                    .Where(p => p.Code.ToLower().Contains(filter) || p.Name.ToLower().Contains(filter));

                return request.CreateResponse(HttpStatusCode.OK, Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(products));
            });
        }

        [Route("details/{id:int}")]
        public HttpResponseMessage Get(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                var product = _productsRepository.GetSingle(id);
                return request.CreateResponse(HttpStatusCode.OK, Mapper.Map<Product, ProductViewModel>(product));
            });
        }

        [HttpPost]
        [Route("add")]
        public HttpResponseMessage Add(HttpRequestMessage request, ProductViewModel product)
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
                    if (_productsRepository.ProductExists(product.Code, product.Name))
                    {
                        ModelState.AddModelError("Invalid product", "Product already exists");
                        response = request.CreateResponse(HttpStatusCode.BadRequest,
                        ModelState.Keys.SelectMany(k => ModelState[k].Errors)
                              .Select(m => m.ErrorMessage).ToArray());
                    }
                    else
                    {
                        Product newProduct = new Product();
                        newProduct.UpdateProduct(product);

                        _productsRepository.Add(newProduct);
                        _unitOfWork.Commit();

                        // Update view model
                        product = Mapper.Map<Product, ProductViewModel>(newProduct);
                        response = request.CreateResponse(HttpStatusCode.Created, product);
                    }
                }

                return response;
            });
        }

        [HttpPost]
        [Route("update")]
        public HttpResponseMessage Update(HttpRequestMessage request, ProductViewModel product)
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
                    Product newProduct = _productsRepository.GetSingle(product.ID);
                    newProduct.UpdateProduct(product);

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
                List<Product> products = null;
                int totalProducts = new int();

                if (!string.IsNullOrEmpty(filter))
                {
                    filter = filter.Trim().ToLower();

                    products = _productsRepository.FindBy(c => c.Code.ToLower().Contains(filter) || c.Name.ToLower().Contains(filter))
                        .OrderBy(c => c.ID)
                        .Skip(currentPage * currentPageSize)
                        .Take(currentPageSize)
                        .ToList();

                    totalProducts = _productsRepository.GetAll()
                        .Where(c => c.Code.ToLower().Contains(filter) || c.Name.ToLower().Contains(filter))
                        .Count();
                }
                else
                {
                    products = _productsRepository.GetAll()
                        .OrderBy(c => c.ID)
                        .Skip(currentPage * currentPageSize)
                        .Take(currentPageSize)
                    .ToList();

                    totalProducts = _productsRepository.GetAll().Count();
                }

                IEnumerable<ProductViewModel> productsViewModels = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(products);

                PaginationSet<ProductViewModel> pagedSet = new PaginationSet<ProductViewModel>()
                {
                    Page = currentPage,
                    TotalCount = totalProducts,
                    TotalPages = (int)Math.Ceiling((decimal)totalProducts / currentPageSize),
                    Items = productsViewModels
                };

                response = request.CreateResponse(HttpStatusCode.OK, pagedSet);
                return response;
            });
        }
    }

    #endregion
}