# PRN232 - SU2026 - Membership Management

Repository này được sử dụng để lưu trữ toàn bộ các bài tập Lab/Assignment trên lớp của môn học **PRN232 (E-Commerce Application Development)** trong kỳ học **Summer 2026**. 

Dự án tập trung vào việc xây dựng hệ thống quản lý thành viên (Membership Management) thông qua nhiều kiến trúc và công nghệ backend khác nhau từ cơ bản đến nâng cao (Microservices, gRPC, GraphQL, Message Queue,...).

---

## 📌 Tổng quan lộ trình Assignments

| Bài làm | Công nghệ & Kiến trúc chủ đạo | Thành phần tích hợp đi kèm |
| :--- | :--- | :--- |
| **ASM 01** | Web API (RESTful) | Báo cáo trực quan với **Power BI** |
| **ASM 02** | **GraphQL** API | Tối ưu truy vấn dữ liệu linh hoạt |
| **ASM 03** | **gRPC** Service | Giao tiếp RPC hiệu năng cao |
| **ASM 04** | **SOAP Service** & **RabbitMQ** | Message Queue xử lý bất đồng bộ & Hệ thống Chat |
| **ASM 05** | **Microservices** | Hệ thống giám sát **Grafana, Loki** & Giả lập Mobile Client |

---

## 🛠️ Chi tiết cách chạy & Test từng Assignment

### 🔹 Assignment 01: RESTful API & Power BI
* **Cách khởi chạy:**
  1. Cấu hình chuỗi kết nối Database trong file `appsettings.json` của dự án Web API.
  2. Mở Terminal tại thư mục ASM 01 và chạy lệnh:
     ```bash
     dotnet run
     ```
* **Cách Test:**
  * Truy cập đường dẫn Swagger UI (thường là `https://localhost:xxxx/swagger`) trên trình duyệt để test trực tiếp các API Endpoint (GET, POST, PUT, DELETE).
  * Mở file `.pbix` bằng **Power BI Desktop**, nhấn nút **Refresh** để cập nhật dữ liệu báo cáo từ database.

### 🔹 Assignment 02: GraphQL Integration
* **Cách khởi chạy:**
  1. Di chuyển vào thư mục dự án GraphQL.
  2. Chạy lệnh:
     ```bash
     dotnet run
     ```
* **Cách Test:**
  * Truy cập vào giao diện UI của GraphQL (ví dụ: Banana Cake Pop hoặc GraphQL Playground) tại đường dẫn mặc định khi ứng dụng start (thường là `https://localhost:xxxx/graphql`).
  * Thực hiện test bằng cách viết các câu truy vấn **Query** (để lấy danh sách/chi tiết thành viên) hoặc **Mutation** (để thêm, sửa, xóa).

### 🔹 Assignment 03: High-Performance với gRPC
* **Cách khởi chạy:**
  1. Bật đồng thời cả **gRPC Server** và **Client** (hoặc Web App gọi gRPC).
  2. Tại thư mục Server: `dotnet run`
  3. Tại thư mục Client: `dotnet run`
* **Cách Test:**
  * Dùng các công cụ test gRPC chuyên dụng như **Postman v10+** hoặc **grpcurl** để gửi request dạng RPC tới `localhost:port` của Server.
  * Hoặc kiểm tra log trên Console của Client để xem dữ liệu phản hồi từ Server qua giao thức gRPC.

### 🔹 Assignment 04: SOAP Service & Message Queue (RabbitMQ Chat)
* **Cách khởi chạy:**
  1. Khởi động **RabbitMQ** thông qua Docker:
     ```bash
     docker run -d --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management
     ```
  2. Khởi chạy dự án SOAP Service và dự án Chat Application.
* **Cách Test:**
  * **SOAP:** Sử dụng phần mềm **SoapUI** hoặc extension **Postman** (chọn kiểu request là HTTP với XML Body) trỏ tới đường dẫn `.asmx` hoặc `WSDL` để gọi các hàm SOAP.
  * **RabbitMQ Chat:** Mở đồng thời 2 hoặc nhiều tab/cửa sổ ứng dụng Chat, gửi tin nhắn qua lại và kiểm tra giao diện quản lý RabbitMQ Management Dashboard (`http://localhost:15672`) để xem các Message Queue hoạt động.

### 🔹 Assignment 05: Microservices & Observability
* **Cách khởi chạy:**
  1. Khởi động toàn bộ hạ tầng giám sát (Grafana, Loki, Prometheus) bằng **Docker Compose**:
     ```bash
     docker-compose up -d
     ```
  2. Chạy lần lượt các dịch vụ Microservices (.NET) của hệ thống.
  3. Mở phần mềm giả lập Mobile (Android Emulator / iOS Simulator) hoặc ứng dụng client tương ứng.
* **Cách Test:**
  * **Hạ tầng:** Truy cập Grafana UI (`http://localhost:3000`), vào mục *Explore*, chọn Data Source là **Loki** để kiểm tra các dòng Log thời gian thực của các microservices đang được gom về.
  * **Client:** Thực hiện các thao tác đăng nhập, quản lý trên thiết bị **giả lập Mobile**, sau đó kiểm tra xem dữ liệu có được đồng bộ qua API Gateway đến các service phía dưới hay không.

---

## 💻 Yêu cầu hệ thống chung
* .NET SDK 8.0 / 9.0
* Docker Desktop (Dành cho ASM 04 & ASM 05)
* SQL Server / MySQL
* Power BI Desktop, SoapUI, Postman

---

## 📝 Cấu trúc thư mục repository
```text
📂 PRN232-SU2026-MembershipManagement
├── 📁 SU26_PRN232_SE1811_ASM01_... (Thư mục bài làm cụ thể)
├── 📁 ... (Các thư mục ASM tiếp theo sẽ được update tại đây)
└── 📄 README.md
