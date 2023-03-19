using CoffeeStore.csControls;
using CoffeeStore.DAO;
using CoffeeStore.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace CoffeeStore
{
    public partial class fHome : Form
    {
        private CultureInfo GE_Culture = new CultureInfo("de-DE");

        // Thông tin của Tài Khoản đang đăng nhập
        private Account accLogined;
        public Account AccLogined
        {
            get => accLogined;
            set
            {
                accLogined = value;
                Decentralization(accLogined.Type);
            }
        }

        public fHome(Account accLogined)
        {
            InitializeComponent();
            AccLogined = accLogined;

            CallLoad();
        }

        #region Methods
        private void CallLoad()
        {
            LoadTables();
            LoadCategory();
        }

        #region ShowLog / Methods
        private void ShowMessSuccess(string mess)
        {
            MessageBox.Show($"{mess}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void ShowMessError(string mess)
        {
            MessageBox.Show($"{mess}", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private bool ShowMessQuestion(string mess)
        {
            return MessageBox.Show($"{mess}", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }
        #endregion

        private void Decentralization(int accType)
        {
            if (accType == 1 ) tsmItemAdmin.Enabled = true;
            tsmItemInfo.Text += $" ({AccLogined.DisplayName})";
        }

        private void LoadCategory()
        {
            List<Category> categories = CategoryDAO.Instance.GetListCategory();
            cbCategory.DataSource = categories;
            // Hiển thị trường Name của lớp Category trên comboBox
            cbCategory.DisplayMember = "Name";
        }
        
        private void LoadFoodListByCategoryID(int categoryID)
        {
            List<Food> foods = FoodDAO.Instance.GetListFoodByCategoryID(categoryID);
            cbBoxFood.DataSource = foods;
            cbBoxFood.DisplayMember = "Name";
            if (cbBoxFood.SelectedItem == null)
            {
                cbBoxFood.Text = string.Empty;
                return;
            }
        }

        private void LoadToppingComboBox(int idFood)
        {
            cBoxTopping1.DataSource = ToppingDAO.Instance.GetListToppingByFoodID(idFood);
            cBoxTopping1.DisplayMember = "Name";
            cBoxTopping2.DataSource = ToppingDAO.Instance.GetListToppingByFoodID(idFood);
            cBoxTopping2.DisplayMember = "Name";
        }

        /// <summary>
        /// Render toàn bộ các button controller để thể hiện cho các bàn của quán
        /// </summary>
        private void LoadTables()
        {
            fLayoutTable.Controls.Clear();

            List<Table> tableList = TableDAO.Instance.GetTablesList();
            foreach (Table table in tableList)
            {
                csButton btn = new csButton()
                {
                    Width = TableDAO.tableWidth,
                    Height = TableDAO.tableHeight,
                    BorderSize = 1,
                    BorderRadius = 0,
                    Margin = new Padding(6),
                    TabStop = false,
                };
                btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                btn.Text = table.Name + Environment.NewLine + $"({table.Status})";
                // Tag của button thể hiện bàn = chính bàn được chọn
                btn.Tag = table;
                btn.Click += Btn_Click;

                switch (table.Status)
                {
                    case "Trống":
                        btn.BackgroundColor = Color.FromArgb(164, 171, 182);
                        break;
                    case "Có khách":
                        btn.TextColor = Color.White;
                        btn.BackgroundColor = Color.MediumSlateBlue;
                        break;
                }

                fLayoutTable.Controls.Add(btn);
            }
        }

        /// <summary>
        /// Render lại đúng 1 button controller thể hiện cho đúng bàn đã chọn
        /// </summary>
        private void ReloadTable()
        {
            DataTable data = DataProvider.Instance.ExecuteQuery($"select TrangThai from Ban where ID = {(lViewBill.Tag as Table).Id}");
            (lblCurrentTable.Tag as csButton).Text = (lViewBill.Tag as Table).Name + Environment.NewLine + $"({data.Rows[0][0].ToString()})";
            switch (data.Rows[0][0].ToString())
            {
                case "Trống":
                    (lblCurrentTable.Tag as csButton).BackgroundColor = Color.FromArgb(164, 171, 182);
                    break;
                case "Có khách":
                    (lblCurrentTable.Tag as csButton).TextColor = Color.White;
                    (lblCurrentTable.Tag as csButton).BackgroundColor = Color.MediumSlateBlue;
                    break;
            }
        }

        private void AddFoodToTable()
        {
            Table table = lViewBill.Tag as Table;
            if (table == null)
            {
                MessageBox.Show("Vui lòng chọn bàn trước khi thêm món", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (cbBoxFood.SelectedItem == null)
            {
                MessageBox.Show("Danh mục này chưa được thêm món ăn nào, xin vui lòng chọn món khác hoặc liên hệ với quản trị viên", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if ((cbBoxFood.SelectedItem as Food).Name == "Món Đã Xóa")
            {
                MessageBox.Show("Món ăn này không thể được thêm vào vì lý do kỹ thuật", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            int idBill = BillDAO.Instance.GetUncheckBillIDByTableID(table.Id);
            int idFood = (cbBoxFood.SelectedItem as Food).Id;
            int foodCount = (int)numUD_FoodCount.Value;

            int idTopping1 = (cBoxTopping1.SelectedItem as Topping).Id;
            int idTopping2 = (cBoxTopping2.SelectedItem as Topping).Id;

            // Bàn chưa tồn tại Hóa Đơn
            if (idBill == -1)
            {
                if (ShowMessQuestion($"Lập hóa đơn mới cho bàn \"{table.Name}\"?"))
                {
                    BillDAO.Instance.InsertBill(table.Id);
                    BillInfoDAO.Instance.InsertBillInfo(BillDAO.Instance.GetMaxIDBill(), idFood, idTopping1, idTopping2, foodCount);
                }
            }
            else
            {
                BillInfoDAO.Instance.InsertBillInfo(idBill, idFood, idTopping1, idTopping2, foodCount);
            }

            numUD_FoodCount.Value = 1;
            ShowBill(table.Id);
            ReloadTable();

            lViewBill.Enabled = true;
        }

        private void PayBillForTable()
        {
            if (lViewBill.Tag != null)
            {
                Table curTable = lViewBill.Tag as Table;
                int idBill = BillDAO.Instance.GetUncheckBillIDByTableID(curTable.Id);

                float totalPrice = Convert.ToSingle(tBoxTotalPrice.Texts.Split(',')[0], GE_Culture);

                if (idBill != -1)
                {
                    if (totalPrice == 0 || totalPrice == null)
                    {
                        if (ShowMessQuestion($"Bạn muốn hủy hóa đơn của bàn \"{curTable.Name}\""))
                        {
                            BillDAO.Instance.CheckOut(idBill, totalPrice);
                            ShowBill(idBill);
                        }
                    }
                    else if (ShowMessQuestion($"Bạn có chắc muốn thanh toán hóa đơn của {curTable.Name}\nTổng hóa đơn là {totalPrice.ToString("#,#", GE_Culture)} VNĐ"))
                    {
                        BillDAO.Instance.CheckOut(idBill, totalPrice);
                        ShowBill(idBill);
                    }
                }
                else
                {
                    ShowMessError("Không tìm thấy hóa đơn để thanh toán");
                }

                ReloadTable();
            }
            else return;
        }

        /// <summary>
        /// Hiển thị hóa đơn chưa thanh toán của một bàn cụ thể
        /// </summary>
        /// <param name="tableID"></param>
        private void ShowBill(int tableID)
        {
            lViewBill.Items.Clear();
            float totalBill = 0;

            lViewBill.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            List<InterfaceBillInfoFullDetails> BillInfos = InterfaceBillInfoFullDetailsDAO.Instance.GetInterfaceBillInfoFullDetailsByTableID(tableID);

            foreach (InterfaceBillInfoFullDetails item in BillInfos)
            {
                ListViewItem listViewItem = new ListViewItem(item.FoodName.ToString());
                //listViewItem.SubItems.Add(item.FoodPrice.ToString("#,#", GE_Culture));
                listViewItem.SubItems.Add(item.Topping1Name.ToString());
                //listViewItem.SubItems.Add(item.Topping1Price.ToString("#,#", GE_Culture));
                listViewItem.SubItems.Add(item.Topping2Name.ToString());
                //listViewItem.SubItems.Add(item.Topping2Price.ToString("#,#", GE_Culture));
                listViewItem.SubItems.Add(item.Price.ToString("#,#", GE_Culture));
                listViewItem.SubItems.Add(item.Count.ToString());
                listViewItem.SubItems.Add(item.TotalPrice.ToString("#,#", GE_Culture));

                totalBill += item.TotalPrice;

                lViewBill.Items.Add(listViewItem);
            }

            CultureInfo culture = new CultureInfo("vi-VN");
            tBoxTotalPrice.Texts = totalBill.ToString("c", culture);
        }
        #endregion

        #region Events
        /// <summary>
        /// Lắng nghe sự kiện click vào các Button Controller thể hiện cho các bàn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Click(object sender, EventArgs e)
        {
            //Button btntemp = sender as Button;
            //Table tbltemp = btntemp.Tag as Table;
            //int tableID = tbltemp.Id;
            int tableID = ((sender as Button).Tag as Table).Id;
            lblCurrentTable.Text = ((sender as Button).Tag as Table).Name;
            // Dùng lViewBill.Tag lưu trữ bàn đang được select 
            lblCurrentTable.Tag = (sender as csButton);
            lViewBill.Tag = (sender as csButton).Tag;

            ShowBill(tableID);
        }

        /// <summary>
        /// Show dialog form Admin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAdmin fAdmin = new fAdmin();

            // Theo dõi thông tin tài khoản đang đăng nhập
            fAdmin.loginedAccount = accLogined;

            // Sự kiện trong fAdmin sẽ gọi đến các hàm ở fHome (nếu sự kiện ở fAdmin != null, có hàm thực thi trỏ vào)
            fAdmin.InsertFood += fAdmin_InsertFood; // hàm sẽ được thực thi khi sự kiện ở fAdmin đc gọi
            fAdmin.EditFood += fAdmin_EditFood;
            fAdmin.DeleteFood += fAdmin_DeleteFood;

            fAdmin.InsertCategory += fAdmin_InsertCategory;
            fAdmin.EditCategory += fAdmin_EditCategory;
            fAdmin.DeleteCategory += fAdmin_DeleteCategory;

            fAdmin.InsertTopping_Food += fAdmin_InsertTopping_Food;
            fAdmin.RemoveTopping_Food += fAdmin_RemoveTopping_Food;

            fAdmin.InsertTable += fAdmin_InsertTable;
            fAdmin.EditTable += fAdmin_EditTable;
            fAdmin.DeleteTable += fAdmin_DeleteTable;

            fAdmin.ShowDialog();
        }

        #region fAdmin event
        /// <summary>
        /// Sự kiện được gọi sau khi thêm món thành công ở fAdmin
        /// Hàm được thực hiện khi sự kiện được gọi, đồng bộ giao diện hiển thị vs dữ liệu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fAdmin_InsertFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategory.SelectedItem as Category).Id);
            if (lViewBill.Tag != null)
                ShowBill((lViewBill.Tag as Table).Id);
        }
        private void fAdmin_EditFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategory.SelectedItem as Category).Id);
            if (lViewBill.Tag != null)
                ShowBill((lViewBill.Tag as Table).Id);
            LoadTables();
        }
        private void fAdmin_DeleteFood(object sender, EventArgs e)
        {
            LoadFoodListByCategoryID((cbCategory.SelectedItem as Category).Id);
            if (lViewBill.Tag != null)
                ShowBill((lViewBill.Tag as Table).Id);
        }
        
        private void fAdmin_InsertCategory(object sender, EventArgs e)
        {
            LoadCategory();
            LoadFoodListByCategoryID((cbCategory.SelectedItem as Category).Id);
        }
        private void fAdmin_EditCategory(object sender, EventArgs e)
        {
            LoadCategory();
            LoadFoodListByCategoryID((cbCategory.SelectedItem as Category).Id);
        }
        private void fAdmin_DeleteCategory(object sender, EventArgs e)
        {
            LoadCategory();
            LoadFoodListByCategoryID((cbCategory.SelectedItem as Category).Id);
        }
        
        private void fAdmin_InsertTopping_Food(object sender, EventArgs e)
        {
            LoadToppingComboBox((cbBoxFood.SelectedItem as Food).Id);
        }
        private void fAdmin_RemoveTopping_Food(object sender, EventArgs e)
        {
            LoadToppingComboBox((cbBoxFood.SelectedItem as Food).Id);
        }

        private void fAdmin_InsertTable(object sender, EventArgs e)
        {
            LoadTables();
        }
        private void fAdmin_EditTable(object sender, EventArgs e)
        {
            LoadTables();
        }
        private void fAdmin_DeleteTable(object sender, EventArgs e)
        {
            LoadTables();
        }
        #endregion

        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int categoryID = 0;
            ComboBox cbBox = sender as ComboBox;
            if (cbBox.SelectedItem == null) return;
            Category selected = cbBox.SelectedItem as Category;
            categoryID = selected.Id;
            LoadFoodListByCategoryID(categoryID);
        }
        private void cbBoxFood_SelectedIndexChanged(object sender, EventArgs e)
        {
            int foodID = 0;
            ComboBox cb = sender as ComboBox;
            if (cb.SelectedItem == null) return;
            Food food = cb.SelectedItem as Food;
            foodID = food.Id;

            LoadToppingComboBox(foodID);
        }
        private void thôngTinCáNhânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAccInfo fAccInfo = new fAccInfo(AccLogined);
            fAccInfo.UpdateInfo += fAccInfo_UpdateInfo;
            fAccInfo.ShowDialog();
        }
        
        private void fAccInfo_UpdateInfo(object sender, AccountEvent e)
        {
            tsmItemInfo.Text = $"Thông tin ({e.Account.DisplayName})";
        }

        private void đăngXuấtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPay_Click_1(object sender, EventArgs e)
        {
            PayBillForTable();
        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            AddFoodToTable();
        }

        #endregion

        private void lViewBill_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            foreach (ListViewItem item in lViewBill.CheckedItems)
            {
                int foodID = 0;
                int index = 0;
                foreach(var subItem in item.SubItems)
                {
                    string[] name = subItem.ToString().Split('{', '}');
                    // Khi chuỗi name[1] đang trả tên món
                    if (index == 0)
                    {
                        foodID = FoodDAO.Instance.GetFoodIDByFoodName(name[1]);

                        string categoryName = CategoryDAO.Instance.GetCategoryByID(CategoryDAO.Instance.GetCategoryIDByFoodID(foodID)).Name;

                        for (int i = 0; i < cbCategory.Items.Count; i++)
                        {
                            if (categoryName == (cbCategory.Items[i] as Category).Name)
                                cbCategory.SelectedIndex = i;
                        }

                        for (int i = 0; i < cbBoxFood.Items.Count; i++)
                        {
                            if (name[1] == (cbBoxFood.Items[i] as Food).Name)
                                cbBoxFood.SelectedIndex = i;
                        }
                    }
                    // Chuỗi name[1] trả ra tên topping1
                    if (index == 1)
                    {
                        for (int i = 0; i < cBoxTopping1.Items.Count; i++)
                        {
                            if (name[1] == (cBoxTopping1.Items[i] as Topping).Name)
                                cBoxTopping1.SelectedIndex = i;
                        }
                    }
                    // Chuỗi name[1] trả ra tên topping1
                    if (index == 2)
                    {
                        for (int i = 0; i < cBoxTopping2.Items.Count; i++)
                        {
                            if (name[1] == (cBoxTopping2.Items[i] as Topping).Name)
                                cBoxTopping2.SelectedIndex = i;
                        }
                        break;
                    }
                    index++;
                }
            }
            if (lViewBill.CheckedItems.Count == 1)
            {
                lViewBill.Enabled = false;
                numUD_FoodCount.Value = 0;
            }
        }
    }
}
