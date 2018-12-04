-- =============================================
-- Author:		<dkamphuoc>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_Right_UpdateModuleIdAndSort]
	@xml xml
AS
BEGIN
	update
		r
	set
		r.ModuleId		=	temp.ModuleId,
		r.Sorting		=	temp.Sorting
	from [Right] r
	inner join (
		SELECT 
			Id							=	xmlTable.value('Id								[1]', 'nvarchar(max)')
			,Name						=	xmlTable.value('Name							[1]', 'nvarchar(max)')
			,ModuleId					=	xmlTable.value('ModuleId						[1]', 'nvarchar(max)')
			,Sorting					=	xmlTable.value('Sorting							[1]', 'nvarchar(max)')
		FROM @xml.nodes('/ArrayOfRightEntity/RightEntity') AS XD(xmlTable)
	) temp on temp.Id = r.Id
END