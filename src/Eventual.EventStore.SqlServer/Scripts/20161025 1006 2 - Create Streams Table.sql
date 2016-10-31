Create Table [EventSourcing].[Snapshot] (
    [RowId] BigInt Not Null Identity(1,1),
    [AggregateId] UniqueIdentifier Not Null Primary Key NonClustered,

    [Signature] VarBinary(Max) Not Null,
    [Snapshot] NVarChar(Max) Not Null,
    [Version] Int Not Null,
    [CreatedDate] DateTimeOffset Not Null
);

Create Clustered Index Index_RowId ON [EventSourcing].[Snapshot](RowId);
Create Index [Snapshot_TimeStamp_Index] On [EventSourcing].[Snapshot]([CreatedDate]);