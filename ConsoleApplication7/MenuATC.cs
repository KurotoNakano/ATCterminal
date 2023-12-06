using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleApplication7
{
    public class MenuATC
    {
        private bool isLoggedIn = false;
        private string loginPhone = "";
        private string phoneToCall = "";
        private ATC atc;

        public MenuATC(ATC atc)
        {
            this.atc = atc;
            atc.ContractSigned += HandleContractSigned;
        }
        private void HandleContractSigned(object sender, ContractSignedEventArgs e)
        {
            Console.WriteLine($"Договор подписан с клиентом: {e.NewClient.Name}, Номер телефона: {e.NewClient.Phone}");
        }

        public void StartMenu()
        {
            while (true)
            {
                if (!isLoggedIn)
                {
                    Console.WriteLine("Добро пожаловать в наш сервис АТС Бархатный звонок. Выберите нужную операцию.");
                    Console.WriteLine("1) Вход в кабинет.");
                    Console.WriteLine("2) Заключить договор.");

                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            // Вход в кабинет
                            Console.WriteLine("Введите номер телефона для входа в кабинет:");
                            loginPhone = Console.ReadLine();
                            if (atc.Clients.Any(c => c.Phone == loginPhone))
                            {
                                isLoggedIn = true;
                                Console.WriteLine("Вход выполнен. Можете начать работу в кабинете.");
                            }
                            else
                            {
                                Console.WriteLine("Пользователь с указанным номером не найден.");
                            }
                            break;
                        case "2":
                            // Заключить договор
                            Console.WriteLine("Введите имя клиента:");
                            string name = Console.ReadLine();
                            Console.WriteLine("Введите номер телефона:");
                            string phone = Console.ReadLine();
                            Console.WriteLine("Выберите тарифный план (Basic/Premium/Business):");
                            string selectedTariffPlan = Console.ReadLine();
                            atc.RegisterClient(name, phone, selectedTariffPlan);
                            isLoggedIn = false;
                            atc.ContractSigned -= HandleContractSigned;
                            break;
                        default:
                            Console.WriteLine("Неправильный выбор. Попробуйте снова.");
                            break;
                    }
                }
                else
                {
                    // Меню для вошедших пользователей
                    Console.WriteLine("Выберите нужную операцию:");
                    Console.WriteLine("3) Изменить состояние телефона.");
                    Console.WriteLine("4) Узнать состояние телефона.");
                    Console.WriteLine("5) Изменить тарифный план.");
                    Console.WriteLine("6) Посмотреть подключенный тарифный план.");
                    Console.WriteLine("7) Провести оплату.");
                    Console.WriteLine("8) Начать звонок.");
                    Console.WriteLine("9) Завершить звонок.");
                    Console.WriteLine("10) Выйти из программы.");
                    Console.WriteLine("0) Вбить в базу.");

                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "3":
                            Console.WriteLine($"Ваш номер телефона: {loginPhone}");
                            string phoneToChangeState = loginPhone;
                            int indexToChangeState = atc.Terminals.FindIndex(t => t.Phone == phoneToChangeState);
                            if (indexToChangeState >= 0)
                            {
                                Console.WriteLine("Терминал находится в режиме подключения (true/false):");
                                bool isConnected = bool.Parse(Console.ReadLine());

                                bool isInCall = false;

                                atc.Terminals[indexToChangeState].ChangeState(isConnected, isInCall);
                            }
                            else
                            {
                                Console.WriteLine("Терминал с указанным номером не найден.");
                            }
                            break;
                        case "4":
                            Console.WriteLine($"Ваш номер телефона: {loginPhone}");
                            string phoneToCheckState = loginPhone;
                            Terminal terminalToCheckState = atc.Terminals.Find(t => t.Phone == phoneToCheckState);
                            if (terminalToCheckState != null)
                            {
                                Console.WriteLine($"Терминал {phoneToCheckState} в состоянии подключения: {terminalToCheckState.IsConnected}");
                                Console.WriteLine($"Терминал {phoneToCheckState} звонит: {terminalToCheckState.IsInCall}");
                            }
                            else
                            {
                                Console.WriteLine("Терминал с указанным номером не найден.");
                            }
                            break;
                        case "5":
                            Console.WriteLine($"Ваш номер телефона: {loginPhone}");
                            string phoneToChangeTariff = loginPhone;
                            int indexToChangeTariff = atc.Clients.FindIndex(c => c.Phone == phoneToChangeTariff);
                            if (indexToChangeTariff >= 0)
                            {
                                Console.WriteLine("Введите новый тарифный план (Basic/Premium/Business):");
                                string newTariffPlan = Console.ReadLine();
                                atc.Clients[indexToChangeTariff].TariffPlan = newTariffPlan;
                                Console.WriteLine($"Тарифный план для клиента {phoneToChangeTariff} изменен на {newTariffPlan}.");
                            }
                            else
                            {
                                Console.WriteLine("Клиент с указанным номером не найден.");
                            }
                            break;
                        case "6":
                            Console.WriteLine($"Ваш номер телефона: {loginPhone}");
                            string phoneToCheckTariff = loginPhone;
                            Client clientToCheckTariff = atc.Clients.Find(c => c.Phone == phoneToCheckTariff);
                            if (clientToCheckTariff != null)
                            {
                                Console.WriteLine($"Клиент {phoneToCheckTariff} подключен к тарифному плану: {clientToCheckTariff.TariffPlan}");
                            }
                            else
                            {
                                Console.WriteLine("Клиент с указанным номером не найден.");
                            }
                            break;
                        case "7":
                            Console.WriteLine($"Ваш номер телефона: {loginPhone}");
                            string phoneToPay = loginPhone;
                            int indexToPay = atc.Clients.FindIndex(c => c.Phone == phoneToPay);
                            if (indexToPay >= 0)
                            {
                                Console.WriteLine("Введите сумму оплаты:");
                                decimal paymentAmount = decimal.Parse(Console.ReadLine());
                                atc.Clients[indexToPay].Balance += paymentAmount;
                                Console.WriteLine($"Оплата для клиента {phoneToPay} проведена. Новый баланс: {atc.Clients[indexToPay].Balance}");
                            }
                            else
                            {
                                Console.WriteLine("Клиент с указанным номером не найден.");
                            }
                            break;
                        case "8":
                            Console.WriteLine("Введите номер телефона, на который будете звонить:");
                            phoneToCall = Console.ReadLine();
                            int indexToCall = atc.Terminals.FindIndex(t => t.Phone == phoneToCall);

                            if (indexToCall >= 0)
                            {
                                atc.Terminals[indexToCall].StartCall(loginPhone, phoneToCall, atc);

                                
                                atc.UpdateTerminalState(loginPhone);

                            }
                            else
                            {
                                Console.WriteLine("Терминал с указанным номером не найден.");
                            }
                            break;
                        case "9":
                            Console.WriteLine("Завершение звонка");
                            string phoneToEndCall = loginPhone;
                            int indexToEndCall = atc.Terminals.FindIndex(t => t.Phone == phoneToEndCall);

                            if (indexToEndCall >= 0)
                            {
                                atc.Terminals[indexToEndCall].EndCall();

                                
                                int indexToEndCall2 = atc.Terminals.FindIndex(t => t.Phone == phoneToCall);
                                if (indexToEndCall2 >= 0)
                                {
                                    atc.Terminals[indexToEndCall2].ChangeState(atc.Terminals[indexToEndCall2].IsConnected, false);
                                }
                                else
                                {
                                    Console.WriteLine("Терминал с указанным номером не найден.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Терминал с указанным номером не найден.");
                            }
                            break;
                        case "10":
                            Console.WriteLine("Программа завершена.");
                            isLoggedIn = false;
                            break;
                        case "0":
                            // Сохранение данных клиентов в файл и завершение программы
                            atc.SaveClientsToFile();
                            Console.WriteLine("Программа завершена.");
                            break;
                        default:
                            Console.WriteLine("Неправильный выбор. Попробуйте снова.");
                            break;
                    }
                }
            }
        }
    }
}
