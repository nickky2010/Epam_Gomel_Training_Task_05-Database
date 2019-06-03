using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Configuration;

namespace Database
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DictionaryAccess"].ConnectionString;
            Dictionary<int, int> dictionaryLenght = new Dictionary<int, int>();
            OleDbConnection connection = null;
            OleDbCommand command = null;
            OleDbDataReader dataReader = null;
            try
            {
                connection = new OleDbConnection(connectionString);
                command = connection.CreateCommand();
                command.CommandText = "select (int(abs(x2-x1)+0.5)) as len, Count(*) as num from Coordinates group by (int(abs(x2-x1)+0.5)) ORDER BY 1";
                //открываем соединение и читаем из таблицы Coordinates в Dictionary
                connection.Open();
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    dictionaryLenght.Add((int)dataReader.GetDouble(0), dataReader.GetInt32(1));
                }
                dataReader.Close();
                //удаляем записи в таблице Frequencies
                command.CommandText = "delete from Frequencies";
                command.ExecuteNonQuery();
                //запись коллекции в таблицу Frequencies используя параметры, чтобы избежать внедрения
                command.CommandText = "insert into Frequencies (len, num) values (@len, @num)";
                command.Parameters.Add("@len", OleDbType.Integer);
                command.Parameters.Add("@num", OleDbType.Integer);
                foreach (int len in dictionaryLenght.Keys)
                {
                    command.Parameters["@len"].Value = len;
                    command.Parameters["@num"].Value = dictionaryLenght[len];
                    command.ExecuteNonQuery();
                }
                // поиск записей в Frequencies в которых len>num
                command.CommandText = "select * from Frequencies where len>num";
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    Console.WriteLine("{0};{1}\n", dataReader.GetInt32(1), dataReader.GetInt32(2));
                }
                dataReader.Close();
                connection.Close();
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Application configuration error has occurred.");
            }
            catch (InvalidCastException)
            {
                Console.WriteLine("Database contains invalid data type. Conversion of an instance of one type to another type is not supported.");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (OleDbException ex)
            {
                string errorMessages = "";
                for (int i = 0; i < ex.Errors.Count; i++)
                {
                    errorMessages += $"Number #{i+1}\n" +
                                     $"Message: {ex.Errors[i].Message}\n" +
                                     $"NativeError: {ex.Errors[i].NativeError}\n" +
                                     $"Source: {ex.Errors[i].Source}\n" +
                                     $"SQLState: {ex.Errors[i].SQLState}\n";
                }
                Console.WriteLine("Error!!!\n"+errorMessages+ "\nPlease contact your system administrator.");
            }
            finally
            {
                if (dataReader != null)
                    dataReader.Dispose();
                if (command != null)
                    command.Dispose();
                if (connection != null)
                    connection.Dispose();
            }
            Console.Write("\nFor exit press any key...");
            Console.ReadKey();
        }
    }
}
