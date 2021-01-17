using System.Collections.Generic;

namespace VSConsole
{
    public static class ListVSConsoleActionsExtensions
    {
        public static void Add(this List<VSConsoleAction> list, VSConsoleActionType actionType, string value = "")
        {
            list.Add(new VSConsoleAction { ActionType = actionType, Value = value });
        }
    }
}
