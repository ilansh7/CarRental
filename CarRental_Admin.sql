USE [CarRental]
GO
SET IDENTITY_INSERT [dbo].[Roles] ON 
GO
INSERT [dbo].[Roles] ([RoleID], [RoleName]) VALUES (1, N'Admin')
GO
INSERT [dbo].[Roles] ([RoleID], [RoleName]) VALUES (2, N'Manager')
GO
INSERT [dbo].[Roles] ([RoleID], [RoleName]) VALUES (3, N'User')
GO
SET IDENTITY_INSERT [dbo].[Roles] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 
GO
INSERT [dbo].[Users] ([UserId], [FirstName], [LastName], [IdNumber], [UserName], [Password], [eMail], [BirthDate]) VALUES (1, N'Ilan', N'Shchori', N'012/0', N'IlanSh7', N'7110EDA4D09E062AA5E4A390B0A572AC0D2C0220', N'ilan.shchori@gmail.com', CAST(N'1965-02-13' AS Date))
GO
INSERT [dbo].[Users] ([UserId], [FirstName], [LastName], [IdNumber], [UserName], [Password], [eMail], [BirthDate]) VALUES (2, N'User', N'User', N'12345678-9', N'User', N'86F7E437FAA5A7FCE15D1DDCB9EAEAEA377667B8', NULL, CAST(N'2001-01-18' AS Date))
GO
INSERT [dbo].[Users] ([UserId], [FirstName], [LastName], [IdNumber], [UserName], [Password], [eMail], [BirthDate]) VALUES (3, N'Admin', N'Admin', N'123/002', N'Admin', N'86F7E437FAA5A7FCE15D1DDCB9EAEAEA377667B8', N'abc@def.com', NULL)
GO
INSERT [dbo].[Users] ([UserId], [FirstName], [LastName], [IdNumber], [UserName], [Password], [eMail], [BirthDate]) VALUES (4, N'Manager', N'Manager', N'0001', N'Manager', N'86F7E437FAA5A7FCE15D1DDCB9EAEAEA377667B8', NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
INSERT [dbo].[UserVsRoles] ([UserID], [RoleID]) VALUES (1, 1)
GO
INSERT [dbo].[UserVsRoles] ([UserID], [RoleID]) VALUES (1, 2)
GO
INSERT [dbo].[UserVsRoles] ([UserID], [RoleID]) VALUES (1, 3)
GO
INSERT [dbo].[UserVsRoles] ([UserID], [RoleID]) VALUES (2, 3)
GO
INSERT [dbo].[UserVsRoles] ([UserID], [RoleID]) VALUES (3, 1)
GO
INSERT [dbo].[UserVsRoles] ([UserID], [RoleID]) VALUES (3, 2)
GO
INSERT [dbo].[UserVsRoles] ([UserID], [RoleID]) VALUES (3, 3)
GO
INSERT [dbo].[UserVsRoles] ([UserID], [RoleID]) VALUES (4, 2)
GO
INSERT [dbo].[UserVsRoles] ([UserID], [RoleID]) VALUES (4, 3)
GO
