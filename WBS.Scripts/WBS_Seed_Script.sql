SET IDENTITY_INSERT [dbo].[Company] ON 

INSERT [dbo].[Company] ([CompanyId], [CompanyCode], [CompanyName]) VALUES (1, 'COM001', 'Company 1')
INSERT [dbo].[Company] ([CompanyId], [CompanyCode], [CompanyName]) VALUES (2, 'COM002', 'Company 2')
INSERT [dbo].[Company] ([CompanyId], [CompanyCode], [CompanyName]) VALUES (3, 'COM003', 'Company 3')

SET IDENTITY_INSERT [dbo].[Company] OFF

SET IDENTITY_INSERT [dbo].[Driver] ON 

INSERT [dbo].[Driver] ([DriverId], [CompanyId], [DriverCode], [VehicleReg], [DriverName], [DriverSurname], [DriverCellNumber] ) VALUES (1,1, 'DRI001', 'CA 123-456', 'Driver Name 1', 'Driver Surname 1', '0761234567')
INSERT [dbo].[Driver] ([DriverId], [CompanyId], [DriverCode], [VehicleReg], [DriverName], [DriverSurname], [DriverCellNumber] ) VALUES (2,2, 'DRI002', 'CA 789-101', 'Driver Name 2', 'Driver Surname 2', '0761234567')
INSERT [dbo].[Driver] ([DriverId], [CompanyId], [DriverCode], [VehicleReg], [DriverName], [DriverSurname], [DriverCellNumber] ) VALUES (3,2, 'DRI003', 'CA 112-134', 'Driver Name 3', 'Driver Surname 3', '0761234567')

SET IDENTITY_INSERT [dbo].[Driver] OFF

SET IDENTITY_INSERT [dbo].[Product] ON 

INSERT [dbo].[Product] ([ProductId], [ProductCode], [ProductName], [ProductDescription], [Cost], [UnitOfMeasure]) VALUES (1,'PRO001', 'Product 1', 'Product 1 Description', 9.99, 'kg')
INSERT [dbo].[Product] ([ProductId], [ProductCode], [ProductName], [ProductDescription], [Cost], [UnitOfMeasure]) VALUES (2,'PRO002', 'Product 2', 'Product 2 Description', 9.99, 'kg')
INSERT [dbo].[Product] ([ProductId], [ProductCode], [ProductName], [ProductDescription], [Cost], [UnitOfMeasure]) VALUES (3,'PRO003', 'Product 3', 'Product 3 Description', 9.99, 'kg')

SET IDENTITY_INSERT [dbo].[Product] OFF