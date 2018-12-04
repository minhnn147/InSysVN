CREATE PROC [dbo].[sp_Stall_GetOrSetStall]
@IpAddress VARCHAR(25),
@StallName NVARCHAR(50) OUT
AS
BEGIN
	SET @StallName = (SELECT [Name] FROM dbo.Stall WHERE IpAddress = @IpAddress);

	IF(ISNULL(@StallName,'')='')
	BEGIN

		SET @StallName = N'00'+CONVERT(NVARCHAR(50),ISNULL((SELECT MAX(Id)+1 FROM dbo.Stall),1));
		PRINT @StallName;

		INSERT INTO dbo.Stall
		(
			IpAddress,
			Name
		)
		VALUES
		(   @IpAddress,
			@StallName
		)

	END
END