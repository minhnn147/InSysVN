--changngoktb
CREATE PROC [dbo].[sp_Users_GetById]
@Id INT
AS
BEGIN
    SELECT TOP 1 u.Id ,
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
                 u.isActive,
				 RoleDisplayName	=	r.DisplayName
	FROM USERS u
	JOIN Roles r on r.Id=u.RoleId
	WHERE u.Id = @Id
END