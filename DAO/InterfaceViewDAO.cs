using CoffeeStore.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoffeeStore.DAO
{
    public class InterfaceBillInfoFullDetailsDAO
    {
        private static InterfaceBillInfoFullDetailsDAO instance;
        public static InterfaceBillInfoFullDetailsDAO Instance
        {
            get
            {
                if (instance == null) instance = new InterfaceBillInfoFullDetailsDAO();
                return instance;
            }
            private set { instance = value; }
        }
        private InterfaceBillInfoFullDetailsDAO() { }
        
        public List<InterfaceBillInfoFullDetails> GetInterfaceBillInfoFullDetailsByTableID(int idTable)
        {
            List<InterfaceBillInfoFullDetails> list = new List<InterfaceBillInfoFullDetails>();

            string query = "select ma.ID as [idMonAn], cthd.idTopping1, cthd.idTopping2, cthd.SoLuong\r\nfrom ChiTietHoaDon as cthd, HoaDon as hd, MonAn as ma, Topping as tp\r\nwhere cthd.idHoaDon = hd.ID and cthd.idMonAn = ma.ID and hd.idBan = @idTable and hd.TrangThai = 0\r\nand cthd.idTopping1 = tp.ID";
            DataTable data = DataProvider.Instance.ExecuteQuery(query, new object[] { idTable });

            foreach (DataRow row in data.Rows)
            {
                InterfaceBillInfoFullDetails item = new InterfaceBillInfoFullDetails(row);
                list.Add(item);
            }

            return list;
        }
    }
    
    //public class InterfaceBillInfoDAO
    //{
    //    private static InterfaceBillInfoDAO instance;
    //    public static InterfaceBillInfoDAO Instance
    //    {
    //        get
    //        {
    //            if (instance == null) instance = new InterfaceBillInfoDAO();
    //            return InterfaceBillInfoDAO.instance;
    //        }
    //        private set => InterfaceBillInfoDAO.instance = value;
    //    }
    //    private InterfaceBillInfoDAO() { }

    //    public List<InterfaceBillInfo> GetListInterfaceBillInfoByTableID(int tableID)
    //    {
    //        List<InterfaceBillInfo> listMenu = new List<InterfaceBillInfo>();

    //        string query = $"select fd.TenMon, ctb.SoLuong, fd.GiaMonAn, fd.GiaMonAn*ctb.SoLuong as ThanhTien from ChiTietHoaDon as ctb, HoaDon as b, MonAn as fd\r\nwhere ctb.idHoaDon = b.ID and ctb.idMonAn = fd.ID and b.idBan = {tableID} and b.TrangThai = 0";
    //        DataTable data = DataProvider.Instance.ExecuteQuery(query);

    //        foreach (DataRow row in data.Rows)
    //        {
    //            InterfaceBillInfo menu = new InterfaceBillInfo(row);
    //            listMenu.Add(menu);
    //        }

    //        return listMenu;
    //    }
    //}
   
    public class InterfaceRevenueByDateDAO
    {
        private static InterfaceRevenueByDateDAO instance;
        public static InterfaceRevenueByDateDAO Instance
        {
            get
            {
                if (instance == null) instance = new InterfaceRevenueByDateDAO();
                return InterfaceRevenueByDateDAO.instance;
            }
            private set => InterfaceRevenueByDateDAO.instance = value;
        }
        private InterfaceRevenueByDateDAO() { }

        public List<InterfaceRevenueByDate> GetListBillByDate(DateTime dateCheckIn, DateTime dateCheckOut)
        {
            List<InterfaceRevenueByDate> list = new List<InterfaceRevenueByDate>();

            string query = $"exec USP_GetListBillByDate @NgayVao , @NgayRa";
            DataTable data = DataProvider.Instance.ExecuteQuery(query, new object[] { dateCheckIn, dateCheckOut });

            foreach (DataRow row in data.Rows)
            {
                InterfaceRevenueByDate bill = new InterfaceRevenueByDate(row);
                list.Add(bill);
            }

            return list;
        }
    }
    
    public class InterfaceRevenueDAO
    {
        private static InterfaceRevenueDAO instance;
        public static InterfaceRevenueDAO Instance
        {
            get
            {
                if (instance == null) instance = new InterfaceRevenueDAO();
                return InterfaceRevenueDAO.instance;
            }
            private set => InterfaceRevenueDAO.instance = value;
        }
        public InterfaceRevenueDAO() { }

        public List<InterfaceRevenue> GetListInterfaceRevenues(string billID)
        {
            List<InterfaceRevenue> list = new List<InterfaceRevenue>();

            string query = $"select TenMon, SoLuong from ChiTietHoaDon, HoaDon, MonAn where HoaDon.ID = @billID and HoaDon.ID = idHoaDon and idMonAn = MonAn.ID";
            DataTable data = DataProvider.Instance.ExecuteQuery(query, new object[] { billID });

            foreach (DataRow row in data.Rows)
            {
                InterfaceRevenue line = new InterfaceRevenue(row);
                list.Add(line);
            }

            return list;
        }
    }

    public class InterfaceFoodInfoDAO
    {
        private static InterfaceFoodInfoDAO instance;
        public static InterfaceFoodInfoDAO Instance
        {
            get
            {
                if (instance == null) instance= new InterfaceFoodInfoDAO();
                return InterfaceFoodInfoDAO.instance;
            }
            private set => instance = value;
        }

        private InterfaceFoodInfoDAO() { }

        public List<InterfaceFoodInfo> GetListInterfaceFoodInfo(string orderBy = "order by ID")
        {
            List<InterfaceFoodInfo> list = new List<InterfaceFoodInfo>();

            string query = $"select * from View_AdminFood {orderBy}";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach(DataRow row in data.Rows)
            {
                InterfaceFoodInfo line = new InterfaceFoodInfo(row);
                if (line.Id == 1) continue;
                list.Add(line);
            }

            return list;
        }

        /// <summary>
        /// Hỗ trợ cấp dữ liệu cho FindFoodByName
        /// </summary>
        /// <param name="approName"></param>
        /// <returns></returns>
        public List<InterfaceFoodInfo> GetListFoodByName(string approName, string orderBy = "order by ID")
        {
            List<InterfaceFoodInfo> list = new List<InterfaceFoodInfo>();

            string query = $"select * from View_AdminFood where dbo.fuConvertToUnsign_STR(TenMon) like N'%' + dbo.fuConvertToUnsign_STR( @approName ) + '%' {orderBy}";
            DataTable data = DataProvider.Instance.ExecuteQuery(query, new object[] { approName });

            foreach (DataRow row in data.Rows)
            {
                InterfaceFoodInfo food = new InterfaceFoodInfo(row);
                if (food.Id == 1) continue;
                list.Add(food);
            }

            return list;
        }
    }
    
    public class InterfaceTopping_FoodDAO
    {
        private static InterfaceTopping_FoodDAO instance;
        public static InterfaceTopping_FoodDAO Instance
        {
            get
            {
                if (instance == null) instance = new InterfaceTopping_FoodDAO();
                return instance;
            }
            private set { instance = value; }
        }
        private InterfaceTopping_FoodDAO() { }

        public List<InterfaceTopping_Food> GetListFoodByIDTopping(int toppingID)
        {
            List<InterfaceTopping_Food> list = new List<InterfaceTopping_Food>();

            string query = "select idMonAn, TenMon from MonAn_Topping, MonAn where idTopping = @toppingID and idMonAn = MonAn.ID";
            DataTable data = DataProvider.Instance.ExecuteQuery(query , new object[] { toppingID });
            foreach (DataRow row in data.Rows)
            {
                InterfaceTopping_Food item = new InterfaceTopping_Food(row);
                list.Add(item);
            }
            
            return list;
        }
        
        public void DeleteTopping_FoodByIDTopping(int toppingID)
        {
            string query = "delete MonAn_Topping where idTopping = @toppingID";
            DataProvider.Instance.ExecuteNonQuery(query, new object[] { toppingID });
        }
        public void DeleteTopping_FoodByIDFood(int foodID)
        {
            string query = "delete MonAn_Topping where idMonAn = @foodID";
            DataProvider.Instance.ExecuteNonQuery(query, new object[] { foodID });
        }
    
        public bool AddFoodToTopping(int foodID, int toppingID)
        {
            string query = "insert MonAn_Topping (idMonAn, idTopping) values ( @foodID , @toppingID )";
            int result = DataProvider.Instance.ExecuteNonQuery(query , new object[] { foodID, toppingID });

            return result > 0;
        }
        public bool DeleteTopping_Food(int foodID, int toppingID)
        {
            if (InterfaceTopping_FoodDAO.Instance.CheckUncheckedBillExistsTopping_FoodBeforeDelete(foodID, toppingID))
                return false;

            string query = "delete MonAn_Topping where idMonAn = @foodID and idTopping = @toppingID";
            int result = DataProvider.Instance.ExecuteNonQuery(query, new object[] { foodID, toppingID });

            return result > 0;
        }
        public bool CheckUncheckedBillExistsTopping_FoodBeforeDelete(int foodID, int toppingID)
        {
            string query = "select idMonAn, idTopping1, idTopping2 from HoaDon, ChiTietHoaDon where HoaDon.TrangThai = 0 and HoaDon.ID = idHoaDon";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            if (data.Rows.Count > 0)
            {
                foreach (DataRow row in data.Rows)
                {
                    if (foodID == Convert.ToInt32(row["idMonAn"]) && (toppingID == Convert.ToInt32(row["idTopping1"]) || toppingID == Convert.ToInt32(row["idTopping2"])))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }

    public class InterfaceAccInfoDAO
    {
        private static InterfaceAccInfoDAO instance;
        public static InterfaceAccInfoDAO Instance
        {
            get
            {
                if (instance == null) instance = new InterfaceAccInfoDAO();
                return InterfaceAccInfoDAO.instance;
            }
            private set => instance = value;
        }

        private InterfaceAccInfoDAO() { }

        public List<InterfaceAccInfo> GetAccountsList()
        {
            List<InterfaceAccInfo> list = new List<InterfaceAccInfo>();

            string query = $"select TenDangNhap, TenHienThi, PhanQuyen, PhanLoai from TaiKhoan, PhanQuyen where PhanQuyen = MaPhanLoai";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow row in data.Rows)
            {
                InterfaceAccInfo acc = new InterfaceAccInfo(row);
                list.Add(acc);
            }

            return list;
        }
    }
}
