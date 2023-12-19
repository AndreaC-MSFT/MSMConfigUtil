using MSMConfigUtil.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSMConfigUtil.CLI
{
    public class ConsoleUIHandler : IUserInterfaceHandler
    {
        public void ShowError(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("ERROR " + message);
            Console.ResetColor();
        }

        public void ShowError(Exception ex, string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("ERROR " + message);
            Console.WriteLine(ex.ToString());
            Console.ResetColor();
        }

        public void ShowInformation(string message)
        {
            Console.WriteLine(message);
        }

        public void ShowWarning(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("WARNING " + message);
            Console.ResetColor();
        }

        public void PromptForConfirmation()
        {
            Console.WriteLine("Press enter to continue");
            Console.ReadLine();
        }
    }
}
