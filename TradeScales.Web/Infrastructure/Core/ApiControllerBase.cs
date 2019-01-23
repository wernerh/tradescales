using System;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TradeScales.Data.Infrastructure;
using TradeScales.Data.Repositories;
using TradeScales.Entities;

namespace TradeScales.Web.Infrastructure.Core
{
    public class ApiControllerBase : ApiController
    {
        protected readonly IEntityBaseRepository<Error> _errorsRepository;
        protected readonly IUnitOfWork _unitOfWork;

        public ApiControllerBase(IEntityBaseRepository<Error> errorsRepository, IUnitOfWork unitOfWork)
        {
            _errorsRepository = errorsRepository;
            _unitOfWork = unitOfWork;
        }

        public ApiControllerBase(IDataRepositoryFactory dataRepositoryFactory, IEntityBaseRepository<Error> errorsRepository, IUnitOfWork unitOfWork)
        {
            _errorsRepository = errorsRepository;
            _unitOfWork = unitOfWork;
        }

        protected HttpResponseMessage CreateHttpResponse(HttpRequestMessage request, Func<HttpResponseMessage> function)
        {
            HttpResponseMessage response = null;

            try
            {
                response = function.Invoke();
            }
            catch (DbUpdateException exception)
            {
                LogError(exception);
                response = request.CreateResponse(HttpStatusCode.BadRequest, exception.InnerException.Message);
            }
            catch (Exception exception)
            {
                LogError(exception);
                response = request.CreateResponse(HttpStatusCode.InternalServerError, exception.Message);
            }

            return response;
        }
        private void LogError(Exception exception)
        {
            try
            {
                Error _error = new Error()
                {
                    Message = exception.Message,
                    StackTrace = exception.StackTrace,
                    DateCreated = DateTime.Now
                };

                _errorsRepository.Add(_error);
                _unitOfWork.Commit();
            }
            catch { }
        }
    }
}