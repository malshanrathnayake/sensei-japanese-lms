CREATE TABLE [dbo].[Student]
(
    [StudentId] BIGINT NOT NULL IDENTITY,
    [UserId] BIGINT NOT NULL,
    [IndexNumber] INT NOT NULL,
    [Email] NVARCHAR(200) NOT NULL,
    [PhoneNo] INT NOT NULL,
    [FirstName] NVARCHAR(200) NOT NULL,
    [MiddleName] NVARCHAR(200) NOT NULL,
    [LastName] NVARCHAR(200) NOT NULL,
    [Initials] NVARCHAR(200) NOT NULL,
    [CallingName] NVARCHAR(200) NOT NULL,
    [NIC] NVARCHAR(200) NOT NULL,
    [IsDeleted] BIT NOT NULL DEFAULT 0,
    [DateOfBirth] DATETIME NOT NULL,
    [CityId] INT NULL,
    [BranchId] INT NOT NULL,
    [StudentLearningModeId] INT NOT NULL,
    [CountryId] INT NOT NULL,

    CONSTRAINT [PK_Student_StudentId] PRIMARY KEY CLUSTERED ([StudentId]),
    CONSTRAINT [FK_Student_UserId] FOREIGN KEY ([UserId]) REFERENCES [User]([UserId]),
    CONSTRAINT [FK_Student_CityId] FOREIGN KEY ([CityId]) REFERENCES [City]([CityId]),
    CONSTRAINT [FK_Student_BranchId] FOREIGN KEY ([BranchId]) REFERENCES [Branch]([BranchId]),
    CONSTRAINT [FK_Student_StudentLearningModeId] FOREIGN KEY ([StudentLearningModeId]) REFERENCES [StudentLearningMode]([StudentLearningModeId])
);