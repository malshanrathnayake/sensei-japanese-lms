using System;
using System.Collections.Generic;
using System.Text;

namespace SENSEI.DOMAIN
{
    public class BatchLessonReference
    {
        public long BatchLessonReferenceId { get; set; }
        public long BatchLessonId { get; set; }
        public string ReferenceUrl { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }

        #region navigation properties
        public BatchLesson BatchLesson { get; set; }
        #endregion
    }
}
