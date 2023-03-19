using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeStore.DTO
{
    public class Topping
    {
        private int id;
        private string name;
        private float price;

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public float Price { get => price; set => price = value; }

        public Topping(int id, string name, float price)
        {
            this.id = id;
            this.name = name;
            this.price = price;
        }

        public Topping(DataRow row)
        {
            this.id = Convert.ToInt32(row["ID"]);
            this.name = row["TenTopping"].ToString();
            this.price = Convert.ToSingle(row["GiaTopping"]);
        }
    }
}
