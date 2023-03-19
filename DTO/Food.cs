using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeStore.DTO
{
    public class Food
    {
        private int id;
        private string name;
        private int iDCategory;
        private float foodPrice;

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public int IDCategory { get => iDCategory; set => iDCategory = value; }
        public float FoodPrice { get => foodPrice; set => foodPrice = value; }

        public Food(int id, string name, int idCategory, float price)
        {
            this.id = id;
            this.name = name;
            iDCategory = idCategory;
            foodPrice = price;
        }
        public Food(DataRow row)
        {
            this.id = (int)row["ID"];
            this.name = row["TenMon"].ToString();
            iDCategory = (int)row["idDM"];
            foodPrice = Convert.ToSingle(row["GiaMonAn"]);
        }
    }
}
