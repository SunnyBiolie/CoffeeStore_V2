using CoffeeStore.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeStore.DAO
{
    public class AccountDAO
    {
        private static AccountDAO instance;
        public static AccountDAO Instance
        {
            get
            {
                if (instance == null) instance = new AccountDAO();
                return AccountDAO.instance;
            }
            private set => AccountDAO.instance = value;
        }
        private AccountDAO() { }

        // Kiểm tra tính hợp lệ của tài khoản đăng nhập
        public bool LoginCheck(string username, string password)
        {
            string query = "select * from dbo.TaiKhoan where TenDangNhap = @username and MatKhau = @password";
            DataTable result = DataProvider.Instance.ExecuteQuery(query, new object[] {username, password});

            return result.Rows.Count > 0;
        }

        // Lấy ra thông tin của Tài Khoản đăng nhập đã được kiểm tra tính hợp lệ
        public Account GetAccByUserName(string userName)
        {
            string query = $"select * from TaiKhoan where TenDangNhap = @username";
            DataTable data = DataProvider.Instance.ExecuteQuery(query, new object[] {userName});
            foreach (DataRow row in data.Rows)
            {
                return new Account(row);
            }

            return null;
        }

        public List<string> GetUserNameAccountsList()
        {
            List<string> list = new List<string>();

            string query = "select TenDangNhap from TaiKhoan";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            foreach (DataRow row in data.Rows)
            {
                string userName = row[0].ToString();
                list.Add(userName);
            }

            return list;
        }

        /// <summary>
        /// Tên đăng nhập không thể được sửa
        /// Nếu mật khẩu mới bằng null hoặc rỗng thì sẽ không cập nhật mật khẩu
        /// </summary>
        /// <param name="displayName"></param>
        /// <param name="userName"></param>
        /// <param name="oldPass"></param>
        /// <param name="newPass"></param>
        /// <returns></returns>
        public bool UpdateCurAccountInfo(string displayName, string userName, string oldPass, string newPass)
        {
            string query = "exec USP_UpdateAccInfo @TenHienThi , @TenDangNhap , @MatKhau , @MatKhauMoi";
            int lineCount = DataProvider.Instance.ExecuteNonQuery(query, new object[] {displayName, userName, oldPass, newPass});

            return lineCount > 0;
        }

        public bool InsertAccount(string userName, string displayName, int type)
        {
            string query = $"insert TaiKhoan (TenDangNhap, TenHienThi, PhanQuyen) values ( @userName , @displayName , @type )";
            int result = DataProvider.Instance.ExecuteNonQuery(query, new object[] { userName, displayName, type });

            return result > 0;
        }

        public bool EditAccInfo(string userName, string displayName, int type)
        {
            string query = $"update TaiKhoan set TenHienThi = @displayName , PhanQuyen = @type where TenDangNhap = @userName";
            int result = DataProvider.Instance.ExecuteNonQuery(query, new object[] { displayName, type, userName });

            return result > 0;
        }

        public bool DeleteAccount(string userName)
        {
            string query = $"Delete TaiKhoan where TenDangNhap = @userName";
            int result = DataProvider.Instance.ExecuteNonQuery(query, new object[] { userName });

            return result > 0;
        }

        public bool ResetPass(string userName)
        {
            string query = "update TaiKhoan set MatKhau = 'thecoffeehouse' where TenDangNhap = @userName";
            int result = DataProvider.Instance.ExecuteNonQuery(query, new object[] { userName });

            return result > 0;
        }
    
        public string GetDisplayNameByUserName(string userName)
        {
            string displayName = "";
            string query = "select TenHienThi from TaiKhoan where TenDangNhap = @userName";
            DataTable data = DataProvider.Instance.ExecuteQuery(query, new object[] { userName});
            displayName = data.Rows[0][0].ToString();
            return displayName;
        }
    }
}
