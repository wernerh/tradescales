using AutoMapper;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WBS.Data.Infrastructure;
using WBS.Data.Repositories;
using WBS.Entities;
using WBS.Web.Infrastructure.Core;
using WBS.Web.Models;

namespace WBS.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    [RoutePrefix("api/statusmessages")]
    public class StatusMessagesController : ApiControllerBase
    {
        #region Fields
        private readonly IEntityBaseRepository<StatusMessage> _statusMessagesRepository;
        #endregion

        #region Constructor
        public StatusMessagesController(IEntityBaseRepository<StatusMessage> statusMessagesRepository, IEntityBaseRepository<Error> _errorsRepository, IUnitOfWork _unitOfWork)
          : base(_errorsRepository, _unitOfWork)
        {
            _statusMessagesRepository = statusMessagesRepository;
        }
        #endregion

        #region Public Methods
        [AllowAnonymous]
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                var messages = _statusMessagesRepository.GetAll();
                return request.CreateResponse(HttpStatusCode.OK, Mapper.Map<IEnumerable<StatusMessage>, IEnumerable<StatusMessageViewModel>>(messages));
            });
        }
        #endregion
    }
}