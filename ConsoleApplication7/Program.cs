using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
namespace ConsoleApplication7
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ATC atc = new ATC();
            MenuATC menu = new MenuATC(atc);
            menu.StartMenu();
        }
    }
}




/*Console.WriteLine("Добро пожаловать в наш сервис АТС Бархатный звонок. Выберите нужную операцию.");
Console.WriteLine(@"
1) Заключить договор. 
2) Изменить состояние телефона. 
3) Узнать состояние телефона. 
4) Изменить тарифный план. 
5) Посмотреть подключенный тарифный план. 
6) Провести оплату.
7) Отчет по звонкам");
Console.ReadLine();*/