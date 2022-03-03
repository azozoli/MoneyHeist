CREATE TABLE [dbo].[HeistSkills] (
    [HeistId]   		INT		NOT NULL,
    [SkillId]			INT		NOT NULL,
    [MemberRequired]	INT		NULL,
    CONSTRAINT [PK_HeistSkills] PRIMARY KEY CLUSTERED ([HeistId], [SkillsId] ASC) WITH (FILLFACTOR = 100),
    CONSTRAINT [FK_HeistSkills_Heist] FOREIGN KEY ([HeistId]) REFERENCES [dbo].[Heist] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_HeistSkills_Skills] FOREIGN KEY ([SkillId]) REFERENCES [dbo].[Skills] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_HeistSkills_Heist]
    ON [dbo].[HeistSkills]([HeistId] ASC) WITH (FILLFACTOR = 80);


GO
CREATE NONCLUSTERED INDEX [IX_HeistSkills_Skills]
    ON [dbo].[HeistSkills]([SkillsId] ASC) WITH (FILLFACTOR = 80);

