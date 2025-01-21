# 📚 Learning Management System (LMS) - Entities Documentation

🚀 **A structured breakdown of all entities in the LMS API and their relationships.**

---

## 📌 Overview
The **Learning Management System (LMS)** is designed to facilitate **online learning** by managing **users, courses, lessons, quizzes, payments, and reports**. The system follows an **entity-based architecture**, where each class represents a key part of the learning platform.

- **Entities** are defined using **ASP.NET Core** and **Entity Framework Core**.
- **Common base entity** (`BaseEntity`) ensures consistency with **ID, timestamps, and other common properties**.

---

# 📂 Entities Breakdown

Below is the **detailed breakdown** of each entity, including attributes, relationships, and descriptions.

---

## 📌 1. Address

🏠 **Stores user address information.**

| **Property**   | **Type**   | **Description** |
|---------------|-----------|----------------|
| `Country`     | `string`  | The country where the user resides. |
| `City`        | `string`  | The city where the user resides. |
| `Region`      | `string`  | The region (state, province) of the address. |
| `Street`      | `string`  | The street name and number. |
| `AppUserId`   | `string`  | Foreign key reference to `AppUser`. |
| `appUser`     | `AppUser` | Navigation property linking the address to the user. |

📌 **Relationships:**  
- **One-to-One** with `AppUser` (A user has one address).

---

## 📌 2. AppUser

👤 **Represents system users (students, teachers, parents, admins).**

| **Property** | **Type** | **Description** |
|-------------|---------|----------------|
| `FullName` | `string` | The full name of the user. |
| `Image` | `string` | Profile picture URL. |
| `IsBlocked` | `bool` | Indicates if the user is blocked. |
| `CreatedTime` | `DateTime?` | Date the account was created. |
| `UpdatedTime` | `DateTime?` | Date the account was last updated. |
| `BlockedUntil` | `DateTime?` | If blocked, the date when it expires. |
| `BirthDate` | `DateTime` | The birthdate of the user. |
| `Age` | `int` | The user's calculated age. |
| `IsFirstTimeLogined` | `bool` | Determines if it's the first login. |
| `IsReportedHighly` | `bool` | Marks users with high reports. |
| `IsEmailVerificationCodeValid` | `bool` | Indicates email verification status. |
| `VerificationCode` | `string` | Stores email verification code. |
| `CustomerId` | `string` | External ID for payment systems. |
| `Student` | `Student` | If the user is a student, this links to `Student`. |
| `Teacher` | `Teacher` | If the user is a teacher, this links to `Teacher`. |
| `Parent` | `Parent` | If the user is a parent, this links to `Parent`. |
| `Notes` | `ICollection<Note>` | A collection of notes made by the user. |
| `Reports` | `ICollection<Report>` | Reports submitted by the user. |

📌 **Relationships:**  
- **One-to-One** with `Student`, `Teacher`, `Parent`.  
- **One-to-Many** with `Reports`, `Notes`.  
- **One-to-One** with `Address`.  

---

## 📌 3. Course

🎓 **Represents courses offered on the platform.**

| **Property**   | **Type**   | **Description** |
|---------------|-----------|----------------|
| `ImageUrl` | `string` | URL of the course image. |
| `Name` | `string` | Course name/title. |
| `Description` | `string` | Brief description of the course. |
| `DifficultyLevel` | `enum` | Beginner, MidLevel, Advanced. |
| `Lessons` | `ICollection<Lesson>` | Collection of lessons in the course. |
| `Duration` | `TimeSpan` | Total duration of the course. |
| `Language` | `string` | Language in which the course is taught. |
| `Requirements` | `string` | Prerequisites for the course. |
| `Price` | `decimal` | Course price. |
| `StartDate` | `DateTime?` | Course start date. |
| `EndDate` | `DateTime?` | Course end date. |
| `CourseStudents` | `ICollection<CourseStudent>` | Enrolled students. |

📌 **Relationships:**  
- **One-to-Many** with `Lesson`, `CourseStudent`.

---

## 📌 4. Fee

💳 **Handles student course fees.**

| **Property**   | **Type**   | **Description** |
|---------------|-----------|----------------|
| `Amount` | `decimal` | Fee amount. |
| `DueDate` | `DateTime` | Due date for payment. |
| `PaidDate` | `DateTime?` | Payment date (if paid). |
| `PaymentStatus` | `enum` | Paid, Pending, AboutToApproachCourseToPay. |
| `PaymentMethod` | `enum` | Cash, CreditCard, BankTransfer. |
| `DiscountPercentage` | `decimal?` | Applied discount (if any). |
| `DiscountedPrice` | `decimal?` | Final price after discount. |
| `StudentId` | `Guid` | Foreign key to `Student`. |
| `Student` | `Student` | Navigation property. |

📌 **Relationships:**  
- **One-to-One** with `Student`.

---

## 📌 5. Lesson

📚 **Represents lessons within a course.**

| **Property**   | **Type**   | **Description** |
|---------------|-----------|----------------|
| `Title` | `string` | Lesson title. |
| `ScheduledDate` | `DateTime` | Date when the lesson is scheduled. |
| `Duration` | `TimeSpan` | Lesson duration. |
| `StartTime` | `TimeSpan` | Lesson start time. |
| `EndTime` | `TimeSpan` | Lesson end time. |
| `Status` | `enum` | Scheduled, Completed, Canceled. |
| `LessonType` | `enum` | Lecture, Lab, Tutorial, Online. |
| `GradingPolicy` | `string` | Grading criteria for the lesson. |
| `MeetingLink` | `string` | Online meeting link (if applicable). |

📌 **Relationships:**  
- **One-to-Many** with `LessonMaterial`, `LessonQuiz`, `LessonVideo`, `LessonStudent`.

---

