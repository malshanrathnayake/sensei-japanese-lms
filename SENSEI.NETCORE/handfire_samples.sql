/* =========================================================
   HANGFIRE MONITORING & DEBUG QUERIES
   ========================================================= */

------------------------------------------------------------
-- 1. ALL JOBS (LATEST FIRST)
------------------------------------------------------------
SELECT TOP 100
    j.Id,
    j.CreatedAt,
    j.InvocationData,
    j.Arguments
FROM Hangfire.Job j
ORDER BY j.Id DESC;


------------------------------------------------------------
-- 2. JOB STATES (SUCCESS / FAILED / PROCESSING)
------------------------------------------------------------
SELECT TOP 100
    s.JobId,
    s.Name AS State,
    s.Reason,
    s.CreatedAt
FROM Hangfire.State s
ORDER BY s.CreatedAt DESC;


------------------------------------------------------------
-- 3. JOB + STATE (BEST OVERVIEW)
------------------------------------------------------------
SELECT 
    j.Id,
    j.CreatedAt,
    s.Name AS State,
    s.Reason,
    s.CreatedAt AS StateChangedAt
FROM Hangfire.Job j
LEFT JOIN Hangfire.State s ON j.Id = s.JobId
ORDER BY j.Id DESC;


------------------------------------------------------------
-- 4. FAILED JOBS
------------------------------------------------------------
SELECT 
    j.Id,
    j.CreatedAt,
    s.Reason,
    s.CreatedAt AS FailedAt
FROM Hangfire.Job j
INNER JOIN Hangfire.State s ON j.Id = s.JobId
WHERE s.Name = 'Failed'
ORDER BY s.CreatedAt DESC;


------------------------------------------------------------
-- 5. SUCCESSFUL JOBS
------------------------------------------------------------
SELECT 
    j.Id,
    j.CreatedAt,
    s.CreatedAt AS SucceededAt
FROM Hangfire.Job j
INNER JOIN Hangfire.State s ON j.Id = s.JobId
WHERE s.Name = 'Succeeded'
ORDER BY s.CreatedAt DESC;


------------------------------------------------------------
-- 6. CURRENTLY PROCESSING JOBS
------------------------------------------------------------
SELECT *
FROM Hangfire.Processing;


------------------------------------------------------------
-- 7. QUEUED (PENDING) JOBS
------------------------------------------------------------
SELECT *
FROM Hangfire.Queue;


------------------------------------------------------------
-- 8. ACTIVE SERVERS
------------------------------------------------------------
SELECT *
FROM Hangfire.Server;


------------------------------------------------------------
-- 9. RECURRING JOB LIST
------------------------------------------------------------
SELECT *
FROM Hangfire.[Set]
WHERE [Key] = 'recurring-jobs';


------------------------------------------------------------
-- 10. RECURRING JOB DETAILS
------------------------------------------------------------
SELECT *
FROM Hangfire.[Hash]
WHERE [Key] LIKE 'recurring-job:%';


------------------------------------------------------------
-- 11. JOB METHOD + ARGUMENTS (DETAILED)
------------------------------------------------------------
SELECT 
    j.Id,
    j.InvocationData,
    j.Arguments
FROM Hangfire.Job j
ORDER BY j.Id DESC;


------------------------------------------------------------
-- 12. RETRIES / ATTEMPTS (STATE HISTORY)
------------------------------------------------------------
SELECT 
    JobId,
    Name,
    Reason,
    CreatedAt
FROM Hangfire.State
ORDER BY JobId, CreatedAt DESC;


------------------------------------------------------------
-- 13. DELETE OLD SUCCEEDED JOBS (OPTIONAL CLEANUP)
------------------------------------------------------------
-- ⚠️ Use carefully in production
-- DELETE FROM Hangfire.Job
-- WHERE Id IN (
--     SELECT j.Id
--     FROM Hangfire.Job j
--     INNER JOIN Hangfire.State s ON j.Id = s.JobId
--     WHERE s.Name = 'Succeeded'
--     AND s.CreatedAt < DATEADD(DAY, -7, GETUTCDATE())
-- );


------------------------------------------------------------
-- 14. DELETE OLD FAILED JOBS (OPTIONAL CLEANUP)
------------------------------------------------------------
-- DELETE FROM Hangfire.Job
-- WHERE Id IN (
--     SELECT j.Id
--     FROM Hangfire.Job j
--     INNER JOIN Hangfire.State s ON j.Id = s.JobId
--     WHERE s.Name = 'Failed'
--     AND s.CreatedAt < DATEADD(DAY, -30, GETUTCDATE())
-- );


------------------------------------------------------------
-- END OF SCRIPT
------------------------------------------------------------