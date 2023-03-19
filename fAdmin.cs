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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace CoffeeStore
{
    public partial class fAdmin : Form
    {
        BindingSource revenueListInfo = new BindingSource();
        BindingSource listFoods = new BindingSource();
        BindingSource categoriesList = new BindingSource();
        BindingSource toppingsList = new BindingSource();
        BindingSource topping_foodsList = new BindingSource();
        BindingSource tablesList = new BindingSource();
        BindingSource accountsList = new BindingSource();

        public Account loginedAccount;

        public fAdmin()
        {
            InitializeComponent();

            CallLoad();
        }

        #region Methods
        private void CallLoad()
        {
            dtGVBill.DataSource = revenueListInfo;
            dtGVFood.DataSource = listFoods;
            dtGVCategory.DataSource = categoriesList;
            dtGVTopping.DataSource = toppingsList;
            dtGVTopping_Food.DataSource = topping_foodsList;
            dtGVTable.DataSource = tablesList;
            dtGVAcc.DataSource = accountsList;

            LoadDTPickerForStatistical();

            LoadListBillByDate(dtPickerFromDate.Value, dtPickerToDate.Value);
            LoadListFoods(GetOrderByFromRdBtn());
            LoadCategoriesList();
            LoadToppingsList();
            LoadInterfaceTopping_Food(1);
            LoadTablesList();
            LoadAccountsList();

            AddRevenueBinding();
            AddFoodBinding();
            AddCategoryBinding();
            AddToppingBinding();
            AddTopping_FoodBinding();
            AddTableBinding();
            AddAccountBinding();
        }

        #region Doanh thu Methods
        private void LoadDTPickerForStatistical()
        {
            DateTime today = DateTime.Now;
            dtPickerFromDate.Value = new DateTime(today.Year, today.Month, 1);
            dtPickerToDate.Value = dtPickerFromDate.Value.AddMonths(1).AddDays(-1);
        }
        private void LoadListBillByDate(DateTime dateCheckIn, DateTime dateCheckOut)
        {
            List<InterfaceRevenueByDate> list = InterfaceRevenueByDateDAO.Instance.GetListBillByDate(dateCheckIn, dateCheckOut);
            revenueListInfo.DataSource = list;

            float totalRevenue = 0;
            foreach (InterfaceRevenueByDate item in list)
            {
                totalRevenue += item.TotalPrice;
            }
            tBoxTotalRevenue.Texts = totalRevenue.ToString("c", new CultureInfo("vi-VN"));
        }
        private void AddRevenueBinding()
        {
            tBoxBillID.DataBindings.Add(new Binding("Text", dtGVBill.DataSource, "Id", true, DataSourceUpdateMode.Never));
            lblTableName.DataBindings.Add(new Binding("Text", dtGVBill.DataSource, "TableName", true, DataSourceUpdateMode.Never));
            // Lưu vào tag rồi chuyển từ object conctrol sang string "c" trong text ở hàm ShowRevenueDetails() -> Cập nhật bị delay nên không dùng đc
            tBoxTotalPrice.DataBindings.Add(new Binding("Texts", dtGVBill.DataSource, "TotalPrice", true, DataSourceUpdateMode.Never));
        }
        private void ShowRevenueDetails()
        {
            lViewDetail.Items.Clear();

            List<InterfaceRevenue> list = InterfaceRevenueDAO.Instance.GetListInterfaceRevenues(tBoxBillID.Text);

            foreach (InterfaceRevenue item in list)
            {
                ListViewItem listViewItem = new ListViewItem(item.FoodName.ToString());
                listViewItem.SubItems.Add(item.FoodCount.ToString());
                lViewDetail.Items.Add(listViewItem);
            }

            //tBoxTotalPrice.Texts = Convert.ToSingle(tBoxTotalPrice.Tag).ToString("#,#", new CultureInfo("vi-VN"));
        }
        #endregion

        #region Food View / Methods
        private void LoadListFoods(string orderBy)
        {
            listFoods.DataSource = InterfaceFoodInfoDAO.Instance.GetListInterfaceFoodInfo(orderBy);
        }
        private void AddFoodBinding()
        {
            tBoxFoodID.DataBindings.Add(new Binding("Text", dtGVFood.DataSource, "ID", true, DataSourceUpdateMode.Never));
            tBoxFoodName.DataBindings.Add(new Binding("Text", dtGVFood.DataSource, "FoodName", true, DataSourceUpdateMode.Never));
            LoadCategoryComboBox(cBoxFoodCategory);
            nudFoodPrice.DataBindings.Add(new Binding("Value", dtGVFood.DataSource, "FoodPrice", true, DataSourceUpdateMode.Never));
        }
        /// <summary>
        /// Load list of categorys for ComboBox's source ans set DisplayMember of this cbBox
        /// </summary>
        /// <param name="cb"></param>
        private void LoadCategoryComboBox(ComboBox cb)
        {
            cb.DataSource = CategoryDAO.Instance.GetListCategory();
            cb.DisplayMember = "Name";
        }
        private List<InterfaceFoodInfo> FindFoodByName(string approName, string orderBy)
        {
            List<InterfaceFoodInfo> list = InterfaceFoodInfoDAO.Instance.GetListFoodByName(approName, orderBy);

            return list;
        }
        private string GetOrderByFromRdBtn()
        {
            string orderBy = "";
            if (rdBtnID.Checked)
                orderBy = "order by ID";

            if (rdBtnFoodName.Checked)
                orderBy = "order by TenMon";

            if (rdBtnCategoryName.Checked)
                orderBy = "order by TenDM";

            if (rdBtnPrice.Checked)
                orderBy = "order by GiaMonAn";

            return orderBy;
        }
        
        private void AddFood()
        {
            string foodName = tBoxFoodName.Text;
            int categoryID = (cBoxFoodCategory.SelectedItem as Category).Id;
            float foodPrice = Convert.ToSingle(nudFoodPrice.Value);
            string categoryName = (cBoxFoodCategory.SelectedItem as Category).Name;

            List<InterfaceFoodInfo> list = InterfaceFoodInfoDAO.Instance.GetListInterfaceFoodInfo();
            foreach (InterfaceFoodInfo item in list)
            {
                if (item.FoodName == foodName)
                {
                    ShowMessError($"\"{foodName}\" đã tồn tại trong cơ sở dữ liệu");
                    return;
                }
            }

            if (ShowMessQuestion($"Thêm mới \"{foodName}\" vào danh mục \"{categoryName}\"?"))
            {
                if (FoodDAO.Instance.InsertFood(foodName, categoryID, foodPrice))
                {
                    ShowMessSuccess($"Thêm thành công \"{foodName}\" vào danh mục \"{categoryName}\"");

                    LoadListFoods(GetOrderByFromRdBtn());
                    if (insertFood != null)
                        insertFood(this, new EventArgs());
                }
                else
                {
                    ShowMessError($"Đã xảy ra lỗi khi thêm món");
                }
            }
        }
        private void EditFoodInfo()
        {
            string foodName = tBoxFoodName.Text;
            int categoryID = (cBoxFoodCategory.SelectedItem as Category).Id;
            float foodPrice = Convert.ToSingle(nudFoodPrice.Value);
            int foodID = Convert.ToInt32(tBoxFoodID.Text);

            List<InterfaceFoodInfo> list = InterfaceFoodInfoDAO.Instance.GetListInterfaceFoodInfo();
            foreach (InterfaceFoodInfo item in list)
            {
                if (item.FoodName == foodName && item.FoodPrice == foodPrice)
                {
                    ShowMessError($"Thông tin về món \"{foodName}\" không có gì thay đổi");
                    return;
                }
            }

            if (ShowMessQuestion($"Thay đổi thông tin cho \"{foodName}\"?"))
            {
                if (FoodDAO.Instance.EditFoodInfo(foodName, categoryID, foodPrice, foodID))
                {
                    ShowMessSuccess($"Thay đổi thông tin \"{foodName}\" thành công");

                    LoadListFoods(GetOrderByFromRdBtn());
                    if (editFood != null)
                        editFood(this, new EventArgs());
                }
                else
                {
                    ShowMessError($"Đã xảy ra lỗi khi thay đổi thông tin món");
                }
            }
        }
        private void RemoveFood()
        {
            int foodID = Convert.ToInt32(tBoxFoodID.Text);
            string foodName = tBoxFoodName.Text;

            if (ShowMessQuestion($"Xóa món \"{foodName}\" khỏi danh mục \"{(cBoxFoodCategory.SelectedItem as Category).Name}\"?"))
            {
                if (FoodDAO.Instance.DeleteFood(foodID))
                {
                    ShowMessSuccess($"Xóa món thành công");

                    LoadListFoods(GetOrderByFromRdBtn());
                    if (deleteFood != null)
                        deleteFood(this, new EventArgs());
                }
                else
                {
                    ShowMessError($"Đã xảy ra lỗi khi xóa món");
                }
            }
        }
        #endregion

        #region CategoryView / Methods
        private void LoadCategoriesList()
        {
            categoriesList.DataSource = CategoryDAO.Instance.GetListCategory();
        }
        private void AddCategoryBinding()
        {
            tBoxCategoryID.DataBindings.Add("Text", dtGVCategory.DataSource, "Id", true, DataSourceUpdateMode.Never);
            tBoxCategoryName.DataBindings.Add("Text", dtGVCategory.DataSource, "Name", true, DataSourceUpdateMode.Never);
        }
        
        private void AddCategory()
        {
            string categoryName = tBoxCategoryName.Text;

            List<Category> list = CategoryDAO.Instance.GetListCategory();
            foreach (Category item in list)
            {
                if (item.Name == categoryName)
                {
                    ShowMessError($"Đã tồn tại danh mục {categoryName}\nXin vui lòng chọn danh mục khác");
                    return;
                }
            }

            if (ShowMessQuestion($"Thêm mới danh mục \"{categoryName}\"?"))
            {
                if (CategoryDAO.Instance.InsertCategory(categoryName))
                {
                    ShowMessSuccess($"Thêm thành công danh mục {categoryName}");
                    LoadCategoriesList();
                    LoadListFoods(GetOrderByFromRdBtn());
                    LoadCategoryComboBox(cBoxFoodCategory);

                    if (insertCategory != null)
                        insertCategory(this, new EventArgs());
                }
                else
                {
                    ShowMessError("Đã xảy ra lỗi khi thêm danh mục món ăn");
                }
            }
        }
        private void EditCategoryInfo()
        {
            string categoryName = tBoxCategoryName.Text;
            int categoryId = Convert.ToInt32(tBoxCategoryID.Text);

            List<Category> list = CategoryDAO.Instance.GetListCategory();
            foreach (Category item in list)
            {
                if (item.Name == categoryName)
                {
                    ShowMessError($"Thông tin về danh mục \"{categoryName}\" không có gì thay đổi");
                    return;
                }
            }

            if (ShowMessQuestion($"Thay đổi tên danh mục có ID = {categoryId} thành \"{categoryName}\""))
            {
                if (CategoryDAO.Instance.EditCategory(categoryName, categoryId))
                {
                    ShowMessSuccess($"Thay đổi thành công thông tin danh mục");
                    LoadCategoriesList();
                    LoadListFoods(GetOrderByFromRdBtn());
                    LoadCategoryComboBox(cBoxFoodCategory);

                    if (editCategory != null)
                        editCategory(this, new EventArgs());
                }
                else
                {
                    ShowMessError("Đã xảy ra lỗi khi thay đổi thông tin danh mục món ăn");
                }
            }
        }
        private void RemoveCategory()
        {
            int categoryId = Convert.ToInt32(tBoxCategoryID.Text);
            string categoryName = tBoxCategoryName.Text;

            if (ShowMessQuestion($"Xóa danh mục \"{categoryName}\"?"))
            {
                if (CategoryDAO.Instance.DeleteCategory(categoryId))
                {
                    ShowMessSuccess($"Xóa danh mục \"{categoryName}\" thành công");
                    LoadCategoriesList();
                    LoadListFoods(GetOrderByFromRdBtn());
                    LoadCategoryComboBox(cBoxFoodCategory);/*reload*/

                    if (deleteCategory != null)
                        deleteCategory(this, new EventArgs());
                }
                else
                {
                    ShowMessError($"Vui lòng xóa các món ăn đang thuộc trong danh mục \"{categoryName}\" trước");
                }
            }
        }
        #endregion

        #region Topping View / Methods
        #region Topping
        private void LoadToppingsList()
        {
            toppingsList.DataSource = ToppingDAO.Instance.GetToppingsList();
        }
        private void AddToppingBinding()
        {
            tBoxToppingID.DataBindings.Add(new Binding("Text", dtGVTopping.DataSource, "ID", true, DataSourceUpdateMode.Never));
            tBoxToppingName.DataBindings.Add(new Binding("Text", dtGVTopping.DataSource, "Name", true, DataSourceUpdateMode.Never));
            numToppingPrice.DataBindings.Add(new Binding("Value", dtGVTopping.DataSource, "Price", true, DataSourceUpdateMode.Never));
        }
        private void FindToppingByApproName(string approName)
        {
            toppingsList.DataSource = ToppingDAO.Instance.GetListToppingByApproToppingName(approName);
        }
        private void AddTopping()
        {
            string toppingName = tBoxToppingName.Text;
            float toppingPrice = Convert.ToSingle(numToppingPrice.Value);

            List<Topping> list = ToppingDAO.Instance.GetToppingsList();
            foreach (Topping item in list)
            {
                if (item.Name == toppingName)
                {
                    ShowMessError($"\"{toppingName}\" đã tồn tại trong cơ sở dữ liệu");
                    return;
                }
            }

            if (ShowMessQuestion($"Thêm mới \"{toppingName}\" vào cơ sở dữ liệu?"))
            {
                if (ToppingDAO.Instance.InsertTopping(toppingName, toppingPrice))
                {
                    ShowMessSuccess($"Thêm thành công \"{toppingName}\" vào cơ sở dữ liệu");

                    LoadToppingsList();
                }
                else
                {
                    ShowMessError($"Đã xảy ra lỗi khi thêm topping");
                }
            }
        }
        private void EditToppingInfo()
        {
            int toppingID = Convert.ToInt32(tBoxToppingID.Text);
            string toppingName = tBoxToppingName.Text;
            float toppingPrice = Convert.ToSingle(numToppingPrice.Value);

            List<Topping> list = ToppingDAO.Instance.GetToppingsList();
            foreach (Topping item in list)
            {
                if (item.Name == toppingName && item.Price == toppingPrice)
                {
                    ShowMessError($"Thông tin về \"{toppingName}\" không có gì thay đổi");
                    return;
                }
            }

            if (ShowMessQuestion($"Thay đổi thông tin về topping \"{toppingName}\"?"))
            {
                if (ToppingDAO.Instance.EditToppingInfo(toppingName, toppingPrice, toppingID))
                {
                    ShowMessSuccess($"Thay đổi thành công thông tin \"{toppingName}\" vào cơ sở dữ liệu");

                    LoadToppingsList();
                }
                else
                {
                    ShowMessError($"Đã xảy ra lỗi khi thay đổi thông tin topping");
                }
            }
        }
        private void RemoveTopping()
        {
            int toppingID = Convert.ToInt32(tBoxToppingID.Text);
            string toppingName = tBoxToppingName.Text;

            if (ShowMessQuestion($"Xóa topping \"{toppingName}\" khỏi cơ sở dữ liệu?"))
            {
                if (ToppingDAO.Instance.DeleteTopping(toppingID))
                {
                    ShowMessSuccess($"Xóa thành công \"{toppingName}\" vào cơ sở dữ liệu");

                    LoadToppingsList();
                }
                else
                {
                    ShowMessError($"Đã xảy ra lỗi khi xóa topping");
                }
            }
        }
        #endregion

        #region Topping_Food
        private void LoadInterfaceTopping_Food(int toppingID)
        {
            topping_foodsList.DataSource = InterfaceTopping_FoodDAO.Instance.GetListFoodByIDTopping(toppingID);
        }
        private void AddTopping_FoodBinding()
        {
            tBoxTopping_FoodID.DataBindings.Add(new Binding("Text", dtGVTopping_Food.DataSource, "FoodId", true, DataSourceUpdateMode.Never));
            // đã Binding Name theo id change
            //tBoxTopping_FoodName.DataBindings.Add(new Binding("Text", dtGVTopping_Food.DataSource, "FoodName", true, DataSourceUpdateMode.Never));
        }

        private void AddTopping_Food()
        {
            int foodID = Convert.ToInt32(tBoxTopping_FoodID.Value);
            string foodName = tBoxTopping_FoodName.Text;
            int toppingID = Convert.ToInt32(tBoxToppingID.Text);
            string toppingName = tBoxToppingName.Text;

            List<InterfaceTopping_Food> list = InterfaceTopping_FoodDAO.Instance.GetListFoodByIDTopping(toppingID);
            foreach (InterfaceTopping_Food item in list)
            {
                if (item.FoodName == foodName)
                {
                    ShowMessError($"Món \"{foodName}\" đã tồn tại topping \"{toppingName}\"");
                    return;
                }
            }

            if (ShowMessQuestion($"Thêm topping \"{toppingName}\" vào món \"{foodName}\"?"))
            {
                if (InterfaceTopping_FoodDAO.Instance.AddFoodToTopping(foodID, toppingID))
                {
                    ShowMessSuccess($"Thêm thành công topping \"{toppingName}\" vào món \"{foodName}\"");

                    LoadInterfaceTopping_Food(toppingID);
                    if (insertTopping_Food != null)
                        insertTopping_Food(this, new EventArgs());
                }
                else
                {
                    ShowMessError("Đã xảy ra lỗi khi thêm topping vào món ăn");
                }
            }
        }
        private void DeleteTopping_Food()
        {
            int foodID = Convert.ToInt32(tBoxTopping_FoodID.Value);
            string foodName = tBoxTopping_FoodName.Text;
            int toppingID = Convert.ToInt32(tBoxToppingID.Text);
            string toppingName = tBoxToppingName.Text;

            List<InterfaceTopping_Food> list = InterfaceTopping_FoodDAO.Instance.GetListFoodByIDTopping(toppingID);
            bool exists = false;
            foreach (InterfaceTopping_Food item in list)
            {
                if (foodName == item.FoodName) exists = true;
            }
            if (!exists)
            {
                ShowMessError($"Không tồn tại topping \"{toppingName}\" trong món \"{foodName}\"");
                return;
            }

            if (ShowMessQuestion($"Xóa topping \"{toppingName}\" khỏi món ăn \"{foodName}\"?"))
            {
                if (InterfaceTopping_FoodDAO.Instance.DeleteTopping_Food(foodID, toppingID))
                {
                    ShowMessSuccess($"Xóa thành công topping \"{toppingName}\" khỏi món ăn \"{foodName}\"");

                    LoadInterfaceTopping_Food(toppingID);
                    if (removeTopping_Food != null)
                        removeTopping_Food(this, new EventArgs());
                }
                else
                {
                    ShowMessError($"Món \"{foodName}\" với topping \"{toppingName}\" đang được khách hàng gọi, không thể xóa");
                }
            }
        }
        #endregion
        #endregion

        #region Table View / Methods
        private void LoadTablesList()
        {
            tablesList.DataSource = TableDAO.Instance.GetTablesList();
        }
        private void AddTableBinding()
        {
            tBoxTableID.DataBindings.Add(new Binding("Text", dtGVTable.DataSource, "Id"));
            tBoxTableName.DataBindings.Add(new Binding("Text", dtGVTable.DataSource, "Name"));
            tBoxTableStatus.DataBindings.Add(new Binding("Text", dtGVTable.DataSource, "Status"));
        }
        private void AddTable()
        {
            string tableName = tBoxTableName.Text;

            List<Table> list = TableDAO.Instance.GetTablesList();
            foreach (Table table in list)
            {
                if (tableName == table.Name)
                {
                    ShowMessError($"Đã tồn tại bàn có tên \"{tableName}\"");
                    return;
                }
            }

            if (ShowMessQuestion($"Thêm mới bàn \"{tableName}\"?"))
            {
                if (TableDAO.Instance.AddTable(tableName))
                {
                    ShowMessSuccess($"Thêm thành công \"{tableName}\"");
                    LoadTablesList();

                    if (insertTable != null)
                        insertTable(this, new EventArgs());
                }   
                else
                {
                    ShowMessError("Đã xảy ra lỗi khi thêm bàn");
                }
            }
        }
        private void EditTableInfo()
        {
            int tableID = Convert.ToInt32(tBoxTableID.Text);
            string tableName = tBoxTableName.Text;

            List<Table> list = TableDAO.Instance.GetTablesList();
            foreach (Table table in list)
            {
                if (tableName == table.Name)
                {
                    ShowMessError($"Thông tin về bàn \"{tableName}\" không có gì thay đổi");
                    return;
                }
            }

            if (ShowMessQuestion($"Thay đổi thông tin cho \"{tableName}\"?"))
            {
                if (TableDAO.Instance.EditTableInfo(tableID, tableName))
                {
                    ShowMessSuccess($"Thay đổi thông tin \"{tableName}\" thành công");
                    LoadTablesList();
                    LoadListBillByDate(dtPickerFromDate.Value, dtPickerToDate.Value);

                    if (editTable != null)
                        editTable(this, new EventArgs());
                }
                else
                {
                    ShowMessError($"Đã xảy ra lỗi khi thay đổi thông tin bàn");
                }
            }
        }
        private void RemoveTable()
        {
            int tableID = Convert.ToInt32(tBoxTableID.Text);
            string tableName = tBoxTableName.Text;
            string tableStatus = tBoxTableStatus.Text;
           

            if (tableStatus == "Có khách")
            {
                ShowMessError($"\"{tableName}\" đang có khách, không thể xóa");
                return;
            }

            if (ShowMessQuestion($"Xóa bàn \"{tableName}\"?"))
            {
                if (TableDAO.Instance.RemoveTable(tableID))
                {
                    ShowMessSuccess($"Xóa bàn thành công");
                    LoadTablesList();
                    LoadListBillByDate(dtPickerFromDate.Value, dtPickerToDate.Value);

                    if (deleteTable != null)
                        deleteTable(this, new EventArgs());
                }
                else
                {
                    ShowMessError($"Đã xảy ra lỗi trong lúc xóa bàn");
                }
            }
        }
        #endregion

        #region Account View / Methods
        private void LoadAccountsList()
        {
            accountsList.DataSource = InterfaceAccInfoDAO.Instance.GetAccountsList();
        }
        private void AddAccountBinding()
        {
            tBoxUserName.DataBindings.Add(new Binding("Text", dtGVAcc.DataSource, "UserName", true, DataSourceUpdateMode.Never));
            tBoxDisplayName.DataBindings.Add(new Binding("Text", dtGVAcc.DataSource, "DisplayName", true, DataSourceUpdateMode.Never));
            LoadTypeAccComboBox(cBoxTypeAcc);
        }
        private void LoadTypeAccComboBox(ComboBox cb)
        {
            cb.DataSource = TypeAccDAO.Instance.GetListTypeAcc();
            cb.DisplayMember = "TypeName";
        }
        
        private void AddAccount()
        {
            string userName = tBoxUserName.Text;
            string displayName = tBoxDisplayName.Text;
            int type = (cBoxTypeAcc.SelectedItem as TypeAcc).IdType;

            List<string> list = AccountDAO.Instance.GetUserNameAccountsList();
            foreach (string item in list)
            {
                if (userName == item)
                {
                    ShowMessError($"Đã tồn tại tài khoản với tên đăng nhập \"{userName}\"");
                    return;
                }
            }

            if (ShowMessQuestion($"Thêm mới tài khoản với tên đăng nhập \"{userName}\"?"))
            {
                if (AccountDAO.Instance.InsertAccount(userName, displayName, type))
                {
                    ShowMessSuccess($"Thêm tài khoản thành công với mật khẩu mặc định\nVui lòng đăng nhập vào tài khoản để đổi mật khẩu");
                    LoadAccountsList();
                }
                else
                {
                    ShowMessError($"Đã xảy ra lỗi trong lúc thêm tài khoản");
                }
            }
        }
        private void RemoveAccount()
        {
            string userName = tBoxUserName.Text;
            if (loginedAccount.UserName.Equals(userName))
            {
                ShowMessError("Không thể xóa tài khoản đang đăng nhập");
                return;
            }

            if (ShowMessQuestion($"Bạn có chắc muốn xóa tài khoản \"{userName}\""))
            {
                if (AccountDAO.Instance.DeleteAccount(userName))
                {
                    ShowMessSuccess($"Xóa tài khoản thành công");
                    LoadAccountsList();
                }
                else
                {
                    ShowMessError($"Đã xảy ra lỗi trong lúc xóa tài khoản");
                }
            }
        }
        private void EditAccountInfo()
        {
            string userName = tBoxUserName.Text;
            string displayName = tBoxDisplayName.Text;
            int type = (cBoxTypeAcc.SelectedItem as TypeAcc).IdType;

            if (ShowMessQuestion($"Bạn muốn thay đổi thông tin tài khoản?"))
            {
                if (AccountDAO.Instance.EditAccInfo(userName, displayName, type))
                {
                    ShowMessSuccess($"Sửa thông tin tài khoản thành công");
                    LoadAccountsList();
                }
                else
                {
                    ShowMessError($"Đã xảy ra lỗi trong lúc sửa thông tin tài khoản");
                }
            }
        }
        private void ResetPass()
        {
            string userName = tBoxUserName.Text;
            if (AccountDAO.Instance.ResetPass(userName))
            {
                MessageBox.Show($"Đặt lại mật khẩu thành mật khẩu mặc định thành công");
            }
            else
                MessageBox.Show($"Đặt lại mật khẩu thất bại");
        }
        #endregion

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

        #endregion


        #region events

        #region Doanh Thu / Events
        private void csBtnStatistical_Click(object sender, EventArgs e)
        {
            LoadListBillByDate(dtPickerFromDate.Value, dtPickerToDate.Value);
        }
        private void tBoxBillID_TextChanged(object sender, EventArgs e)
        {
            ShowRevenueDetails();
        }
        #endregion
        
        #region FoodView / Events
        private void btnViewFood_Click(object sender, EventArgs e)
        {
            LoadListFoods(GetOrderByFromRdBtn());
        }

        // TextChanged đồng nghĩa người dùng select sang món có id khác
        private void tBoxFoodID_TextChanged(object sender, EventArgs e)
        {
            if (dtGVFood.SelectedCells.Count > 0)
            {
                string categoryName = (string)dtGVFood.SelectedCells[0].OwningRow.Cells["CategoryName"].Value;

                // int id = (int)dtGVFood.SelectedCells[0].OwningRow.Cells["Id"].Value;
                //Category category = CategoryDAO.Instance.GetCategoryByID(id);
                //cBoxFoodCategory.SelectedItem = category;

                int index = -1;
                int i = 0;
                // duyệt qua item trong DanhMuc, nào trùng vs idDM đc select thì trả ra index
                foreach (Category item in cBoxFoodCategory.Items)
                {
                    if (item.Name == categoryName)
                    {
                        index = i; break;
                    }
                    i++;
                }
                // đổi sang hiển thị item có index là id của DanhMuc
                cBoxFoodCategory.SelectedIndex = index;
            }
        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            AddFood();
        }

        private void btnEditFood_Click(object sender, EventArgs e)
        {
            EditFoodInfo();
        }

        private void btnRemoveFood_Click(object sender, EventArgs e)
        {
            RemoveFood();
        }

        private event EventHandler insertFood;
        private event EventHandler editFood;
        private event EventHandler deleteFood;

        public event EventHandler InsertFood
        {
            add { insertFood += value; }
            remove { insertFood -= value; }
        }
        public event EventHandler EditFood
        {
            add { editFood += value; }
            remove { editFood -= value; }
        }
        public event EventHandler DeleteFood
        {
            add { deleteFood += value; }
            remove { deleteFood -= value; }
        }

        private void btnFindFood_Click(object sender, EventArgs e)
        {
            listFoods.DataSource = FindFoodByName(tBoxFindFood.Text, GetOrderByFromRdBtn());
        }
        private void tBoxFindFood_TextChanged(object sender, EventArgs e)
        {
            listFoods.DataSource = FindFoodByName(tBoxFindFood.Text, GetOrderByFromRdBtn());
        }
        #endregion
        
        #region Category View / Events
        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            AddCategory();
        }
        private void btnAdjustCategory_Click(object sender, EventArgs e)
        {
            EditCategoryInfo();
        }
        private void btnRemoveCategory_Click(object sender, EventArgs e)
        {
            RemoveCategory();
        }
        private void tBoxCategoryID_TextChanged(object sender, EventArgs e)
        {
            btnAddCategory.Enabled = false;
            btnAdjustCategory.Enabled = false;
        }
        private void tBoxCategoryName_TextChanged(object sender, EventArgs e)
        {
            btnAddCategory.Enabled = true;
            btnAdjustCategory.Enabled = true;
        }

        private event EventHandler insertCategory;
        private event EventHandler editCategory;
        private event EventHandler deleteCategory;

        public event EventHandler InsertCategory
        {
            add { insertCategory += value; }
            remove { insertCategory -= value; }
        }
        public event EventHandler EditCategory
        {
            add { editCategory += value; }
            remove { editCategory -= value; }
        }
        public event EventHandler DeleteCategory
        {
            add { deleteCategory += value; }
            remove { deleteCategory -= value; }
        }
        #endregion

        #region Topping View / Events
        #region Topping
        private void tBoxFindTopping_TextChanged(object sender, EventArgs e)
        {
            string approName = tBoxFindTopping.Text;
            FindToppingByApproName(approName);
        }
        private void tBoxToppingID_TextChanged(object sender, EventArgs e)
        {
            if (tBoxToppingID.Text == "" || tBoxToppingID.Text == null) return;
            int toppingID = Convert.ToInt32(tBoxToppingID.Text);
            LoadInterfaceTopping_Food(toppingID);

            //btnAddTopping.Enabled = false;
            //btnAdjustTopping.Enabled = false;
        }
        private void tBoxToppingName_TextChanged(object sender, EventArgs e)
        {
            //btnAddTopping.Enabled = true;
            //btnAdjustTopping.Enabled = true;
        }
        private void btnAddTopping_Click(object sender, EventArgs e)
        {
            AddTopping();
        }
        private void btnAdjustTopping_Click(object sender, EventArgs e)
        {
            EditToppingInfo();
        }
        private void btnRemoveTopping_Click(object sender, EventArgs e)
        {
            RemoveTopping();
        }
        #endregion

        #region Topping_Food
        private void tBoxTopping_FoodID_ValueChanged(object sender, EventArgs e)
        {
            int foodID = Convert.ToInt32(tBoxTopping_FoodID.Value);
            tBoxTopping_FoodName.Text = FoodDAO.Instance.GetFoodNameByFoodID(foodID);
        }
        private void btnAddTopping_Food_Click(object sender, EventArgs e)
        {
            AddTopping_Food();
        }
        private void btnRemoveTopping_Food_Click(object sender, EventArgs e)
        {
            DeleteTopping_Food();
        }

        private event EventHandler insertTopping_Food;
        private event EventHandler removeTopping_Food;

        public event EventHandler InsertTopping_Food
        {
            add { insertTopping_Food += value; }
            remove { insertTopping_Food -= value; }
        }
        public event EventHandler RemoveTopping_Food
        {
            add { removeTopping_Food += value; }
            remove { removeTopping_Food -= value; }

        }
        #endregion
        #endregion

        #region Table View / Events
        private void btnAddTable_Click(object sender, EventArgs e)
        {
            AddTable();
        }
        private void btnAdjustTable_Click(object sender, EventArgs e)
        {
            EditTableInfo();
        }
        private void btnDeleteTable_Click(object sender, EventArgs e)
        {
            RemoveTable();
        }

        private event EventHandler insertTable;
        private event EventHandler editTable;
        private event EventHandler deleteTable;

        public event EventHandler InsertTable
        {
            add { insertTable += value; }
            remove { insertTable -= value; }
        }
        public event EventHandler EditTable
        {
            add { editTable += value; }
            remove { editTable -= value; }
        }
        public event EventHandler DeleteTable
        {
            add { deleteTable += value; }
            remove { deleteTable -= value; }
        }
        #endregion

        #region AccountView
        // Binding phan loai
        private void tBoxUserName_TextChanged(object sender, EventArgs e)
        {
            if (dtGVAcc.SelectedCells.Count > 0)
            {
                string typeName = (string)dtGVAcc.SelectedCells[0].OwningRow.Cells["PhanLoai"].Value;

                // int id = (int)dtGVFood.SelectedCells[0].OwningRow.Cells["Id"].Value;
                //Category category = CategoryDAO.Instance.GetCategoryByID(id);
                //cBoxFoodCategory.SelectedItem = category;

                int index = -1;
                int i = 0;
                foreach (TypeAcc item in cBoxTypeAcc.Items)
                {
                    if (item.TypeName == typeName)
                    {
                        index = i; break;
                    }
                    i++;
                }
                cBoxTypeAcc.SelectedIndex = index;
            }
        }
        private void btnAddAcc_Click(object sender, EventArgs e)
        {
            AddAccount();
        }
        private void btnAdjustAcc_Click(object sender, EventArgs e)
        {
            EditAccountInfo();
        }
        private void btnRemoveAcc_Click(object sender, EventArgs e)
        {
            RemoveAccount();
        }
        private void btnResetPass_Click(object sender, EventArgs e)
        {
            ResetPass();
        }



        #endregion

        #endregion


    }
}
