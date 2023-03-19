using CoffeeStore.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeStore.DAO
{
    public class ToppingDAO
    {
        private static ToppingDAO instance;

        public static ToppingDAO Instance
        {
            get
            {
                if (instance == null) instance = new ToppingDAO(); return instance;
            }
            private set { instance = value; }
        }

        private ToppingDAO() { }

        public List<Topping> GetToppingsList()
        {
            List<Topping> list = new List<Topping>();

            string query = "select * from Topping";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            foreach (DataRow row in data.Rows)
            {
                Topping tp = new Topping(row);
                if (tp.Id == 1) continue;
                list.Add(tp);
            }

            return list;
        }
        public List<Topping> GetListToppingByFoodID(int foodID)
        {
            List<Topping> list = new List<Topping>();

            string query = "select idTopping from MonAn_Topping where idMonAn = @foodID";
            DataTable data = DataProvider.Instance.ExecuteQuery(query, new object[] { foodID });

            foreach (DataRow row in data.Rows)
            {
                int idTopping = Convert.ToInt32(row[0]);
                string query2 = "select * from Topping where ID = @idTopping";
                DataTable data2 = DataProvider.Instance.ExecuteQuery(query2, new object[] { idTopping });
                Topping topping = new Topping(data2.Rows[0]);
                list.Add(topping);
            }

            return list;
        }
        public List<Topping> GetListToppingByApproToppingName(string approName)
        {
            List<Topping> list = new List<Topping>();

            string query = $"select * from Topping where dbo.fuConvertToUnsign_STR(TenTopping) like N'%' + dbo.fuConvertToUnsign_STR( @approName ) + '%'";
            DataTable data = DataProvider.Instance.ExecuteQuery(query, new object[] { approName });

            foreach (DataRow row in data.Rows)
            {
                Topping topping = new Topping(row);
                list.Add(topping);
            }

            return list;
        }

        public bool InsertTopping(string toppingName, float toppingPrice)
        {
            string query = "insert Topping (TenTopping, GiaTopping) values ( @toppingName , @toppingPrice )";
            int result = DataProvider.Instance.ExecuteNonQuery(query, new object[] { toppingName, toppingPrice });

            return result > 0;
        }
        public bool EditToppingInfo(string toppingName, float toppingPrice, int toppingID)
        {
            string query = "update Topping set TenTopping = @toppingName , GiaTopping = @toppingPrice where ID = @toppingID";
            int result = DataProvider.Instance.ExecuteNonQuery(query, new Object[] { toppingName, toppingPrice, toppingID });

            return result > 0;
        }
        public bool DeleteTopping(int toppingID)
        {
            InterfaceTopping_FoodDAO.Instance.DeleteTopping_FoodByIDTopping(toppingID);

            string query = "delete Topping where id = @toppingID";
            int result = DataProvider.Instance.ExecuteNonQuery(@query, new Object[] { toppingID });

            return result > 0;
        }
    }
}
