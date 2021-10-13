using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using System.Globalization;
using System.IO;

namespace BrokerStatistics
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Начинаю работу...");
            Statistics statistics = new Statistics();
        }
    }
    public class Statistics
    {
        string[] fileLines;
        public Statistics()
        {
            var exePath = AppDomain.CurrentDomain.BaseDirectory;
            var path = Path.Combine(exePath, "..\\..\\..\\..\\Trade.txt");
            fileLines = File.ReadAllLines(path);
            DataTable Table = MakeParentTable();
            MinMax(Table);
            HourRange(Table);
            CheckingFiles(Table);
            //ShowTable(Table);
            Console.WriteLine("Готово!");
        }
        private DataTable MakeParentTable()
        {
            DataTable table = new DataTable();
            DataColumn column;

            column = new DataColumn();
            column.DataType = Type.GetType("System.Int32");
            column.ColumnName = "id";
            column.ReadOnly = false;
            column.Unique = true;
            table.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Symbol";
            column.AutoIncrement = false;
            column.Caption = "Symbol";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "Description";
            column.AutoIncrement = false;
            column.Caption = "Description";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.DateTime");
            column.ColumnName = "Date";
            column.AutoIncrement = false;
            column.Caption = "Date";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.TimeSpan");
            column.ColumnName = "Time";
            column.AutoIncrement = false;
            column.Caption = "Time";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.Single");
            column.ColumnName = "Open";
            column.AutoIncrement = false;
            column.Caption = "Open";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.Single");
            column.ColumnName = "High";
            column.AutoIncrement = false;
            column.Caption = "High";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.Single");
            column.ColumnName = "Low";
            column.AutoIncrement = false;
            column.Caption = "Low";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.Single");
            column.ColumnName = "Close";
            column.AutoIncrement = false;
            column.Caption = "Close";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.UInt32");
            column.ColumnName = "TotalVolume";
            column.AutoIncrement = false;
            column.Caption = "TotalVolume";
            column.ReadOnly = false;
            column.Unique = false;
            table.Columns.Add(column);

            DataColumn[] PrimaryKeyColumns = new DataColumn[1];
            PrimaryKeyColumns[0] = table.Columns["id"];
            table.PrimaryKey = PrimaryKeyColumns;

            int index = 0;
            foreach (var v in fileLines)
            {
                DataRow row = table.NewRow();
                row["id"] = index;
                var arrLine = v.Split(',');
                for (int i = 0; i<10; i++)
                {
                    switch (i)
                    {
                        case 0:
                            row["Symbol"] = arrLine[i];
                            break;
                        case 1:
                            row["Description"] = arrLine[i];
                            break;
                        case 2:
                            DateTime date = DateTime.ParseExact(arrLine[i], "dd.MM.yyyy", null);
                            row["Date"] = date;
                            break;
                        case 3:
                            TimeSpan time = DateTime.Parse(arrLine[i]).TimeOfDay;
                            row["Time"] = time;
                            break;
                        case 4:
                            row["Open"] = Convert.ToSingle(arrLine[i].Replace(".", ","));
                            break;
                        case 5:
                            row["High"] = Convert.ToSingle(arrLine[i].Replace(".", ","));
                            break;
                        case 6:
                            row["Low"] = Convert.ToSingle(arrLine[i].Replace(".", ","));
                            break;
                        case 7:
                            row["Close"] = Convert.ToSingle(arrLine[i].Replace(".", ","));
                            break;
                        case 8:
                            row["TotalVolume"] = arrLine[i];
                            break;
                    }
                }
                table.Rows.Add(row);
                index ++;
            }

            return table;
        }

        public void MinMax(DataTable table)
        {
            if (File.Exists("Task1.txt"))
                    File.Delete("Task1.txt");
            var file = File.Create("Task1.txt");
            file.Close();
            File.AppendAllText("Task1.txt", "    Date       Min    Max \n");
            float min = 4294967295;
            float max = 0;
            int Day = Convert.ToInt32(table.Rows[0][table.Columns["Date"]].ToString().Substring(0, 2));
            int Month = 0;
            int Year = 0;
            int day;
            foreach (DataRow row in table.Rows)
            {
                string dateString = row[table.Columns["Date"]].ToString();
                day = Convert.ToInt32(dateString.Substring(0, 2));
                Month = Convert.ToInt32(dateString.Substring(3, 2));
                Year = Convert.ToInt32(dateString.ToString().Substring(6, 4));
                if (day == Day)
                {
                    if (Convert.ToSingle(row[table.Columns["High"]]) > max)
                        max = Convert.ToSingle(row[table.Columns["High"]]);
                    if (Convert.ToSingle(row[table.Columns["Low"]]) < min)
                        min = Convert.ToSingle(row[table.Columns["Low"]]);
                }
                else
                {
                    File.AppendAllText("Task1.txt", Day + " - " + Month + " - " + Year + "  " + min + "  " + max + "\n");
                    max = 0;
                    min = 4294967295;
                }
                Day = Convert.ToInt32(dateString.Substring(0, 2));

            }
            File.AppendAllText("Task1.txt", Day + " - " + Month + " - " + Year + "  " + min + "  " + max + "\n");
            Console.WriteLine("Первое задание выполнено!");
        }

        public void HourRange(DataTable table)
        {
            if (File.Exists("Task2.txt"))
                File.Delete("Task2.txt");
            var file = File.Create("Task2.txt");
            file.Close();
            File.AppendAllText("Task2.txt", "Symbol Description Date Time Open High Low Close TotalVolume" + "\n");
            int sumTotalVolume = 0;
            float Open = Convert.ToSingle(table.Rows[0][table.Columns["Open"]]);
            float Close = 0;
            float min = 4294967295;
            float max = 0;
            int Hour = Convert.ToInt32(table.Rows[0][table.Columns["Time"]].ToString().Substring(0, 2));
            int hour;
            foreach (DataRow row in table.Rows)
            {
                string dateString = row[table.Columns["Time"]].ToString();
                hour = Convert.ToInt32(dateString.Substring(0, 2));
                if (hour == Hour)
                {
                    if (Convert.ToSingle(row[table.Columns["High"]]) > max)
                        max = Convert.ToSingle(row[table.Columns["High"]]);
                    if (Convert.ToSingle(row[table.Columns["Low"]]) < min)
                        min = Convert.ToSingle(row[table.Columns["Low"]]);
                    sumTotalVolume += Convert.ToInt32(row[table.Columns["TotalVolume"]]);
                }
                else if (min == 4294967295)
                {
                    int i = Convert.ToInt32(row[table.Columns["id"]]);
                    File.AppendAllText("Task2.txt", table.Rows[i - 1][table.Columns["Symbol"]].ToString() + ", " + table.Rows[i - 1][table.Columns["Description"]].ToString() + ", " + table.Rows[i - 1][table.Columns["Date"]].ToString().Substring(0, 8) + ", " + table.Rows[i - 1][table.Columns["Time"]].ToString().Substring(0, 2) + " часов, " + table.Rows[i - 1][table.Columns["Open"]].ToString() + ", " + table.Rows[i - 1][table.Columns["High"]].ToString() + ", " + table.Rows[i - 1][table.Columns["Low"]].ToString() + ", " + table.Rows[i - 1][table.Columns["Close"]].ToString() + ", " + table.Rows[i - 1][table.Columns["TotalVolume"]].ToString() + "\n");
                }
                else
                {
                    int i = Convert.ToInt32(row[table.Columns["id"]]);
                    Close = Convert.ToSingle(table.Rows[i - 1][table.Columns["Close"]]);
                    File.AppendAllText("Task2.txt", row[table.Columns["Symbol"]].ToString() + ", " + row[table.Columns["Description"]].ToString() + ", " + row[table.Columns["Date"]].ToString().Substring(0, 8) + ", " + Hour.ToString() + " часов, " + Open.ToString() + ", " + max.ToString() + ", " + min.ToString() + ", " + Close.ToString() +", " + sumTotalVolume.ToString() + "\n");
                    max = 0;
                    min = 4294967295;
                    Open = Convert.ToSingle(row[table.Columns["Open"]]);
                    sumTotalVolume = 0;
                }
                Hour = Convert.ToInt32(dateString.Substring(0, 2));
            }
            Console.WriteLine("Второе задание выполнено!");
        }
        public void CheckingFiles(DataTable table)
        {
            Console.WriteLine("Начинаю выполнять третье задание:");
            var exePath = AppDomain.CurrentDomain.BaseDirectory;
            var path = Path.Combine(exePath, "..\\..\\..\\..\\Trade.txt");
            string[] fileLinesSource = File.ReadAllLines(path);
            string[] fileLinesTask2 = File.ReadAllLines("Task2.txt");
            if (File.Exists("Task3_1.txt"))
                File.Delete("Task3_1.txt");
            var file1 = File.Create("Task3_1.txt");
            file1.Close();
            if (File.Exists("Task3_2.txt"))
                File.Delete("Task3_2.txt");
            var file2 = File.Create("Task3_2.txt");
            file2.Close();
            if (File.Exists("Task3_3.txt"))
                File.Delete("Task3_3.txt");
            var file3 = File.Create("Task3_3.txt");
            file3.Close();
            int count = 0;
            int hourNow;
            int hourLast = 0;
            foreach (var lineTask2 in fileLinesTask2)
            {
                switch (count)
                {
                    case 0:
                        count++;
                        break;
                    default:
                        var arrLineTask2 = lineTask2.Split(", ");
                        int countHour = 0;
                        foreach (var lineSource in fileLinesSource)
                        {
                            hourNow = Convert.ToInt16(arrLineTask2[3].Substring(0, 2));
                            if (hourLast == hourNow)
                                countHour++;
                            else
                                countHour = 0;
                            var arrLineSource = lineSource.Split(",");
                            if (Convert.ToDateTime(arrLineTask2[2]) == Convert.ToDateTime(arrLineSource[2]) && Convert.ToInt16(arrLineTask2[3].Substring(0, 2)) == Convert.ToInt16(arrLineSource[3].Substring(0, 2)))
                            {
                                float open1 = Convert.ToSingle(arrLineTask2[5]);
                                float open2 = Convert.ToSingle(arrLineSource[5].Replace('.', ','));
                                if (Convert.ToSingle(arrLineTask2[4]) == Convert.ToSingle(arrLineSource[4].Replace('.', ',')) && Convert.ToSingle(arrLineTask2[5]) == Convert.ToSingle(arrLineSource[5].Replace('.', ',')) && Convert.ToSingle(arrLineTask2[6]) == Convert.ToSingle(arrLineSource[6].Replace('.', ',')) && Convert.ToSingle(arrLineTask2[7]) == Convert.ToSingle(arrLineSource[7].Replace('.', ',')) && arrLineTask2[8] == arrLineSource[8])
                                {
                                    Console.WriteLine(arrLineTask2[2] + ": " + open1.ToString() + " - " + open2.ToString());
                                }
                                else if (countHour == 0)
                                {
                                    File.AppendAllText("Task3_1.txt", lineTask2 + "\n");
                                    File.AppendAllText("Task3_3.txt", lineTask2 + "\n");
                                    File.AppendAllText("Task3_3.txt", lineSource + "\n");
                                    File.AppendAllText("Task3_2.txt", lineSource + "\n");
                                }
                                else
                                {
                                    File.AppendAllText("Task3_2.txt", lineSource + "\n");
                                    File.AppendAllText("Task3_3.txt", lineSource + "\n");
                                }
                                hourLast = Convert.ToInt16(arrLineTask2[3].Substring(0, 2));
                            }
                            else continue;
                        }
                        break;
                }
            }
            Console.WriteLine("Третье задание выполнено!");
        }
        private static void ShowTable(DataTable table)
        {
            foreach (DataColumn col in table.Columns)
            {
                Console.Write("{0,-14}", col.ColumnName);
            }
            Console.WriteLine();

            foreach (DataRow row in table.Rows)
            {
                foreach (DataColumn col in table.Columns)
                {
                    if (col.DataType.Equals(typeof(DateTime)))
                        Console.Write("{0,-14:d}", row[col]);
                    else if (col.DataType.Equals(typeof(Decimal)))
                        Console.Write("{0,-14:C}", row[col]);
                    else
                        Console.Write("{0,-14}", row[col]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

    }
}
