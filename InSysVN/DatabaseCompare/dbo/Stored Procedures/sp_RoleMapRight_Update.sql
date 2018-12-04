-- =============================================
-- Author:		<dkamphuoc>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_RoleMapRight_Update]
	@xml xml,
	@roleId int
AS
BEGIN

	delete RoleMapRight where RoleId = @roleId

	insert into RoleMapRight(
		RoleId,
		RightId
	)
	SELECT 
		@roleId,
		RightId						=	xmlTable.value('Id								[1]', 'nvarchar(max)')
	FROM @xml.nodes('/ArrayOfRightEntity/RightEntity') AS XD(xmlTable)
END