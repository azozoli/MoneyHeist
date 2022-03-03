CREATE TABLE [dbo].[Heist] (
    [Id] 		INT        		IDENTITY(1,1)  NOT NULL,
    [Name] 		VARCHAR (100) 	UNIQUE NOT NULL,
	[Location] 	VARCHAR (100) 	NOT NULL,
	[StartTime] DATETIME 		NOT NULL,
	[EndTime] 	DATETIME 		NOT NULL,
	[Status] 	BYTE  			NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

