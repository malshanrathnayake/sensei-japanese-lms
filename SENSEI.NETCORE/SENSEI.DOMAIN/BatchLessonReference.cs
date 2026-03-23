using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace SENSEI.DOMAIN
{
    public class BatchLessonReference
    {
        [DisplayName("Batch Lesson Reference")]
        public long BatchLessonReferenceId { get; set; }
        [DisplayName("Batch Lesson")]
        public long BatchLessonId { get; set; }
        [DisplayName("URL")]
        public string ReferenceUrl { get; set; }
        [DisplayName("Description")]
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
        public string EncryptedKey { get; set; }
        #region navigation properties
        public BatchLesson BatchLesson { get; set; }
        #endregion
    }
}
