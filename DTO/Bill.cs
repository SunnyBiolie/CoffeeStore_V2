using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeStore.DTO
{
    public class Bill
    {
        private int billID;
        private DateTime? dateCheckIn;
        private DateTime? dateCheckOut;
        private int status;
        public int BillID { get => billID; set => billID = value; }
        public DateTime? DateCheckIn { get => dateCheckIn; set => dateCheckIn = value; }
        public DateTime? DateCheckOut { get => dateCheckOut; set => dateCheckOut = value; }
        public int Status { get => status; set => status = value; }

        public Bill(int id, DateTime? dateCheckIn, DateTime? dateCheckOut, int status)
        {
            billID = id;
            this.dateCheckIn = dateCheckIn;
            this.dateCheckOut = dateCheckOut;
            this.status = status;
        }

        public Bill(DataRow row)
        {
            billID = (int)row["ID"];
            this.dateCheckIn = (DateTime?)row["ThoiGianVao"];
            var dateCheckOutTemp = row["ThoiGianRa"];
            if (dateCheckOutTemp.ToString() != "")
                this.dateCheckOut = (DateTime?)dateCheckOutTemp;
            this.status = (int)row["TrangThai"];
        }
    }
}
