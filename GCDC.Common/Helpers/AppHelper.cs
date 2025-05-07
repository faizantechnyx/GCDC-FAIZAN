
namespace GCDC.Common.Helpers
{
    public static class AppHelper
    {
        public static bool IsValidCommand(string command)
        {
			// Whitelist of allowed commands
			var allowedCommands = new string[] { };
			return allowedCommands.Contains(command.ToLower());
        }

    }

}
