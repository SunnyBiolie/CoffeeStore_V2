using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace CoffeeStore.DTO
{
    public class Account
    {
        private string userName;
        private string displayName;
        private string password;
        private int type;

        public string UserName { get => userName; set => userName = value; }
        public string DisplayName { get => displayName; set => displayName = value; }
        public string Password { get => password; set => password = value; }
        public int Type { get => type; set => type = value; }

        public Account(string username, string displayname, int type, string password = null)
        {
            userName = username;
            displayName = displayname;
            this.type = type;
            this.password = password;
        }

        public Account(DataRow row)
        {
            userName = row["TenDangNhap"].ToString();
            displayName = row["TenHienThi"].ToString();
            this.type = (int)row["PhanQuyen"];
            this.password = row["MatKhau"].ToString();
        }
    }
}
