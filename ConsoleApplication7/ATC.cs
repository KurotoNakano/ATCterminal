using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleApplication7
{
    
    public class ATC
    {
        public delegate void ContractSignedEventHandler(object sender, ContractSignedEventArgs e);
        public event ContractSignedEventHandler ContractSigned;

        public List<Client> Clients { get; set; }
        public List<Terminal> Terminals { get; set; }
        public List<TariffPlan> AvailableTariffPlans { get; set; }
        public int NextClientId { get; set; }
        public int NextTerminalId { get; set; }
        public void LoadClientsFromFile()
        {
            if (File.Exists("clients.txt"))
            {
                string[] lines = File.ReadAllLines("clients.txt");
                foreach (var line in lines)
                {
                    string[] data = line.Split(';');
                    if (data.Length == 3)
                    {
                        string phone = data[0];
                        string name = data[1];
                        string tariffPlan = data[2];

                        RegisterClient(name, phone, tariffPlan);
                    }
                }
            }
        }
        public void SaveClientsToFile()
        {
            List<string> lines = new List<string>();
            foreach (var client in Clients)
            {
                lines.Add($"{client.Phone};{client.Name};{client.TariffPlan}");
            }
            File.WriteAllLines("clients.txt", lines);
        }

        public ATC()
        {
            Clients = new List<Client>();
            Terminals = new List<Terminal>();
            AvailableTariffPlans = new List<TariffPlan>
            {
                new TariffPlan { Name = "Basic", MonthlyFee = 10.0m },
                new TariffPlan { Name = "Premium", MonthlyFee = 20.0m },
                new TariffPlan { Name = "Business", MonthlyFee = 30.0m }
                
            };
            NextClientId = 1;
            NextTerminalId = 1;
            LoadClientsFromFile();
        }

        public void RegisterClient(string name, string phone, string selectedTariffPlan)
        {
            TariffPlan tariffPlan = AvailableTariffPlans.Find(plan => plan.Name == selectedTariffPlan);
            if (tariffPlan == null)
            {
                Console.WriteLine("Указанный тарифный план не найден.");
                return;
            }

            Client client = new Client
            {
                Id = NextClientId,
                Name = name,
                Phone = phone,
                Balance = 0.0m,
                TariffPlan = selectedTariffPlan
            };

            Clients.Add(client);

            Terminal terminal = new Terminal
            {
                Id = NextTerminalId,
                Phone = phone,
                IsConnected = false,
                IsInCall = false,
                PortNumber = NextTerminalId
            };
            NextTerminalId++;

            terminal.TerminalStateChanged += TerminalStateChangedHandler;
            Terminals.Add(terminal);

            OnContractSigned(new ContractSignedEventArgs(client));
        }
        protected virtual void OnContractSigned(ContractSignedEventArgs e)
        {
            ContractSigned?.Invoke(this, e);
        }
        public void UpdateTerminalState(string phone)
        {
            Terminal terminalToUpdate = Terminals.Find(t => t.Phone == phone);
            if (terminalToUpdate != null)
            {
                terminalToUpdate.ChangeState(terminalToUpdate.IsConnected, terminalToUpdate.IsInCall);
            }
        }
        private void TerminalStateChangedHandler(object sender, TerminalEventArgs e)
        {
            Terminal terminal = e.Terminal;

            if (terminal.IsInCall)
            {
                Console.WriteLine($"Терминал {terminal.Phone} подключен и состояние телефона: звонок.");
            }
            else if (terminal.IsConnected)
            {
                Console.WriteLine($"Терминал {terminal.Phone} подключен.");
            }
            else
            {
                Console.WriteLine($"Терминал {terminal.Phone} отключен.");
            }
        }
    }
}
