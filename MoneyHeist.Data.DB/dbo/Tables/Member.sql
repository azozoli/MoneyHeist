CREATE TABLE [dbo].[Member] (
    [Id]         INT      IDENTITY (1, 1) NOT NULL,
    [Name]       NVARCHAR  NULL,
    [Sex] 		 INT      NULL,
    [Email]      INT      NOT NULL,
    [Status]     AS       ([dbo].[GetAccountingPeriodStatus]([Id])),
	[HeistId]    INT
    CONSTRAINT [PK_AccountingPeriod] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_AccountingPeriod_BMP] FOREIGN KEY ([BMPId]) REFERENCES [dbo].[BMP] ([Id]),
    CONSTRAINT [FK_AccountingPeriod_Contract] FOREIGN KEY ([ContractId]) REFERENCES [dbo].[Contract] ([Id]),
    CONSTRAINT [FK_NextAP] FOREIGN KEY ([NextId]) REFERENCES [dbo].[AccountingPeriod] ([Id]),
    CONSTRAINT [FK_PreviousAP] FOREIGN KEY ([PreviousId]) REFERENCES [dbo].[AccountingPeriod] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_AccountingPeriod_Contract]
    ON [dbo].[AccountingPeriod]([ContractId] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IX_AccountingPeriod_BMP]
    ON [dbo].[AccountingPeriod]([BMPId] ASC)
    INCLUDE([Id]) WITH (FILLFACTOR = 80);
	 
GO
CREATE NONCLUSTERED INDEX [IX_AccountingPeriod_PreviousId]
    ON [dbo].[AccountingPeriod]([PreviousId] ASC, [BMPId] ASC, [ContractId] ASC);
	 
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_AccountingPeriod_OrderNo]
    ON [dbo].[AccountingPeriod]([BMPId] ASC, [ContractId] ASC, [OrderNo] ASC) WITH (FILLFACTOR = 80);
	 
GO
CREATE NONCLUSTERED INDEX [IX_AccountingPeriod_NextId]
    ON [dbo].[AccountingPeriod]([NextId] ASC, [BMPId] ASC, [ContractId] ASC);
