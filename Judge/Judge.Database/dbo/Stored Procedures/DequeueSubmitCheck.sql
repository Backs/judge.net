CREATE PROCEDURE dbo.DequeueSubmitCheck
AS
BEGIN

	;WITH CTE(SubmitResultId, CreationDateUtc)
	AS
	(
		SELECT TOP 1 *
		FROM dbo.CheckQueue cq WITH(UPDLOCK, READPAST)
		ORDER BY cq.CreationDateUtc
	)
	DELETE CTE
	OUTPUT
		DELETED.*
END