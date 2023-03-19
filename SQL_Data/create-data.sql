use TheCoffeeHouse

-- Insert dữ liệu cho table PhanQuyen
insert into PhanQuyen values (1, N'Quản trị viên')
insert into PhanQuyen values (0, N'Nhân viên')
go

-- Insert dữ liệu cho table TaiKhoan
insert into TaiKhoan values ('admin', N'Chi Khoa', '1234', 1)
insert into TaiKhoan values ('thu123', N'Thu Thủy', 'qwer', 0)
insert into TaiKhoan values ('tiendz', N'Minh Tiến', 'zxcv', 0)
go

-- Insert dữ liệu cho table Ban
insert dbo.Ban (TenBan) values (N'Bàn Đã Xóa')
insert dbo.Ban (TenBan) values (N'Bàn 1')
insert dbo.Ban (TenBan) values (N'Bàn 2')
insert dbo.Ban (TenBan) values (N'Bàn 3')
insert dbo.Ban (TenBan) values (N'Bàn 4')
insert dbo.Ban (TenBan) values (N'Bàn 5')
insert dbo.Ban (TenBan) values (N'Bàn 6')
insert dbo.Ban (TenBan) values (N'Bàn 7')
insert dbo.Ban (TenBan) values (N'Bàn 8')
insert dbo.Ban (TenBan) values (N'Bàn 9')
insert dbo.Ban (TenBan) values (N'Bàn 10')
go

-- Insert dữ liệu cho table DanhMuc
insert DanhMuc (TenDM) values (N'Danh Mục Đã Xóa')
insert DanhMuc (TenDM) values (N'Cà Phê')
insert DanhMuc (TenDM) values (N'Trà')
insert DanhMuc (TenDM) values (N'Hi-Tea Healthy')
insert DanhMuc (TenDM) values (N'Bánh và Snack')
insert DanhMuc (TenDM) values (N'CloudFee')
insert DanhMuc (TenDM) values (N'CloudTee')
go

-- Insert dữ liệu cho table MonAn
insert MonAn (TenMon, idDM, GiaMonAn) values (N'Món Đã Xóa', 1, 0)

insert MonAn (TenMon, idDM, GiaMonAn) values (N'Cà Phê Sữa Đá', 2, 29000)
insert MonAn (TenMon, idDM, GiaMonAn) values (N'Cà Phê Sữa Nóng', 2, 39000)
insert MonAn (TenMon, idDM, GiaMonAn) values (N'Bạc Sỉu', 2, 29000)
insert MonAn (TenMon, idDM, GiaMonAn) values (N'Bạc Sỉu Nóng', 2, 39000)
insert MonAn (TenMon, idDM, GiaMonAn) values (N'Cà Phê Đen Đá', 2, 29000)
insert MonAn (TenMon, idDM, GiaMonAn) values (N'Cà Phê Đen Nóng', 2, 39000)
insert MonAn (TenMon, idDM, GiaMonAn) values (N'Caramel Macchiato Đá', 2, 55000)
insert MonAn (TenMon, idDM, GiaMonAn) values (N'Americano Đá', 2, 45000)
insert MonAn (TenMon, idDM, GiaMonAn) values (N'Cold Brew Phúc Bồn Tử', 2, 49000)

insert MonAn (TenMon, idDM, GiaMonAn) values (N'Trà Lài Thơm', 3, 69000)
insert MonAn (TenMon, idDM, GiaMonAn) values (N'Trà Hạt Sen - Đá', 3, 49000)
insert MonAn (TenMon, idDM, GiaMonAn) values (N'Trà Sữa Oolong Nướng Trân Châu', 3, 55000)
insert MonAn (TenMon, idDM, GiaMonAn) values (N'Hồng Trà Sữa Nóng', 3, 55000)

insert MonAn (TenMon, idDM, GiaMonAn) values (N'Hi-Tea Đào', 4, 49000)
insert MonAn (TenMon, idDM, GiaMonAn) values (N'Hi-Tea Thơm', 4, 72000)
insert MonAn (TenMon, idDM, GiaMonAn) values (N'Hi-Tea Đá Tuyết Yuzu Vải', 4, 69000)

insert MonAn (TenMon, idDM, GiaMonAn) values (N'Bánh Mì Que Pate', 5, 15000)
insert MonAn (TenMon, idDM, GiaMonAn) values (N'Mochi Kem Việt Quất', 5, 19000)
insert MonAn (TenMon, idDM, GiaMonAn) values (N'Mít Sấy', 5, 20000)
go

-- Insert dữ liệu cho table Topping
insert Topping (TenTopping, GiaTopping) values (N'Topping Đã Xóa', 0)
insert Topping (TenTopping, GiaTopping) values (N'Không Chọn', 0)
insert Topping (TenTopping, GiaTopping) values (N'Trân Châu Trắng', 10000)
insert Topping (TenTopping, GiaTopping) values (N'Đào Miếng', 10000)
insert Topping (TenTopping, GiaTopping) values (N'Trái Vải', 10000)
insert Topping (TenTopping, GiaTopping) values (N'Hạt Sen', 10000)
insert Topping (TenTopping, GiaTopping) values (N'Trái Nhãn', 10000)
insert Topping (TenTopping, GiaTopping) values (N'Nha Đam', 10000)
go

--Insert dữ liệu cho table MonAn_Topping
declare @number int = 2
while @number <= 20
begin
	insert MonAn_Topping (idMonAn, idTopping) values (@number, 2)
	set @number = @number + 1
end
go
insert MonAn_Topping (idMonAn, idTopping) values (2, 3)
insert MonAn_Topping (idMonAn, idTopping) values (11, 3)
insert MonAn_Topping (idMonAn, idTopping) values (11, 4)
insert MonAn_Topping (idMonAn, idTopping) values (11, 5)
insert MonAn_Topping (idMonAn, idTopping) values (11, 6)
go

-- Insert dữ liệu cho table HoaDon
declare @date1_1 datetime, @time1_1 time; set @date1_1 = '2023-01-01'; set @time1_1 = '08:12:34'; set @date1_1 = @date1_1 + cast(@time1_1 as datetime)
declare @date1_2 datetime, @time1_2 time; set @date1_2 = '2023-01-01'; set @time1_2 = '11:24:57'; set @date1_2 = @date1_2 + cast(@time1_2 as datetime)
declare @date2_1 datetime, @time2_1 time; set @date2_1 = '2023-02-04'; set @time2_1 = '13:43:27'; set @date2_1 = @date2_1 + cast(@time2_1 as datetime)
declare @date2_2 datetime, @time2_2 time; set @date2_2 = '2023-02-04'; set @time2_2 = '17:09:02'; set @date2_2 = @date2_2 + cast(@time2_2 as datetime)
declare @date3_1 datetime, @time3_1 time; set @date3_1 = '2023-02-04'; set @time3_1 = '13:43:27'; set @date3_1 = @date3_1 + cast(@time3_1 as datetime)
declare @date3_2 datetime, @time3_2 time; set @date3_2 = '2023-02-04'; set @time3_2 = '17:09:02'; set @date3_2 = @date3_2 + cast(@time3_2 as datetime)
insert HoaDon (ThoiGianVao, ThoiGianRa, idBan, TrangThai, TongTien) values (@date1_1, @date1_2, 2, 1, 138000)
insert HoaDon (ThoiGianVao, ThoiGianRa, idBan, TrangThai, TongTien) values (@date2_1, @date2_2, 3, 1, 111000)
insert HoaDon (ThoiGianVao, ThoiGianRa, idBan, TrangThai, TongTien) values (@date3_1, @date3_2, 8, 1, 87000)
go

-- Insert dữ liệu cho table ChiTietHoaDon
insert ChiTietHoaDon (idHoaDon, idMonAn, SoLuong) values (1, 2, 1)
insert ChiTietHoaDon (idHoaDon, idMonAn, SoLuong) values (1, 4, 1)
insert ChiTietHoaDon (idHoaDon, idMonAn, SoLuong) values (1, 11, 2)

insert ChiTietHoaDon (idHoaDon, idMonAn, SoLuong) values (2, 2, 1)
insert ChiTietHoaDon (idHoaDon, idMonAn, SoLuong) values (2, 7, 1)

insert ChiTietHoaDon (idHoaDon, idMonAn, SoLuong) values (3, 3, 3)
go