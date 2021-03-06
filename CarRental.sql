USE [CarRental]
GO
/****** Object:  StoredProcedure [dbo].[GetAvailableCars]    Script Date: 23/08/2015 00:39:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetAvailableCars] 
	@isAutomatic int,
	@year int,
	@manufactor int,
	@model nvarchar(30),
	@fromDate datetime,
	@toDate datetime,
	@keywords nvarchar(50)

AS
BEGIN
	DECLARE @@isAutomatic bit
	DECLARE @@transmittion nvarchar(8)
	DECLARE @@n_year int
	DECLARE @@manufactor_id int
	DECLARE @@manufactor_name nvarchar(50)
	--DECLARE @@model_id int
	--DECLARE @@keyword nvarchar(500)

	IF @isAutomatic < 0
		SELECT @@isAutomatic = NULL
	ELSE
		--BEGIN
			SELECT @@isAutomatic = @isAutomatic
		--	IF @@isAutomatic = 0
		--		SELECT @@transmittion = 'Manual'
		--	ELSE
		--		SELECT @@transmittion = 'Automatic'
		--END
	--SELECT 
	--	@@IsAutomatic = CASE
	--		WHEN @IsAutomatic < 0
	--			THEN null
	--		WHEN @IsAutomatic = 0
	--			THEN 0
	--		ELSE 1
	--END

	IF @year <= 0
		SELECT @@n_year = NULL
	ELSE		
		SELECT @@n_year = @year

	IF @manufactor <= 0
		SELECT @@manufactor_id = NULL
	ELSE
		BEGIN		
			SELECT @@manufactor_id = @manufactor
			SELECT @@manufactor_name = Name FROM Manufactors WHERE ManufactorId = @@manufactor_id
		END
	--IF @model <= 0
	--	SELECT @@model_id = NULL
	--ELSE		
	--	SELECT @@model_id = @model

	--SELECT @@keyword = '%' + @keywords + '%'
	SELECT f.FleetId,
		f.LicencePlate,
		v.Year,
		v.Transmission as Auto,
		m.Name as Manufactor,
		v.Model,
		f.Kilometrage,
		f.Image,
		v.DailyRentalRate,
		v.PenaltyDailyRate
		--CHARINDEX(UPPER(m.Name), UPPER(@keywords)) kkk,
		--@keywords kk1

	FROM [CarRental].[dbo].[Fleets] f 
	JOIN [CarRental].[dbo].[Vehicles] v ON (f.TypeId = v.TypeId) 
	JOIN [CarRental].[dbo].[Manufactors] m ON (m.ManufactorId = v.ManufactorId)
	--JOIN [CarRental].[dbo].[Rentals] r ON (f.FleetId = r.FleetId)
	WHERE (v.ManufactorId = @@manufactor_id or @@manufactor_id IS NULL)
	and (v.Model = @model or @model = '0')
	and [dbo].[IsCarOccupied](f.FleetId, @fromDate, @toDate) > 0
	--and @fromDate not between r.StartRentalDate and ISNULL(r.ActualEndRental, r.EndRentalDate)
	--and @toDate not between r.StartRentalDate and ISNULL(r.ActualEndRental, r.EndRentalDate)
	--and r.StartRentalDate not between @fromDate and @toDate
	--and ISNULL(r.ActualEndRental, r.EndRentalDate) not between @fromDate and @toDate
	and (v.Year = @@n_year or @@n_year IS NULL)
	and (v.Transmission = @@IsAutomatic or @@IsAutomatic IS NULL)
	and (CHARINDEX(UPPER(m.Name), UPPER(@keywords)) > 0 or CHARINDEX(UPPER(@keywords), UPPER(m.Name)) > 0 or
		 CHARINDEX(UPPER(v.Model), UPPER(@keywords)) > 0 or CHARINDEX(UPPER(@keywords), UPPER(v.Model)) > 0 or 
		 CHARINDEX(UPPER(v.Year), UPPER(@keywords)) > 0 or 
		 CHARINDEX(IIF(v.Transmission = 1, 'AUTO', 'MANUAL'), UPPER(@keywords)) > 0 or 
		 LEN(@keywords) = 0)

	ORDER BY Manufactor asc, Model asc
END
GO
/****** Object:  StoredProcedure [dbo].[GetCarForRent]    Script Date: 23/08/2015 00:39:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetCarForRent] 
	@id int

AS
BEGIN
	SELECT f.FleetId,
		f.LicencePlate,
		v.Year,
		v.Transmission as Auto,
		m.Name as Manufactor,
		v.Model,
		f.Kilometrage,
		f.Image,
		v.DailyRentalRate,
		v.PenaltyDailyRate

	FROM [CarRental].[dbo].[Fleets] f 
	JOIN [CarRental].[dbo].[Vehicles] v ON (f.TypeId = v.TypeId) 
	JOIN [CarRental].[dbo].[Manufactors] m ON (m.ManufactorId = v.ManufactorId)
	--JOIN [CarRental].[dbo].[Rentals] r ON (f.FleetId = r.FleetId)
	WHERE f.FleetId = @id 
	ORDER BY Year desc, Manufactor asc, Model asc
END

GO
/****** Object:  StoredProcedure [dbo].[GetUnlinkedVehicles]    Script Date: 23/08/2015 00:39:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[GetUnlinkedVehicles] 
--	@id int

AS
BEGIN
	SELECT v.TypeId as ID,
		m.Name + ' ' + v.Model as Name,
		v.DailyRentalRate,
		v.PenaltyDailyRate

  	FROM [CarRental].[dbo].[Vehicles] v  
	JOIN [CarRental].[dbo].[Manufactors] m ON (m.ManufactorId = v.ManufactorId)
	LEFT JOIN [CarRental].[dbo].[Fleets] f ON (f.TypeId = v.TypeId)
	WHERE f.FleetId IS NULL
	ORDER BY Name asc
END


GO
/****** Object:  StoredProcedure [dbo].[InsertOrder]    Script Date: 23/08/2015 00:39:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[InsertOrder] 
	@carId int,
	@user nvarchar(50),
	@fromDate datetime,
	@toDate datetime

AS
BEGIN
	
	DECLARE @@userId int
	
	SELECT @@userId = UserId FROM Users WHERE UserName = @user

	INSERT INTO dbo.Rentals (
            FleetId,
            UserId,
            StartRentalDate,
            EndRentalDate                  
          ) 
     VALUES 
          ( 
            @carId,
            @@userId,
            @fromDate,
            @toDate                  
          ) 
	
	select MAX(RentalId)
	FROM [CarRental].[dbo].[Rentals] 

END

GO
/****** Object:  UserDefinedFunction [dbo].[IsCarOccupied]    Script Date: 23/08/2015 00:39:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[IsCarOccupied](@FleetId int, @FromDate date, @ToDate date)
	RETURNS bit
AS 
-- Returns the stock level for the product.
BEGIN
    DECLARE @ret int;
    SELECT @ret = count(RentalId) 
    FROM [dbo].[Rentals]
    WHERE FleetId = @FleetId
    and (@FromDate between StartRentalDate and ISNULL(ActualEndRental, EndRentalDate)
	or @ToDate between StartRentalDate and ISNULL(ActualEndRental, EndRentalDate));

    IF (@ret > 0) 
		RETURN 0;
    RETURN 1;
END;


GO
/****** Object:  Table [dbo].[Fleets]    Script Date: 23/08/2015 00:39:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Fleets](
	[FleetId] [int] IDENTITY(1,1) NOT NULL,
	[LicencePlate] [nvarchar](20) NOT NULL,
	[TypeId] [int] NOT NULL,
	[Kilometrage] [int] NOT NULL,
	[Image] [nvarchar](100) NOT NULL,
	[ReadyToUse] [bit] NOT NULL,
 CONSTRAINT [PK_Fleets] PRIMARY KEY CLUSTERED 
(
	[FleetId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Manufactors]    Script Date: 23/08/2015 00:39:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Manufactors](
	[ManufactorId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Manufactors] PRIMARY KEY CLUSTERED 
(
	[ManufactorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Rentals]    Script Date: 23/08/2015 00:39:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rentals](
	[RentalId] [int] IDENTITY(100000,1) NOT NULL,
	[FleetId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[StartRentalDate] [date] NOT NULL,
	[EndRentalDate] [date] NOT NULL,
	[ActualEndRental] [date] NULL,
 CONSTRAINT [PK_Rentals] PRIMARY KEY CLUSTERED 
(
	[RentalId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Roles]    Script Date: 23/08/2015 00:39:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Roles](
	[RoleID] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [varchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Users]    Script Date: 23/08/2015 00:39:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[IdNumber] [nvarchar](15) NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
	[eMail] [nvarchar](100) NULL,
	[BirthDate] [date] NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserVsRoles]    Script Date: 23/08/2015 00:39:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserVsRoles](
	[UserID] [int] NOT NULL,
	[RoleID] [int] NOT NULL,
 CONSTRAINT [PK_UserVsRoles] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC,
	[RoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Vehicles]    Script Date: 23/08/2015 00:39:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Vehicles](
	[TypeId] [int] IDENTITY(1,1) NOT NULL,
	[ManufactorId] [int] NOT NULL,
	[Model] [nvarchar](50) NOT NULL,
	[Year] [int] NOT NULL,
	[Transmission] [bit] NOT NULL,
	[DailyRentalRate] [money] NOT NULL,
	[PenaltyDailyRate] [money] NOT NULL,
 CONSTRAINT [PK_Vehicles] PRIMARY KEY CLUSTERED 
(
	[TypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[Fleets] ON 

INSERT [dbo].[Fleets] ([FleetId], [LicencePlate], [TypeId], [Kilometrage], [Image], [ReadyToUse]) VALUES (1, N'25-888-21', 4, 999999, N'Sussita.png', 1)
INSERT [dbo].[Fleets] ([FleetId], [LicencePlate], [TypeId], [Kilometrage], [Image], [ReadyToUse]) VALUES (2, N'59-333-30', 2, 250, N'toyota_verso_2015.png', 1)
INSERT [dbo].[Fleets] ([FleetId], [LicencePlate], [TypeId], [Kilometrage], [Image], [ReadyToUse]) VALUES (3, N'11-222-33', 1, 2000, N'toyota_corolla_2015.png', 1)
INSERT [dbo].[Fleets] ([FleetId], [LicencePlate], [TypeId], [Kilometrage], [Image], [ReadyToUse]) VALUES (4, N'83-552-98', 5, 100, N'Mazda_323f_1999.png', 1)
INSERT [dbo].[Fleets] ([FleetId], [LicencePlate], [TypeId], [Kilometrage], [Image], [ReadyToUse]) VALUES (5, N'55-888-88', 11, 2700, N'Toyota_FJCruiser_2015.png', 1)
INSERT [dbo].[Fleets] ([FleetId], [LicencePlate], [TypeId], [Kilometrage], [Image], [ReadyToUse]) VALUES (1005, N'77-777-77', 6, 1500, N'BMW_525_2010.png', 1)
INSERT [dbo].[Fleets] ([FleetId], [LicencePlate], [TypeId], [Kilometrage], [Image], [ReadyToUse]) VALUES (1006, N'852-774', 7, 125000, N'Ford_Fiesta.png', 0)
INSERT [dbo].[Fleets] ([FleetId], [LicencePlate], [TypeId], [Kilometrage], [Image], [ReadyToUse]) VALUES (1007, N'799-885', 8, 255000, N'_323f.png', 1)
INSERT [dbo].[Fleets] ([FleetId], [LicencePlate], [TypeId], [Kilometrage], [Image], [ReadyToUse]) VALUES (1011, N'21-000-12', 12, 120, N'Kia_Sportage_2015.png', 1)
INSERT [dbo].[Fleets] ([FleetId], [LicencePlate], [TypeId], [Kilometrage], [Image], [ReadyToUse]) VALUES (1012, N'9988888', 15, 1000, N'Vw_Bus_1970.png', 1)
INSERT [dbo].[Fleets] ([FleetId], [LicencePlate], [TypeId], [Kilometrage], [Image], [ReadyToUse]) VALUES (1013, N'11-111-33', 1, 1222, N'toyota_corolla_2015.png', 1)
INSERT [dbo].[Fleets] ([FleetId], [LicencePlate], [TypeId], [Kilometrage], [Image], [ReadyToUse]) VALUES (1014, N'11-998-55', 9, 1200, N'Peugeot_106.png', 1)
INSERT [dbo].[Fleets] ([FleetId], [LicencePlate], [TypeId], [Kilometrage], [Image], [ReadyToUse]) VALUES (1015, N'28-258-58', 16, 2000, N'Mazda_cx5_2015.png', 1)
INSERT [dbo].[Fleets] ([FleetId], [LicencePlate], [TypeId], [Kilometrage], [Image], [ReadyToUse]) VALUES (1016, N'11-333-33', 1, 2500, N'toyota_corolla_2015.png', 1)
INSERT [dbo].[Fleets] ([FleetId], [LicencePlate], [TypeId], [Kilometrage], [Image], [ReadyToUse]) VALUES (1017, N'11-444-33', 17, 2500, N'toyota_corolla_2015.png', 1)
SET IDENTITY_INSERT [dbo].[Fleets] OFF
SET IDENTITY_INSERT [dbo].[Manufactors] ON 

INSERT [dbo].[Manufactors] ([ManufactorId], [Name]) VALUES (1, N'Toyota')
INSERT [dbo].[Manufactors] ([ManufactorId], [Name]) VALUES (2, N'Mazda')
INSERT [dbo].[Manufactors] ([ManufactorId], [Name]) VALUES (3, N'Porche')
INSERT [dbo].[Manufactors] ([ManufactorId], [Name]) VALUES (4, N'Sussita')
INSERT [dbo].[Manufactors] ([ManufactorId], [Name]) VALUES (5, N'Peaugeot')
INSERT [dbo].[Manufactors] ([ManufactorId], [Name]) VALUES (6, N'Renault')
INSERT [dbo].[Manufactors] ([ManufactorId], [Name]) VALUES (7, N'Skoda')
INSERT [dbo].[Manufactors] ([ManufactorId], [Name]) VALUES (8, N'Audi')
INSERT [dbo].[Manufactors] ([ManufactorId], [Name]) VALUES (9, N'BMW')
INSERT [dbo].[Manufactors] ([ManufactorId], [Name]) VALUES (10, N'Ford')
INSERT [dbo].[Manufactors] ([ManufactorId], [Name]) VALUES (13, N'Kia')
INSERT [dbo].[Manufactors] ([ManufactorId], [Name]) VALUES (18, N'Jaguar')
SET IDENTITY_INSERT [dbo].[Manufactors] OFF
SET IDENTITY_INSERT [dbo].[Rentals] ON 

INSERT [dbo].[Rentals] ([RentalId], [FleetId], [UserId], [StartRentalDate], [EndRentalDate], [ActualEndRental]) VALUES (100056, 1007, 1, CAST(N'2015-08-14' AS Date), CAST(N'2015-08-21' AS Date), CAST(N'2015-08-13' AS Date))
INSERT [dbo].[Rentals] ([RentalId], [FleetId], [UserId], [StartRentalDate], [EndRentalDate], [ActualEndRental]) VALUES (100057, 1005, 2, CAST(N'2015-08-14' AS Date), CAST(N'2015-08-21' AS Date), CAST(N'2015-08-22' AS Date))
INSERT [dbo].[Rentals] ([RentalId], [FleetId], [UserId], [StartRentalDate], [EndRentalDate], [ActualEndRental]) VALUES (100058, 1006, 3, CAST(N'2015-08-14' AS Date), CAST(N'2015-08-21' AS Date), CAST(N'2015-08-20' AS Date))
INSERT [dbo].[Rentals] ([RentalId], [FleetId], [UserId], [StartRentalDate], [EndRentalDate], [ActualEndRental]) VALUES (100059, 1011, 2, CAST(N'2015-08-18' AS Date), CAST(N'2015-08-25' AS Date), NULL)
INSERT [dbo].[Rentals] ([RentalId], [FleetId], [UserId], [StartRentalDate], [EndRentalDate], [ActualEndRental]) VALUES (100061, 3, 1, CAST(N'2015-08-22' AS Date), CAST(N'2015-08-29' AS Date), NULL)
SET IDENTITY_INSERT [dbo].[Rentals] OFF
SET IDENTITY_INSERT [dbo].[Roles] ON 

INSERT [dbo].[Roles] ([RoleID], [RoleName]) VALUES (1, N'Admin')
INSERT [dbo].[Roles] ([RoleID], [RoleName]) VALUES (2, N'Manager')
INSERT [dbo].[Roles] ([RoleID], [RoleName]) VALUES (3, N'User')
SET IDENTITY_INSERT [dbo].[Roles] OFF
SET IDENTITY_INSERT [dbo].[Users] ON 

INSERT [dbo].[Users] ([UserId], [FirstName], [LastName], [IdNumber], [UserName], [Password], [eMail], [BirthDate]) VALUES (1, N'Ilan', N'Shchori', N'012/0', N'IlanSh7', N'7110EDA4D09E062AA5E4A390B0A572AC0D2C0220', N'ilan.shchori@gmail.com', CAST(N'1900-01-01' AS Date))
INSERT [dbo].[Users] ([UserId], [FirstName], [LastName], [IdNumber], [UserName], [Password], [eMail], [BirthDate]) VALUES (2, N'User', N'User', N'12345678-9', N'User', N'86F7E437FAA5A7FCE15D1DDCB9EAEAEA377667B8', NULL, NULL)
INSERT [dbo].[Users] ([UserId], [FirstName], [LastName], [IdNumber], [UserName], [Password], [eMail], [BirthDate]) VALUES (3, N'a', N'a', N'258856635', N'a0', N'86F7E437FAA5A7FCE15D1DDCB9EAEAEA377667B8', N'qqq@qqq.com', NULL)
INSERT [dbo].[Users] ([UserId], [FirstName], [LastName], [IdNumber], [UserName], [Password], [eMail], [BirthDate]) VALUES (4, N'Manager', N'Manager', N'0001', N'Manager', N'86F7E437FAA5A7FCE15D1DDCB9EAEAEA377667B8', NULL, NULL)
INSERT [dbo].[Users] ([UserId], [FirstName], [LastName], [IdNumber], [UserName], [Password], [eMail], [BirthDate]) VALUES (5, N'Admin', N'Admin', N'123/002', N'Admin', N'86F7E437FAA5A7FCE15D1DDCB9EAEAEA377667B8', N'abc@def.com', NULL)
INSERT [dbo].[Users] ([UserId], [FirstName], [LastName], [IdNumber], [UserName], [Password], [eMail], [BirthDate]) VALUES (7, N'ass', N'ass', N'990-11', N'as', N'DF211CCDD94A63E0BCB9E6AE427A249484A49D60', NULL, NULL)
INSERT [dbo].[Users] ([UserId], [FirstName], [LastName], [IdNumber], [UserName], [Password], [eMail], [BirthDate]) VALUES (8, N'sdfsd', N'fdfggsd', N'55882', N'qqq', N'F29BC91BBDAB169FC0C0A326965953D11C7DFF83', NULL, CAST(N'2001-01-02' AS Date))
SET IDENTITY_INSERT [dbo].[Users] OFF
INSERT [dbo].[UserVsRoles] ([UserID], [RoleID]) VALUES (1, 1)
INSERT [dbo].[UserVsRoles] ([UserID], [RoleID]) VALUES (1, 2)
INSERT [dbo].[UserVsRoles] ([UserID], [RoleID]) VALUES (1, 3)
INSERT [dbo].[UserVsRoles] ([UserID], [RoleID]) VALUES (2, 3)
INSERT [dbo].[UserVsRoles] ([UserID], [RoleID]) VALUES (3, 3)
INSERT [dbo].[UserVsRoles] ([UserID], [RoleID]) VALUES (4, 2)
INSERT [dbo].[UserVsRoles] ([UserID], [RoleID]) VALUES (4, 3)
INSERT [dbo].[UserVsRoles] ([UserID], [RoleID]) VALUES (5, 1)
INSERT [dbo].[UserVsRoles] ([UserID], [RoleID]) VALUES (5, 2)
INSERT [dbo].[UserVsRoles] ([UserID], [RoleID]) VALUES (5, 3)
INSERT [dbo].[UserVsRoles] ([UserID], [RoleID]) VALUES (7, 3)
INSERT [dbo].[UserVsRoles] ([UserID], [RoleID]) VALUES (8, 3)
SET IDENTITY_INSERT [dbo].[Vehicles] ON 

INSERT [dbo].[Vehicles] ([TypeId], [ManufactorId], [Model], [Year], [Transmission], [DailyRentalRate], [PenaltyDailyRate]) VALUES (1, 1, N'Corolla', 2014, 1, 62.0000, 78.0000)
INSERT [dbo].[Vehicles] ([TypeId], [ManufactorId], [Model], [Year], [Transmission], [DailyRentalRate], [PenaltyDailyRate]) VALUES (2, 1, N'Verso', 2015, 1, 100.0000, 100.0000)
INSERT [dbo].[Vehicles] ([TypeId], [ManufactorId], [Model], [Year], [Transmission], [DailyRentalRate], [PenaltyDailyRate]) VALUES (3, 2, N'3', 2015, 1, 50.0000, 55.0000)
INSERT [dbo].[Vehicles] ([TypeId], [ManufactorId], [Model], [Year], [Transmission], [DailyRentalRate], [PenaltyDailyRate]) VALUES (4, 4, N'Cube', 1971, 0, 1.0000, 1.0000)
INSERT [dbo].[Vehicles] ([TypeId], [ManufactorId], [Model], [Year], [Transmission], [DailyRentalRate], [PenaltyDailyRate]) VALUES (5, 2, N'323F', 2010, 1, 12.0000, 15.0000)
INSERT [dbo].[Vehicles] ([TypeId], [ManufactorId], [Model], [Year], [Transmission], [DailyRentalRate], [PenaltyDailyRate]) VALUES (6, 9, N'525', 2014, 1, 175.0000, 195.0000)
INSERT [dbo].[Vehicles] ([TypeId], [ManufactorId], [Model], [Year], [Transmission], [DailyRentalRate], [PenaltyDailyRate]) VALUES (7, 10, N'Fiesta', 1984, 0, 25.0000, 27.0000)
INSERT [dbo].[Vehicles] ([TypeId], [ManufactorId], [Model], [Year], [Transmission], [DailyRentalRate], [PenaltyDailyRate]) VALUES (8, 8, N'80', 1972, 0, 24.0000, 25.0000)
INSERT [dbo].[Vehicles] ([TypeId], [ManufactorId], [Model], [Year], [Transmission], [DailyRentalRate], [PenaltyDailyRate]) VALUES (9, 5, N'106', 2014, 1, 22.0000, 30.0000)
INSERT [dbo].[Vehicles] ([TypeId], [ManufactorId], [Model], [Year], [Transmission], [DailyRentalRate], [PenaltyDailyRate]) VALUES (10, 5, N'404', 1974, 0, 19.0000, 10.0000)
INSERT [dbo].[Vehicles] ([TypeId], [ManufactorId], [Model], [Year], [Transmission], [DailyRentalRate], [PenaltyDailyRate]) VALUES (11, 1, N'FJ Cruiser', 2015, 1, 88.0000, 97.0000)
INSERT [dbo].[Vehicles] ([TypeId], [ManufactorId], [Model], [Year], [Transmission], [DailyRentalRate], [PenaltyDailyRate]) VALUES (12, 13, N'Sportage', 2015, 1, 85.0000, 95.0000)
INSERT [dbo].[Vehicles] ([TypeId], [ManufactorId], [Model], [Year], [Transmission], [DailyRentalRate], [PenaltyDailyRate]) VALUES (15, 1, N'Rav4', 2015, 1, 85.0000, 95.0000)
INSERT [dbo].[Vehicles] ([TypeId], [ManufactorId], [Model], [Year], [Transmission], [DailyRentalRate], [PenaltyDailyRate]) VALUES (16, 2, N'cx5', 2015, 1, 65.0000, 70.0000)
INSERT [dbo].[Vehicles] ([TypeId], [ManufactorId], [Model], [Year], [Transmission], [DailyRentalRate], [PenaltyDailyRate]) VALUES (17, 1, N'Corolla Stick shift', 2015, 0, 58.0000, 62.0000)
SET IDENTITY_INSERT [dbo].[Vehicles] OFF
SET ANSI_PADDING ON

GO
/****** Object:  Index [UQ__Roles__8A2B6160E4842046]    Script Date: 23/08/2015 00:39:12 ******/
ALTER TABLE [dbo].[Roles] ADD UNIQUE NONCLUSTERED 
(
	[RoleName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Fleets]  WITH CHECK ADD  CONSTRAINT [FK_Fleets_Vehicles] FOREIGN KEY([TypeId])
REFERENCES [dbo].[Vehicles] ([TypeId])
GO
ALTER TABLE [dbo].[Fleets] CHECK CONSTRAINT [FK_Fleets_Vehicles]
GO
ALTER TABLE [dbo].[Rentals]  WITH CHECK ADD  CONSTRAINT [FK_Rentals_Fleets] FOREIGN KEY([FleetId])
REFERENCES [dbo].[Fleets] ([FleetId])
GO
ALTER TABLE [dbo].[Rentals] CHECK CONSTRAINT [FK_Rentals_Fleets]
GO
ALTER TABLE [dbo].[UserVsRoles]  WITH CHECK ADD FOREIGN KEY([RoleID])
REFERENCES [dbo].[Roles] ([RoleID])
GO
ALTER TABLE [dbo].[UserVsRoles]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[Vehicles]  WITH CHECK ADD  CONSTRAINT [FK_Vehicles_Manufactors] FOREIGN KEY([ManufactorId])
REFERENCES [dbo].[Manufactors] ([ManufactorId])
GO
ALTER TABLE [dbo].[Vehicles] CHECK CONSTRAINT [FK_Vehicles_Manufactors]
GO
