using SENSEI.DOMAIN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace devspark_core_data_access_layer
{
    public class DataTransactionManager : IDisposable
    {
        private string _connectionString;

        public DataTransactionManager(string connectionString)
        {
            _connectionString = connectionString;
        }

        #region Admin Portal Manager

        private DataManager<Course> _courseDatamanager;
        public DataManager<Course> CourseDataManager
        {
            get
            {
                if (this._courseDatamanager == null)
                {
                    this._courseDatamanager = new DataManager<Course>(_connectionString);
                }

                return this._courseDatamanager;
            }
        }

        private DataManager<Lesson> _lessonDatamanager;
        public DataManager<Lesson> LessonDataManager
        {
            get
            {
                if (this._lessonDatamanager == null)
                {
                    this._lessonDatamanager = new DataManager<Lesson>(_connectionString);
                }

                return this._lessonDatamanager;
            }
        }

        private DataManager<Batch> _batchDatamanager;
        public DataManager<Batch> BatchDataManager
        {
            get
            {
                if (this._batchDatamanager == null)
                {
                    this._batchDatamanager = new DataManager<Batch>(_connectionString);
                }

                return this._batchDatamanager;
            }
        }

        private DataManager<UserNotification> _userNotificationDataManager;
        public DataManager<UserNotification> UserNotificationDataManager
        {
            get
            {
                if (this._userNotificationDataManager == null)
                {
                    this._userNotificationDataManager = new DataManager<UserNotification>(_connectionString);
                }

                return this._userNotificationDataManager;
            }
        }

        private DataManager<UserNotificationRead> _userNotificationReadDataManager;
        public DataManager<UserNotificationRead> UserNotificationReadDataManager
        {
            get
            {
                if (this._userNotificationReadDataManager == null)
                {
                    this._userNotificationReadDataManager = new DataManager<UserNotificationRead>(_connectionString);
                }

                return this._userNotificationReadDataManager;
            }
        }

        private DataManager<Country> _countryDataManager;
        public DataManager<Country> CountryDataManager
        {
            get
            {
                if (this._countryDataManager == null)
                {
                    this._countryDataManager = new DataManager<Country>(_connectionString);
                }

                return this._countryDataManager;
            }
        }

        private DataManager<State> _stateDataManager;
        public DataManager<State> StateDataManager
        {
            get
            {
                if (this._stateDataManager == null)
                {
                    this._stateDataManager = new DataManager<State>(_connectionString);
                }

                return this._stateDataManager;
            }
        }

        private DataManager<City> _cityDataManager;
        public DataManager<City> CityDataManager
        {
            get
            {
                if (this._cityDataManager == null)
                {
                    this._cityDataManager = new DataManager<City>(_connectionString);
                }

                return this._cityDataManager;
            }
        }

        private DataManager<Branch> _branchDataManager;
        public DataManager<Branch> BranchDataManager
        {
            get
            {
                if (this._branchDataManager == null)
                {
                    this._branchDataManager = new DataManager<Branch>(_connectionString);
                }

                return this._branchDataManager;
            }
        }

        private DataManager<StudentRegistration> _studentRegistrationDataManager;
        public DataManager<StudentRegistration> StudentRegistrationDataManager
        {
            get
            {
                if (this._studentRegistrationDataManager == null)
                {
                    this._studentRegistrationDataManager = new DataManager<StudentRegistration>(_connectionString);
                }

                return this._studentRegistrationDataManager;
            }
        }

        private DataManager<Student> _studentDataManager;
        public DataManager<Student> StudentDataManager
        {
            get
            {
                if (this._studentDataManager == null)
                {
                    this._studentDataManager = new DataManager<Student>(_connectionString);
                }

                return this._studentDataManager;
            }
        }

        private DataManager<StudentAddress> _studentAddressDataManager;
        public DataManager<StudentAddress> StudentAddressDataManager
        {
            get
            {
                if (this._studentAddressDataManager == null)
                {
                    this._studentAddressDataManager = new DataManager<StudentAddress>(_connectionString);
                }

                return this._studentAddressDataManager;
            }
        }

        private DataManager<User> _userDataManager;
        public DataManager<User> UserDataManager
        {
            get
            {
                if (this._userDataManager == null)
                {
                    this._userDataManager = new DataManager<User>(_connectionString);
                }

                return this._userDataManager;
            }
        }

        private DataManager<StudentBatch> _studentBatchDataManager;
        public DataManager<StudentBatch> StudentBatchDataManager
        {
            get
            {
                if (this._studentBatchDataManager == null)
                {
                    this._studentBatchDataManager = new DataManager<StudentBatch>(_connectionString);
                }

                return this._studentBatchDataManager;
            }
        }

        private DataManager<StudentBatchPayment> _studentBatchPaymentDataManager;
        public DataManager<StudentBatchPayment> StudentBatchPaymentDataManager
        {
            get
            {
                if (this._studentBatchPaymentDataManager == null)
                {
                    this._studentBatchPaymentDataManager = new DataManager<StudentBatchPayment>(_connectionString);
                }

                return this._studentBatchPaymentDataManager;
            }
        }

        private DataManager<StudentLearningMode> _studentLearningModeDataManager;
        public DataManager<StudentLearningMode> StudentLearningModeDataManager
        {
            get
            {
                if (this._studentLearningModeDataManager == null)
                {
                    this._studentLearningModeDataManager = new DataManager<StudentLearningMode>(_connectionString);
                }

                return this._studentLearningModeDataManager;
            }
        }

        private DataManager<Staff> _staffDataManager;
        public DataManager<Staff> StaffDataManager
        {
            get
            {
                if (this._staffDataManager == null)
                {
                    this._staffDataManager = new DataManager<Staff>(_connectionString);
                }

                return this._staffDataManager;
            }
        }

        private DataManager<NotificationMessage> _notificationMessageDataManager;
        public DataManager<NotificationMessage> NotificationMessageDataManager
        {
            get
            {
                if (this._notificationMessageDataManager == null)
                {
                    this._notificationMessageDataManager = new DataManager<NotificationMessage>(_connectionString);
                }

                return this._notificationMessageDataManager;
            }
        }

        private DataManager<EmployeeAddress> _employeeAddressDataManager;
        public DataManager<EmployeeAddress> EmployeeAddressDataManager
        {
            get
            {
                if (this._employeeAddressDataManager == null)
                {
                    this._employeeAddressDataManager = new DataManager<EmployeeAddress>(_connectionString);
                }

                return this._employeeAddressDataManager;
            }
        }

        private DataManager<BatchStudentLessonAccessRequest> _batchStudentLessonAccessRequestDataManager;
        public DataManager<BatchStudentLessonAccessRequest> BatchStudentLessonAccessRequestDataManager
        {
            get
            {
                if (this._batchStudentLessonAccessRequestDataManager == null)
                {
                    this._batchStudentLessonAccessRequestDataManager = new DataManager<BatchStudentLessonAccessRequest>(_connectionString);
                }

                return this._batchStudentLessonAccessRequestDataManager;
            }
        }

        private DataManager<BatchStudentLessonAccess> _batchStudentLessonAccessDataManager;
        public DataManager<BatchStudentLessonAccess> BatchStudentLessonAccessDataManager
        {
            get
            {
                if (this._batchStudentLessonAccessDataManager == null)
                {
                    this._batchStudentLessonAccessDataManager = new DataManager<BatchStudentLessonAccess>(_connectionString);
                }

                return this._batchStudentLessonAccessDataManager;
            }
        }

        private DataManager<BatchLesson> _batchLessonDataManager;
        public DataManager<BatchLesson> BatchLessonDataManager
        {
            get
            {
                if (this._batchLessonDataManager == null)
                {
                    this._batchLessonDataManager = new DataManager<BatchLesson>(_connectionString);
                }

                return this._batchLessonDataManager;
            }
        }

        private DataManager<StudentPaymentSummary> _studentBatchPaymentSummaryDataManager;
        public DataManager<StudentPaymentSummary> StudentBatchPaymentSummaryDataManager
        {
            get
            {
                if (this._studentBatchPaymentSummaryDataManager == null)
                {
                    this._studentBatchPaymentSummaryDataManager = new DataManager<StudentPaymentSummary>(_connectionString);
                }

                return this._studentBatchPaymentSummaryDataManager;
            }
        }

        #endregion

        private bool _disposed = false;
        protected void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {

                }
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
