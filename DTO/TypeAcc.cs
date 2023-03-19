using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeStore.DTO
{
    public class TypeAcc
    {
        private int idType;
        private string typeName;

        public int IdType { get => idType; set => idType = value; }
        public string TypeName { get => typeName; set => typeName = value; }

        public TypeAcc(int idType, string typeName)
        {
            this.idType = idType;
            this.typeName = typeName;
        }

        public TypeAcc(DataRow row)
        {
            this.idType = (int)row["MaPhanLoai"];
            this.typeName = row["PhanLoai"].ToString();
        }
    }
}
