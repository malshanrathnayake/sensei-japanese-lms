CREATE TABLE [dbo].[Staff]
(
    [EmployeeId] BIGINT NOT NULL IDENTITY,
    [UserId] BIGINT NOT NULL,
    [EmployeeCode] NVARCHAR(200) NOT NULL,
    [Email] NVARCHAR(200) NOT NULL,
    [FirstName] NVARCHAR(200) NOT NULL,
    [MiddleName] NVARCHAR(200) NOT NULL,
    [LastName] NVARCHAR(200) NOT NULL,
    [Initials] NVARCHAR(200) NOT NULL,
    [CallingName] NVARCHAR(200) NOT NULL,
    [NIC] NVARCHAR(200) NOT NULL,
    [IsDeleted] BIT NOT NULL,
    CONSTRAINT [PK_Staff] PRIMARY KEY CLUSTERED ([EmployeeId])
);