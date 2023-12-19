using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSMConfigUtil.Logic
{
    public interface IUserInterfaceHandler
    {
        void PromptForConfirmation();
        void ShowInformation(string message);
        void ShowWarning(string message);
        void ShowError(string message);
        void ShowError(Exception ex, string message);
    }
}
