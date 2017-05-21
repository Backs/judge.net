IF (NOT EXISTS(SELECT 1 FROM dbo.DatabasePatches WHERE Name = N'OpenTask'))
BEGIN
    
    UPDATE u
    SET
        u.IsOpened = 1
    FROM Tasks as u

    INSERT INTO dbo.DatabasePatches (Name) VALUES (N'OpenTask')
END