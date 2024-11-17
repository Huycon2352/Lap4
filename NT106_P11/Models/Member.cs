namespace NT106_P11.Models
{
    public class Member
    {
        public int Id { get; set; } // Khóa chính
        public string Name { get; set; } = string.Empty; // Tên người dùng
        public string Role { get; set; } = string.Empty; // Vai trò: Sinh viên hoặc Giảng viên
    }
}
