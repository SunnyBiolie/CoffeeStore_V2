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
    public partial class fAccInfo : Form
    {
        private Account accLogined;

        public Account AccLogined
        {
            get => accLogined;
            set
            {
                accLogined = value;
            }
        }

        public fAccInfo(Account accLogined)
        {
            InitializeComponent();

            AccLogined = accLogined;
            ShowAccInfo(accLogined);
        }

        #region Methods
        private void ShowAccInfo(Account accLogined)
        {
            tBoxUserName.Texts = accLogined.UserName;
            tBoxDisplayName.Texts = AccountDAO.Instance.GetDisplayNameByUserName(accLogined.UserName);
        }

        private void UpdateAccInfo()
        {
            string displayName = tBoxDisplayName.Texts;
            string userName = tBoxUserName.Texts;
            string oldPass = tBoxOldPass.Texts;
            string newPass = tBoxNewPass.Texts;
            string reEnterPass = tBoxReNewPass.Texts;

            if (chBoxPass.Checked == true && (newPass == null || newPass == ""))
            {
                MessageBox.Show("Mật khẩu mới không được để trống", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (chBoxPass.Checked == true && !newPass.Equals(reEnterPass))
            {
                MessageBox.Show("Nhập lại mật khẩu không chính xác, xin vui lòng kiểm tra lại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (AccountDAO.Instance.UpdateCurAccountInfo(displayName, userName, oldPass, newPass))
            {
                if (MessageBox.Show("Cập nhật thông tin thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK)
                {
                    btnOut_Click(new object[] { }, new EventArgs());
                }
                if (updateInfo != null)
                    updateInfo(this, new AccountEvent(AccountDAO.Instance.GetAccByUserName(userName)));
            }
            else
            {
                MessageBox.Show("Xác thực mật khẩu không thành công, vui lòng kiểm tra lại mật khẩu đã nhập", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void CheckchBoxStatus()
        {
            // Tên hiển thị
            if (chBoxAccInfo.Checked == true)
            {
                tBoxDisplayName.Enabled = true;
                tBoxDisplayName.BackColor = Color.White;
            }
            else
            {
                tBoxDisplayName.Enabled = false;
                tBoxDisplayName.BackColor = SystemColors.Control;
                tBoxDisplayName.Texts = accLogined.DisplayName;
            }
            // Mật khẩu mới & nhập lại mật khẩu mới
            if (chBoxPass.Checked == true)
            {
                tBoxNewPass.Enabled = true;
                tBoxReNewPass.Enabled = true;
                tBoxNewPass.BackColor = Color.White;
                tBoxReNewPass.BackColor = Color.White;
            }
            else
            {
                tBoxNewPass.Enabled = false;
                tBoxReNewPass.Enabled = false;
                tBoxNewPass.BackColor = SystemColors.Control;
                tBoxReNewPass.BackColor = SystemColors.Control;
                tBoxNewPass.Texts = "";
                tBoxReNewPass.Texts = "";
            }
            // Mật khẩu xác thực & btn cập nhật
            if (chBoxAccInfo.Checked == true || chBoxPass.Checked == true)
            {
                tBoxOldPass.Enabled = true;
                tBoxOldPass.BackColor = Color.White;

                btnUpdate.Enabled = true;
                btnUpdate.BackgroundColor = Color.SlateBlue;
            }
            else
            {
                tBoxOldPass.Enabled = false;
                tBoxOldPass.BackColor = SystemColors.Control;

                btnUpdate.Enabled = false;
                btnUpdate.BackgroundColor = SystemColors.ButtonShadow;
            }
        }
        #endregion

        #region events
        private event EventHandler<AccountEvent> updateInfo;
        public event EventHandler<AccountEvent> UpdateInfo
        {
            add { updateInfo += value; }
            remove { updateInfo -= value; }
        }

        private void chBoxAccInfo_CheckedChanged(object sender, EventArgs e)
        {
            CheckchBoxStatus();
        }

        private void chBoxPass_CheckedChanged(object sender, EventArgs e)
        {
            CheckchBoxStatus();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateAccInfo();
        }

        private void btnOut_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion
    }

    public class AccountEvent : EventArgs
    {
        private Account account;

        public Account Account { get => account; set => account = value; }

        public AccountEvent(Account acc)
        {
            this.account = acc;
        }
    }
}
