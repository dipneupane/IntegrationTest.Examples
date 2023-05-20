/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

Print 'INSERTING ASPNETROLES'
IF NOT EXISTS (SELECT * FROM [dbo].[AspNetRoles]  WHERE [Name] = 'user')
BEGIN
INSERT INTO [dbo].[AspNetRoles] ([Name], [NormalizedName], [ConcurrencyStamp]) VALUES ('user', 'USER', '02458bfb-1a30-4f32-452a-a0ee910e6f6d')
END


Print 'INSERTING ASPNETUSERS'
IF NOT EXISTS (SELECT * FROM [dbo].[AspNetUsers]  WHERE [UserName] = 'iamuser@yopmail.com')
BEGIN
INSERT INTO [dbo].[AspNetUsers] ([UserName],[NormalizedUserName],[Email],[NormalizedEmail], [EmailConfirmed],[PhoneNumberConfirmed],[TwoFactorEnabled],[LockoutEnabled],[AccessFailedCount], [PasswordHash], [SecurityStamp]) VALUES 
('iamuser@yopmail.com','iamuser@yopmail.com', 'IAMUSER@YOPMAIL.COM','IAMUSER@YOPMAIL.COM', 1, 1, 0, 0, 0, 'AQAAAAEAACcQAAAAEBr/wcAPW9vkZEjRqr3nO4Dad3br5KTttPIEXi5XkZWMaS+J1uCcUM4lWpp5dBgCjw==', '27KRQBTZRUKFX6J7OFCEM6BN2N3CNXR4')
END


Print 'INSERTING ASPNETUSERROLES'
IF NOT EXISTS (SELECT * FROM [dbo].[AspNetUserRoles]  WHERE [UserId] = 1)
BEGIN
INSERT INTO [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (1, 1)
END