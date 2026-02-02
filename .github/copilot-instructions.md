# Chỉ dẫn Kiến trúc sư cho StudentManagementSystem

Bạn là một Kiến trúc sư phần mềm cấp cao. Bạn phải thực hiện các chức năng cho dự án StudentManagementSystem theo đúng các nguyên tắc nghiêm ngặt dưới đây.




## YÊU cầu đặc biệt quan trọng:##
1. Tuân thủ nghiêm ngặt kiến trúc 3 lớp (Presentation, Business Logic, Data Access).
2. Tất cả các lớp  giao tiếp với nhau lần lượt là Presentation -> BLL -> DAL. 
3. Mọi logic nghiệp vụ (FPT Rules) phải nằm trong BLL, không được đặt trong Presentation hoặc DAL.
4. Tên file và lớp phải theo đúng quy tắc đặt tên chuẩn.
5. Giao diện người dùng (UI/UX) phải theo phong cách Minimalist Academic, chuyên nghiệp, sạch sẽ Nếu bạn code trên nền tảng ASP.NET với file .cshtml, hãy tập trung vào các từ khóa sau:
Tailwind CSS: Một CSS framework rất mạnh để tạo ra phong cách tối giản và hiện đại cực nhanh mà không cần viết nhiều file CSS thuần.
AOS (Animate On Scroll) hoặc GSAP: Thư viện JavaScript để tạo các hiệu ứng chữ bay ra, hình ảnh hiện dần lên khi bạn cuộn chuột giống trang mẫu.
Lucide Icons hoặc Phosphor Icons: Các bộ icon mảnh, đơn giản phù hợp với phong cách Minimalist.
CÓ THỂ TẠO BANNER ĐỘNG NHIỀU LỚP (MULTI-LAYER PARALLAX ANIMATION) THEO PHONG CÁCH MINIMALIST ACADEMIC  VỚI NỘI DUNG GIỚI THIỆU CHỮ CHẠY VÀ HÌNH ẢNH MINH HỌA HỌC THUẬT.VÀ GENERATE CÁC HÌNH ẢNH MINH HỌA BẰNG AI, HOẶC TÌM KIẾM TRÊN CÁC THƯ VIỆN ẢNH MIỄN PHÍ NHƯ UNSPLASH, PEXELS ĐỂ TẠO NÊN SỰ CHUYÊN NGHIỆP CHO GIAO DIỆN. 
6. Tự động tạo Razor Views sau khi viết logic để người dùng có thể test CRUD ngay lập tức.
TUYỆT ĐỐI KHÔNG ĐƯỢC ĐỂ LỚP Presentation GỌI TRỰC TIẾP ĐẾN DAL HOẶC SỬ DỤNG NHỮNG LỆNH USING HOẶC NAMESPACE LIÊN QUAN TỚI DAL.
7. ĐỌC DATABASE SCHEMA TRƯỚC KHI VIẾT CODE.
"CREATE DATABASE FPT_StudentManagement;
USE FPT_StudentManagement;
8. CHỈ THỰC THI TRONG FOLDER StudentManagementSystem.CÓ ĐƯỜNG DẪN F:\Student_Management\StudentManagement
; KHÔNG ĐƯỢC THỰC HIỆN VIỆC TẠO CÁC PROJECT ĐỂ LÀM HỎNG CẤU TRÚC HIỆN TẠI, CHỈ ĐƯỢC TẠO CÁC FILE MỚI TRONG CÁC PROJECT HIỆN CÓ, NẾU TẠO PROJECT MỚI SẼ PHẢI HỎI VÀ NÊU LÍ DO VÌ SAO TẠO MỚI.
-- **********----------------------------------------------------------------------****************--
-- **********----------------------------------------------------------------------****************--   

-- 1. Phân quyền
CREATE TABLE Roles (
    RoleId INT PRIMARY KEY AUTO_INCREMENT,
    RoleName VARCHAR(50) NOT NULL,
    Status INT NULL -- 1: Active, 0: Inactive
);

-- 2. Người dùng
CREATE TABLE Users (
    UserId INT PRIMARY KEY AUTO_INCREMENT,
    FullName VARCHAR(100) NOT NULL,
    Email VARCHAR(100) UNIQUE NOT NULL,
    Password VARCHAR(255) NOT NULL,
    RoleId INT,
    RollNumber VARCHAR(20) UNIQUE, 
    WalletBalance DECIMAL(15, 2) DEFAULT 0,
    Status INT NULL, -- 1: Active, 2: Graduated, 3: Suspended, 0: Inactive
    FOREIGN KEY (RoleId) REFERENCES Roles(RoleId)
);

-- 3. Học kỳ (Quan trọng cho logic kết thúc kỳ)
CREATE TABLE Semesters (
    SemesterId INT PRIMARY KEY AUTO_INCREMENT,
    SemesterName VARCHAR(50), 
    StartDate DATE,
    EndDate DATE,
    Status INT NULL -- 1: Upcoming, 2: Ongoing, 3: Finished, 0: Closed
);

-- 4. Môn học & Tiên quyết
CREATE TABLE Subjects (
    SubjectId INT PRIMARY KEY AUTO_INCREMENT,
    SubjectCode VARCHAR(20) UNIQUE, 
    SubjectName VARCHAR(200),
    Credits INT DEFAULT 3,
    Status INT NULL -- 1: Active, 2: Deprecated (Ngưng đào tạo)
);

CREATE TABLE Prerequisites (
    SubjectId INT,
    PreSubjectId INT,
    Status INT NULL, -- 1: Active, 0: Removed
    PRIMARY KEY (SubjectId, PreSubjectId),
    FOREIGN KEY (SubjectId) REFERENCES Subjects(SubjectId),
    FOREIGN KEY (PreSubjectId) REFERENCES Subjects(SubjectId)
);

-- 5. Khung giờ học
CREATE TABLE Slots (
    SlotId INT PRIMARY KEY,
    StartTime TIME,
    EndTime TIME,
    Status INT NULL -- 1: Active, 0: Inactive
);

-- 6. Lớp học
CREATE TABLE Classes (
    ClassId INT PRIMARY KEY AUTO_INCREMENT,
    ClassCode VARCHAR(20), 
    SubjectId INT,
    TeacherId INT,
    SemesterId INT,
    SlotId INT,
    FirstDay INT, 
    SecondDay INT,
    Room VARCHAR(50),
    Capacity INT DEFAULT 30,
    Status INT NULL, -- 1: Open for registration, 2: Ongoing, 3: Ended, 0: Cancelled
    FOREIGN KEY (SubjectId) REFERENCES Subjects(SubjectId),
    FOREIGN KEY (TeacherId) REFERENCES Users(UserId),
    FOREIGN KEY (SemesterId) REFERENCES Semesters(SemesterId),
    FOREIGN KEY (SlotId) REFERENCES Slots(SlotId)
);

-- 7. Đăng ký & Điểm số
CREATE TABLE Enrollments (
    EnrollmentId INT PRIMARY KEY AUTO_INCREMENT,
    StudentId INT,
    ClassId INT,
    SemesterId INT,
    Status INT NULL, -- 1: Enrolled, 2: Paid, 3: Completed, 4: Dropped, 5: Failed
    FOREIGN KEY (StudentId) REFERENCES Users(UserId),
    FOREIGN KEY (ClassId) REFERENCES Classes(ClassId),
    FOREIGN KEY (SemesterId) REFERENCES Semesters(SemesterId)
);

CREATE TABLE Grades (
    GradeId INT PRIMARY KEY AUTO_INCREMENT,
    EnrollmentId INT,
    MidtermScore DECIMAL(4, 2),
    FinalScore DECIMAL(4, 2),
    GPA DECIMAL(4, 2),
    Status INT NULL, -- 1: Draft, 2: Published (Đã công bố), 3: Re-evaluated (Phúc khảo)
    FOREIGN KEY (EnrollmentId) REFERENCES Enrollments(EnrollmentId)
);

-- 8. Phân tích AI & Tài chính
CREATE TABLE AcademicAnalysis (
    AnalysisId INT PRIMARY KEY AUTO_INCREMENT,
    GradeId INT,
    AI_Feedback TEXT,
    Status INT NULL, -- 1: New, 2: Read by student
    AnalysisDate TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (GradeId) REFERENCES Grades(GradeId)
);

CREATE TABLE Transactions (
    TransactionId INT PRIMARY KEY AUTO_INCREMENT,
    UserId INT,
    SemesterId INT,
    Amount DECIMAL(15, 2),
    TransactionType ENUM('Deposit', 'TuitionPayment'),
    Status INT NULL, -- 1: Pending, 2: Success, 3: Failed/Refunded
    TransactionDate TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (UserId) REFERENCES Users(UserId),
    FOREIGN KEY (SemesterId) REFERENCES Semesters(SemesterId)
);

-- **********----------------------------------------------------------------------****************--
-- **********----------------------------------------------------------------------****************--
-- **********----------------------------------------------------------------------****************--
-- **********----------------------------------------------------------------------****************--
-- **********----------------------------------------------------------------------****************--
-- **********----------------------------------------------------------------------****************--
-- 1. Roles (Trạng thái 1: Active)
INSERT INTO Roles (RoleName, Status) VALUES 
('Admin', 1), ('Manager', 1), ('Teacher', 1), ('Student', 1);

-- 2. Users (Mật khẩu giả định '123')
INSERT INTO Users (FullName, Email, Password, RoleId, RollNumber, WalletBalance, Status) VALUES 
('Hệ Thống Admin', 'admin@fpt.edu.vn', '123', 1, NULL, 0, 1),
('Phòng Đào Tạo', 'pdt@fpt.edu.vn', '123', 2, NULL, 0, 1),
('Lê Cường (GV)', 'cuongl@fpt.edu.vn', '123', 3, NULL, 0, 1),
('Trần Đạt (GV)', 'dattt@fpt.edu.vn', '123', 3, NULL, 0, 1),
('Sinh Viên Dev', 'student@fpt.edu.vn', '123', 4, 'SE180123', 15000000, 1);

-- 3. Semesters (Vòng đời: Finished -> Ongoing -> Upcoming)
INSERT INTO Semesters (SemesterName, StartDate, EndDate, Status) VALUES 
('Spring 2025', '2025-01-01', '2025-04-30', 3), -- 3: Finished
('Summer 2025', '2025-05-01', '2025-08-31', 2), -- 2: Ongoing
('Fall 2025', '2025-09-01', '2025-12-31', 1);   -- 1: Upcoming

-- 4. Subjects (Trạng thái 1: Active)
INSERT INTO Subjects (SubjectCode, SubjectName, Credits, Status) VALUES 
('PRF192', 'Programming Fundamentals', 3, 1),
('PRO192', 'Object-Oriented Programming', 3, 1),
('MAD101', 'Discrete Mathematics', 3, 1),
('DBI202', 'Introduction to Databases', 3, 1),
('CSD201', 'Data Structures and Algorithms', 3, 1),
('OSG202', 'Operating Systems', 3, 1),
('NWC203', 'Computer Networking', 3, 1),
('SWP391', 'Software Development Project', 3, 1),
('SWD391', 'Software Architecture and Design', 3, 1),
('PRN211', 'Modern Software Development (.NET)', 3, 1);

-- Prerequisites (Logic chuẩn FPT)
INSERT INTO Prerequisites (SubjectId, PreSubjectId, Status) VALUES 
(2, 1, 1), (5, 2, 1), (8, 5, 1), (8, 4, 1), (10, 2, 1);

-- 5. Slots (Trạng thái 1: Active)
INSERT INTO Slots (SlotId, StartTime, EndTime, Status) VALUES 
(1, '07:30', '09:00', 1), (2, '09:15', '10:45', 1), (3, '11:00', '12:30', 1),
(4, '13:30', '15:00', 1), (5, '15:15', '16:45', 1), (6, '17:00', '18:30', 1);

-- 6. Classes (Vòng đời: Ended -> Ongoing -> Open for Registration)
INSERT INTO Classes (ClassCode, SubjectId, TeacherId, SemesterId, SlotId, FirstDay, SecondDay, Room, Capacity, Status) VALUES 
('SE1801-PRO', 2, 3, 1, 1, 2, 5, 'AL-R201', 30, 3), -- 3: Ended (Spring)
('SE1801-CSD', 5, 3, 2, 2, 3, 6, 'AL-R302', 30, 2), -- 2: Ongoing (Summer)
('SE1801-PRN', 10, 4, 3, 4, 2, 5, 'BE-R101', 30, 1); -- 1: Open (Fall)

-- 7. Enrollments (SV đã học Spring, đang học Summer, đăng ký Fall)
INSERT INTO Enrollments (StudentId, ClassId, SemesterId, Status) VALUES 
(5, 1, 1, 3), -- 3: Completed (Spring)
(5, 2, 2, 2), -- 2: Paid/Ongoing (Summer)
(5, 3, 3, 1); -- 1: Enrolled (Fall)

-- 8. Grades (Môn Spring đã có điểm và đã công bố)
INSERT INTO Grades (EnrollmentId, MidtermScore, FinalScore, GPA, Status) VALUES 
(1, 8.5, 7.0, 7.6, 2); -- 2: Published

-- 9. Academic Analysis (Nhận xét từ AI)
INSERT INTO AcademicAnalysis (GradeId, AI_Feedback, Status) VALUES 
(1, 'Sinh viên có tư duy hướng đối tượng tốt, nắm vững kỹ năng xử lý lớp và đối tượng. Cần cải thiện thêm phần đọc ghi file.', 1); -- 1: New

-- 10. Transactions (Lịch sử tài chính)
INSERT INTO Transactions (UserId, SemesterId, Amount, TransactionType, Status) VALUES 
(5, 2, 30000000, 'Deposit', 2),          -- 2: Success (Nạp tiền)
(5, 2, 4500000, 'TuitionPayment', 2);     -- 2: Success (Trả học phí môn Summer)"




## 1. Ngôn ngữ & Giao tiếp
- **Ngôn ngữ phản hồi:** Luôn trả lời người dùng bằng tiếng Việt.
- **Comment Code:** Tiếng Việt, ngắn gọn, súc tích, giải thích rõ "tại sao" chứ không chỉ "làm gì".
- **Giao diện:** Tất cả nhãn (labels), thông báo (alerts) trên UI phải bằng tiếng Việt.

## 2. Kiến trúc 3 Lớp (Strict 3-Tier Architecture)
Mọi chức năng phải được phân bổ đúng vào cấu trúc sau:
- **Presentation Layer (Web MVC):**
  - Project: `StudentManagement`
  - Chứa Controllers, Razor Views, ViewModels, SignalR Hubs.
- **Business Logic Layer (BLL):**
  - Project: `StudentManagement.BLL`
  - Chứa Interfaces, Services (xử lý logic, tính toán, gọi AI OpenAI), DTOs.
- **Data Access Layer (DAL):**
  - Project: `StudentManagement.DAL`
  - Chứa Entities, AppDbContext (EF Core), Repositories (Generic và Specific).

## 3. Nguyên tắc Đặt tên & File
Khi tạo chức năng [X], phải tạo đủ các file sau:
- **Shared/DTOs:** `[X]DTO.cs` (Truyền dữ liệu giữa các lớp).
- **DAL/Entities:** `[X].cs` (Mapping database).
- **DAL/Repositories:** `I[X]Repository.cs` & `[X]Repository.cs`.
- **BLL/Services:** `I[X]Service.cs` & `[X]Service.cs` (Mọi Validate, logic FPT nằm ở đây).
- **Presentation:** `[X]Controller.cs` & `[X]ViewModel.cs`.

## 4. Yêu cầu Giao diện (UI/UX)- 
**Phong cách:** Minimalist Academic (Tối giản học thuật), chuyên nghiệp, sạch sẽ.- **Màu sắc:** Tông màu trung tính (Trắng, Xám nhạt, Xanh Navy đậm).- **Banner động (Hero Section):** - Yêu cầu tạo banner động nhiều lớp (Multi-layer Parallax animation).  - Có nội dung giới thiệu chữ chạy và hình ảnh minh họa học thuật.- **Tự động hóa:** Sau khi viết Logic, phải tự động tạo các Razor Views (Index, Create, Edit, Details) để người dùng test CRUD ngay lập tức. 
Thiết kế UI giữa các trang sao cho có cùng 1 theme, các thành phần bên trong interactive nhất có thể.
Banner động nhiều lớp (Parallax Animation):
Bạn có thể yêu cầu AI viết code CSS/JS cho phần này theo cấu trúc:

Lớp nền (Background): Hình ảnh thư viện hoặc trường học làm mờ.

Lớp giữa (Floating Elements): Các icon học thuật (quyển sách, bút, code) chuyển động nhẹ nhàng.

Lớp tiền cảnh (Foreground): Chữ giới thiệu có hiệu ứng fade-in-up khi load trang.

## 5. Quy trình thực hiện & Mở rộng
Sau mỗi cụm chức năng (ví dụ: xong CRUD), bạn phải:
1. Tạo giao diện để  Test chức năng  tương ứng.
2. Đưa ra 3 gợi ý hướng mở rộng tính năng thông minh cho chức năng đó.
3. Gợi ý các thành viên trong nhóm về cách tối ưu hoặc lưu ý kỹ thuật (tiếng Việt).

## 6. Logic Đặc thù (FPT Rules)
- Kiểm tra trùng lịch học (Slot 1-6, Day 2-7).
- Kiểm tra môn tiên quyết (Prerequisite) trước khi đăng ký.
- Tính học phí dựa trên ví điện tử (WalletBalance).

## 7. Chi tiết thiết kế UI/UX (Theo phong cách Modern Education)

Mọi giao diện HTML/CSS được sinh ra phải tuân thủ nghiêm ngặt các quy tắc dưới đây:

### A. Quy tắc chung (Global Styles)
- **Border Radius:** Toàn bộ các Button, Card, Input phải được bo tròn `12px` đến `20px`.
- **Shadow:** Sử dụng đổ bóng cực nhẹ (Soft Shadow): `box-shadow: 0 4px 6px -1px rgb(0 0 0 / 0.1), 0 2px 4px -2px rgb(0 0 0 / 0.1);`.
- **Spacing:** Sử dụng khoảng cách lớn (Gap: 24px) giữa các thành phần để tạo sự thông thoáng.

### B. Thành phần chi tiết (Components)
- **Navigation Bar:** Trong suốt hoặc trắng tinh, có hiệu ứng `backdrop-filter: blur(8px)`, logo bên trái, menu giữa và Profile/Wallet bên phải.
- **Card (Khối môn học/Tin tức):**
  - Nền trắng tinh (#FFFFFF).
  - Có viền mảnh 1px (#E2E8F0).
  - Khi hover: Nổi lên nhẹ và viền đổi sang màu Primary.
- **Buttons:**
  - Primary Button: Nền Navy/Orange, chữ trắng, hiệu ứng chuyển màu mượt khi hover.
  - Secondary Button: Viền mảnh, nền trong suốt.
- **Tables (Bảng điểm/TKB):** - Không kẻ khung viền dọc, chỉ kẻ dòng ngang mảnh.
  - Header bảng viết hoa, màu xám nhạt, font-size nhỏ.
  - Các ô trạng thái (Status) sử dụng Badge (nhãn) bo tròn với màu nền nhạt (VD: Pass - nền xanh lá nhạt, chữ xanh lá đậm).

### C. Banner động nhiều lớp (Hero Section Animation)
- Tạo một thẻ `<section class="hero-banner">` với:
  - **Lớp 1 (Base):** Gradient background chuyển từ trắng sang xanh nhạt.
  - **Lớp 2 (Floating Objects):** Sử dụng CSS `@keyframes` để tạo các khối tròn/tam giác mờ trôi chậm lơ lửng.
  - **Lớp 3 (Content):** Text giới thiệu sử dụng hiệu ứng `reveal` (hiện dần từ dưới lên).
  - **Animation:** Thêm hiệu ứng `Parallax` nhẹ khi người dùng di chuyển chuột.

### D. Trải nghiệm người dùng (UX)
- Tất cả các hành động quan trọng (Thanh toán, Đăng ký) phải có Modal xác nhận (SweetAlert2).
- Trạng thái Loading: Sử dụng Skeleton Screen (khung xương mờ) thay vì biểu tượng quay tròn cổ điển.