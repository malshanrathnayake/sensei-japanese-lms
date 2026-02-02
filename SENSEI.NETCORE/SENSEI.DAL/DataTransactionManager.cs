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

        #region User Manager

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
