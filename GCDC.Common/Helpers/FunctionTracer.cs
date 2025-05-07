using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Reflection;

namespace GCDC.Common.Helpers
{
	public class FunctionTracer : IDisposable
    {
        private readonly Stopwatch _stopwatch;
        private readonly string _className;
        private readonly string _methodName;
        private readonly string _label;
        private readonly bool _loginfile;

        public FunctionTracer(bool loginfile = false, string alias = "")
        {
            // Create a StackTrace to examine the call stack
            StackTrace stackTrace = new StackTrace();

            // Get the calling method (the second frame in the stack trace)
            StackFrame frame = stackTrace.GetFrame(1);
            MethodBase method = frame.GetMethod();

            // Get the class name and method name
            _className = (method.DeclaringType.FullName.ToLower().Contains("aspnetcore")) ? method.DeclaringType.DeclaringType.Name : method.DeclaringType.Name; // Class name
            _methodName = !(method.DeclaringType.FullName.ToLower().Contains("aspnetcore")) ? method.Name : "ScreenBinding";              // Method name
            alias = !string.IsNullOrEmpty(alias) ? alias + " -> " : string.Empty;
            _label = $"{alias}{_className} -> {_methodName}";
            _loginfile = loginfile;
            if (_loginfile)
            {
                _stopwatch = Stopwatch.StartNew();
                Common._logger.Information($"{_label} - Execution Start");
            }
        }

        public void Dispose()
        {
            if (_loginfile)
            {
                _stopwatch.Stop();
                Common._logger.Information($"{_label} - Total Execution Time: {_stopwatch.Elapsed.TotalSeconds} sec(s)");
            }
        }
    }
}
