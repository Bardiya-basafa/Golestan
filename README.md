# Golestan: University Management System

**Golestan** is a university administration web application built with **ASP.NET Core MVC (.NET 9)**, **Entity Framework Core** and **ASP.NET Core Identity** on **SQL Server**. Its name and goals come from *Golestan*, the well-known education management system used by Iranian universities. It models faculties, instructors, students, courses, classrooms, course sections and weekly time slots, and gives administrators a dashboard to manage them.

The solution follows a **layered (Clean / Onion-style) architecture** that separates the domain model, the application logic, the persistence layer, shared helpers and the MVC presentation layer.

> **Status:** in active development. Faculty, instructor and classroom management work end to end. Section scheduling, course management and the student area are partly built.

---

## Table of Contents

- [Tech Stack](#tech-stack)
- [Solution Architecture](#solution-architecture)
- [Domain Model](#domain-model)
- [Database Design](#database-design)
- [Application Layer](#application-layer)
- [Validation Strategy](#validation-strategy)
- [Web Layer (MVC)](#web-layer-mvc)
- [Feature Walkthrough](#feature-walkthrough)
- [Routes Reference](#routes-reference)
- [Getting Started](#getting-started)
- [Migrations History](#migrations-history)
- [Roadmap](#roadmap)

---

## Tech Stack

| Layer | Technology |
|---|---|
| Runtime | .NET 9 |
| Web framework | ASP.NET Core MVC with Razor views |
| ORM | Entity Framework Core 9 (SQL Server provider) |
| Identity | ASP.NET Core Identity (`IdentityDbContext<AppUser>`), cookie authentication |
| Database | Microsoft SQL Server / SQL Express |
| UI | Tailwind CSS, UIkit, Font Awesome, Lucide icons |
| Client validation | jQuery Validation + Unobtrusive Validation, `[Remote]` attributes |
| Dynamic forms | Vanilla JS `fetch()` that fills cascading dropdowns from JSON endpoints |

---

## Solution Architecture

```
Golestan.sln
├── Golestan.Domain           ← Entities and enums. No dependencies on other projects.
├── Golestan.Shared           ← Cross-cutting constants and helpers (roles, Result)
├── Golestan.Infrastructure   ← EF Core AppDbContext, Fluent API config, migrations
├── Golestan.Application      ← Services, interfaces, DTOs, custom validation attributes
└── Golestan.Web              ← MVC controllers, Razor views, static assets, Program.cs
```

```
           ┌──────────────────────┐
           │     Golestan.Web     │  Controllers / Views / DI composition root
           └──────────┬───────────┘
                      │ depends on interfaces
           ┌──────────▼───────────┐
           │ Golestan.Application │  IFacultyService, IClassroomService, ...
           └──────────┬───────────┘     DTOs + validation attributes
                      │
           ┌──────────▼───────────┐
           │Golestan.Infrastructure│ AppDbContext (EF Core + Identity)
           └──────────┬───────────┘
                      │
     ┌────────────────▼───────────────┐   ┌─────────────────┐
     │        Golestan.Domain         │   │ Golestan.Shared │
     │  Entities · Enums              │   │ AppRoles·Result │
     └────────────────────────────────┘   └─────────────────┘
```

**Design principles in the code:**

- **Dependency inversion:** controllers depend only on service *interfaces* (`IFacultyService`, `IUserService`, …), which are registered in `Program.cs` with a scoped lifetime.
- **DTO projection:** services never return entities to views. EF Core `Select` projections build purpose-specific DTOs (`DetailsFacultyDto`, `InstructorsDto`, …), so SQL fetches only the columns it needs and aggregate counts are computed **in the database**.
- **Result pattern:** `Golestan.Shared.Helpers.Result { Succeeded, Message }` carries operation outcomes and user-facing messages back to the UI.
- **Centralised role names:** `AppRoles.Admin`, `AppRoles.Student` and `AppRoles.Instructor` avoid magic strings.

---

## Domain Model

```
                ┌──────────────┐
                │   Faculty    │ MajorName, BuildingName, Budget, StartDate
                └──┬──┬──┬──┬──┘
       1..*  ┌─────┘  │  │  └──────┐ 1..*
     ┌───────▼──┐ ┌───▼──▼───┐ ┌───▼──────┐
     │ Student  │ │Instructor│ │Classroom │ ClassNumber, Capacity
     └──┬───▲───┘ └──┬───▲───┘ └────┬─────┘
        │   │1:1     │   │1:1       │ 1..*
        │ ┌─┴────────┴───┴┐         │
        │ │   AppUser     │ (IdentityUser + FirstName, LastName, UserType)
        │ └───────────────┘         │
        │ *..*          1..*        │
        │      ┌─────────────┐◄─────┘
        └─────►│   Section   │◄────── Course (Name, Unit, Description, ExamTime)
               └──────┬──────┘
                      │ 1:1
               ┌──────▼──────┐
               │  TimeSlot   │ DayOfWeek, StartTime, EndTime, IsActive
               └─────────────┘
```

| Entity | Key properties | Relationships |
|---|---|---|
| **AppUser** | extends `IdentityUser`, `FirstName`, `LastName`, `UserType` | 1:1 → `Student` profile, 1:1 → `Instructor` profile |
| **Faculty** | `MajorName`, `BuildingName`, `Budget`, `StartDate` | 1:N → Students, Instructors, Courses, Classrooms |
| **Instructor** | `FullName`, `HireDate`, `Salary` | N:1 → Faculty, 1:N → Sections, 1:1 → AppUser |
| **Student** | `FullName`, `StudentNumber` (unique), `EnteredDate` | N:1 → Faculty, N:M → Sections, 1:1 → AppUser |
| **Course** | `Name`, `Unit`, `Description`, `ExamTime` | N:1 → Faculty, 1:N → Sections |
| **Classroom** | `ClassNumber`, `Capacity` | N:1 → Faculty, 1:N → Sections |
| **Section** | an offering of a course | N:1 → Course, Classroom, Instructor; 1:1 → TimeSlot; N:M → Students |
| **TimeSlot** | `DayOfWeek`, `StartTime`, `EndTime`, `IsActive` | 1:1 → Section |

`UserType` enum: `Student`, `Instructor`, `Admin`.

A **Section** is the central scheduling entity. It ties together *what* is taught (Course), *who* teaches it (Instructor), *where* (Classroom) and *when* (TimeSlot), and holds the enrolled Students.

---

## Database Design

`AppDbContext` inherits from `IdentityDbContext<AppUser>` and configures everything with the **Fluent API**.

### Renamed Identity tables

| Default | Golestan |
|---|---|
| `AspNetUsers` | `Users` |
| `AspNetRoles` | `Roles` |
| `AspNetUserRoles` | `UserRoles` |
| `AspNetUserClaims` | `UserClaims` |
| `AspNetUserLogins` | `UserLogins` |
| `AspNetRoleClaims` | `RoleClaims` |
| `AspNetUserTokens` | `UserTokens` |

### Notable configuration

- **`DeleteBehavior.NoAction` on every relationship.** SQL Server rejects schemas with multiple cascade paths, and Section is reachable from Faculty through Course, Classroom and Instructor. Turning off cascades avoids the error and prevents accidental mass deletes.
- **Many-to-many** Student ↔ Section through an explicit join table named `StudentSections`.
- **Unique index** on `Student.StudentNumber`.
- **One-to-one** Section ↔ TimeSlot, with the foreign key on `TimeSlot.SectionId`.
- **Role seeding:** `Admin`, `Student` and `Instructor` are inserted with `HasData`, so they exist after the first migration.
- Collection navigation properties are initialised to `new HashSet<T>()` to avoid null-reference errors (see migration `Fix_ICollection_Uninitialized`).

---

## Application Layer

### Services

| Service | Responsibilities |
|---|---|
| **FacultyService** | List faculties with **aggregated counts** of students, instructors, classes and courses; get details and edit models; add and edit faculties (rejecting duplicate major or building names); return `Id → Name` dictionaries of a faculty's classrooms, courses and instructors for dropdowns; uniqueness checks `VerifyMajor` / `VerifyBuilding`. |
| **InstructorService** | Paged list of instructors (first 10, ordered by Id) with e-mail, faculty name and a full name built from the Identity user. |
| **ClassroomService** | Classrooms of a faculty; classroom management view including its sections (course, instructor, time slot); add a classroom; check that a class number is unique **within a faculty**. |
| **UserService** | `RegisterNewInstructor`: an end-to-end onboarding flow, shown below. |
| **SectionService** | Placeholder for section scheduling (interface defined). |

### Instructor registration flow (`UserService.RegisterNewInstructor`)

```
AddInstructorDto
   │
   ├─► UserManager.CreateAsync(AppUser{FirstName, LastName, Email, UserName=Email,
   │                                     UserType=Instructor}, Password)
   │        └─ fails → Result{Message = first Identity error}
   │
   ├─► UserManager.AddToRoleAsync(user, "Instructor")
   │        └─ fails → Result{Message = first role error}
   │
   ├─► new Instructor{FullName, HireDate, Salary, FacultyId, AppUser=user}
   │   user.InstructorProfile = instructor
   │   _context.Instructors.Add(instructor); SaveChangesAsync()
   │
   └─► Result{Succeeded = true, Message = "Instructor created"}
```

### Identity password policy (`Program.cs`)

- At least 8 characters
- At least one digit
- At least one non-alphanumeric character
- Unique e-mail per user

---

## Validation Strategy

Validation happens on **three layers**, so users get immediate feedback and the server stays authoritative.

### 1. Data annotations on DTOs

| DTO | Rules |
|---|---|
| `AddFacultyDto` / `EditFacultyDto` | Required major, building, budget and start date |
| `AddClassroomDto` | `ClassNumber` must match `^\d{3}$`; `Capacity` must be in **[10, 150]** |
| `AddInstructorDto` | Names are letters only; valid e-mail; strong password regex `^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[\W_]).{8,}$`; `[Compare]` on the confirmation password |

### 2. Custom `ValidationAttribute`s (`Golestan.Application/Validations`)

| Attribute | Rule |
|---|---|
| `[NotInFuture]` | A `DateTime` (hire date) cannot be later than now |
| `[BiggerThanZero]` | Salary must be **greater than 100** |
| `[UniqueEmail]` | Gets `UserManager<AppUser>` from the `ValidationContext` service provider and rejects e-mails that are already registered |

### 3. Remote (AJAX) validation

`[Remote]` attributes call JSON endpoints **while the user types**:

| Field | Endpoint | Scope |
|---|---|---|
| Faculty major | `GET /Faculties/VerifyMajor` | global |
| Faculty building | `GET /Faculties/VerifyBuildingName` | global |
| Classroom number | `GET /Classrooms/VerifyClassNumber` | per faculty (`AdditionalFields = "FacultyId"`) |
| Instructor e-mail | `GET /Instructors/VerifyEmail` | global |

Every endpoint returns `true` when the value is **available**, which is the contract jQuery Unobtrusive Validation expects.

---

## Web Layer (MVC)

### Controllers

| Controller | Purpose |
|---|---|
| `AdminController` | Admin dashboard, faculty, instructor and course management, classroom management per faculty, add-section page, and JSON endpoints for cascading dropdowns. This is the **default route**. |
| `FacultiesController` | Details, add and edit faculty pages, plus remote uniqueness checks |
| `InstructorsController` | Add-instructor form (with faculty dropdown) and remote e-mail check |
| `ClassroomsController` | Add classroom, manage a classroom (view its sections), remote class-number check |
| `StudentsController` | Student area (UI scaffold) |
| `ErrorController` | Generic error page that receives a message |
| `Base/BaseController` | `ShowMessage(message, success)` writes to `TempData` so the layout can show a toast notification after a redirect (Post/Redirect/Get) |

### Cascading dropdowns (Add Section)

On the **Add Section** page, choosing a faculty triggers three parallel `fetch` calls:

```js
fetch(`/Admin/ClassroomOptions?facultyId=${id}`)   // → [{ value, text: "Class number : 101" }]
fetch(`/Admin/CourseOptions?facultyId=${id}`)      // → [{ value, text: "Course : Algorithms" }]
fetch(`/Admin/InstructorOptions?facultyId=${id}`)  // → [{ value, text: "#3 Jane Doe" }]
```

The returned JSON fills the Classroom, Course and Instructor `<select>` elements, so an admin can only combine resources from the same faculty.

### Views

```
Views/
├── Admin/        Dashboard, FacultiesManagement, InstructorsManagement, CourseManagement,
│                 ChooseFaculty, ManageFacultyClassrooms, AddSection
├── Faculties/    AddFaculty, EditFaculty, DetailsFaculty
├── Instructors/  Index, AddInstructor
├── Classrooms/   AddClassroom, Manage
├── Students/     Index
├── Error/        Index
└── Shared/       _Layout (sidebar navigation, TempData toast, CDN assets)
```

---

## Feature Walkthrough

1. **Dashboard** (`/`): cards for every faculty showing major, building, budget, start date and live counts of students, instructors, classrooms and courses.
2. **Faculties:** create a faculty (major and building names must be unique and are checked live), view details, edit.
3. **Instructors:** register an instructor. This creates an Identity user, assigns the `Instructor` role and creates the linked `Instructor` profile in one flow. The management page lists instructors with e-mail, faculty, salary and hire date.
4. **Classrooms:** choose a faculty, list its classrooms, add a classroom (3-digit number, unique within the faculty, capacity 10–150), and open a classroom to see its scheduled sections with course, instructor and time slot.
5. **Sections:** build a section by choosing a faculty and then picking from that faculty's classrooms, courses and instructors.

---

## Routes Reference

| Method | Route | Description |
|---|---|---|
| GET | `/` → `/Admin/Dashboard` | Faculty overview |
| GET | `/Admin/FacultiesManagement` | Faculty list |
| GET | `/Admin/InstructorsManagement` | Instructor list |
| GET | `/Admin/CourseManagement` | Course management UI |
| GET | `/Admin/ChooseFaculty` | Choose a faculty before managing its classrooms |
| GET | `/Admin/ManageFacultyClassrooms?facultyId=` | Classrooms of a faculty |
| GET | `/Admin/AddSection` | Section creation form |
| GET | `/Admin/ClassroomOptions?facultyId=` | JSON options |
| GET | `/Admin/CourseOptions?facultyId=` | JSON options |
| GET | `/Admin/InstructorOptions?facultyId=` | JSON options |
| GET/POST | `/Faculties/AddFaculty` | Create a faculty |
| GET/POST | `/Faculties/EditFaculty/{id}` | Edit a faculty |
| GET | `/Faculties/DetailsFaculty/{id}` | Faculty details |
| GET/POST | `/Instructors/AddInstructor` | Register an instructor |
| GET/POST | `/Classrooms/AddClassroom?facultyId=` | Create a classroom |
| GET | `/Classrooms/Manage?classroomId=` | Classroom with its sections |

All POST actions use `[ValidateAntiForgeryToken]`.

---

## Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- SQL Server or SQL Server Express (LocalDB works too)
- EF Core CLI: `dotnet tool install --global dotnet-ef`

### 1. Clone

```bash
git clone https://github.com/Bardiya-basafa/Golestan.git
cd Golestan
```

### 2. Configure the connection string

Edit `Golestan.Web/appsettings.json`:

```json
"ConnectionStrings": {
  "GolestanDB": "Server=YOUR_SERVER\\SQLEXPRESS;Database=GolestanDB;Integrated Security=True;TrustServerCertificate=True;"
}
```

You can also override it with an environment variable: `ConnectionStrings__GolestanDB`.

### 3. Create the database

```bash
dotnet ef database update \
  --project Golestan.Infrastructure \
  --startup-project Golestan.Web
```

### 4. Run

```bash
dotnet run --project Golestan.Web
```

Then open the URL printed in the console. You land on the **Admin Dashboard**.

---

## Migrations History

| Migration | Purpose |
|---|---|
| `Initial` | First schema |
| `Change_Table_Names` | Rename Identity tables (`Users`, `Roles`, …) |
| `InitialSchema` | Full academic schema |
| `Fix_Major_Property` | Faculty major property fix |
| `Fix_ICollection_Uninitialized` | Initialise navigation collections |

---

## Roadmap

- [ ] Finish **section scheduling**: save sections with time slots and detect classroom and instructor **time conflicts**
- [ ] **Course** CRUD (`CoursesDto` is already defined)
- [ ] **Student** registration and **course enrollment** with capacity checks
- [ ] Login pages and `[Authorize(Roles = ...)]` on the admin area
- [ ] Seed an initial admin account
- [ ] Pagination and search for faculty, instructor and classroom lists
- [ ] Persian (`fa-IR`) localization (already sketched in `Program.cs`)
- [ ] Unit tests for services and validators
