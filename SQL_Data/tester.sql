select * from ChiTietHoaDon
select * from HoaDon
select * from DanhMuc
select * from MonAn
select * from Topping
select * from MonAn_Topping
select * from Ban

select MAX(ID) from MonAn
select cthd.idTopping1, cthd.idTopping2 from Topping as tp, HoaDon as hd, ChiTietHoaDon as cthd where hd.TrangThai = 0 and hd.ID = cthd.idHoaDon and tp.ID = cthd.idTopping1

---- Hiển thị cả những hóa đơn đã thanh toán
--select hd.ID, ma.TenMon as [Tên Món], cthd.idTopping1 as [Topping 1], cthd.idTopping2 as [Topping 2], ma.GiaMonAn as [Đơn Giá], cthd.SoLuong as [Số Lượng], ma.GiaMonAn*cthd.SoLuong as [Thành Tiền]
--select ma.ID as [idMonAn], cthd.idTopping1, cthd.idTopping2, cthd.SoLuong
--from ChiTietHoaDon as cthd, HoaDon as hd, MonAn as ma, Topping as tp
--where cthd.idHoaDon = hd.ID and cthd.idMonAn = ma.ID and hd.idBan = 11 and hd.TrangThai = 0
--and cthd.idTopping1 = tp.ID
--and cthd.idTopping2 = tp.ID

select idMonAn, idTopping1, idTopping2 from HoaDon, ChiTietHoaDon where HoaDon.TrangThai = 0 and HoaDon.ID = idHoaDon

--select ID, TenTopping, tp
--from (select hd.ID, TenTopping
--from HoaDon as hd, ChiTietHoaDon as cthd, MonAn as ma, Topping as tp
--where cthd.idHoaDon = hd.ID and hd.idBan = 5 and hd.TrangThai = 0 and cthd.idMonAn = ma.ID and cthd.idTopping1 = tp.ID) as f
--inner join
--(select hd.ID as idHD, TenTopping as tp
--from HoaDon as hd, ChiTietHoaDon as cthd, MonAn as ma, Topping as tp
--where cthd.idHoaDon = hd.ID and hd.idBan = 5 and hd.TrangThai = 0 and cthd.idMonAn = ma.ID and cthd.idTopping2 = tp.ID) as s
--on f.ID = s.idHD

--select idMonAn, TenTopping
--from MonAn_Topping
--join Topping
--on Topping.ID = idTopping

--delete from HoaDon
--where ID = 7 or ID = 6 or ID = 7 or ID = 8
--go
--Xóa món nên sửa id lại
--DBCC CHECKIDENT ('[HoaDon]', RESEED, 6);
--GO

