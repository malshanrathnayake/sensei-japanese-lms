using SENSEI.DOMAIN;
using System;
using System.Collections.Generic;
using System.Text;

namespace SENSEI.BLL.AdminPortalService.Interface
{
    public interface IBatchService
    {

        Task<(bool, long)> UpdateBatch(Batch batch);
        Task<Batch> GetBatch(long batchId);
        Task<(IEnumerable<Batch>, long)> SearchBatches(long courseId = 0, int start = 0, int length = 10, string searchValue = "", string sortColumn = "", string sortDirection = "");
        Task<bool> DeleteBatch(long batchId);
        Task<IEnumerable<Batch>> GetBatches(int courseId = 0);
    }
}
