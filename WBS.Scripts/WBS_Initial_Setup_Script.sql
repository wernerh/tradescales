CREATE TABLE [dbo].[Ticket](
	[TicketId] [int] IDENTITY(1,1) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[TicketNumber] [varchar](8) NOT NULL,
	[CompanyId] [int] NOT NULL,
	[DriverId] [int] NOT NULL,
	[ProductId] [int] NULL,
	[Comment] [varchar](150) NULL,
 CONSTRAINT [PK_Ticket] PRIMARY KEY CLUSTERED 
(
	[TicketId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[Company](
	[CompanyId] [int] IDENTITY(1,1) NOT NULL,
	[CompanyCode] [varchar](8) NOT NULL,
	[CompanyName] [varchar](15) NOT NULL,	
 CONSTRAINT [PK_Company] PRIMARY KEY CLUSTERED 
(
	[CompanyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[Driver](
	[DriverId] [int] IDENTITY(1,1) NOT NULL,
	[CompanyId] [int] NOT NULL,
	[DriverCode] [varchar](8) NOT NULL,
	[VehicleReg] [varchar](15) NOT NULL,
	[DriverName] [varchar](50) NOT NULL,
	[DriverSurname] [varchar](50) NOT NULL,
	[DriverCellNumber] [varchar](50) NOT NULL,	
 CONSTRAINT [PK_Driver] PRIMARY KEY CLUSTERED 
(
	[DriverId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[Product](
	[ProductId] [int] IDENTITY(1,1) NOT NULL,
	[ProductCode] [varchar](8) NOT NULL,
	[ProductName] [varchar](50) NOT NULL,
	[ProductDescription] [varchar](50) NULL,
	[Cost] [decimal] NULL,
	[UnitOfMeasure] [varchar](50) NULL,	
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[Weight](
	[WeightId] [int] IDENTITY(1,1) NOT NULL,
	[TicketId] [int] NOT NULL,
	[WeighedDate] [datetime] NOT NULL,
	[Weight] [float] NOT NULL,
	[LoadType] [varchar](15) NULL,
	[Description] [varchar](15) NULL,
 CONSTRAINT [PK_Weight] PRIMARY KEY CLUSTERED 
(
	[WeightId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[Ticket]  WITH CHECK ADD  CONSTRAINT [FK_Ticket_Company] FOREIGN KEY([CompanyId]) REFERENCES [dbo].[Company] ([CompanyId])
ALTER TABLE [dbo].[Ticket]  WITH CHECK ADD  CONSTRAINT [FK_Ticket_Driver] FOREIGN KEY([DriverId]) REFERENCES [dbo].[Driver] ([DriverId])
ALTER TABLE [dbo].[Ticket]  WITH CHECK ADD  CONSTRAINT [FK_Ticket_Product] FOREIGN KEY([ProductId]) REFERENCES [dbo].[Product] ([ProductId])

ALTER TABLE [dbo].[Weight]  WITH CHECK ADD  CONSTRAINT [FK_Weight_Ticket] FOREIGN KEY([TicketId]) REFERENCES [dbo].[Ticket] ([TicketId])

ALTER TABLE [dbo].[Driver]  WITH CHECK ADD  CONSTRAINT [FK_Driver_Company] FOREIGN KEY([CompanyId]) REFERENCES [dbo].[Company] ([CompanyId])



