-- =============================================
-- Author:		<dkamphuoc>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE procedure [dbo].[spTemplate_Get_VarValue]
	@TemplateID						int,
	@TypeID							int,
	@ID								nvarchar(200),
	@IDs							nvarchar(max),
	@IDCol							nvarchar(50),
	@TableName						nvarchar(50),
	@VarName						nvarchar(50),
	@Value							nvarchar(max)	out

as

	begin try

		declare	@SQL				nvarchar(max)
		
		set	@Value					= ''

		select
			top 1
			@SQL				= [SQL]
		from	TemplateVariable (nolock)
		where	Name				= @VarName and (TypeId = @TypeID or isnull(TypeId,0) = 0)
		order by case when  TypeId = @TypeID then 0 else  1 end

		if @SQL is not null
		begin
			exec sp_executesql @SQL, N'@TemplateID int, @IDs nvarchar(max), @ID nvarchar(200), @Value nvarchar(max) out', @TemplateID = @TemplateID, @IDs = @IDs, @ID = @ID, @Value = @Value out

			if @@rowcount > 0
				set	@Value			= isnull(@Value,'')

			return
		end

		-- second, get from master table
		if @TableName <> '' and @IDCol <> ''
			set	@SQL				= N'select @Value = ' + @VarName + ' from ' + @TableName + ' (nolock) where ' + @IDCol + ' = ''' + convert(varchar,@ID) + ''''

		--print @SQL
		exec sp_executesql @SQL, N'@ID nvarchar(200), @Value nvarchar(max) out', @ID = @ID, @Value = @Value out

	end try
	
	begin catch

		declare	@ErrorNum			int,
				@ErrorMsg			varchar(200),
				@ErrorProc			varchar(50),
				@SessionID			int,
				@AddlInfo			varchar(max)

		set @ErrorNum				= error_number()
		set @ErrorMsg				= 'em_Get_VarValue: ' + error_message()
		set @ErrorProc				= error_procedure()
		set	@AddlInfo				= '@ID=' + convert(varchar,@ID) + ',@IDCol=' + @IDCol + ',@TableName=' + @TableName + ',@VarName=' + @VarName + ',@Value=' + @Value + ',@TemplateID=' + convert(varchar,isnull(@TemplateID,0))

	end catch