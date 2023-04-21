Use "C23-E01-MSCQRSES"
GO
    
IF NOT EXISTS(SELECT * FROM sys.server_principals WHERE name = 'SmUser')
BEGIN
    CREATE LOGIN SmUser WITH PASSWORD = 'DevPasswordE01', DEFAULT_DATABASE = "C23-E01-MSCQRSES"
END

IF NOT EXISTS(SELECT * FROM sys.database_principals WHERE name = 'SmUser')
BEGIN
    EXEC sp_adduser 'SmUser', 'SmUser', 'db_owner';
END