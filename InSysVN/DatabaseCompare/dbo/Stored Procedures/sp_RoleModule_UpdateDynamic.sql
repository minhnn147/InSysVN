
CREATE PROCEDURE [dbo].[sp_RoleModule_UpdateDynamic]

	@roleId int,
	@moduleId int,
	@action varchar(50),
	@access bit
AS
BEGIN
	SET NOCOUNT ON;
	declare @sql nvarchar(4000)
	
	if (exists (select top 1 moduleId from [RoleModule] where RoleId = @roleId and ModuleId = @ModuleId))
	begin
		SET @sql = N' Update [RoleModule] SET  ['+@action+ ']=' + CAST( @access as varchar) +' WHERE RoleId = '+ CAST(@roleId as varchar) +' And ModuleId = '+ cast (@ModuleId as varchar);
	end
	else begin
		SET @sql = N' insert into [RoleModule]( ['+@action+ '],RoleId,ModuleId )   Values(' + CAST( @access as varchar) +' , '+ CAST(@roleId as varchar) +' ,'+ cast (@ModuleId as varchar) +')'
	end

	--Print @sql;
	EXECUTE sp_executesql @sql
    
END