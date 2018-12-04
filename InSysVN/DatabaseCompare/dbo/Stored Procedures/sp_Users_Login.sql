CREATE PROC [dbo].[sp_Users_Login] 
@UserName NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT	TOP 1 
			u.Id ,
            u.UserCode ,
            u.FullName ,
            u.UserName ,
            u.Password ,
            u.Gender ,
            u.Address ,
            u.Phone ,
            u.Birthday ,
            u.CreatedDate ,
            u.ModifiedDate ,
            u.Email ,
            u.AvatarImg ,
            u.ResetPassCode ,
            u.RoleId ,
            u.isActive
			,RoleLevel = r.[Level]
    FROM dbo.Users u
	JOIN dbo.Roles r ON u.RoleId = r.Id
    WHERE [UserName] = @UserName AND u.isActive = 1
END