# Hệ Thống Thuyền Nổi (Boat Floating System)

## Tổng Quan
Hệ thống này bao gồm 3 script chính để tạo hiệu ứng thuyền nổi trên mặt nước:

1. **BoatFloating.cs** - Điều khiển vật lý nổi
2. **BoatController.cs** - Điều khiển di chuyển thuyền
3. **SimpleWater.cs** - Tạo mặt nước với sóng

## Cách Thiết Lập

### 1. Thiết Lập Thuyền
1. Chọn GameObject thuyền của bạn
2. Thêm component `BoatFloating`
3. Thêm component `BoatController`
4. Đảm bảo thuyền có `Rigidbody` component
5. Thêm `Mesh Collider` hoặc `Box Collider`

### 2. Thiết Lập Mặt Nước
1. Tạo GameObject mới (Empty)
2. Đặt tên là "Water"
3. Thêm component `SimpleWater`
4. Điều chỉnh `Water Level` để phù hợp với thuyền

### 3. Cấu Hình BoatFloating
- **Buoyancy Force**: Lực đẩy nước (15-30)
- **Water Level**: Mức nước (thường là 0)
- **Water Drag**: Lực cản nước (0.5-2.0)
- **Stability Force**: Lực ổn định (20-50)
- **Wave Settings**: Cài đặt sóng

### 4. Cấu Hình BoatController
- **Motor Force**: Lực động cơ (1000-2000)
- **Steer Force**: Lực lái (10-30)
- **Water Resistance**: Lực cản (0.01-0.05)

## Điều Khiển
- **W/S hoặc Mũi tên lên/xuống**: Tiến/lùi
- **A/D hoặc Mũi tên trái/phải**: Quay trái/phải

## Tùy Chỉnh Nâng Cao

### Floating Points
Script sẽ tự động tạo 4 điểm nổi, nhưng bạn có thể tự tạo:
1. Tạo Empty GameObjects làm con của thuyền
2. Đặt chúng ở 4 góc thuyền (dưới mặt nước)
3. Gán vào mảng `Floating Points`

### Sóng Tùy Chỉnh
Trong `SimpleWater.cs`:
- `Wave Height`: Độ cao sóng
- `Wave Speed`: Tốc độ sóng
- `Wave Length`: Chiều dài sóng

## Lưu Ý
- Đảm bảo thuyền có khối lượng phù hợp (500-2000)
- Điều chỉnh `Water Level` để thuyền nổi đúng cách
- Sử dụng `Layer` khác nhau cho thuyền và nước để tránh xung đột
- Script hoạt động tốt nhất với thuyền có kích thước trung bình

## Khắc Phục Sự Cố
- **Thuyền bay lên**: Giảm `Buoyancy Force`
- **Thuyền chìm**: Tăng `Buoyancy Force` hoặc giảm `Mass`
- **Thuyền không ổn định**: Tăng `Stability Force`
- **Thuyền quay quá nhanh**: Giảm `Steer Force`
- **Thuyền di chuyển chậm**: Tăng `Motor Force` 