CREATE PROC [dbo].[sp_Category_Insert_Update]
@Id INT,
@Name nvarchar(250),
@Description NVARCHAR(max)
AS
BEGIN
	IF @Id IS NULL
	BEGIN
		INSERT INTO dbo.Category
		        ( Name ,
		          CreatedDate ,
		          isActive ,
		          ImagePath ,
		          Description
		        )
		VALUES  ( @Name , -- Name - nvarchar(250)
		          GETDATE() , -- CreatedDate - datetime
		          NULL , -- isActive - bit
		          N'' , -- ImagePath - nvarchar(max)
		          @Description  -- Description - nvarchar(max)
		        )
		SET @Id = SCOPE_IDENTITY();
	END
	ELSE
	BEGIN
		UPDATE dbo.Category
		SET	 Name = @Name
			,ModifiedDate = GETDATE()
		WHERE Id = @Id
	END
	PRINT @Id
	SELECT TOP 1 * FROM dbo.Category WHERE Id = @Id
END