using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TradeScales.Data.Infrastructure;
using TradeScales.Data.Repositories;
using TradeScales.Entities;

namespace TradeScales.Web.Infrastructure.Core
{
    public class ApiControllerBaseExtended : ApiController
    {
        protected List<Type> _requiredRepositories;

        protected readonly IDataRepositoryFactory _dataRepositoryFactory;
        protected IEntityBaseRepository<Error> _errorsRepository;
        protected IEntityBaseRepository<Customer> _companyRepository;
        protected IEntityBaseRepository<Driver> _driverRepository;
        protected IEntityBaseRepository<Product> _productRepository;
        protected IEntityBaseRepository<Ticket> _ticketRepository;
        protected IUnitOfWork _unitOfWork;

        private HttpRequestMessage RequestMessage;

        public ApiControllerBaseExtended(IDataRepositoryFactory dataRepositoryFactory, IUnitOfWork unitOfWork)
        {
            _dataRepositoryFactory = dataRepositoryFactory;
            _unitOfWork = unitOfWork;
        }

        protected HttpResponseMessage CreateHttpResponse(HttpRequestMessage request, List<Type> repos, Func<HttpResponseMessage> function)
        {
            HttpResponseMessage response = null;

            try
            {
                RequestMessage = request;
                InitRepositories(repos);
                response = function.Invoke();
            }
            catch (DbUpdateException ex)
            {
                LogError(ex);
                response = request.CreateResponse(HttpStatusCode.BadRequest, ex.InnerException.Message);
            }
            catch (Exception ex)
            {
                LogError(ex);
                response = request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

            return response;
        }

        private void InitRepositories(List<Type> entities)
        {
            _errorsRepository = _dataRepositoryFactory.GetDataRepository<Error>(RequestMessage);

            if (entities.Any(e => e.FullName == typeof(Customer).FullName))
            {
                _companyRepository = _dataRepositoryFactory.GetDataRepository<Customer>(RequestMessage);
            }

            if (entities.Any(e => e.FullName == typeof(Driver).FullName))
            {
                _driverRepository = _dataRepositoryFactory.GetDataRepository<Driver>(RequestMessage);
            }

            if (entities.Any(e => e.FullName == typeof(Product).FullName))
            {
                _productRepository = _dataRepositoryFactory.GetDataRepository<Product>(RequestMessage);
            }

            if (entities.Any(e => e.FullName == typeof(Ticket).FullName))
            {
                _ticketRepository = _dataRepositoryFactory.GetDataRepository<Ticket>(RequestMessage);
            }        
        }

        private void LogError(Exception ex)
        {
            try
            {
                Error _error = new Error()
                {
                    Message = ex.Message,
                    StackTrace = ex.StackTrace,
                    DateCreated = DateTime.Now
                };

                _errorsRepository.Add(_error);
                _unitOfWork.Commit();
            }
            catch { }
        }
    }
}