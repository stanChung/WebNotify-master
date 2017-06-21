# WebNotify
![ScreenShot](https://github.com/stanChung/WebNotify/blob/master/WebNotify_H.PNG)
  
### **功能描述：**
> - 使用 ASP.NET MVC/WebSocket/SqlDependecy
> - 可辨識出連接上SERVER的使用者
> - 可監視DB資料表的資料改變並通知遠端使用者

### **資料表建立：**
```sql
CREATE TABLE THICOM.[dbo].[TestMonitor](
	[DataID] [varchar](50) NOT NULL CONSTRAINT [DF_TestMonitor_DataID]  DEFAULT (newid()),
	[ReportName] [nvarchar](50) NULL,
	[ReportStatus] [nvarchar](50) NULL,
	[ReportOwner] [nvarchar](50) NULL,
	[IsComplete] [bit] NULL,
 CONSTRAINT [PK_TestMonitor] PRIMARY KEY CLUSTERED 
(
	[DataID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
```

### **開啟Service Broker：**
```sql
alter database THICOM set single_user with rollback immediate
go
alter database THICOM set enable_broker
go
alter database THICOM set multi_user
go
```
