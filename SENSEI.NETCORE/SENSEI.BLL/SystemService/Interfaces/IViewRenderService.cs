using System;
using System.Collections.Generic;
using System.Text;

namespace SENSEI.BLL.SystemService.Interfaces
{
    public interface IViewRenderService
    {
        Task<string> RenderViewToString<T>(string viewName, T model);
    }
}
