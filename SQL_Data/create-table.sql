create database TheCoffeeHouse
go

use TheCoffeeHouse
go

create table PhanQuyen (
	MaPhanLoai int not null primary key,
	PhanLoai nvarchar(20) not null,
)
go

create table TaiKhoan (
	TenDangNhap nvarchar(50) not null primary key,
	TenHienThi nvarchar(50) not null,
	MatKhau nvarchar(100) not null default 'thecoffeehouse',
	PhanQuyen int not null default 0	-- 1 là admin, 0 là nhân viên
)
go

create table Ban (
	ID int identity primary key,
	TenBan nvarchar(50) not null,
	TrangThai nvarchar(50) not null default N'Trống',	-- trống, có khách, được đặt
)
go

create table DanhMuc (
	ID int identity primary key,
	TenDM nvarchar(50) not null default N'Đang cập nhật',
)
go

create table MonAn (
	ID int identity primary key,
	TenMon nvarchar(50) not null default N'Đang cập nhật',
	idDM int not null,
	GiaMonAn float not null default 0,
)
go

-- New
create table Topping (
	ID int identity primary key,
	TenTopping nvarchar(100) not null,
	GiaTopping float not null default 0,
)
go

create table MonAn_Topping (
	ID int identity primary key,
	idMonAn int not null,
	idTopping int not null,
)
go
-- End New

create table HoaDon (
	ID int identity primary key,
	ThoiGianVao DateTime not null default getDate(),
	ThoiGianRa DateTime,
	idBan int not null,
	TrangThai int not null default 0,		-- 0 chua thanh toan, 1 da thanh toan
	TongTien float default 0
)
go

create table ChiTietHoaDon (
	ID int identity primary key,
	idHoaDon int not null,
	idMonAn int not null,
	idTopping1 int not null default 2,
	idTopping2 int not null default 2,
	SoLuong int not null default 0,
)
go

-- Tạo khóa ngoại cho các bảng
alter table TaiKhoan add foreign key (PhanQuyen) references PhanQuyen(MaPhanLoai)

alter table MonAn add foreign key (idDM) references dbo.DanhMuc(ID)

alter table MonAn_Topping add foreign key (idMonAn) references MonAn(ID)
alter table MonAn_Topping add foreign key (idTopping) references Topping(ID)

alter table HoaDon add foreign key (idBan) references dbo.Ban(id)

alter table ChiTietHoaDon add foreign key (idHoaDon) references dbo.HoaDon(id)
alter table ChiTietHoaDon add foreign key (idMonAn) references dbo.MonAn(id)
go

-- Tạo store proceduce
create proc USP_GetTableList
as select * from dbo.Ban
go

create proc USP_InsertBill
@idBan int
as
begin
	insert HoaDon (ThoiGianVao, ThoiGianRa, idBan, TrangThai)
	values (GETDATE(), null, @idBan, 0)
end
go

--drop proc USP_InsertBillInfo
create proc USP_InsertBillInfo
@idHoaDon int, @idMonAn int, @idTopping1 int, @idTopping2 int, @SoLuong int
as
begin
	declare @isExitsBillInfo int
	declare @foodCount int

	select @isExitsBillInfo = ID, @foodCount = SoLuong
	from ChiTietHoaDon
	where idHoaDon = @idHoaDon and idMonAn = @idMonAn and ((idTopping1 = @idTopping1 and idTopping2 = @idTopping2) or (idTopping1 = @idTopping2 and idTopping2 = @idTopping1))

	-- Đã tồn tại món trong hóa đơn của bàn
	if (@isExitsBillInfo > 0)
	begin
		declare @newCount int = @foodCount + @SoLuong
		if (@newCount > 0)
			update ChiTietHoaDon set SoLuong = @newCount where idMonAn = @idMonAn and ((idTopping1 = @idTopping1 and idTopping2 = @idTopping2) or (idTopping1 = @idTopping2 and idTopping2 = @idTopping1))
		else	-- Trường hợp tổng số lượng của món âm -> xóa món
			delete ChiTietHoaDon where idHoaDon = @idHoaDon and idMonAn = @idMonAn and ((idTopping1 = @idTopping1 and idTopping2 = @idTopping2) or (idTopping1 = @idTopping2 and idTopping2 = @idTopping1))
	end
	-- Bàn chưa có hóa đơn chưa thanh toán --Bàn trống
	else
	begin
		if (@SoLuong > 0) insert ChiTietHoaDon (idHoaDon, idMonAn, idTopping1, idTopping2, SoLuong) values (@idHoaDon, @idMonAn, @idTopping1, @idTopping2, @SoLuong)
	end
end
go

create proc USP_GetListBillByDate	-- Dùng cho bảng doanh thu theo ngày
@NgayVao date, @NgayRa date
as
begin
	select hd.ID, b.TenBan, convert(nvarchar(20), ThoiGianVao, 0) as ThoiGianVao, convert(nvarchar(20), ThoiGianRa, 0) as ThoiGianRa, hd.TongTien
	from HoaDon as hd, Ban as b
	where (
		hd.idBan = b.ID and hd.TrangThai = 1 and
		cast(ThoiGianVao as date) >= @NgayVao and cast(ThoiGianRa as date) <= @NgayRa
	)
end
go

create proc USP_UpdateAccInfo
@TenHienThi nvarchar(50), @TenDangNhap nvarchar(50), @MatKhau nvarchar(50), @MatKhauMoi nvarchar(50)
as
begin
	declare @isRightPass int = 0
	select @isRightPass = count(*) from TaiKhoan where @TenDangNhap = TenDangNhap and @MatKhau = MatKhau

	if (@isRightPass > 0)
	begin
		if (@MatKhauMoi = null or @MatKhauMoi = '')
			update TaiKhoan set TenHienThi = @TenHienThi where TenDangNhap = @TenDangNhap
		else
			update TaiKhoan set TenHienThi = @TenHienThi, MatKhau = @MatKhauMoi where TenDangNhap = @TenDangNhap
	end
end
go

-- Tạo Trigger
create trigger UTG_UpdateBillInfo	-- Thêm món
on ChiTietHoaDon for insert, update
as
begin
	declare @idBill int
	declare @idTable int

	select @idBill = idHoaDon from inserted
	select @idTable = idBan from HoaDon where ID = @idBill and TrangThai = 0

	update Ban set TrangThai = N'Có khách' where ID = @idTable
end
go

create trigger UTG_UpdateBill	-- Thanh toán
on HoaDon for update
as
begin
	declare @idBill int
	declare @idTable int
	declare @soDongHoaDon int = 0

	select @idBill = ID from inserted
	select @idTable = idBan from HoaDon where ID = @idBill
	select @soDongHoaDon = count(*) from HoaDon where idBan = @idTable and TrangThai = 0

	if (@soDongHoaDon = 0)
		update Ban set TrangThai = N'Trống' where ID = @idTable
end
go

--
--	Chú ý sửa
--
--drop trigger UTG_DeleteBillInfo
--create trigger UTG_DeleteBillInfo
--on ChiTietHoaDon for delete
--as
--begin
--		declare @idChiTietHoaDon int
--		declare @idHoaDon int
--		select @idChiTietHoaDon = id, @idHoaDon = idHoaDon from deleted

--		declare @idBan int
--		select @idBan = idBan from HoaDon where ID = @idHoaDon

--		declare @soLuong int = 0
--		select @soLuong = count(*) from ChiTietHoaDon as cthd, HoaDon as hd where hd.ID = cthd.idHoaDon and hd.ID = @idHoaDon and hd.TrangThai = 0

--		if (@soLuong = 0)
--			update Ban set TrangThai = N'Trống' where ID = @idBan
--			update HoaDon set TrangThai = 1 where ID = @idHoaDon
--end
--go

-- Tạo Views
create view View_AdminFood
as
select ma.ID, TenMon, TenDM, GiaMonAn from MonAn as ma, DanhMuc as dm 
where dm.ID = ma.idDM
go

-- Tạo Function
create function fuConvertToUnsign_STR
( @strInput nvarchar(4000) )
returns nvarchar(4000)
as
begin
	if (@strInput is null or @strInput = '')
		return @strInput
	declare @RT nvarchar(4000)
	declare @sign_chars nvarchar(136)
	declare @unsign_chars nvarchar(136)

	set @sign_chars = N'ăâđêôơưàảãạáằẳẵặắầẩẫậấèẻẽẹéềểễệếìỉĩịíòỏõọóồổỗộốờởỡợớùủũụúừửữựứỳỷỹỵýĂÂĐÊÔÔƯÀẢÃẠÁẰẲẴẶẮẦẨẪẬẤÈẺẼẸÉỀỂỄỆẾ ÌỈĨỊÍÒỎÕỌÓỒỔỖỘỐỜỞỠỢỚÙỦŨỤÚỪỬỮỰỨỲỶỸỴÝ'
	set @unsign_chars = N'aadeoouaaaaaaaaaaaaaaaeeeeeeeeeeiiiiiooooooooooooooouuuuuuuuuuyyyyyAADEOOUAAAAAAAAAAAAAAAEEEEEEEEEEIIIIIOOOOOOOOOOOOOOOUUUUUUUUUUYYYYY'
	declare @counter int
	declare @counter1 int
	set @counter = 1
	while (@counter <= len(@strInput))
	begin
		set @counter1 = 1
		while (@counter1 <= len(@unsign_chars) + 1)
		begin
			if (unicode(substring(@sign_chars, @counter1, 1)) = unicode(substring(@strInput, @counter, 1)))
			begin
				if (@counter = 1)
					set @strInput = substring(@sign_chars, @counter1, 1) + substring(@strInput, @counter + 1, len(@strInput) - 1)
				else
					set @strInput = substring(@strInput, 1, @counter - 1) + substring(@unsign_chars, @counter1, 1) + substring(@strInput, @counter + 1, len(@strInput) - @counter)
				break
			end
			set @counter1 = @counter1 + 1
		end
		set @counter = @counter + 1
	end
	set @strInput=  replace(@strInput, ' ', '-')
	return @strInput
end
go




-- Tự thêm
--create table ChietKhau (
--	ID int identity primary key,
--	TenChietKhau nvarchar(100) not null,
--	MucChietKhau float not null default 0,
--	ApDungCho nvarchar(100) not null
--)
--create table NhanVien (
--	ID int identity primary key,
--	HoTen nvarchar(100) not null,
--	Email nvarchar(100),
--	SDT nvarchar(20),
--	TenTaiKhoan nvarchar(50),
--)
--create table DatBan (
--	ID int identity primary key,
--	HoTen nvarchar(100) not null,
--	SDT nvarchar(20) not null,
--	idBan int not null,
--)
