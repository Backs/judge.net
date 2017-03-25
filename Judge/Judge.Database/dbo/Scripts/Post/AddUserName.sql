IF (NOT EXISTS(SELECT 1 FROM dbo.DatabasePatches WHERE Name = N'AddUserName'))
BEGIN
    
    UPDATE u
    SET
        u.Email = u.UserName
    FROM Users as u

    INSERT INTO dbo.DatabasePatches (Name) VALUES (N'AddUserName')
END