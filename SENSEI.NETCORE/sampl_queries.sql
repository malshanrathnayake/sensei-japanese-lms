--select
SELECT
    SR.PhoneNo AS OldPhoneNo,
    X.NewPhoneNo
FROM dbo.StudentRegistration SR
CROSS APPLY
(
    SELECT
        CASE
            WHEN SR.PhoneNo IS NULL OR LTRIM(RTRIM(SR.PhoneNo)) = '' THEN SR.PhoneNo
            WHEN LEFT(LTRIM(RTRIM(SR.PhoneNo)), 3) = '+94' THEN SR.PhoneNo
            ELSE
                '+94' +
                CASE
                    WHEN LEFT(REPLACE(SR.PhoneNo, ' ', ''), 1) = '0'
                        THEN STUFF(REPLACE(SR.PhoneNo, ' ', ''), 1, 1, '')
                    ELSE REPLACE(SR.PhoneNo, ' ', '')
                END
        END AS NewPhoneNo
) X;


--update
UPDATE SR
SET SR.PhoneNo = X.NewPhoneNo
FROM dbo.StudentRegistration SR
CROSS APPLY
(
    SELECT
        CASE
            WHEN SR.PhoneNo IS NULL OR LTRIM(RTRIM(SR.PhoneNo)) = '' THEN SR.PhoneNo
            WHEN LEFT(LTRIM(RTRIM(SR.PhoneNo)), 3) = '+94' THEN SR.PhoneNo
            ELSE
                '+94' +
                CASE
                    WHEN LEFT(REPLACE(SR.PhoneNo, ' ', ''), 1) = '0'
                        THEN STUFF(REPLACE(SR.PhoneNo, ' ', ''), 1, 1, '')
                    ELSE REPLACE(SR.PhoneNo, ' ', '')
                END
        END AS NewPhoneNo
) X;