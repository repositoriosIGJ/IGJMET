using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts.Common
{
    public interface ILogger
    {
        void Error(Exception ex);
        void Error(string mensaje);
        void Info(Exception ex);
        void Info(string mensaje);
        void Debug(Exception ex);
        void Debug(string mensaje);
    }
}
