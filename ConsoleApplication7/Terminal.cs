using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication7
{
    // класс терминала АТС
    public class Terminal
    {
        public int Id { get; set; }
        public string Phone { get; set; }
        public bool IsConnected { get; set; }
        public bool IsInCall { get; set; }
        public int PortNumber { get; set; }

        //  информация о текущем звонке
        public string CurrentCallPhoneNumber { get; set; }
        public DateTime CallStartTime { get; set; }

        public event EventHandler<TerminalEventArgs> TerminalStateChanged;

        protected virtual void OnTerminalStateChanged(TerminalEventArgs e)
        {
            TerminalStateChanged?.Invoke(this, e);
        }


        public void ChangeState(bool isConnected, bool isInCall)
        {
            IsConnected = isConnected;
            IsInCall = isInCall;
            OnTerminalStateChanged(new TerminalEventArgs(this));
        }

        public void StartCall(string callingPhone, string targetPhone, ATC atc)
        {
            if (callingPhone == targetPhone)
            {
                Console.WriteLine("Невозможно позвонить самому себе.");
                return;
            }

            int indexCallingPhone = atc.Terminals.FindIndex(t => t.Phone == callingPhone);
            int indexTargetPhone = atc.Terminals.FindIndex(t => t.Phone == targetPhone);

            if (indexCallingPhone >= 0 && indexTargetPhone >= 0)
            {
                Terminal callingTerminal = atc.Terminals[indexCallingPhone];
                Terminal targetTerminal = atc.Terminals[indexTargetPhone];


                if (callingTerminal.IsConnected && !callingTerminal.IsInCall && targetTerminal.IsConnected)
                {
                    callingTerminal.CurrentCallPhoneNumber = targetPhone;
                    callingTerminal.CallStartTime = DateTime.Now;
                    callingTerminal.IsInCall = true;

                    targetTerminal.CurrentCallPhoneNumber = callingPhone;
                    targetTerminal.IsInCall = true;

                    Console.WriteLine($"Начат звонок с номера {callingPhone} на номер {targetPhone}");

                    targetTerminal.ChangeState(targetTerminal.IsConnected, targetTerminal.IsInCall);
                }
                else if (!targetTerminal.IsConnected)
                {
                    Console.WriteLine($"Невозможно начать звонок. Терминал {targetPhone} не подключен.");
                }
                else
                {
                    Console.WriteLine("Невозможно начать звонок в текущем состоянии.");
                }
            }
            else
            {
                Console.WriteLine("Терминал с указанным номером не найден.");
            }
        }

        public void EndCall()
        {
            if (IsInCall)
            {
                TimeSpan callDuration = DateTime.Now - CallStartTime;
                Console.WriteLine($"Завершен звонок на номер {CurrentCallPhoneNumber}. Продолжительность звонка: {callDuration.TotalSeconds} сек.");
                CurrentCallPhoneNumber = null;
                IsInCall = false;

                if (IsConnected)
                {
                    ChangeState(IsConnected, IsInCall);
                }
            }
            else
            {
                Console.WriteLine("Нет активного звонка для завершения.");
            }
        }

    }
}
