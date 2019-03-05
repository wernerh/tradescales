﻿using AutoMapper;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using TradeScales.Data.Infrastructure;
using TradeScales.Data.Repositories;
using TradeScales.Entities;
using TradeScales.Wpf.Model;
using TradeScales.Wpf.Properties;
using TradeScales.Wpf.Resources.Services.Interfaces;
using TradeScales.Wpf.ViewModel.EntityViewModels;
using MVVMRelayCommand = TradeScales.Wpf.Model.RelayCommand;

namespace TradeScales.Wpf.ViewModel.Tools
{
   
    public class ToolThreeViewModel : ToolViewModel
    {

        #region Fields

        public const string ToolContentID = "ToolThree";

        private static IMessageBoxService _MessageBoxService = ServiceLocator.Instance.GetService<IMessageBoxService>();

        private IEntityBaseRepository<Ticket> _ticketsRepository = BootStrapper.Resolve<IEntityBaseRepository<Ticket>>();
        private IEntityBaseRepository<Haulier> _hauliersRepository = BootStrapper.Resolve<IEntityBaseRepository<Haulier>>();
        private IEntityBaseRepository<Customer> _customersRepository = BootStrapper.Resolve<IEntityBaseRepository<Customer>>();
        private IEntityBaseRepository<Product> _productsRepository = BootStrapper.Resolve<IEntityBaseRepository<Product>>();
        private IEntityBaseRepository<Destination> _destinationsRepository = BootStrapper.Resolve<IEntityBaseRepository<Destination>>();
        private IEntityBaseRepository<Driver> _driversRepository = BootStrapper.Resolve<IEntityBaseRepository<Driver>>();
        private IEntityBaseRepository<Vehicle> _vehiclesRepository = BootStrapper.Resolve<IEntityBaseRepository<Vehicle>>();
        private IUnitOfWork _unitOfWork = BootStrapper.Resolve<IUnitOfWork>();

        #endregion

        #region Constructor

        public ToolThreeViewModel()
            : base("Ticket List Filter")
        {
            ContentID = ToolContentID;
            LoadDropdowns();
            SetDefaultValues();
        }

        #endregion

        #region Properties

        private DateTime _DateFrom;
        public DateTime DateFrom
        {
            get { return _DateFrom; }
            set
            {
                if (_DateFrom != value)
                {
                    _DateFrom = value;
                    OnPropertyChanged("DateFrom");
                }
            }
        }

        private DateTime _DateTo;
        public DateTime DateTo
        {
            get { return _DateTo; }
            set
            {
                if (_DateTo != value)
                {
                    _DateTo = value;
                    OnPropertyChanged("DateTo");
                }
            }
        }

        public IEnumerable<HaulierViewModel> Hauliers { get; set; }

        private HaulierViewModel _SelectedHaulier;
        public HaulierViewModel SelectedHaulier
        {
            get { return _SelectedHaulier; }
            set
            {
                _SelectedHaulier = value;
                OnPropertyChanged("SelectedHaulier");
            }
        }

        public IEnumerable<CustomerViewModel> Customers { get; set; }

        private CustomerViewModel _SelectedCustomer;
        public CustomerViewModel SelectedCustomer
        {
            get { return _SelectedCustomer; }
            set
            {
                _SelectedCustomer = value;
                OnPropertyChanged("SelectedCustomer");
            }
        }

        public IEnumerable<DestinationViewModel> Destinations { get; set; }

        private DestinationViewModel _SelectedDestination;
        public DestinationViewModel SelectedDestination
        {
            get { return _SelectedDestination; }
            set
            {
                _SelectedDestination = value;
                OnPropertyChanged("SelectedDestination");
            }
        }

        public IEnumerable<ProductViewModel> Products { get; set; }

        private ProductViewModel _SelectedProduct;
        public ProductViewModel SelectedProduct
        {
            get { return _SelectedProduct; }
            set
            {
                _SelectedProduct = value;
                OnPropertyChanged("SelectedProduct");
            }
        }

        public IEnumerable<DriverViewModel> Drivers { get; set; }

        private DriverViewModel _SelectedDriver;
        public DriverViewModel SelectedDriver
        {
            get { return _SelectedDriver; }
            set
            {
                _SelectedDriver = value;
                OnPropertyChanged("SelectedDriver");
            }
        }

        public IEnumerable<VehicleViewModel> Vehicles { get; set; }

        private VehicleViewModel _SelectedVehicle;
        public VehicleViewModel SelectedVehicle
        {
            get { return _SelectedVehicle; }
            set
            {
                _SelectedVehicle = value;
                OnPropertyChanged("SelectedVehicle");
            }
        }

        #endregion

        #region Commands

        private ICommand _FilterTicketListCommand;    
        public ICommand FilterTicketListCommand
        {
            get
            {
                if (_FilterTicketListCommand == null)
                {
                    _FilterTicketListCommand = new MVVMRelayCommand(
                        execute =>
                        {
                            try
                            {
                                FilterTicketList();
                            }
                            catch (Exception ex)
                            {
                                MainViewModel.This.ShowExceptionMessageBox(ex);
                            }
                        });
                }
                return _FilterTicketListCommand;
            }
        }

        #endregion

        #region Private Methods

        private void LoadDropdowns()
        {
            var hauliers = new List<HaulierViewModel>();
            hauliers.Add(new HaulierViewModel() { Name = "All", ID = -1 });
            hauliers.AddRange(Mapper.Map<IEnumerable<Haulier>, IEnumerable<HaulierViewModel>>(_hauliersRepository.GetAll()));
            Hauliers = hauliers;


            Hauliers = hauliers;

            var customers = new List<CustomerViewModel>();
            customers.Add(new CustomerViewModel() { Name = "All", ID = -1 });
            customers.AddRange(Mapper.Map<IEnumerable<Customer>, IEnumerable<CustomerViewModel>>(_customersRepository.GetAll().OrderBy(x => x.Name)));
            Customers = customers;

            var products = new List<ProductViewModel>();
            products.Add(new ProductViewModel() { Name = "All", ID = -1 });
            products.AddRange(Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(_productsRepository.GetAll().OrderBy(x => x.Name)));
            Products = products;

            var destinations = new List<DestinationViewModel>();
            destinations.Add(new DestinationViewModel() { Name = "All", ID = -1 });
            destinations.AddRange(Mapper.Map<IEnumerable<Destination>, IEnumerable<DestinationViewModel>>(_destinationsRepository.GetAll().OrderBy(x => x.Name)));
            Destinations = destinations;

            var drivers = new List<DriverViewModel>();
            drivers.Add(new DriverViewModel() { FirstName = "All", ID = -1 });
            drivers.AddRange(Mapper.Map<IEnumerable<Driver>, IEnumerable<DriverViewModel>>(_driversRepository.GetAll()).OrderBy(d => d.FirstName));
            Drivers = drivers;

            var vehicles = new List<VehicleViewModel>();
            vehicles.Add(new VehicleViewModel() { Registration = "All", ID = -1 });
            vehicles.AddRange(Mapper.Map<IEnumerable<Vehicle>, IEnumerable<VehicleViewModel>>(_vehiclesRepository.GetAll()).OrderBy(x => x.Registration));
            Vehicles = vehicles;
        }

        private void SetDefaultValues()
        {
            DateFrom = DateTime.Now;
            DateTo = DateTime.Now.AddDays(1);

            SelectedHaulier = Hauliers.First();
            SelectedCustomer = Customers.First();
            SelectedProduct = Products.First();
            SelectedDestination = Destinations.First();
            SelectedDriver = Drivers.First();
            SelectedVehicle = Vehicles.First();
        }  

        private void FilterTicketList()
        {
            MainViewModel.This.TicketList.FilterTickets(DateFrom, DateTo, SelectedHaulier.ID, SelectedCustomer.ID, SelectedDestination.ID, SelectedProduct.ID, SelectedDriver.ID, SelectedVehicle.ID);  
        }
     
        #endregion

    }
}
