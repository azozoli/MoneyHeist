CREATE TABLE [dbo].[MemberSkills] (
    [MemberId]     INT             NOT NULL,
    [SkillId]      INT             NOT NULL,
    [IsMain]       BOOLEAN         NULL,
    CONSTRAINT [PK_PartnerBillDocument] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 100),
    CONSTRAINT [FK_PartnerBillDocument_Bill] FOREIGN KEY ([BillId]) REFERENCES [dbo].[Bill] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_PartnerBillDocument_Partner] FOREIGN KEY ([PartnerId]) REFERENCES [dbo].[Partner] ([Id]),
    CONSTRAINT [FK_PartnerBillDocument_Type] FOREIGN KEY ([TypeId]) REFERENCES [dbo].[PartnerBillDocumentType] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_PartnerBillDocument_Partner]
    ON [dbo].[PartnerBillDocument]([PartnerId] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IX_PartnerBillDocument_Bill]
    ON [dbo].[PartnerBillDocument]([BillId] ASC) WITH (FILLFACTOR = 80);

