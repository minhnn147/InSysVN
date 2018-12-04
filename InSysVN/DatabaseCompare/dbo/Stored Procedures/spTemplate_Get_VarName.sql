-- =============================================
-- Author:		<dkamphuoc>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spTemplate_Get_VarName]

	@Html						varchar(max),
	@Idx						int			out,
	@VarName					nvarchar(50)	out

as
begin
	declare	@EndIdx				int

	set	@VarName					= ''
	set	@Idx					= charindex( '{{', @Html, @Idx)
	if @Idx = 0
		return					-- no Var

	set	@EndIdx					= charindex( '}}', @Html, @Idx)

	set	@VarName				= substring( @Html, @Idx + 2, @EndIdx - @Idx - 2)
	set	@Idx					= @Idx + 2
end