using CoffeeStore.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeStore.DAO
{
    public class TableDAO
    {
        public static int tableWidth = 90;
        public static int tableHeight = 90;

        private static TableDAO instance;
        public static TableDAO Instance
        {
            get
            {
                if (instance == null) instance = new TableDAO();
                return TableDAO.instance;
            }
            private set { TableDAO.instance = value; }
        }
        private TableDAO() { }

        public List<Table> GetTablesList()
        {
            List<Table> tables = new List<Table>();

            DataTable data = DataProvider.Instance.ExecuteQuery("USP_GetTableList");

            foreach (DataRow row in data.Rows)
            {
                Table table = new Table(row);
                if (table.Id == 1) continue;
                tables.Add(table);
            }

            return tables;
        }

        public bool AddTable(string tableName)
        {
            string query = "insert Ban (TenBan) values ( @tableName )";
            int result = DataProvider.Instance.ExecuteNonQuery(query, new object[] { tableName });

            return result > 0;
        }
        public bool EditTableInfo(int tableID, string tableName)
        {
            string query = "update Ban set TenBan = @tableName where ID = @tableID";
            int result = DataProvider.Instance.ExecuteNonQuery(query, new object[] { tableName, tableID });

            return result > 0;
        }
        public bool RemoveTable(int tableID)
        {
            BillDAO.Instance.UpdateBillToDeletedTableByTableID(tableID);

            string query = "delete Ban where ID = @tableID";
            int result = DataProvider.Instance.ExecuteNonQuery(query, new object[] { tableID });

            return result > 0;
        }
    }
}
