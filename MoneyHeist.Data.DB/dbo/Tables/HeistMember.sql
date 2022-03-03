CREATE TABLE [dbo].[HeistMember] (
    [HeistId]     INT             NOT NULL,
    [MemberId]     INT             NOT NULL,
    CONSTRAINT [PK_HeistMember] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 100),
    CONSTRAINT [FK_HeistMember_Bill] FOREIGN KEY ([BillId]) REFERENCES [dbo].[Bill] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_HeistMember_Partner] FOREIGN KEY ([PartnerId]) REFERENCES [dbo].[Partner] ([Id]),
    CONSTRAINT [FK_HeistMember_Type] FOREIGN KEY ([TypeId]) REFERENCES [dbo].[HeistMemberType] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_HeistMember_Partner]
    ON [dbo].[HeistMember]([PartnerId] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IX_HeistMember_Bill]
    ON [dbo].[HeistMember]([BillId] ASC) WITH (FILLFACTOR = 80);

