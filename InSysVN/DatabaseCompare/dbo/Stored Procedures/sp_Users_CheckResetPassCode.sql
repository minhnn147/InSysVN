create PROC [dbo].[sp_Users_CheckResetPassCode] @Code nvarchar(500)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM [dbo].[Users]
    WHERE ResetPassCode = @Code
END