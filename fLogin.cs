using CoffeeStore.DAO;
using CoffeeStore.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoffeeStore
{
    public partial class fLogin : Form
    {
        public fLogin()
        {
            InitializeComponent();
        }

        bool isMember(string userName, string pass)
        {
            return AccountDAO.Instance.LoginCheck(userName, pass);
        }

        private void csBtnLogin_Click(object sender, EventArgs e)
        {
            string userName = csTBoxUserName.Texts;
            string pass = csTBoxPass.Texts;
            if (userName == null || userName == "" || pass == null || pass == "")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (isMember(userName, pass))
            {
                Account accLogined = AccountDAO.Instance.GetAccByUserName(userName);
                this.Hide();
                fHome fHome = new fHome(accLogined);
                fHome.ShowDialog();
                this.Show();
            }
            else
            {
                MessageBox.Show("Tên đăng nhập hoặc mật khẩu không chính xác\nXin vui lòng kiểm tra lại thông tin đăng nhập", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
