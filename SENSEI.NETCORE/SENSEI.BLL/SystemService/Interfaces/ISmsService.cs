using System;
using System.Collections.Generic;
using System.Text;

namespace SENSEI.BLL.SystemService.Interfaces
{
    public interface ISmsService
    {
        Task<bool> SendSingleAsync(string phoneNumber, string message);
        Task<bool> SendBulkAsync(List<string> phoneNumbers, string message);
    }
}
