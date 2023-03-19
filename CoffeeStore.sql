create proc GetAccByUserName
@userName nvarchar(50)
as
begin
	select * from dbo.TaiKhoan where TenDangNhap = @userName
end
go

exec dbo.GetAccByUserName @userName = 'admin'

select * from dbo.TaiKhoan where TenDangNhap = N'' and MatKhau = N'' or 1 = 1