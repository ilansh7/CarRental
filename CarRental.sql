USE [master]
GO
/****** Object:  Database [CarRental]    Script Date: 4/30/2022 10:18:03 PM ******/
CREATE DATABASE [CarRental]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'CarRental', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\CarRental.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'CarRental_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\CarRental_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [CarRental] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [CarRental].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [CarRental] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [CarRental] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [CarRental] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [CarRental] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [CarRental] SET ARITHABORT OFF 
GO
ALTER DATABASE [CarRental] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [CarRental] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [CarRental] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [CarRental] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [CarRental] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [CarRental] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [CarRental] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [CarRental] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [CarRental] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [CarRental] SET  DISABLE_BROKER 
GO
ALTER DATABASE [CarRental] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [CarRental] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [CarRental] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [CarRental] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [CarRental] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [CarRental] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [CarRental] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [CarRental] SET RECOVERY FULL 
GO
ALTER DATABASE [CarRental] SET  MULTI_USER 
GO
ALTER DATABASE [CarRental] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [CarRental] SET DB_CHAINING OFF 
GO
ALTER DATABASE [CarRental] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [CarRental] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [CarRental] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [CarRental] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'CarRental', N'ON'
GO
ALTER DATABASE [CarRental] SET QUERY_STORE = OFF
GO
USE [CarRental]
GO
/****** Object:  UserDefinedFunction [dbo].[IsCarOccupied]    Script Date: 4/30/2022 10:18:03 PM ******/
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
/****** Object:  Table [dbo].[EventExtention]    Script Date: 4/30/2022 10:18:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EventExtention](
	[EventExtentionId] [int] IDENTITY(1,1) NOT NULL,
	[Type] [nvarchar](50) NULL,
	[Description] [nvarchar](300) NULL,
	[Ext_String_1] [nvarchar](100) NULL,
	[Ext_String_2] [nvarchar](100) NULL,
	[Ext_Numeric_1] [decimal](10, 2) NULL,
	[Ext_Date_1] [datetime] NULL,
 CONSTRAINT [PK_EventExtention] PRIMARY KEY CLUSTERED 
(
	[EventExtentionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Events]    Script Date: 4/30/2022 10:18:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Events](
	[EventID] [int] IDENTITY(1,1) NOT NULL,
	[Subject] [nvarchar](100) NOT NULL,
	[UserId] [int] NOT NULL,
	[Description] [nvarchar](300) NULL,
	[Start] [datetime] NOT NULL,
	[DurationInMin] [int] NOT NULL,
	[ThemeColor] [nvarchar](10) NULL,
	[IsFullDay] [bit] NOT NULL,
 CONSTRAINT [PK_Events] PRIMARY KEY CLUSTERED 
(
	[EventID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vwAllEvents]    Script Date: 4/30/2022 10:18:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vwAllEvents] as
SELECT	EventId, Subject as EventSubject, Ext_String_1 as EventType, Start as EventStartDate, DATEADD(minute, 60 * [Ext_Numeric_1], [Start]) as EventEndDate, DurationInMin as EventDuration
FROM	[dbo].[Events] ev left join [dbo].[EventExtention] ex on ex.EventExtentionId = CAST(ev.Description as INT)
WHERE	ev.Description IS NOT NULL



GO
/****** Object:  Table [dbo].[Fleets]    Script Date: 4/30/2022 10:18:03 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Manufactors]    Script Date: 4/30/2022 10:18:03 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Rentals]    Script Date: 4/30/2022 10:18:03 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 4/30/2022 10:18:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[RoleID] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [varchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[RoleName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 4/30/2022 10:18:03 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[UserVsRoles]    Script Date: 4/30/2022 10:18:03 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Vehicles]    Script Date: 4/30/2022 10:18:03 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IDX_EventExtention_Type]    Script Date: 4/30/2022 10:18:03 PM ******/
CREATE NONCLUSTERED INDEX [IDX_EventExtention_Type] ON [dbo].[EventExtention]
(
	[Type] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
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
/****** Object:  StoredProcedure [dbo].[GetAvailableCars]    Script Date: 4/30/2022 10:18:03 PM ******/
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

/****** Object:  StoredProcedure [dbo].[GetCarForRent]    Script Date: 4/30/2022 10:18:03 PM ******/
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
/****** Object:  StoredProcedure [dbo].[GetUnlinkedVehicles]    Script Date: 4/30/2022 10:18:03 PM ******/
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
/****** Object:  StoredProcedure [dbo].[InsertOrder]    Script Date: 4/30/2022 10:18:03 PM ******/
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
/****** Object:  StoredProcedure [dbo].[IsSlotAvailable]    Script Date: 4/30/2022 10:18:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[IsSlotAvailable] 
	@eventTypeId int,
	@fromDate datetime,
	@allDayFlag tinyint

AS
BEGIN

	DECLARE @evId int
	DECLARE @evType nvarchar(100)
	DECLARE @evSubject nvarchar(100)
	DECLARE @evStartDate DateTime
	DECLARE @evEndDate DatetIme
	DECLARE @evDuration int

	DECLARE @slotAvaiable int = 1
	DECLARE @isOnTheEdge int = 0
	DECLARE @eventDuration int
	DECLARE @eventWithDuration DateTime
	
	DECLARE AllEvents CURSOR 
	--LOCAL STATIC READ_ONLY FORWARD_ONLY
	FOR 
		SELECT	EventId, EventSubject, EventType, EventStartDate, EventEndDate, EventDuration
		FROM	[dbo].[vwAllEvents] 
		ORDER BY EventStartDate desc;
	
	--SET @slotAvaiable = 0
	SELECT	@eventDuration = Ext_Numeric_1
	FROM	[dbo].[EventExtention]
	WHERE	EventExtentionId = @eventTypeId

	SET @eventWithDuration = DATEADD(minute, 60 * @eventDuration, @fromDate)
	
	OPEN AllEvents
	FETCH NEXT FROM AllEvents INTO @evId, @evType, @evSubject, @evStartDate, @evEndDate, @evDuration
	WHILE (@@FETCH_STATUS = 0 AND @slotAvaiable = 1)
	BEGIN
		-- Start time request is out of existing start event in the log
		IF (@evStartDate < @fromDate AND @fromDate < @evEndDate)
			SET @slotAvaiable = 0
				--BREAK;

        -- Start time request is exact like existing start event in the log
        IF (@evStartDate = @fromDate OR @fromDate = @evEndDate)
			--BEGIN
			SET @isOnTheEdge = @isOnTheEdge + 1
			--END

        -- End time request is out of existing end event in the log
        IF (@evStartDate < @eventWithDuration AND @eventWithDuration < @evEndDate)
			--BEGIN
			SET @slotAvaiable = 0
				--BREAK;
			--END

        -- End time request is exact like existing end event in the log
        IF (@evStartDate = @eventWithDuration OR @eventWithDuration = @evEndDate)
            SET @isOnTheEdge = @isOnTheEdge + 1

        -- Either request time for start / end coincides with the start / end time of the existing event in the log - or none.
        IF (@isOnTheEdge < 2)
			BEGIN
				-- Check if the other end is out : the request is overlap and over
				IF ((@evStartDate = @fromDate AND @evEndDate < @eventWithDuration) OR (@evEndDate = @eventWithDuration AND @evStartDate > @fromDate))
					--BEGIN
						SET @slotAvaiable = 0
						--BREAK;
					--END
				--CONTINUE;
			END
        ELSE
			--BEGIN
				SET @slotAvaiable = 0;

				--BREAK;
			--END

		FETCH NEXT FROM AllEvents INTO @evId, @evType, @evSubject, @evStartDate, @evEndDate, @evDuration
	END

	CLOSE AllEvents
	DEALLOCATE AllEvents

	SELECT @slotAvaiable

END

GO
USE [master]
GO
ALTER DATABASE [CarRental] SET  READ_WRITE 
GO
