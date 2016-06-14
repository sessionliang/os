using BaiRong.Model.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace BaiRong.Service.Execution
{
    public interface IExecution
    {
        bool Execute(TaskInfo taskInfo);
    }
}
