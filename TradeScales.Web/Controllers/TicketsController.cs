using AutoMapper;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Hosting;
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
    [RoutePrefix("api/tickets")]
    public class TicketsController : ApiControllerBase
    {
        #region Fields

        private readonly IEntityBaseRepository<Ticket> _ticketsRepository;

        #endregion

        #region Constructor

        public TicketsController(IEntityBaseRepository<Ticket> ticketsRepository, IEntityBaseRepository<Error> _errorsRepository, IUnitOfWork _unitOfWork)
          : base(_errorsRepository, _unitOfWork)
        {
            _ticketsRepository = ticketsRepository;
        }

        #endregion

        #region Public Methods

        [AllowAnonymous]
        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            return CreateHttpResponse(request, () =>
            {
                var tickets = _ticketsRepository.GetAll();
                return request.CreateResponse(HttpStatusCode.OK, Mapper.Map<IEnumerable<Ticket>, IEnumerable<TicketViewModel>>(tickets));
            });
        }

        public HttpResponseMessage Get(HttpRequestMessage request, string filter)
        {
            filter = filter.ToLower().Trim();

            return CreateHttpResponse(request, () =>
            {
                var tickets = _ticketsRepository.GetAll()
                    .Where(t => t.TicketNumber.ToLower().Contains(filter));

                return request.CreateResponse(HttpStatusCode.OK, Mapper.Map<IEnumerable<Ticket>, IEnumerable<TicketViewModel>>(tickets));
            });
        }

        [Route("details/{id:int}")]
        public HttpResponseMessage Get(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                var ticket = _ticketsRepository.GetSingle(id);
                return request.CreateResponse(HttpStatusCode.OK, Mapper.Map<Ticket, TicketViewModel>(ticket));
            });
        }

        [HttpGet]
        [Route("loadnextticketnumber")]
        public HttpResponseMessage LoadNextTicketNumber(HttpRequestMessage request)
        {
            var numberOfTickets = _ticketsRepository.GetAll().Count();
            var newTicketNumber = $"{(numberOfTickets + 1).ToString().PadLeft(6, '0')}";

            return CreateHttpResponse(request, () =>
            {
                return request.CreateResponse(HttpStatusCode.OK, newTicketNumber);
            });
        }

        [HttpPost]
        [Route("add")]
        public HttpResponseMessage Add(HttpRequestMessage request, TicketViewModel ticket)
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
                    if (_ticketsRepository.TicketExists(ticket.TicketNumber))
                    {
                        ModelState.AddModelError("Invalid ticket", "Ticket already exists");
                        response = request.CreateResponse(HttpStatusCode.BadRequest,
                        ModelState.Keys.SelectMany(k => ModelState[k].Errors)
                              .Select(m => m.ErrorMessage).ToArray());
                    }
                    else
                    {
                        Ticket newTicket = new Ticket();
                        newTicket.UpdateTicket(ticket);

                        _ticketsRepository.Add(newTicket);
                        _unitOfWork.Commit();

                        // Update view model
                        ticket = Mapper.Map<Ticket, TicketViewModel>(newTicket);
                        response = request.CreateResponse(HttpStatusCode.Created, ticket);
                    }
                }
                return response;
            });
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("saveweight")]
        public string SaveWeight([FromBody]Weight test)
        {
            return "Test";
        }

        [HttpPost]
        [Route("update")]
        public HttpResponseMessage Update(HttpRequestMessage request, TicketViewModel ticket)
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
                    Ticket _ticket = _ticketsRepository.GetSingle(ticket.ID);
                    _ticket.UpdateTicket(ticket);
                    _unitOfWork.Commit();

                    response = request.CreateResponse(HttpStatusCode.OK);
                }

                return response;
            });
        }

        [HttpPost]
        [Route("generatePdf/{ticketId:int}")]
        public HttpResponseMessage GeneratePdf(HttpRequestMessage request, int ticketId)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                var ticket = _ticketsRepository.GetSingle(ticketId);
                if (ticket == null)
                {
                    response = request.CreateErrorResponse(HttpStatusCode.NotFound, "Invalid ticket");
                }
                else
                {
                    ticket.TimeOut = DateTime.Now;
                    ticket.Status = "Complete";
                    _unitOfWork.Commit();

                    var filePath = $"/Content/files/weighbridgecertificate.pdf";
                    CreatePdf(filePath, ticket);

                    response = request.CreateResponse(HttpStatusCode.OK, filePath);
                }

                return response;
            });
        }
        #endregion

        public void CreatePdf(string filePath, Ticket ticket)
        {
            // Create document
            Document document = new Document(PageSize.A4);
            var output = new FileStream(HostingEnvironment.MapPath(filePath), FileMode.Create);
            var writer = PdfWriter.GetInstance(document, output);
            document.Open();

            // Get logo path
            var logoPath = HostingEnvironment.MapPath("/Content/images/tradescales.png");

            // Read in the contents of the Receipt.htm file...
            string contents = File.ReadAllText(HostingEnvironment.MapPath("/Content/templates/WeighbridgeTicketTemplate.html"));

            // Replace the placeholders with the user-specified text
            contents = contents.Replace("[IMAGESOURCE]", logoPath);
            contents = contents.Replace("[TICKETNUMBER]", ticket.TicketNumber);
            contents = contents.Replace("[ORDERNUMBER]", ticket.OrderNumber);
            contents = contents.Replace("[DELIVERYNUMBER]", ticket.DeliveryNumber);
            contents = contents.Replace("[TIMEIN]", ticket.TimeIn.ToString());
            contents = contents.Replace("[TIMEOUT]", ticket.TimeOut.ToString());

            contents = contents.Replace("[CUSTOMERCODE]", ticket.Customer.Code);
            contents = contents.Replace("[CUSTOMERNAME]", ticket.Customer.Name);
            contents = contents.Replace("[HAULIERCODE]", ticket.Haulier.Code);
            contents = contents.Replace("[HAULIERNAME]", ticket.Haulier.Name);
            contents = contents.Replace("[DRIVERCODE]", ticket.Driver.Code);
            contents = contents.Replace("[DRIVERFIRSTNAME]", ticket.Driver.FirstName);
            contents = contents.Replace("[DRIVERLASTNAME]", ticket.Driver.LastName);
            contents = contents.Replace("[DRIVERMOBILE]", ticket.Driver.Mobile);
            contents = contents.Replace("[VEHICLEREGISTRATION]", ticket.Driver.VehicleRegistration);
            contents = contents.Replace("[DESTINATIONCODE]", ticket.Destination.Code);
            contents = contents.Replace("[DESTINATIONNAME]", ticket.Destination.Name);
            contents = contents.Replace("[PODUCTCODE]", ticket.Product.Code);
            contents = contents.Replace("[PRODUCTNAME]", ticket.Product.Name);

            contents = contents.Replace("[TARREWEIGHT]", $"{ticket.TareWeight} kg");
            contents = contents.Replace("[GROSSWEIGHT]", $"{ticket.GrossWeight} kg");
            contents = contents.Replace("[NETTWEIGHT]", $"{ticket.NettWeight} kg");

            StringReader sr = new StringReader(contents);

            // Parse the HTML string into a collection of elements...
            XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, sr);
            document.Close();
        }
    }

    public class Weight
    {
        public DateTime Time { get; set; }
        public string Reading { get; set; }
    }
}