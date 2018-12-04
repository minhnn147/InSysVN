--changngoktb
CREATE proc [dbo].[sp_Users_InsertOrUpdate]
@strxml XML,
@UserId INT,
@Prefix NVARCHAR(MAX)
as
BEGIN
	IF @userId IS NOT NULL
	BEGIN
	--UPDATE
		UPDATE Users
		SET
			[Address]		=	tblxml.[Address]
			,[Birthday]		=	tblxml.[Birthday]
			,[ModifiedDate]	=	tblxml.[ModifiedDate]
			,[Email]		=	tblxml.[Email]
			,[FullName]		=	tblxml.[FullName]
			,[Gender]		=	tblxml.[Gender]
			,[Phone]		=	tblxml.[Phone]
			,[RoleId]		=	tblxml.[RoleId]
			,[UserName]		=	tblxml.[UserName]
		FROM Users as u
		JOIN 
		(
			SELECT
				[Address]		=	tbl.value('(Address)[1]','NVARCHAR(500)')
				,[Birthday]		=	tbl.value('(Birthday)[1]','DATETIME')
				,[ModifiedDate]	=	GETDATE()
				,[Email]		=	tbl.value('(Email)[1]','NVARCHAR(255)')
				,[FullName]		=	tbl.value('(FullName)[1]','NVARCHAR(255)')
				,[Gender]		=	tbl.value('(Gender)[1]','BIT')
				,[Phone]		=	tbl.value('(Phone)[1]','NVARCHAR(50)')
				,[RoleId]		=	tbl.value('(RoleId)[1]','int')
				,[UserName]		=	tbl.value('(UserName)[1]','NVARCHAR(50)')
				,[Id]			=	@userId
			FROM @strxml.nodes('/UserEntity') as xd(tbl)
		) as tblxml
		on u.Id=tblxml.Id
		where u.Id = @UserId
	END
	ELSE
	BEGIN
		---insert
		INSERT INTO Users
		(
			[UserCode]
			,[Address]
			,[Birthday]
			,[CreatedDate]
			,[Email]
			,[FullName]
			,[Gender]
			,[Password]
			,[Phone]
			,[RoleId]
			,[UserName]
			,isActive
		)
		SELECT
			(SELECT	CASE
							WHEN (SELECT TOP 1 1 FROM dbo.Users WHERE UserCode LIKE ''+@Prefix+'%'+'') IS NULL
							THEN
								@Prefix + '000000001'
							ELSE @Prefix + FORMAT(MAX(CONVERT(BIGINT,REPLACE(UserCode,@Prefix,''))) +1,'000000000')
						END
				FROM dbo.Users
				WHERE UserCode LIKE ''+@Prefix+'%'+'')
			,[Address]		=	tbl.value('(Address)[1]','NVARCHAR(500)')
			,[Birthday]		=	tbl.value('(Birthday)[1]','DATETIME')
			,[CreatedDate]	=	GETDATE()
			,[Email]		=	tbl.value('(Email)[1]','NVARCHAR(255)')
			,[FullName]		=	tbl.value('(FullName)[1]','NVARCHAR(255)')
			,[Gender]		=	tbl.value('(Gender)[1]','BIT')
			,[Password]		=	tbl.value('(Password)[1]','NVARCHAR(255)')
			,[Phone]		=	tbl.value('(Phone)[1]','NVARCHAR(50)')
			,[RoleId]		=	tbl.value('(RoleId)[1]','int')
			,[UserName]		=	tbl.value('(UserName)[1]','NVARCHAR(50)')
			,1
		FROM @strxml.nodes('/UserEntity') as xd(tbl)
		SET @userId = SCOPE_IDENTITY();
	END
	SELECT TOP 1 * FROM dbo.Users WHERE Id = @userId AND isActive = 1
END