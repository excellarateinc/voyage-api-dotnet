CREATE PROCEDURE [dbo].[ActivityAuditInsert]
	@RequestId nvarchar(64),
	@Method nvarchar(32),
	@Path nvarchar(128),
	@IpAddress nvarchar(64),
	@Date datetime,
	@StatusCode int,
	@Error nvarchar(max),
	@UserName nvarchar(50)
AS

INSERT INTO [dbo].[ActivityAudit]
	([RequestId]
	,[Method]
	,[Path]
	,[IpAddress]
	,[Date]
	,[StatusCode]
	,[Error]
	,[UserName])
VALUES
	(@RequestId
	,@Method
	,@Path
	,@IpAddress
	,@Date
	,@StatusCode
	,@Error
	,@UserName)

RETURN 0
GO