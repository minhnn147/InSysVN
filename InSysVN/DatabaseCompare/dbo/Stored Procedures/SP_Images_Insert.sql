CREATE PROC SP_Images_Insert
@ImagesPath NVARCHAR(max)
as
BEGIN
	INSERT INTO Images
		(ImagePath)
	VALUES (@ImagesPath)
END