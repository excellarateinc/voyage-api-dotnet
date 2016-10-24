using Launchpad.Models;
using System.Collections.Generic;

namespace Launchpad.Services.Interfaces
{
    public interface IRequestMetricsService
    {
        void Log(RequestDataPointModel model);
        IEnumerable<RequestDataPointModel> GetActivity();
    }
}
