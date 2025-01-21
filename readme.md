# 📚 EduBridge - Learning Management System (LMS) Documentation

🚀 **EduBridge** is a **comprehensive Learning Management System (LMS)** designed to facilitate **online education** by efficiently managing **users, courses, lessons, quizzes, payments, and reports**.

This document provides an **in-depth breakdown** of the entities within the EduBridge system, detailing their **attributes, relationships, and functionalities**.

---

## 👉 1. Address Entity
### **Purpose:**
Manages user address information.

### **Properties:**
| Property       | Type      | Description  |
|---------------|----------|-------------|
| `Country`     | `string` | Country where the user resides. |
| `City`        | `string` | City where the user lives. |
| `Region`      | `string` | State or province of the address. |
| `Street`      | `string` | Detailed street address. |
| `AppUserId`   | `string` | Foreign key referencing `AppUser`. |
| `AppUser`     | `AppUser` | Navigation property linking to the user. |

### **Relationships:**
- **One-to-One** with `AppUser` (Each user has one address).

---

## 👉 2. AppUser Entity
### **Purpose:**
Represents a user in the system (students, teachers, parents, and admins).

### **Properties:**
| Property                     | Type        | Description  |
|------------------------------|------------|-------------|
| `FullName`                   | `string`   | User's full name. |
| `Image`                      | `string`   | Profile picture URL. |
| `IsBlocked`                  | `bool`     | Indicates if the user is blocked. |
| `CreatedTime`                | `DateTime?` | Date when the account was created. |
| `UpdatedTime`                | `DateTime?` | Date when the account was last updated. |
| `BlockedUntil`               | `DateTime?` | If blocked, expiry date of the ban. |
| `BirthDate`                  | `DateTime` | The birthdate of the user. |
| `Age`                        | `int`      | Auto-calculated age of the user. |
| `IsFirstTimeLogined`         | `bool`     | Indicates if this is the user's first login. |
| `IsReportedHighly`           | `bool`     | Marks users with high reports. |
| `IsEmailVerificationCodeValid` | `bool`     | Email verification status. |
| `VerificationCode`           | `string`   | Stores email verification code. |
| `CustomerId`                 | `string`   | External ID for payment processing. |
| `Student`                    | `Student`  | If applicable, links to the `Student` entity. |
| `Teacher`                    | `Teacher`  | If applicable, links to the `Teacher` entity. |
| `Parent`                     | `Parent`   | If applicable, links to the `Parent` entity. |
| `Notes`                      | `ICollection<Note>` | Notes created by the user. |
| `Reports`                    | `ICollection<Report>` | Reports filed by the user. |

### **Relationships:**
- **One-to-One** with `Student`, `Teacher`, `Parent`.
- **One-to-Many** with `Reports`, `Notes`.
- **One-to-One** with `Address`.

---

## 👉 3. Course Entity
### **Purpose:**
Defines a course offered on the platform.

### **Properties:**
| Property       | Type        | Description  |
|---------------|------------|-------------|
| `ImageUrl`    | `string`   | Course cover image URL. |
| `Name`        | `string`   | Course title. |
| `Description` | `string`   | Overview of the course content. |
| `DifficultyLevel` | `enum`  | Course difficulty (Beginner, MidLevel, Advanced). |
| `Lessons`     | `ICollection<Lesson>` | Lessons within the course. |
| `Duration`    | `TimeSpan` | Total course duration. |
| `Language`    | `string`   | Language of instruction. |
| `Requirements` | `string`  | Prerequisites for enrolling. |
| `Price`       | `decimal`  | Course price. |
| `StartDate`   | `DateTime?` | Course start date. |
| `EndDate`     | `DateTime?` | Course end date. |
| `CourseStudents` | `ICollection<CourseStudent>` | Enrolled students. |

### **Relationships:**
- **One-to-Many** with `Lesson`, `CourseStudent`.

---

## 👉 4. Fee Entity
### **Purpose:**
Handles student course payments.

### **Properties:**
| Property       | Type      | Description  |
|---------------|----------|-------------|
| `Amount`      | `decimal` | Fee amount. |
| `DueDate`     | `DateTime` | Payment due date. |
| `PaidDate`    | `DateTime?` | If paid, date of payment. |
| `PaymentStatus` | `enum` | Payment status (Paid, Pending, Due). |
| `PaymentMethod` | `enum` | Payment method (Cash, CreditCard, BankTransfer). |
| `DiscountPercentage` | `decimal?` | Applied discount (if any). |
| `DiscountedPrice` | `decimal?` | Final price after discount. |
| `StudentId`   | `Guid` | Reference to `Student`. |
| `Student`     | `Student` | Navigation property. |

### **Relationships:**
- **One-to-One** with `Student`.

---

## 👉 5. Lesson Entity
### **Purpose:**
Represents individual lessons in a course.

### **Properties:**
| Property       | Type      | Description  |
|---------------|----------|-------------|
| `Title`       | `string` | Lesson title. |
| `ScheduledDate` | `DateTime` | Scheduled date. |
| `Duration`    | `TimeSpan` | Lesson length. |
| `StartTime`   | `TimeSpan` | Lesson start time. |
| `EndTime`     | `TimeSpan` | Lesson end time. |
| `Status`      | `enum` | Lesson status (Scheduled, Completed, Canceled). |
| `LessonType`  | `enum` | Type of lesson (Lecture, Lab, Online, Tutorial). |
| `GradingPolicy` | `string` | Evaluation criteria. |
| `MeetingLink` | `string` | Link for virtual lessons. |

### **Relationships:**
- **One-to-Many** with `LessonMaterial`, `LessonQuiz`, `LessonVideo`, `LessonStudent`.

---

# 📚 More Documentation Coming Soon...
This document is a **work in progress**. Additional details about **Quizzes, Reports, Teachers, Students, and Parents** will be included shortly!

