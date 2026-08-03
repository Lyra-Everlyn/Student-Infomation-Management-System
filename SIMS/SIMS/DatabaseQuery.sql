-- =========================================
-- 1. DEPARTMENTS
-- =========================================
CREATE TABLE Departments (
    DepartmentId INT IDENTITY(1,1) PRIMARY KEY,
    DepartmentName NVARCHAR(100) NOT NULL UNIQUE,
    DepartmentCode NVARCHAR(20)
);

-- =========================================
-- 2. USERS
-- =========================================
CREATE TABLE Users (
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    Role NVARCHAR(10) NOT NULL DEFAULT 'Applicant',
    FullName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    PhoneNumber NVARCHAR(20),
    Address NVARCHAR(255),
    DateOfBirth DATE,
    Status NVARCHAR(10) NOT NULL DEFAULT 'Active',
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME NOT NULL DEFAULT GETDATE(),

    CHECK (Role IN ('Admin','Faculty','Student', 'Applicant')),
    CHECK (Status IN ('Active','Inactive', 'Locked'))
);

-- =========================================
-- 3. ADMISSION APPLICATIONS
-- =========================================
CREATE TABLE AdmissionApplications (
    ApplicationId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    DepartmentId INT,
    Status NVARCHAR(10) NOT NULL DEFAULT 'Incomplete',
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),

    CHECK (Status IN ('Incomplete','Pending','Approved','Rejected')),
    FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE,
    FOREIGN KEY (DepartmentId) REFERENCES Departments(DepartmentId)
);

-- =========================================
-- 4. CLASSES
-- =========================================
CREATE TABLE Classes (
    ClassId INT IDENTITY(1,1) PRIMARY KEY,
    ClassName NVARCHAR(50) NOT NULL UNIQUE,
    DepartmentId INT NOT NULL,

    FOREIGN KEY (DepartmentId) REFERENCES Departments(DepartmentId)
);

-- =========================================
-- 5. SUBJECTS
-- =========================================
CREATE TABLE Subjects (
    SubjectId INT IDENTITY(1,1) PRIMARY KEY,
    DepartmentId INT NOT NULL,
    SubjectCode VARCHAR(20) NOT NULL UNIQUE,
    SubjectName NVARCHAR(150) NOT NULL,
    Credits INT NOT NULL,

    FOREIGN KEY (DepartmentId) REFERENCES Departments(DepartmentId)
);

-- =========================================
-- 6. FACULTIES
-- =========================================
CREATE TABLE Faculties (
    FacultyId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL UNIQUE,
    DepartmentId INT,

    FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE,
    FOREIGN KEY (DepartmentId) REFERENCES Departments(DepartmentId)
);

-- =========================================
-- 7. STUDENTS
-- =========================================
CREATE TABLE Students (
    StudentId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL UNIQUE,
    StudentCode VARCHAR(20) NOT NULL UNIQUE,
    DepartmentId INT NOT NULL,
    ClassId INT,

    FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE,
    FOREIGN KEY (DepartmentId) REFERENCES Departments(DepartmentId),
    FOREIGN KEY (ClassId) REFERENCES Classes(ClassId)
);

-- =========================================
-- 8. COURSES
-- =========================================
CREATE TABLE Courses (
    CourseId INT IDENTITY(1,1) PRIMARY KEY,
    CourseCode VARCHAR(50) NOT NULL UNIQUE,
    CourseName NVARCHAR(150) NOT NULL,
    SubjectId INT NOT NULL,
    FacultyId INT NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    UpdatedAt DATETIME NOT NULL DEFAULT GETDATE(),

    FOREIGN KEY (SubjectId) REFERENCES Subjects(SubjectId),
    FOREIGN KEY (FacultyId) REFERENCES Faculties(FacultyId)
);

-- =========================================
-- 9. ASSIGNMENTS
-- =========================================
CREATE TABLE Assignments (
    AssignmentId INT IDENTITY(1,1) PRIMARY KEY,
    AssignmentName NVARCHAR(100) NOT NULL,
    CourseId INT NOT NULL,
    Description NVARCHAR(MAX),
    StartTime DATETIME,
    EndTime DATETIME,

    FOREIGN KEY (CourseId) REFERENCES Courses(CourseId)
);

-- =========================================
-- 10. ENROLLMENTS
-- =========================================
CREATE TABLE Enrollments (
    EnrollmentId INT IDENTITY(1,1) PRIMARY KEY,
    CourseId INT NOT NULL,
    StudentId INT NOT NULL,
    EnrollmentDate DATETIME NOT NULL DEFAULT GETDATE(),

    UNIQUE (CourseId, StudentId),
    FOREIGN KEY (CourseId) REFERENCES Courses(CourseId),
    FOREIGN KEY (StudentId) REFERENCES Students(StudentId)
);

-- =========================================
-- 11. GRADES
-- =========================================
CREATE TABLE Grades (
    GradeId INT IDENTITY(1,1) PRIMARY KEY,
    AssignmentId INT NOT NULL,
    StudentId INT NOT NULL,
    SubmitDate DATETIME,
    FileUrl NVARCHAR(255),
    Score DECIMAL(5,2),
    Feedback NVARCHAR(MAX),
    GradedAt DATETIME,

    CHECK (Score BETWEEN 0 AND 100),
    UNIQUE (AssignmentId, StudentId),
    FOREIGN KEY (AssignmentId) REFERENCES Assignments(AssignmentId),
    FOREIGN KEY (StudentId)REFERENCES Students(StudentId)
);



INSERT INTO Users (Role, FullName, Email, PasswordHash, PhoneNumber, Address, DateOfBirth, Status, CreatedAt, UpdatedAt)
VALUES 
(
    'Admin', 
    'System Administrator', 
    'nguyenducminh.1204@gmail.com', 
    'AQAAAAIAAYagAAAAEE66EyvSxsHJfecr/VIEL44FqRqa00dbiPpIpVm89m2gLnurlzJsjQWvKlPG/AGYNg==', 
    '0123456789', 
    'SIMS Headquarter', 
    '2006-11-06', 
    'Active', 
    GETDATE(), 
    GETDATE()
);

DECLARE @TableName NVARCHAR(256);
DECLARE @Sql NVARCHAR(MAX);

-- Khai báo cursor để lấy danh sách tất cả các bảng người dùng (User Tables)
DECLARE TableCursor CURSOR FOR
SELECT QUOTENAME(TABLE_SCHEMA) + '.' + QUOTENAME(TABLE_NAME)
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_TYPE = 'BASE TABLE';

OPEN TableCursor;
FETCH NEXT FROM TableCursor INTO @TableName;

WHILE @@FETCH_STATUS = 0
BEGIN
    -- Tạo câu lệnh SELECT cho từng bảng
    SET @Sql = 'SELECT ''' + @TableName + ''' AS [Table_Name], * FROM ' + @TableName;
    EXEC sp_executesql @Sql;

    FETCH NEXT FROM TableCursor INTO @TableName;
END;

CLOSE TableCursor;
DEALLOCATE TableCursor;




-- 1. Xóa các bảng cấp 4 và 3 (Chứa nhiều khóa ngoại nhất)
DROP TABLE IF EXISTS Grades;
DROP TABLE IF EXISTS Enrollments;
DROP TABLE IF EXISTS Assignments;
DROP TABLE IF EXISTS Courses;
DROP TABLE IF EXISTS AdmissionApplications;
DROP TABLE IF EXISTS Students;
DROP TABLE IF EXISTS Classes;
DROP TABLE IF EXISTS Faculties;
DROP TABLE IF EXISTS Subjects;

DROP TABLE IF EXISTS Departments];
DROP TABLE IF EXISTS Users;