CREATE PROC [dbo].[sp_RoleModule_UpdateModule]
@RoleId INT,
@ModulesId NVARCHAR(max)
AS
DECLARE @queryDelete NVARCHAR(MAX);
SET @queryDelete = 'DELETE	FROM dbo.RoleModule 
					WHERE	RoleId = @RoleId ';
IF ISNULL(@ModulesId,'') <> ''
SET @queryDelete = @queryDelete + '
									AND NOT EXISTS (
														SELECT	*
														FROM dbo.SplitString(@ModulesId,'','') 
														WHERE ID = ModuleId
													)';

EXECUTE sp_executesql @queryDelete,N'@RoleId INT,@ModulesId NVARCHAR(max)',@RoleId,@ModulesId

IF ISNULL(@ModulesId,'') <> ''
BEGIN
	INSERT INTO dbo.RoleModule
	( 
		RoleId ,
		ModuleId ,
		[Add] ,
		Edit ,
		[View] ,
		[Delete] ,
		Import ,
		Export ,
		Upload ,
		Publish ,
		Report ,
		Sync ,
		Accept ,
		Cancel ,
		Record
	)
	SELECT
		@RoleId,
		ID,
		1,
		1,
		1,
		1,
		1,
		1,
		1,
		1,
		1,
		1,
		1,
		1,
		1
	FROM dbo.SplitString(@ModulesId,',')
	WHERE NOT EXISTS
				(
					SELECT * 
					FROM dbo.RoleModule
					WHERE	RoleId = @RoleId 
							AND ModuleId = ID
				)
END