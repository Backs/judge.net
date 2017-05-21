SET XACT_ABORT ON;  
BEGIN TRAN

    :r .\Post\AddUserName.sql                                
    :r .\Post\OpenTasks.sql        

COMMIT TRAN
