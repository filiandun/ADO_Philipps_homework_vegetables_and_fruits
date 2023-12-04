using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading;
using System.Linq.Expressions;

namespace VegetablesFruits
{

    internal class Program
    {
        const byte MINMENU = 0;
        const byte MAXMENU = 14;

        byte currentMenu = 0;

        SqlConnection sqlConnection;

        static void Main(string[] args)
        {
            Program program = new Program();

            program.OpenConnection(); program.PressToContinue();

            while (true)
            {
                program.ShowMenu();
                program.Navigation();
            }
        }

        public string InputStr()
        {
            string str;

            try
            {
                Console.Write("Введите желаемый цвет: ");
                str = Console.ReadLine();

                if (String.IsNullOrEmpty(str))
                {
                    throw new ArgumentNullException(nameof(str));
                }

                return str;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine(ex.Message + "\n"); Console.ResetColor();
                this.InputStr();
            }

            return null;
        }

        public int InputNum(string text)
        {
            int num;

            try
            {
                Console.Write(text);

                if (!int.TryParse(Console.ReadLine(), out num))
                {
                    throw new Exception("Вы ввели не число!");
                }

                return num;
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine(ex.Message + "\n"); Console.ResetColor();
                this.InputNum(text);
            }

            return 0;
        }

        public void Do()
        {
            string colour;
            int calories;
            int calories2;

            Console.Clear();
            switch (this.currentMenu)
            {
                case 0: this.ReadData("SELECT Id AS 'ИД', Name AS 'Название', Type AS 'Тип', Colour AS 'Цвет', Calories AS 'Каллории' FROM VegetablesAndFruits"); break;
                case 1: this.ReadData("SELECT Name AS Название, Colour AS Цвет FROM VegetablesAndFruits"); break;
                case 2: this.ReadData("SELECT Colour AS Цвет FROM VegetablesAndFruits"); break;

                case 3: this.ReadData("SELECT MAX(Calories) AS 'Максимальная калорийность' FROM VegetablesAndFruits"); break;
                case 4: this.ReadData("SELECT MIN(Calories) AS 'Мнимальная калорийность' FROM VegetablesAndFruits"); break;
                case 5: this.ReadData("SELECT AVG(Calories) AS 'Средняя калорийность' FROM VegetablesAndFruits"); break;

                case 6: this.ReadData("SELECT Count(*) AS 'Количество овощей' FROM VegetablesAndFruits WHERE Type = 0"); break;
                case 7: this.ReadData("SELECT Count(*) AS 'Количество фруктов' FROM VegetablesAndFruits WHERE Type = 1"); break;
                case 8: this.ReadData("SELECT Count(*) AS 'Количество ягод' FROM VegetablesAndFruits WHERE Type = 2"); break;

                case 9: colour = this.InputStr(); this.ReadData($"SELECT Count(*) AS 'Количество овощей, фруктов и ягод цвета {colour}' FROM VegetablesAndFruits WHERE Colour = '{colour}'"); break;
                case 10: this.ReadData($"SELECT Colour AS 'Цвет', Count(*) AS 'Количество овощей, фруктов и ягод' FROM VegetablesAndFruits GROUP BY Colour"); break;

                case 11: calories = this.InputNum("Введите желаемое число каллорий: "); this.ReadData($"SELECT * FROM VegetablesAndFruits WHERE Calories < {calories}"); break;
                case 12: calories = this.InputNum("Введите желаемое число каллорий: "); this.ReadData($"SELECT * FROM VegetablesAndFruits WHERE Calories > {calories}"); break;
                case 13: calories = this.InputNum("Введите первое желаемое число каллорий: "); calories2 = this.InputNum("Введите второе желаемое число каллорий: "); this.ReadData($"SELECT * FROM VegetablesAndFruits WHERE Calories BETWEEN {calories} AND {calories2}"); break;

                case 14: this.CloseConnection(); Environment.Exit(0); return;
            }

            this.PressToContinue();
        }

        public void Navigation()
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey();

            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow: if (this.currentMenu > MINMENU) { this.currentMenu--; } break;
                case ConsoleKey.DownArrow: if (this.currentMenu < MAXMENU) { this.currentMenu++; } break;
                case ConsoleKey.Enter: this.Do(); break;
            }

        }

        public void ShowMenu()
        {
            Console.Clear();
            Console.WriteLine("ВЫБЕРИТЕ ЖЕЛАЕМОЕ ДЕЙСТВИЕ:\n");
            Console.WriteLine($"{(this.currentMenu == 0 ? ">" : " ")} Отображение всей информации из таблицы с овощами и фруктами.");
            Console.WriteLine($"{(this.currentMenu == 1 ? ">" : " ")} Отображение всех названий овощей и фруктов.");
            Console.WriteLine($"{(this.currentMenu == 2 ? ">" : " ")} Отображение всех цветов.\n");

            Console.WriteLine($"{(this.currentMenu == 3 ? ">" : " ")} Показать максимальную калорийность.");
            Console.WriteLine($"{(this.currentMenu == 4 ? ">" : " ")} Показать минимальную калорийность.");
            Console.WriteLine($"{(this.currentMenu == 5 ? ">" : " ")} Показать среднюю калорийность.\n");

            Console.WriteLine($"{(this.currentMenu == 6 ? ">" : " ")} Показать количество овощей.");
            Console.WriteLine($"{(this.currentMenu == 7 ? ">" : " ")} Показать количество фруктов.");
            Console.WriteLine($"{(this.currentMenu == 8 ? ">" : " ")} Показать количество ягод.\n");

            Console.WriteLine($"{(this.currentMenu == 9 ? ">" : " ")} Показать количество овощей, фруктов и ягод заданного цвета.");
            Console.WriteLine($"{(this.currentMenu == 10 ? ">" : " ")} Показать количество овощей, фруктов и ягод каждого цвета.\n");

            Console.WriteLine($"{(this.currentMenu == 11 ? ">" : " ")} Показать овощи, фрукты и ягоды с калорийностью ниже указанной.");
            Console.WriteLine($"{(this.currentMenu == 12 ? ">" : " ")} Показать овощи, фрукты и ягоды с калорийностью выше указанной.");
            Console.WriteLine($"{(this.currentMenu == 13 ? ">" : " ")} Показать овощи, фрукты и ягоды с калорийностью в указанном диапазоне.\n");

            Console.WriteLine($"{(this.currentMenu == 14 ? ">" : " ")} Отключиться от базы данных.");
        }

        public void PressToContinue()
        {
            Console.Write("\nНажмите любую клавишу для продолжения..."); Console.ReadKey(); Console.Clear();
        }

        public void OpenConnection()
        {
            try
            {
                this.sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
                this.sqlConnection.Open();

                for (int i = 0; i < 5; i++)
                {
                    Console.WriteLine("Подключение..");
                    Thread.Sleep(400);
                }

                Console.ForegroundColor = ConsoleColor.Green; Console.WriteLine($"Подключение - успешно: {this.sqlConnection.ConnectionString}"); Console.ResetColor();
            }
            catch (SqlException se)
            {
                Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine($"Ошибка при подключении:\n{se.Message}"); Console.ResetColor();
            }
        }

        public void ReadData(string cmdText)
        {
            SqlDataReader sqlDataReader = null;

            try
            {
                SqlCommand sqlCommand = new SqlCommand(cmdText, this.sqlConnection);
                sqlDataReader = sqlCommand.ExecuteReader();

                // ВЫВОД НАЗВАНИЙ СТОЛБЦОВ
                for (int i = 0; i < sqlDataReader.FieldCount; i++)
                {
                    Console.Write(sqlDataReader.GetName(i).ToString() + (sqlDataReader.GetName(i).ToString().Length > 7 ? "\t" : "\t\t"));
                }
                Console.WriteLine();

                // ВЫВОД ЗНАЧЕНИЙ СТРОК
                while (sqlDataReader.Read())
                {
                    for (int i = 0; i < sqlDataReader.FieldCount; i++)
                    {
                        Console.Write(sqlDataReader[i] + (sqlDataReader[i].ToString().Length > 7 ? "\t" : "\t\t"));
                    }
                    Console.WriteLine();
                }
            }
            catch (SqlException se)
            {
                Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine($"Ошибка при считывании данных:\n{se.Message}"); Console.ResetColor();
            }
            finally
            {
                sqlDataReader?.Close();
            }
        }

        public void CloseConnection()
        {
            this.sqlConnection.Close();

            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine("Отключение..");
                Thread.Sleep(400);
            }

            Console.ForegroundColor = ConsoleColor.Green; Console.WriteLine($"Отключение - успешно."); Console.ResetColor();
        }
    }
}
