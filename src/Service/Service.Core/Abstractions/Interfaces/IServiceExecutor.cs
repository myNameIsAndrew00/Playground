using Service.Core.Abstractions.Structures;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.Core.Abstractions.Interfaces
{
    /// <summary>
    /// Provide methods to execute service actions
    /// </summary>
    public interface IServiceExecutor
    {
        void SetDispatcherResult(DispatchResult dispatchResult);
    }
}
