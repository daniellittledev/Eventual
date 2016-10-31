Create Table [EventSourcing].[EventStore] (
    [RowId] BigInt Not Null Identity(1,1),
    [Id] UniqueIdentifier Not Null Primary Key NonClustered,

    [AggregateId] UniqueIdentifier Not Null,
    [EventType] VarChar(128) Not Null,
    [EventData] NVarChar(Max) Not Null,

    [Version] Int,

    [CreatedDate] DateTimeOffset Not Null,
    [CreatedBy] NVarChar(128)
);

Create Clustered Index Index_RowId ON [EventSourcing].[EventStore]([RowId]);
Create Unique Index [EventStore_AggregateSequence_Unique] On [EventSourcing].[EventStore](AggregateId, [Version]);