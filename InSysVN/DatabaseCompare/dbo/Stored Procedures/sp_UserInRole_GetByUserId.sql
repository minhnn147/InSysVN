CREATE PROC [dbo].[sp_UserInRole_GetByUserId] @UserId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT r.Id
        ,r.[Name]
    FROM [dbo].[Roles] r
    INNER JOIN [dbo].[UserInRole] ur ON r.Id = ur.RoleId
    WHERE ur.UserId = @UserId
END