using CoffeeStore.DAO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeStore.DTO
{
    public class InterfaceBillInfoFullDetails
    {
        private int foodID;
        private string foodName;
        private float foodPrice;
        private int topping1ID;
        private string topping1Name;
        private float topping1Price;
        private int topping2ID;
        private string topping2Name;
        private float topping2Price;
        private float price;
        private int count;
        private float totalPrice;

        public int FoodID { get => foodID; set => foodID = value; }
        public string FoodName { get => foodName; set => foodName = value; }
        public float FoodPrice { get => foodPrice; set => foodPrice = value; }
        public int Topping1ID { get => topping1ID; set => topping1ID = value; }
        public string Topping1Name { get => topping1Name; set => topping1Name = value; }
        public float Topping1Price { get => topping1Price; set => topping1Price = value; }
        public int Topping2ID { get => topping2ID; set => topping2ID = value; }
        public string Topping2Name { get => topping2Name; set => topping2Name = value; }
        public float Topping2Price { get => topping2Price; set => topping2Price = value; }
        public float Price { get => price; set => price = value; }
        public int Count { get => count; set => count = value; }
        public float TotalPrice { get => totalPrice; set => totalPrice = value; }

        public InterfaceBillInfoFullDetails(DataRow row)
        {
            this.foodID = Convert.ToInt32(row["idMonAn"]);
            this.topping1ID = Convert.ToInt32(row["idTopping1"]);
            this.topping2ID = Convert.ToInt32(row["idTopping2"]);
            this.count = Convert.ToInt32(row["SoLuong"]);

            string queryFoodName = "select TenMon, GiaMonAn from MonAn where ID = @foodID";
            DataTable dataFood = DataProvider.Instance.ExecuteQuery(queryFoodName, new object[] { foodID });
            this.foodName = dataFood.Rows[0][0].ToString();
            this.foodPrice = Convert.ToSingle(dataFood.Rows[0][1]);

            string queryTopping1Name = "select TenTopping, GiaTopping from Topping where ID = @topping1ID";
            DataTable dataTopping1 = DataProvider.Instance.ExecuteQuery(queryTopping1Name, new object[] { topping1ID });
            this.topping1Name = dataTopping1.Rows[0][0].ToString();
            this.topping1Price = Convert.ToSingle(dataTopping1.Rows[0][1]);

            string queryTopping2Name = "select TenTopping, GiaTopping from Topping where ID = @topping2ID";
            DataTable dataTopping2 = DataProvider.Instance.ExecuteQuery(queryTopping2Name, new object[] { topping2ID });
            this.topping2Name = dataTopping2.Rows[0][0].ToString();
            this.topping2Price = Convert.ToSingle(dataTopping2.Rows[0][1]);

            this.price = foodPrice + topping1Price + topping2Price;
            this.totalPrice = price * count;
        }
    }
    
    //public class InterfaceBillInfo
    //{
    //    private string foodName;
    //    private int foodCount;
    //    private float foodPrice;
    //    private float totalPrice;

    //    public string FoodName { get => foodName; set => foodName = value; }
    //    public int FoodCount { get => foodCount; set => foodCount = value; }
    //    public float FoodPrice { get => foodPrice; set => foodPrice = value; }
    //    public float TotalPrice { get => totalPrice; set => totalPrice = value; }

    //    public InterfaceBillInfo(string foodName, int foodCount, float foodPrice, float totalPrice = 0)
    //    {
    //        this.foodName = foodName;
    //        this.foodCount = foodCount;
    //        this.foodPrice = foodPrice;
    //    }
    //    public InterfaceBillInfo(DataRow row)
    //    {
    //        this.foodName = row["TenMon"].ToString();
    //        this.foodCount = (int)row["SoLuong"];
    //        this.foodPrice = Convert.ToSingle(row["GiaMonAn"].ToString());
    //        this.totalPrice = Convert.ToSingle(row["ThanhTien"].ToString()); ;
    //    }
    //}

    public class InterfaceRevenueByDate
    {
        private int id;
        private string tableName;
        private DateTime dateCheckIn;
        private DateTime dateCheckOut;
        private float totalPrice;

        public int Id { get => id; set => id = value; }
        public string TableName { get => tableName; set => tableName = value; }
        public DateTime DateCheckIn { get => dateCheckIn; set => dateCheckIn = value; }
        public DateTime DateCheckOut { get => dateCheckOut; set => dateCheckOut = value; }
        public float TotalPrice { get => totalPrice; set => totalPrice = value; }

        public InterfaceRevenueByDate(int id, string tableName, DateTime dateCheckIn, DateTime dateCheckOut, float totalPrice)
        {
            this.id = id;
            this.tableName = tableName;
            this.dateCheckIn = dateCheckIn;
            this.dateCheckOut = dateCheckOut;
            this.totalPrice = totalPrice;
        }
        public InterfaceRevenueByDate(DataRow row)
        {
            this.id = (int)row["ID"];
            this.tableName = row["TenBan"].ToString();
            this.dateCheckIn = DateTime.Parse(row["ThoiGianVao"].ToString());
            this.dateCheckOut = DateTime.Parse(row["ThoiGianRa"].ToString());
            this.totalPrice = Convert.ToSingle(row["TongTien"]);
        }
    }

    public class InterfaceRevenue
    {
        private string foodName;
        private int foodCount;

        public string FoodName { get => foodName; set => foodName = value; }
        public int FoodCount { get => foodCount; set => foodCount = value; }

        public InterfaceRevenue(string foodName, int foodCount)
        {
            this.foodName = foodName;
            this .foodCount = foodCount;
        }

        public InterfaceRevenue(DataRow row)
        {
            this.foodName = row["TenMon"].ToString();
            this.foodCount = (int)row["SoLuong"];
        }
    }

    public class InterfaceFoodInfo
    {
        private int id;
        private string foodName;
        private string categoryName;
        private float foodPrice;

        public int Id { get => id; set => id = value; }
        public string FoodName { get => foodName; set => foodName = value; }
        public string CategoryName { get => categoryName; set => categoryName = value; }
        public float FoodPrice { get => foodPrice; set => foodPrice = value; }

        public InterfaceFoodInfo(int id, string foodName, string categoryName, float foodPrice)
        {
            this.id = id;
            this.foodName= foodName;
            this.categoryName= categoryName;
            this.foodPrice = foodPrice;
        }

        public InterfaceFoodInfo(DataRow row)
        {
            this.id = (int)row["ID"];
            this.foodName = row["TenMon"].ToString();
            this.categoryName = row["TenDM"].ToString();
            this.foodPrice = Convert.ToSingle(row["GiaMonAn"]);
        }
    }

    public class InterfaceTopping_Food
    {
        private int foodId;
        private string foodName;

        public int FoodId { get => foodId; set => foodId = value; }
        public string FoodName { get => foodName; set => foodName = value; }

        public InterfaceTopping_Food(int foodID, string foodName)
        {
            this.foodId = foodID;
            this.foodName = foodName;
        }
        public InterfaceTopping_Food(DataRow row)
        {
            this.foodId = Convert.ToInt32(row["idMonAn"]);
            this.foodName = row["TenMon"].ToString();
        }
    }

    public class InterfaceAccInfo
    {
        private string userName;
        private string displayName;
        private int phanQuyen;
        private string phanLoai;
        public string UserName { get => userName; set => userName = value; }
        public string DisplayName { get => displayName; set => displayName = value; }
        public int PhanQuyen { get => phanQuyen; set => phanQuyen = value; }
        public string PhanLoai { get => phanLoai; set => phanLoai = value; }


        public InterfaceAccInfo(string userName, string displayName, int phanQuyen, string phanLoai)
        {
            this.userName = userName;
            this.displayName = displayName;
            this.phanQuyen = phanQuyen;
            this.phanLoai = phanLoai;
        }

        public InterfaceAccInfo(DataRow row)
        {
            this.userName = row["TenDangNhap"].ToString();
            this.displayName = row["TenHienThi"].ToString();
            this.PhanQuyen = (int)row["PhanQuyen"];
            this.phanLoai = row["PhanLoai"].ToString();
        }
    }
}
