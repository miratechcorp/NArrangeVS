using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Shell.Interop;
using NArrange.Core;

namespace NArrangeVS
{
    public class OutputPaneLogger : ILogger
    {
        private readonly Lazy<IVsOutputWindowPane> outputPane;

        public OutputPaneLogger(NArrangeVSPackage package)
        {
            outputPane = new Lazy<IVsOutputWindowPane>(() => package.GetOutputPane(new Guid("8e253769-b9af-405c-9d30-386e23d8234f"), "NArrange"));
        }

        public void LogMessage(LogLevel level, string message, params object[] args)
        {
            switch (level) {
                case LogLevel.Error:
                    outputPane.Value.OutputString("Error:");
                    outputPane.Value.OutputString(Environment.NewLine);
                    outputPane.Value.OutputString(message);
                    outputPane.Value.OutputString(Environment.NewLine);
                    outputPane.Value.OutputString(Environment.NewLine);
                    break;
                case LogLevel.Warning:
                    outputPane.Value.OutputString("Warning:");
                    outputPane.Value.OutputString(Environment.NewLine);
                    outputPane.Value.OutputString(message);
                    outputPane.Value.OutputString(Environment.NewLine);
                    outputPane.Value.OutputString(Environment.NewLine);
                    break;
                case LogLevel.Info:
                case LogLevel.Trace:
                default:
                    // Do nothing.
                    break;
            }
        }
    }
}