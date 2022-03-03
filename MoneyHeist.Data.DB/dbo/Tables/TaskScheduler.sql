CREATE TABLE [dbo].[DunningDetails] (
    [Id]                    INT             IDENTITY (1, 1) NOT NULL,
    [DunningId]             INT             NOT NULL,
    [Time]                  DATETIME        CONSTRAINT [DF_DunningDetails_Time] DEFAULT (getdate()) NOT NULL,
    [Status]                TINYINT         NOT NULL,
    [Action]                TINYINT         NOT NULL,
    [Balance]               DECIMAL (12, 2) NULL,
    [PartnerBalance]        DECIMAL (12, 2) NULL,
    [LetterType]            TINYINT         NULL,
    [ExternalBillIntNumber] VARCHAR (20)    NULL,
	[UsePartnerBalance]     bit constraint [DF_DunningDetails_UsePartnerBalance] default (0) not null,
    [SendBalance]           AS              ( case [UsePartnerBalance] when 1 then [PartnerBalance] else [Balance] end ),
    [DoxId]                 INT             NULL, 
    CONSTRAINT [PK_DunningDetails] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 100),
    CONSTRAINT [FK_DunningDetails_Dunning] FOREIGN KEY ([DunningId]) REFERENCES [dbo].[Dunning] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_DunningDetails]
    ON [dbo].[DunningDetails]([DunningId] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IX_DunningDetails_Time]
    ON [dbo].[DunningDetails]([Time] DESC)
    INCLUDE([Id]) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IX_DunningDetails_Status_Action]
    ON [dbo].[DunningDetails]([Status] ASC, [Action] ASC, [DunningId] ASC)
    INCLUDE([Id], [Time], [Balance]) WITH (FILLFACTOR = 80);

GO
CREATE NONCLUSTERED INDEX [IX_GetLastDetail] ON [dbo].[DunningDetails]
(
	[DunningId] ASC,
	[Time] ASC,
	[Status] ASC,
	[Action] ASC,
	[ExternalBillIntNumber] ASC
)
INCLUDE ( 	[Balance],
	[Id]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

