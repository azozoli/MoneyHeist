CREATE TABLE [dbo].[Skills] (
    [Id]          INT          NOT NULL,
    [CurrencyId]  INT          NOT NULL,
    [Name]        VARCHAR (50) NOT NULL,
    [DisplayName] VARCHAR (3)  NOT NULL,
    CONSTRAINT [PK_Market] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Market_Currency] FOREIGN KEY ([CurrencyId]) REFERENCES [dbo].[Currency] ([Id]),
    CONSTRAINT [FK_Market_TimeSeries] FOREIGN KEY ([Id]) REFERENCES [dbo].[TimeSeries] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE
);

