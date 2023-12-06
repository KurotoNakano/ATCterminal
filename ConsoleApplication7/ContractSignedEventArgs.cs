using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication7
{
    public class ContractSignedEventArgs : EventArgs
    {
        public Client NewClient { get; }

        public ContractSignedEventArgs(Client newClient)
        {
            NewClient = newClient;
        }
    }

}
