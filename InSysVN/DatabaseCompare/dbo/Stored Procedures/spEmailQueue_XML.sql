CREATE PROC spEmailQueue_XML (@xml xml)
AS
BEGIN
			INSERT INTO EmailQueue (
			EmailType,
			EmailFrom,
			EmailTo,
			Subject,
			BodyHTML,
			AttachFile,
			isHTML,
			Status,
			CreatedDate,
			ModifiedDate
			)
			SELECT	
					EmailType = tblxml.value('(EmailType)[1]','int'),
					EmailFrom = tblxml.value('(EmailFrom)[1]','NVARCHAR(500)'),
					EmailTo = tblxml.value('(EmailTo)[1]','NVARCHAR(500)'),
					Subject = tblxml.value('(Subject)[1]','NVARCHAR(1000)'),
					BodyHTML = tblxml.value('(BodyHTML)[1]','NVARCHAR(MAX)'),
					AttachFile = tblxml.value('(AttachFile)[1]','NVARCHAR(500)'),
					isHTML = tblxml.value('(isHTML)[1]','BIT'),
					Status = tblxml.value('(Status)[1]','INT'),
					GETDATE(),
					GETDATE()
			FROM @xml.nodes('/EmailQueueModel') AS XD(tblxml)
END