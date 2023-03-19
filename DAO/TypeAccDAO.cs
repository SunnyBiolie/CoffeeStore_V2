using CoffeeStore.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeStore.DAO
{
    public class TypeAccDAO
    {
        private static TypeAccDAO instance;
        public static TypeAccDAO Instance
        {
            get
            {
                if (instance == null) instance = new TypeAccDAO();
                return instance;
            }
            private set => instance = value;
        }
        private TypeAccDAO() { }

        public List<TypeAcc> GetListTypeAcc()
        {
            List<TypeAcc> list = new List<TypeAcc>();

            string query = "select * from PhanQuyen";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow row in data.Rows)
            {
                TypeAcc type = new TypeAcc(row);
                list.Add(type);
            }

            return list;
        }
    }
}
