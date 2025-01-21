# 📚 EduBridge - Learning Management System (LMS) Documentation

🚀 **EduBridge** is a **comprehensive Learning Management System (LMS)** designed to enhance **online education** by providing a structured platform for managing **users, courses, lessons, quizzes, payments, reports, and other essential features**. The system is built using **ASP.NET Core** and leverages **Entity Framework Core** for efficient database management.

This document serves as a **detailed reference** for understanding the core **entities**, their **attributes**, and **relationships** within the EduBridge system.

---

## 📌 Overview of the EduBridge System
EduBridge is a **robust and scalable** learning platform that allows:
- **Students** to enroll in courses, attend lessons, take quizzes, and track their learning progress.
- **Teachers** to create courses, schedule lessons, manage student performance, and provide feedback.
- **Parents** to monitor their children's progress and payments.
- **Administrators** to oversee the entire system, manage reports, and handle payments efficiently.

The system is designed to be **user-friendly**, **secure**, and **scalable**, supporting **various learning methodologies**, including **online lectures, quizzes, assignments, and real-time video lessons**.

---

# 📂 Detailed Entities Breakdown

Each entity in the system represents a crucial part of the **EduBridge learning ecosystem**. Below is an in-depth explanation of each entity, including **its role, attributes, and relationships**.

---

## 🏠 1. Address Entity
### **Purpose:**
The `Address` entity stores **detailed address information** of a user.

### **Attributes:**
- **`Country` (`string`)** – Specifies the country where the user resides.
- **`City` (`string`)** – Stores the city name of the user's residence.
- **`Region` (`string`)** – Defines the state or province.
- **`Street` (`string`)** – Contains the street address and house/building number.
- **`AppUserId` (`string`)** – Acts as a **foreign key** linking to `AppUser`.
- **`AppUser` (`AppUser`)** – Navigation property connecting the address with a user.

### **Relationships:**
- **One-to-One** with `AppUser` – Each user has only **one** registered address.

---

## 👤 2. AppUser Entity
### **Purpose:**
The `AppUser` entity represents **users** in the system. This includes **students, teachers, parents, and administrators**.

### **Attributes:**
- **Personal Details:**
  - `FullName` (`string`) – The full name of the user.
  - `Image` (`string`) – URL to the user's profile picture.
  - `BirthDate` (`DateTime`) – The user's date of birth.
  - `Age` (`int`) – Automatically calculated based on `BirthDate`.

- **Account Information:**
  - `IsBlocked` (`bool`) – Indicates whether the user is **banned** from the system.
  - `BlockedUntil` (`DateTime?`) – Specifies when the block **expires**.
  - `CreatedTime` (`DateTime?`) – The **account creation date**.
  - `UpdatedTime` (`DateTime?`) – Last **modification date**.

- **Verification & Security:**
  - `IsFirstTimeLogined` (`bool`) – Identifies whether the user is **logging in for the first time**.
  - `IsReportedHighly` (`bool`) – Flags **users** who have been **reported multiple times**.
  - `VerificationCode` (`string`) – Stores the **email verification code**.
  - `CustomerId` (`string`) – External **payment system ID**.

### **Relationships:**
- **One-to-One:** `Student`, `Teacher`, `Parent` – A user can be **either a student, teacher, or parent**.
- **One-to-Many:** `Reports`, `Notes` – A user can **create multiple reports and notes**.
- **One-to-One:** `Address` – Each user has **one registered address**.

---

## 🎓 3. Course Entity
### **Purpose:**
The `Course` entity defines **learning courses** available on the platform.

### **Attributes:**
- **Course Details:**
  - `Name` (`string`) – Title of the course.
  - `Description` (`string`) – Summary of the course content.
  - `DifficultyLevel` (`enum`) – Defines difficulty as **Beginner, MidLevel, or Advanced**.
  - `ImageUrl` (`string`) – Course cover image URL.
  - `Language` (`string`) – Specifies the language of instruction.
  - `Requirements` (`string`) – Lists prerequisites for taking the course.

- **Schedule & Pricing:**
  - `StartDate` (`DateTime?`) – The course **start date**.
  - `EndDate` (`DateTime?`) – The course **end date**.
  - `Duration` (`TimeSpan`) – Total **duration** of the course.
  - `Price` (`decimal`) – Cost of enrollment.

### **Relationships:**
- **One-to-Many:** `Lesson`, `CourseStudent` – Each course has **multiple lessons and enrolled students**.

---

## 💳 4. Fee Entity
### **Purpose:**
Manages **student payments** related to course enrollments.

### **Attributes:**
- `Amount` (`decimal`) – Course **fee amount**.
- `DueDate` (`DateTime`) – The **deadline** for payment.
- `PaidDate` (`DateTime?`) – Stores the **payment date**, if completed.
- `PaymentStatus` (`enum`) – Defines the payment status (**Paid, Pending, Due**).
- `PaymentMethod` (`enum`) – Specifies the mode of payment (**Cash, CreditCard, BankTransfer**).
- `DiscountPercentage` (`decimal?`) – Stores **applied discounts**.
- `DiscountedPrice` (`decimal?`) – The **final price** after applying a discount.
- `StudentId` (`Guid`) – Foreign key linking to `Student`.

### **Relationships:**
- **One-to-One:** `Student` – A fee is directly linked to **one student**.

---

## 📚 5. Lesson Entity
### **Purpose:**
Represents individual **lessons** within a course.

### **Attributes:**
- `Title` (`string`) – Name of the lesson.
- `ScheduledDate` (`DateTime`) – The **date** when the lesson is scheduled.
- `Duration` (`TimeSpan`) – Lesson **length**.
- `StartTime` (`TimeSpan`) – The **start time** of the lesson.
- `EndTime` (`TimeSpan`) – The **end time** of the lesson.
- `Status` (`enum`) – Lesson **status** (**Scheduled, Completed, Canceled**).
- `LessonType` (`enum`) – Type of lesson (**Lecture, Lab, Online, Tutorial**).
- `MeetingLink` (`string`) – URL for **virtual lessons**.

### **Relationships:**
- **One-to-Many:** `LessonMaterial`, `LessonQuiz`, `LessonVideo`, `LessonStudent`.

---

# 📚 More Documentation Coming Soon...
This document will be updated with **detailed explanations** of more entities such as **Quizzes, Reports, Teachers, Students, and Parents**.

