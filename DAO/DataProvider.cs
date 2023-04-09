using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeStore.DAO
{
    public class DataProvider
    {
        // Pattern Singleton
        private static DataProvider instance;
        public static DataProvider Instance
        {
            get
            {
                if (instance == null) instance = new DataProvider();
                return DataProvider.instance;
            }
            private set => DataProvider.instance = value;
        }

        private DataProvider() { }

        private static string remoteConnectionSTR = @"workstation id=TheCoffeeHouse.mssql.somee.com;packet size=4096;user id=SunnyBiolie_SQLLogin_1;pwd=wu6kexltjr;data source=TheCoffeeHouse.mssql.somee.com;persist security info=False;initial catalog=TheCoffeeHouse";
        private static string localConnectionSTR = @"Data Source=KHOAPHAM;Initial Catalog=TheCoffeeHouse;Integrated Security=True";

        private string connectionSTR = localConnectionSTR;

        public DataTable ExecuteQuery(string query, object[] parameters = null)
        {
            DataTable data = new DataTable();
            // Giải phóng bộ nhớ sau khi block code hoàn thành
            using (SqlConnection connection = new SqlConnection(connectionSTR))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);

                if (parameters != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            cmd.Parameters.AddWithValue(item, parameters[i++]);
                        }
                    }
                }

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(data);
                connection.Close();
            }
            return data;
        }

        /// <summary>
        /// Return: số dòng thành công. Dùng cho câu lệnh update, insert
        /// </summary>
        /// <param name="query"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string query, object[] parameters = null)
        {
            int data = 0;
            // Giải phóng bộ nhớ sau khi block code hoàn thành
            using (SqlConnection connection = new SqlConnection(connectionSTR))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);

                if (parameters != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            cmd.Parameters.AddWithValue(item, parameters[i]);
                            i++;
                        }
                    }
                }
                data = cmd.ExecuteNonQuery();
                connection.Close();
            }

            return data;
        }

        public object ExecuteScalar(string query, object[] parameters = null)
        {
            object data = 0;
            // Giải phóng bộ nhớ sau khi block code hoàn thành
            using (SqlConnection connection = new SqlConnection(connectionSTR))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(query, connection);

                if (parameters != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            cmd.Parameters.AddWithValue(item, parameters[i++]);
                        }
                    }
                }
                data = cmd.ExecuteScalar();
                connection.Close();
            }

            return data;
        }
    }
}
