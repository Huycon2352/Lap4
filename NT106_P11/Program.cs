using Microsoft.EntityFrameworkCore;
using NT106_P11.Data;
using NT106_P11.Models;

var builder = WebApplication.CreateBuilder(args);

// Cấu hình DbContext với cơ sở dữ liệu In-Memory
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("NT106_P11"));

// Thêm dịch vụ API
builder.Services.AddControllers();

// Thêm Swagger để kiểm thử API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Cấu hình Swagger để test API
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Seed dữ liệu mẫu vào cơ sở dữ liệu khi ứng dụng bắt đầu
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (!dbContext.Members.Any())  // Kiểm tra nếu không có dữ liệu thì thêm dữ liệu mẫu
    {
        dbContext.Members.AddRange(MemberData.Members);
        dbContext.SaveChanges();
    }
}

app.MapControllers();

app.Run();
