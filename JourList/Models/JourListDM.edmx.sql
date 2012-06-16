
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 05/09/2012 16:37:05
-- Generated from EDMX file: C:\Users\shadowguy\Documents\Visual Studio 2010\Projects\JourList\JourList\Models\JourListDM.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [jourlist];
GO
IF SCHEMA_ID(N'jourlist') IS NULL EXECUTE(N'CREATE SCHEMA [jourlist]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[jourlist].[FK_RecurrenceIntervalGoal]', 'F') IS NOT NULL
    ALTER TABLE [jourlist].[Goals] DROP CONSTRAINT [FK_RecurrenceIntervalGoal];
GO
IF OBJECT_ID(N'[jourlist].[FK_JournalAccomplishment]', 'F') IS NOT NULL
    ALTER TABLE [jourlist].[Accomplishments] DROP CONSTRAINT [FK_JournalAccomplishment];
GO
IF OBJECT_ID(N'[jourlist].[FK_GoalAccomplishment]', 'F') IS NOT NULL
    ALTER TABLE [jourlist].[Accomplishments] DROP CONSTRAINT [FK_GoalAccomplishment];
GO
IF OBJECT_ID(N'[jourlist].[FK_InventoryActionInventoryLog]', 'F') IS NOT NULL
    ALTER TABLE [jourlist].[InventoryLogs] DROP CONSTRAINT [FK_InventoryActionInventoryLog];
GO
IF OBJECT_ID(N'[jourlist].[FK_JournalActivityLog]', 'F') IS NOT NULL
    ALTER TABLE [jourlist].[ActivityLogs] DROP CONSTRAINT [FK_JournalActivityLog];
GO
IF OBJECT_ID(N'[jourlist].[FK_ActivityActivityLog]', 'F') IS NOT NULL
    ALTER TABLE [jourlist].[ActivityLogs] DROP CONSTRAINT [FK_ActivityActivityLog];
GO
IF OBJECT_ID(N'[jourlist].[FK_UnitTypeUnit]', 'F') IS NOT NULL
    ALTER TABLE [jourlist].[Units] DROP CONSTRAINT [FK_UnitTypeUnit];
GO
IF OBJECT_ID(N'[jourlist].[FK_UnitActivityLog]', 'F') IS NOT NULL
    ALTER TABLE [jourlist].[ActivityLogs] DROP CONSTRAINT [FK_UnitActivityLog];
GO
IF OBJECT_ID(N'[jourlist].[FK_UnitTypeItem]', 'F') IS NOT NULL
    ALTER TABLE [jourlist].[Items] DROP CONSTRAINT [FK_UnitTypeItem];
GO
IF OBJECT_ID(N'[jourlist].[FK_ItemCategoryItem]', 'F') IS NOT NULL
    ALTER TABLE [jourlist].[Items] DROP CONSTRAINT [FK_ItemCategoryItem];
GO
IF OBJECT_ID(N'[jourlist].[FK_InventoryInventoryLog]', 'F') IS NOT NULL
    ALTER TABLE [jourlist].[InventoryLogs] DROP CONSTRAINT [FK_InventoryInventoryLog];
GO
IF OBJECT_ID(N'[jourlist].[FK_ActivityGoal]', 'F') IS NOT NULL
    ALTER TABLE [jourlist].[Goals] DROP CONSTRAINT [FK_ActivityGoal];
GO
IF OBJECT_ID(N'[jourlist].[FK_ItemInventory]', 'F') IS NOT NULL
    ALTER TABLE [jourlist].[Inventories] DROP CONSTRAINT [FK_ItemInventory];
GO
IF OBJECT_ID(N'[jourlist].[FK_MemberGoal]', 'F') IS NOT NULL
    ALTER TABLE [jourlist].[Goals] DROP CONSTRAINT [FK_MemberGoal];
GO
IF OBJECT_ID(N'[jourlist].[FK_MemberJournal]', 'F') IS NOT NULL
    ALTER TABLE [jourlist].[Journals] DROP CONSTRAINT [FK_MemberJournal];
GO
IF OBJECT_ID(N'[jourlist].[FK_MemberPersonalItem]', 'F') IS NOT NULL
    ALTER TABLE [jourlist].[Items_PersonalItem] DROP CONSTRAINT [FK_MemberPersonalItem];
GO
IF OBJECT_ID(N'[jourlist].[FK_MemberInventory]', 'F') IS NOT NULL
    ALTER TABLE [jourlist].[Inventories] DROP CONSTRAINT [FK_MemberInventory];
GO
IF OBJECT_ID(N'[jourlist].[FK_ItemItemInfo]', 'F') IS NOT NULL
    ALTER TABLE [jourlist].[ItemInfoes] DROP CONSTRAINT [FK_ItemItemInfo];
GO
IF OBJECT_ID(N'[jourlist].[FK_ItemInfoInventoryLog]', 'F') IS NOT NULL
    ALTER TABLE [jourlist].[InventoryLogs] DROP CONSTRAINT [FK_ItemInfoInventoryLog];
GO
IF OBJECT_ID(N'[jourlist].[FK_InventoryShoppingList]', 'F') IS NOT NULL
    ALTER TABLE [jourlist].[ShoppingLists] DROP CONSTRAINT [FK_InventoryShoppingList];
GO
IF OBJECT_ID(N'[jourlist].[FK_InventoryUnit]', 'F') IS NOT NULL
    ALTER TABLE [jourlist].[Inventories] DROP CONSTRAINT [FK_InventoryUnit];
GO
IF OBJECT_ID(N'[jourlist].[FK_UnitTypeUnit1]', 'F') IS NOT NULL
    ALTER TABLE [jourlist].[Units] DROP CONSTRAINT [FK_UnitTypeUnit1];
GO
IF OBJECT_ID(N'[jourlist].[FK_UnitInventoryLog]', 'F') IS NOT NULL
    ALTER TABLE [jourlist].[InventoryLogs] DROP CONSTRAINT [FK_UnitInventoryLog];
GO
IF OBJECT_ID(N'[jourlist].[FK_ActivityUnit]', 'F') IS NOT NULL
    ALTER TABLE [jourlist].[Activities] DROP CONSTRAINT [FK_ActivityUnit];
GO
IF OBJECT_ID(N'[jourlist].[FK_PersonalItem_inherits_Item]', 'F') IS NOT NULL
    ALTER TABLE [jourlist].[Items_PersonalItem] DROP CONSTRAINT [FK_PersonalItem_inherits_Item];
GO
IF OBJECT_ID(N'[jourlist].[FK_StandardItem_inherits_Item]', 'F') IS NOT NULL
    ALTER TABLE [jourlist].[Items_StandardItem] DROP CONSTRAINT [FK_StandardItem_inherits_Item];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[jourlist].[Journals]', 'U') IS NOT NULL
    DROP TABLE [jourlist].[Journals];
GO
IF OBJECT_ID(N'[jourlist].[Items]', 'U') IS NOT NULL
    DROP TABLE [jourlist].[Items];
GO
IF OBJECT_ID(N'[jourlist].[InventoryLogs]', 'U') IS NOT NULL
    DROP TABLE [jourlist].[InventoryLogs];
GO
IF OBJECT_ID(N'[jourlist].[InventoryActions]', 'U') IS NOT NULL
    DROP TABLE [jourlist].[InventoryActions];
GO
IF OBJECT_ID(N'[jourlist].[Activities]', 'U') IS NOT NULL
    DROP TABLE [jourlist].[Activities];
GO
IF OBJECT_ID(N'[jourlist].[RecurrenceIntervals]', 'U') IS NOT NULL
    DROP TABLE [jourlist].[RecurrenceIntervals];
GO
IF OBJECT_ID(N'[jourlist].[Accomplishments]', 'U') IS NOT NULL
    DROP TABLE [jourlist].[Accomplishments];
GO
IF OBJECT_ID(N'[jourlist].[ActivityLogs]', 'U') IS NOT NULL
    DROP TABLE [jourlist].[ActivityLogs];
GO
IF OBJECT_ID(N'[jourlist].[Units]', 'U') IS NOT NULL
    DROP TABLE [jourlist].[Units];
GO
IF OBJECT_ID(N'[jourlist].[UnitTypes]', 'U') IS NOT NULL
    DROP TABLE [jourlist].[UnitTypes];
GO
IF OBJECT_ID(N'[jourlist].[ItemCategories]', 'U') IS NOT NULL
    DROP TABLE [jourlist].[ItemCategories];
GO
IF OBJECT_ID(N'[jourlist].[Inventories]', 'U') IS NOT NULL
    DROP TABLE [jourlist].[Inventories];
GO
IF OBJECT_ID(N'[jourlist].[Goals]', 'U') IS NOT NULL
    DROP TABLE [jourlist].[Goals];
GO
IF OBJECT_ID(N'[jourlist].[Members]', 'U') IS NOT NULL
    DROP TABLE [jourlist].[Members];
GO
IF OBJECT_ID(N'[jourlist].[ItemInfoes]', 'U') IS NOT NULL
    DROP TABLE [jourlist].[ItemInfoes];
GO
IF OBJECT_ID(N'[jourlist].[ShoppingLists]', 'U') IS NOT NULL
    DROP TABLE [jourlist].[ShoppingLists];
GO
IF OBJECT_ID(N'[jourlist].[Items_PersonalItem]', 'U') IS NOT NULL
    DROP TABLE [jourlist].[Items_PersonalItem];
GO
IF OBJECT_ID(N'[jourlist].[Items_StandardItem]', 'U') IS NOT NULL
    DROP TABLE [jourlist].[Items_StandardItem];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Journals'
CREATE TABLE [jourlist].[Journals] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [Story] nvarchar(max)  NOT NULL,
    [HeartRate] smallint  NOT NULL,
    [Weight] smallint  NOT NULL,
    [EntryDate] datetime  NOT NULL,
    [Encrypted] bit  NOT NULL,
    [Member_Id] bigint  NOT NULL
);
GO

-- Creating table 'Items'
CREATE TABLE [jourlist].[Items] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [Active] bit  NOT NULL,
    [UnitType_Id] int  NOT NULL,
    [ItemCategory_Id] int  NOT NULL
);
GO

-- Creating table 'InventoryLogs'
CREATE TABLE [jourlist].[InventoryLogs] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [TransactionDate] datetime  NOT NULL,
    [Quantity] float  NOT NULL,
    [Cost] float  NOT NULL,
    [UnitId] int  NOT NULL,
    [Size] float  NOT NULL,
    [InventoryAction_Id] int  NOT NULL,
    [Inventory_Id] bigint  NOT NULL,
    [ItemInfo_Id] bigint  NULL
);
GO

-- Creating table 'InventoryActions'
CREATE TABLE [jourlist].[InventoryActions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Description] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Activities'
CREATE TABLE [jourlist].[Activities] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [Points] int  NOT NULL,
    [Active] bit  NOT NULL,
    [Quantity] float  NOT NULL,
    [Unit_Id] int  NOT NULL
);
GO

-- Creating table 'RecurrenceIntervals'
CREATE TABLE [jourlist].[RecurrenceIntervals] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [Minutes] bigint  NOT NULL,
    [Active] bit  NOT NULL
);
GO

-- Creating table 'Accomplishments'
CREATE TABLE [jourlist].[Accomplishments] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [Notes] nvarchar(max)  NOT NULL,
    [Journal_Id] bigint  NOT NULL,
    [Goal_Id] bigint  NOT NULL
);
GO

-- Creating table 'ActivityLogs'
CREATE TABLE [jourlist].[ActivityLogs] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [Notes] nvarchar(max)  NOT NULL,
    [Quantity] float  NOT NULL,
    [Hyperlink] nvarchar(max)  NOT NULL,
    [Journal_Id] bigint  NOT NULL,
    [Activity_Id] bigint  NOT NULL,
    [Unit_Id] int  NOT NULL
);
GO

-- Creating table 'Units'
CREATE TABLE [jourlist].[Units] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [ConversionFactor] float  NOT NULL,
    [Abbreviation] nvarchar(max)  NOT NULL,
    [Active] bit  NOT NULL,
    [UnitType_Id] int  NOT NULL,
    [UnitTypeUnit1_Unit_Id] int  NULL
);
GO

-- Creating table 'UnitTypes'
CREATE TABLE [jourlist].[UnitTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [Active] bit  NOT NULL
);
GO

-- Creating table 'ItemCategories'
CREATE TABLE [jourlist].[ItemCategories] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [Active] bit  NOT NULL
);
GO

-- Creating table 'Inventories'
CREATE TABLE [jourlist].[Inventories] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [OnHand] float  NOT NULL,
    [RequiredQuantity] float  NOT NULL,
    [RestockThreshold] float  NOT NULL,
    [Active] bit  NOT NULL,
    [Item_Id] bigint  NOT NULL,
    [Member_Id] bigint  NOT NULL,
    [Unit_Id] int  NOT NULL
);
GO

-- Creating table 'Goals'
CREATE TABLE [jourlist].[Goals] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [Active] bit  NOT NULL,
    [DateBegin] datetime  NOT NULL,
    [DateEnd] datetime  NOT NULL,
    [RecurrenceInterval_Id] int  NOT NULL,
    [Activity_Id] bigint  NOT NULL,
    [Member_Id] bigint  NOT NULL
);
GO

-- Creating table 'Members'
CREATE TABLE [jourlist].[Members] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [UserId] uniqueidentifier  NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'ItemInfoes'
CREATE TABLE [jourlist].[ItemInfoes] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [Barcode] nvarchar(max)  NOT NULL,
    [Hyperlink] nvarchar(max)  NOT NULL,
    [Quantity] nvarchar(max)  NOT NULL,
    [Brand] nvarchar(max)  NOT NULL,
    [Item_Id] bigint  NOT NULL
);
GO

-- Creating table 'ShoppingLists'
CREATE TABLE [jourlist].[ShoppingLists] (
    [Id] uniqueidentifier  NOT NULL,
    [InCart] bit  NOT NULL,
    [QuantityNeeded] float  NOT NULL,
    [SizeNeeded] float  NOT NULL,
    [Inventory_Id] bigint  NOT NULL
);
GO

-- Creating table 'Items_PersonalItem'
CREATE TABLE [jourlist].[Items_PersonalItem] (
    [Id] bigint  NOT NULL,
    [Member_Id] bigint  NOT NULL
);
GO

-- Creating table 'Items_StandardItem'
CREATE TABLE [jourlist].[Items_StandardItem] (
    [Id] bigint  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'Journals'
ALTER TABLE [jourlist].[Journals]
ADD CONSTRAINT [PK_Journals]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Items'
ALTER TABLE [jourlist].[Items]
ADD CONSTRAINT [PK_Items]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InventoryLogs'
ALTER TABLE [jourlist].[InventoryLogs]
ADD CONSTRAINT [PK_InventoryLogs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InventoryActions'
ALTER TABLE [jourlist].[InventoryActions]
ADD CONSTRAINT [PK_InventoryActions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Activities'
ALTER TABLE [jourlist].[Activities]
ADD CONSTRAINT [PK_Activities]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'RecurrenceIntervals'
ALTER TABLE [jourlist].[RecurrenceIntervals]
ADD CONSTRAINT [PK_RecurrenceIntervals]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Accomplishments'
ALTER TABLE [jourlist].[Accomplishments]
ADD CONSTRAINT [PK_Accomplishments]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ActivityLogs'
ALTER TABLE [jourlist].[ActivityLogs]
ADD CONSTRAINT [PK_ActivityLogs]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Units'
ALTER TABLE [jourlist].[Units]
ADD CONSTRAINT [PK_Units]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UnitTypes'
ALTER TABLE [jourlist].[UnitTypes]
ADD CONSTRAINT [PK_UnitTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ItemCategories'
ALTER TABLE [jourlist].[ItemCategories]
ADD CONSTRAINT [PK_ItemCategories]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Inventories'
ALTER TABLE [jourlist].[Inventories]
ADD CONSTRAINT [PK_Inventories]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Goals'
ALTER TABLE [jourlist].[Goals]
ADD CONSTRAINT [PK_Goals]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Members'
ALTER TABLE [jourlist].[Members]
ADD CONSTRAINT [PK_Members]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ItemInfoes'
ALTER TABLE [jourlist].[ItemInfoes]
ADD CONSTRAINT [PK_ItemInfoes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ShoppingLists'
ALTER TABLE [jourlist].[ShoppingLists]
ADD CONSTRAINT [PK_ShoppingLists]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Items_PersonalItem'
ALTER TABLE [jourlist].[Items_PersonalItem]
ADD CONSTRAINT [PK_Items_PersonalItem]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Items_StandardItem'
ALTER TABLE [jourlist].[Items_StandardItem]
ADD CONSTRAINT [PK_Items_StandardItem]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [RecurrenceInterval_Id] in table 'Goals'
ALTER TABLE [jourlist].[Goals]
ADD CONSTRAINT [FK_RecurrenceIntervalGoal]
    FOREIGN KEY ([RecurrenceInterval_Id])
    REFERENCES [jourlist].[RecurrenceIntervals]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_RecurrenceIntervalGoal'
CREATE INDEX [IX_FK_RecurrenceIntervalGoal]
ON [jourlist].[Goals]
    ([RecurrenceInterval_Id]);
GO

-- Creating foreign key on [Journal_Id] in table 'Accomplishments'
ALTER TABLE [jourlist].[Accomplishments]
ADD CONSTRAINT [FK_JournalAccomplishment]
    FOREIGN KEY ([Journal_Id])
    REFERENCES [jourlist].[Journals]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_JournalAccomplishment'
CREATE INDEX [IX_FK_JournalAccomplishment]
ON [jourlist].[Accomplishments]
    ([Journal_Id]);
GO

-- Creating foreign key on [Goal_Id] in table 'Accomplishments'
ALTER TABLE [jourlist].[Accomplishments]
ADD CONSTRAINT [FK_GoalAccomplishment]
    FOREIGN KEY ([Goal_Id])
    REFERENCES [jourlist].[Goals]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_GoalAccomplishment'
CREATE INDEX [IX_FK_GoalAccomplishment]
ON [jourlist].[Accomplishments]
    ([Goal_Id]);
GO

-- Creating foreign key on [InventoryAction_Id] in table 'InventoryLogs'
ALTER TABLE [jourlist].[InventoryLogs]
ADD CONSTRAINT [FK_InventoryActionInventoryLog]
    FOREIGN KEY ([InventoryAction_Id])
    REFERENCES [jourlist].[InventoryActions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_InventoryActionInventoryLog'
CREATE INDEX [IX_FK_InventoryActionInventoryLog]
ON [jourlist].[InventoryLogs]
    ([InventoryAction_Id]);
GO

-- Creating foreign key on [Journal_Id] in table 'ActivityLogs'
ALTER TABLE [jourlist].[ActivityLogs]
ADD CONSTRAINT [FK_JournalActivityLog]
    FOREIGN KEY ([Journal_Id])
    REFERENCES [jourlist].[Journals]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_JournalActivityLog'
CREATE INDEX [IX_FK_JournalActivityLog]
ON [jourlist].[ActivityLogs]
    ([Journal_Id]);
GO

-- Creating foreign key on [Activity_Id] in table 'ActivityLogs'
ALTER TABLE [jourlist].[ActivityLogs]
ADD CONSTRAINT [FK_ActivityActivityLog]
    FOREIGN KEY ([Activity_Id])
    REFERENCES [jourlist].[Activities]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ActivityActivityLog'
CREATE INDEX [IX_FK_ActivityActivityLog]
ON [jourlist].[ActivityLogs]
    ([Activity_Id]);
GO

-- Creating foreign key on [UnitType_Id] in table 'Units'
ALTER TABLE [jourlist].[Units]
ADD CONSTRAINT [FK_UnitTypeUnit]
    FOREIGN KEY ([UnitType_Id])
    REFERENCES [jourlist].[UnitTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UnitTypeUnit'
CREATE INDEX [IX_FK_UnitTypeUnit]
ON [jourlist].[Units]
    ([UnitType_Id]);
GO

-- Creating foreign key on [Unit_Id] in table 'ActivityLogs'
ALTER TABLE [jourlist].[ActivityLogs]
ADD CONSTRAINT [FK_UnitActivityLog]
    FOREIGN KEY ([Unit_Id])
    REFERENCES [jourlist].[Units]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UnitActivityLog'
CREATE INDEX [IX_FK_UnitActivityLog]
ON [jourlist].[ActivityLogs]
    ([Unit_Id]);
GO

-- Creating foreign key on [UnitType_Id] in table 'Items'
ALTER TABLE [jourlist].[Items]
ADD CONSTRAINT [FK_UnitTypeItem]
    FOREIGN KEY ([UnitType_Id])
    REFERENCES [jourlist].[UnitTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UnitTypeItem'
CREATE INDEX [IX_FK_UnitTypeItem]
ON [jourlist].[Items]
    ([UnitType_Id]);
GO

-- Creating foreign key on [ItemCategory_Id] in table 'Items'
ALTER TABLE [jourlist].[Items]
ADD CONSTRAINT [FK_ItemCategoryItem]
    FOREIGN KEY ([ItemCategory_Id])
    REFERENCES [jourlist].[ItemCategories]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ItemCategoryItem'
CREATE INDEX [IX_FK_ItemCategoryItem]
ON [jourlist].[Items]
    ([ItemCategory_Id]);
GO

-- Creating foreign key on [Inventory_Id] in table 'InventoryLogs'
ALTER TABLE [jourlist].[InventoryLogs]
ADD CONSTRAINT [FK_InventoryInventoryLog]
    FOREIGN KEY ([Inventory_Id])
    REFERENCES [jourlist].[Inventories]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_InventoryInventoryLog'
CREATE INDEX [IX_FK_InventoryInventoryLog]
ON [jourlist].[InventoryLogs]
    ([Inventory_Id]);
GO

-- Creating foreign key on [Activity_Id] in table 'Goals'
ALTER TABLE [jourlist].[Goals]
ADD CONSTRAINT [FK_ActivityGoal]
    FOREIGN KEY ([Activity_Id])
    REFERENCES [jourlist].[Activities]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ActivityGoal'
CREATE INDEX [IX_FK_ActivityGoal]
ON [jourlist].[Goals]
    ([Activity_Id]);
GO

-- Creating foreign key on [Item_Id] in table 'Inventories'
ALTER TABLE [jourlist].[Inventories]
ADD CONSTRAINT [FK_ItemInventory]
    FOREIGN KEY ([Item_Id])
    REFERENCES [jourlist].[Items]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ItemInventory'
CREATE INDEX [IX_FK_ItemInventory]
ON [jourlist].[Inventories]
    ([Item_Id]);
GO

-- Creating foreign key on [Member_Id] in table 'Goals'
ALTER TABLE [jourlist].[Goals]
ADD CONSTRAINT [FK_MemberGoal]
    FOREIGN KEY ([Member_Id])
    REFERENCES [jourlist].[Members]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_MemberGoal'
CREATE INDEX [IX_FK_MemberGoal]
ON [jourlist].[Goals]
    ([Member_Id]);
GO

-- Creating foreign key on [Member_Id] in table 'Journals'
ALTER TABLE [jourlist].[Journals]
ADD CONSTRAINT [FK_MemberJournal]
    FOREIGN KEY ([Member_Id])
    REFERENCES [jourlist].[Members]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_MemberJournal'
CREATE INDEX [IX_FK_MemberJournal]
ON [jourlist].[Journals]
    ([Member_Id]);
GO

-- Creating foreign key on [Member_Id] in table 'Items_PersonalItem'
ALTER TABLE [jourlist].[Items_PersonalItem]
ADD CONSTRAINT [FK_MemberPersonalItem]
    FOREIGN KEY ([Member_Id])
    REFERENCES [jourlist].[Members]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_MemberPersonalItem'
CREATE INDEX [IX_FK_MemberPersonalItem]
ON [jourlist].[Items_PersonalItem]
    ([Member_Id]);
GO

-- Creating foreign key on [Member_Id] in table 'Inventories'
ALTER TABLE [jourlist].[Inventories]
ADD CONSTRAINT [FK_MemberInventory]
    FOREIGN KEY ([Member_Id])
    REFERENCES [jourlist].[Members]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_MemberInventory'
CREATE INDEX [IX_FK_MemberInventory]
ON [jourlist].[Inventories]
    ([Member_Id]);
GO

-- Creating foreign key on [Item_Id] in table 'ItemInfoes'
ALTER TABLE [jourlist].[ItemInfoes]
ADD CONSTRAINT [FK_ItemItemInfo]
    FOREIGN KEY ([Item_Id])
    REFERENCES [jourlist].[Items]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ItemItemInfo'
CREATE INDEX [IX_FK_ItemItemInfo]
ON [jourlist].[ItemInfoes]
    ([Item_Id]);
GO

-- Creating foreign key on [ItemInfo_Id] in table 'InventoryLogs'
ALTER TABLE [jourlist].[InventoryLogs]
ADD CONSTRAINT [FK_ItemInfoInventoryLog]
    FOREIGN KEY ([ItemInfo_Id])
    REFERENCES [jourlist].[ItemInfoes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ItemInfoInventoryLog'
CREATE INDEX [IX_FK_ItemInfoInventoryLog]
ON [jourlist].[InventoryLogs]
    ([ItemInfo_Id]);
GO

-- Creating foreign key on [Inventory_Id] in table 'ShoppingLists'
ALTER TABLE [jourlist].[ShoppingLists]
ADD CONSTRAINT [FK_InventoryShoppingList]
    FOREIGN KEY ([Inventory_Id])
    REFERENCES [jourlist].[Inventories]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_InventoryShoppingList'
CREATE INDEX [IX_FK_InventoryShoppingList]
ON [jourlist].[ShoppingLists]
    ([Inventory_Id]);
GO

-- Creating foreign key on [Unit_Id] in table 'Inventories'
ALTER TABLE [jourlist].[Inventories]
ADD CONSTRAINT [FK_InventoryUnit]
    FOREIGN KEY ([Unit_Id])
    REFERENCES [jourlist].[Units]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_InventoryUnit'
CREATE INDEX [IX_FK_InventoryUnit]
ON [jourlist].[Inventories]
    ([Unit_Id]);
GO

-- Creating foreign key on [UnitTypeUnit1_Unit_Id] in table 'Units'
ALTER TABLE [jourlist].[Units]
ADD CONSTRAINT [FK_UnitTypeUnit1]
    FOREIGN KEY ([UnitTypeUnit1_Unit_Id])
    REFERENCES [jourlist].[UnitTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UnitTypeUnit1'
CREATE INDEX [IX_FK_UnitTypeUnit1]
ON [jourlist].[Units]
    ([UnitTypeUnit1_Unit_Id]);
GO

-- Creating foreign key on [UnitId] in table 'InventoryLogs'
ALTER TABLE [jourlist].[InventoryLogs]
ADD CONSTRAINT [FK_UnitInventoryLog]
    FOREIGN KEY ([UnitId])
    REFERENCES [jourlist].[Units]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_UnitInventoryLog'
CREATE INDEX [IX_FK_UnitInventoryLog]
ON [jourlist].[InventoryLogs]
    ([UnitId]);
GO

-- Creating foreign key on [Unit_Id] in table 'Activities'
ALTER TABLE [jourlist].[Activities]
ADD CONSTRAINT [FK_ActivityUnit]
    FOREIGN KEY ([Unit_Id])
    REFERENCES [jourlist].[Units]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ActivityUnit'
CREATE INDEX [IX_FK_ActivityUnit]
ON [jourlist].[Activities]
    ([Unit_Id]);
GO

-- Creating foreign key on [Id] in table 'Items_PersonalItem'
ALTER TABLE [jourlist].[Items_PersonalItem]
ADD CONSTRAINT [FK_PersonalItem_inherits_Item]
    FOREIGN KEY ([Id])
    REFERENCES [jourlist].[Items]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Id] in table 'Items_StandardItem'
ALTER TABLE [jourlist].[Items_StandardItem]
ADD CONSTRAINT [FK_StandardItem_inherits_Item]
    FOREIGN KEY ([Id])
    REFERENCES [jourlist].[Items]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------