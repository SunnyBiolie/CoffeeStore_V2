using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeStore.DTO
{
    public class BillInfo
    {
        private int billInfoID;
        private int billID;
        private int foodID;
        private int foodCount;

        public int BillInfoID { get => BillInfoID; set => BillInfoID = value; }
        public int BillID { get => billID; set => billID = value; }
        public int FoodID { get => foodID; set => foodID = value; }
        public int FoodCount { get => foodCount; set => foodCount = value; }

        public BillInfo(int id, int billID, int foodID, int foodCount)
        {
            billInfoID = id;
            this.billID = billID;
            this.foodID = foodID;
            this.foodCount = foodCount;
        }
        public BillInfo(DataRow row)
        {
            billInfoID = (int)row["ID"];
            this.billID = (int)row["idHoaDon"];
            this.foodID = (int)row["idMonAn"];
            this.foodCount = (int)row["SoLuong"];
        }
    }
}
