using System;
using System.Collections.Generic;
using System.Text;

namespace SENSEI.DOMAIN
{
    public class StudentBatchLessonView
    {
        public long StudentId { get; set; }
        public long BatchLessonId { get; set; }
        public bool IsCompleted { get; set; }

        #region Navigational Properties
        public Student Student { get; set; }
        public BatchLesson BatchLesson { get; set; }
        #endregion
    }
}
