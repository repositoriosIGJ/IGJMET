using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Domain.Contracts.Common;
using log4net;

namespace Utils
{
    public class Logger: ILogger
    {
        private ILog _logger;

        public Logger()
        {
            _logger = LogManager.GetLogger("MetLog");
        }

        public void Debug(Exception ex)
        {
            _logger.Debug(ex);
        }

        public void Debug(string mensaje)
        {
            _logger.Debug(mensaje);
        }

        public void Info(Exception ex)
        {
            _logger.Info(ex);
        }

        public void Info(string mensaje)
        {
            _logger.Info(mensaje);
        }

        public void Error(Exception ex)
        {
            _logger.Error(ex);
        }

        public void Error(string mensaje)
        {
            _logger.Error(mensaje);
        }
    }
}
