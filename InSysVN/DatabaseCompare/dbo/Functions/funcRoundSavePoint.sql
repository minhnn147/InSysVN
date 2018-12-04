CREATE FUNCTION [dbo].[funcRoundSavePoint](@pointsave float)
RETURNS bigint
AS 
BEGIN
	DECLARE	@intdigits INT;
	DECLARE @fractionaldigits FLOAT;
	DECLARE @Point bigint;
		
	SET @intdigits = FLOOR(@pointsave);
	SET @fractionaldigits = @pointsave - @intdigits;

	IF @fractionaldigits <= 0.5 SET @Point = @intdigits
	ELSE SET @Point = @intdigits + 1;

	RETURN @Point
END