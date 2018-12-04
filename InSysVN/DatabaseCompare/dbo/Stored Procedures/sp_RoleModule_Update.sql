-- =============================================
-- Author:		Changngoktb
-- Create date: 22/05/2018
-- Description:	update when create and update Order
-- =============================================
CREATE PROCEDURE [dbo].[sp_RoleModule_Update]
	@xml			XML
AS
BEGIN
	UPDATE dbo.RoleModule
	SET [Add]				=		x.[Add]	
		,[Edit]				=		x.[Edit]	
		,[View]				=		x.[View]	
		,[Delete]			=		x.[Delete]		
		,[Import]			=		x.[Import]		
		,[Export]			=		x.[Export]		
		,[Upload]			=		x.[Upload]		
		,[Publish]			=		x.[Publish]
		,[Report]			=		x.[Report]		
		,[Sync]				=		x.[Sync]	
		,[Accept]			=		x.[Accept]		
		,[Cancel]			=		x.[Cancel]		
		,[Record]			=		x.[Record]		
	FROM dbo.RoleModule AS rm
	INNER JOIN 
	(
		SELECT
			[RoleId]		=		xmlTblRoleModule.value('(RoleId)[1]', 'int')
			,[ModuleId]		=		xmlTblRoleModule.value('(ModuleId)[1]', 'int')
		  
			,[Add]			=		xmlTblRoleModule.value('(Add)[1]', 'bit')
			,[Edit]			=		xmlTblRoleModule.value('(Edit)[1]', 'bit')
			,[View]			=		xmlTblRoleModule.value('(View)[1]', 'bit')
			,[Delete]		=		xmlTblRoleModule.value('(Delete)[1]', 'bit')
			,[Import]		=		xmlTblRoleModule.value('(Import)[1]', 'bit')
			,[Export]		=		xmlTblRoleModule.value('(Export)[1]', 'bit')
			,[Upload]		=		xmlTblRoleModule.value('(Upload)[1]', 'bit')
			,[Publish]		=		xmlTblRoleModule.value('(Publish)[1]', 'bit')
			,[Report]		=		xmlTblRoleModule.value('(Report)[1]', 'bit')
			,[Sync]			=		xmlTblRoleModule.value('(Sync)[1]', 'bit')
			,[Accept]		=		xmlTblRoleModule.value('(Accept)[1]', 'bit')
			,[Cancel]		=		xmlTblRoleModule.value('(Cancel)[1]', 'bit')
			,[Record]		=		xmlTblRoleModule.value('(Record)[1]', 'bit')
		FROM	@xml.nodes('/ArrayOfRoleModuleEntity/RoleModuleEntity') AS XRM(xmlTblRoleModule)
	)	X
	ON	rm.RoleId				=	X.RoleId
	AND rm.ModuleId				=	X.ModuleId
		
END