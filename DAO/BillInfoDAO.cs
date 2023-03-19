using CoffeeStore.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeStore.DAO
{
    public class BillInfoDAO
    {
        private static BillInfoDAO instance;
        public static BillInfoDAO Instance
        {
            get
            {
                if (instance == null) { instance = new BillInfoDAO(); }
                return instance;
            }
            private set => BillInfoDAO.instance = value;
        }
        private BillInfoDAO() { }

        /// <summary>
        /// Return danh sách chi tiết hóa đơn từ ID của một hóa đơn
        /// </summary>
        /// <param name="billID"></param>
        /// <returns></returns>
        public List<BillInfo> GetListBillInfo(int billID)
        {
            List<BillInfo> billInfos = new List<BillInfo>();
            string query = $"select * from ChiTietHoaDon where idHoaDon = @billID";
            DataTable data = DataProvider.Instance.ExecuteQuery(query, new object[] { billID });
            foreach (DataRow row in data.Rows)
            {
                BillInfo BillInfo = new BillInfo(row);
                billInfos.Add(BillInfo);
            }

            return billInfos;
        }

        public void InsertBillInfo(int idBill, int idFood, int idTopping1, int idTopping2, int foodCount)
        {
            string query = "USP_InsertBillInfo @idHoaDon , @idMonAn , @idTopping1 , @idTopping2 , @SoLuong";
            DataProvider.Instance.ExecuteNonQuery(query, new object[] { idBill, idFood, idTopping1, idTopping2, foodCount });
        }

        public void UpdateBillInfoToDeletedFoodByFoodID(int foodID)
        {
            string query = $"update ChiTietHoaDon set idMonAn = 1 where idMonAn = @foodID";
            DataProvider.Instance.ExecuteNonQuery(query, new object[] { foodID });
        }
    }
}
