CREATE PROC [dbo].[sp_Users_UpdateAvatar]
@Id INT,
@srcAvatar NVARCHAR(MAX)
AS
UPDATE dbo.Users
SET AvatarImg = @srcAvatar
WHERE Id = @Id