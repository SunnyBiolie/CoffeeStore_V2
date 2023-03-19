using CoffeeStore.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeStore.DAO
{
    // Lấy ra hóa đơn từ id của bàn
    public class BillDAO
    {
        private static BillDAO instance;
        public static BillDAO Instance
        {
            get
            {
                if (instance == null) instance = new BillDAO();
                return BillDAO.instance;
            }
            private set { BillDAO.instance = value; }
        }
        private BillDAO() { }

        /// <summary>
        /// Return billID nếu bàn có hóa đơn chưa thanh toán;
        /// Return -1 nếu bàn không có hóa đơn chưa thanh toán
        /// </summary>
        /// <param name="tableID"></param>
        /// <returns>billID || -1</returns>
        public int GetUncheckBillIDByTableID(int tableID)
        {
            string query = $"select * from HoaDon where idBan = @tableID and TrangThai = 0";
            DataTable data = DataProvider.Instance.ExecuteQuery(query, new object[] { tableID });
            if (data.Rows.Count > 0)
            {
                Bill bill = new Bill(data.Rows[0]);
                return bill.BillID;

                //return (int)data.Rows[0][0];
            }
            return -1;
        }

        public void InsertBill(int idTable)
        {
            string query = "exec USP_InsertBill @idBan";
            DataProvider.Instance.ExecuteNonQuery(query, new object[] {idTable});
        }
        
        /// <summary>
        /// Được gọi: btnAddFood_Click / fHome.cs
        /// </summary>
        /// <returns>ID của HoaDon vừa được insert</returns>
        public int GetMaxIDBill()
        {
            return (int)DataProvider.Instance.ExecuteScalar("select max(ID) from HoaDon");
        }
        
        public void CheckOut(int idBill, float totalPrice)
        {
            string query = $"update HoaDon set ThoiGianRa = GETDATE(), TrangThai = 1, TongTien = @totalPrice where ID = @idBill";
            DataProvider.Instance.ExecuteNonQuery(query, new object[] { totalPrice, idBill });
        }

        public DataTable GetListBillByDate(DateTime dateCheckIn, DateTime dateCheckOut)
        {
            string query = $"exec USP_GetListBillByDate @NgayVao , @NgayRa";
            return DataProvider.Instance.ExecuteQuery(query, new object[] { dateCheckIn, dateCheckOut });
        }
        
        public void UpdateBillToDeletedTableByTableID(int tableID)
        {
            string query = "update HoaDon set idBan = 1 where idBan = @tableID";
            DataProvider.Instance.ExecuteNonQuery(query, new object[] { tableID });
        }
    }
}
