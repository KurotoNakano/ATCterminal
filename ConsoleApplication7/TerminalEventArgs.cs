using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication7
{
    
    public class TerminalEventArgs : EventArgs
    {
        public Terminal Terminal { get; }

        public TerminalEventArgs(Terminal terminal)
        {
            Terminal = terminal;
        }
    }
}
