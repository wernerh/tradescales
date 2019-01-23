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
    [RoutePrefix("api/users")]
    public class UsersController : ApiControllerBase
    {
        #region Fields

        private readonly IEntityBaseRepository<User> _usersRepository;

        #endregion

        #region Constructor

        public UsersController(IEntityBaseRepository<User> usersRepository, IEntityBaseRepository<Error> _errorsRepository, IUnitOfWork _unitOfWork)
            : base(_errorsRepository, _unitOfWork)
        {
            _usersRepository = usersRepository;
        }

        #endregion

        #region Public Methods

        public HttpResponseMessage Get(HttpRequestMessage request, string filter)
        {
            filter = filter.ToLower().Trim();

            return CreateHttpResponse(request, () =>
            {
                var users = _usersRepository.GetAll()
                    .Where(u => u.Email.ToLower().Contains(filter) || u.Username.ToLower().Contains(filter));

                return request.CreateResponse(HttpStatusCode.OK, Mapper.Map<IEnumerable<User>, IEnumerable<UserViewModel>>(users));
            });
        }

        [Route("details/{id:int}")]
        public HttpResponseMessage Get(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                var user = _usersRepository.GetSingle(id);

                return request.CreateResponse(HttpStatusCode.OK, Mapper.Map<User, UserViewModel>(user));
            });
        }

        [HttpPost]
        [Route("add")]
        public HttpResponseMessage Add(HttpRequestMessage request, UserViewModel user)
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
                    if (_usersRepository.UserExists(user.Email, user.Username))
                    {
                        ModelState.AddModelError("Invalid user", "Email or username already exists");
                        response = request.CreateResponse(HttpStatusCode.BadRequest,
                        ModelState.Keys.SelectMany(k => ModelState[k].Errors)
                              .Select(m => m.ErrorMessage).ToArray());
                    }
                    else
                    {
                        User newUser = new User();
                        newUser.UpdateUser(user);

                        _usersRepository.Add(newUser);
                        _unitOfWork.Commit();

                        // Update view model
                        user = Mapper.Map<User, UserViewModel>(newUser);
                        response = request.CreateResponse(HttpStatusCode.Created, user);
                    }
                }

                return response;
            });
        }

        [HttpPost]
        [Route("update")]
        public HttpResponseMessage Update(HttpRequestMessage request, UserViewModel user)
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
                    User newUser = _usersRepository.GetSingle(user.ID);
                    newUser.UpdateUser(user);

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
                List<User> users = null;
                int totalUsers = new int();

                if (!string.IsNullOrEmpty(filter))
                {
                    filter = filter.Trim().ToLower();

                    users = _usersRepository.FindBy(c => c.Username.ToLower().Contains(filter) || c.Email.ToLower().Contains(filter))
                        .OrderBy(c => c.ID)
                        .Skip(currentPage * currentPageSize)
                        .Take(currentPageSize)
                        .ToList();

                    totalUsers = _usersRepository.GetAll()
                        .Where(c => c.Username.ToLower().Contains(filter) ||
                            c.Email.ToLower().Contains(filter))
                        .Count();
                }
                else
                {
                    users = _usersRepository.GetAll()
                        .OrderBy(c => c.ID)
                        .Skip(currentPage * currentPageSize)
                        .Take(currentPageSize)
                    .ToList();

                    totalUsers = _usersRepository.GetAll().Count();
                }

                IEnumerable<UserViewModel> usersViewModel = Mapper.Map<IEnumerable<User>, IEnumerable<UserViewModel>>(users);

                PaginationSet<UserViewModel> pagedSet = new PaginationSet<UserViewModel>()
                {
                    Page = currentPage,
                    TotalCount = totalUsers,
                    TotalPages = (int)Math.Ceiling((decimal)totalUsers / currentPageSize),
                    Items = usersViewModel
                };

                response = request.CreateResponse(HttpStatusCode.OK, pagedSet);
                return response;
            });
        }

        #endregion
    }
}