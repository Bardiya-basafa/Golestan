namespace Golestan.Application.Services;

using Domain.Entities;
using Domain.Enums;
using DTOs.Course;
using DTOs.Exam;
using Interfaces;
using RepositoryInterfaces;
using Shared.Helpers;


public class CourseService(ICourseRepository courseRepository, ITermService termService) : ICourseService {

    public async Task<List<CourseDto>> GetFacultyCourses(int facultyId)
    {
        return await courseRepository.GetFacultyCourses(facultyId);
    }

    public async Task<CourseDto> GetCourseDtoById(int courseId)
    {
        return await courseRepository.GetCourseDtoById(courseId);
    }

    public async Task<CourseInstructorDto> GetAvailableInsturctorsForCourse(int facultyId, int courseId)
    {
        return await courseRepository.GetAvailableInstructorsForCourse(facultyId, courseId);
    }

    public async Task<List<CourseDto>> GetPrerequisiteCourses(int courseId)
    {
        var ids = courseRepository.GetCourseDtoById(courseId).GetAwaiter().GetResult().PrerequisiteCourses;

        return await courseRepository.GetPrerequisiteCourses(ids);
    }

    public async Task<Dictionary<int, string>?> GetCourseInstructors(int courseId)
    {
        return await courseRepository.GetCourseInstructors(courseId);
    }

    public async Task<List<CourseDto>> GetAvailableCoursesForPrerequisite(int courseId)
    {
        return await courseRepository.GetAvailableCoursesForPrerequisite(courseId);
    }


    public async Task<List<Course>> GetAvailableCoursesForStudent(string studentId)
    {
        return await courseRepository.GetAvailableCoursesForStudent(studentId);
    }

    public async Task<Dictionary<int, string>?> GetExamClassrooms(int courseId)
    {
        return await courseRepository.GetExamClassrooms(courseId);
    }

    public async Task<ExamDto?> GetCourseExam(int courseId)
    {
        return await courseRepository.GetCourseExam(courseId);
    }

    public async Task<Result> ApplyInstructorToCourse(CourseInstructorDto dto)
    {
        return await courseRepository.ApplyInstructorToCourse(dto.CourseId, dto.InstructorId);
    }

    public async Task<Result> AddCourse(AddCourseDto dto)
    {
        var course = new Course()
        {
            CourseName = dto.Name,
            Description = dto.Description,
            FacultyId = dto.FacultyId,
            Unit = dto.Unit,
        };


        if (await courseRepository.CourseNameExist(course.CourseName, course.FacultyId)){
            return new Result() { Message = $"Course {dto.Name}  already exist" };
        }

        var validationResult = ValidateCourse(course);

        if (!validationResult.Succeeded){
            return validationResult;
        }

        return await courseRepository.AddCourse(course);
    }

    public async Task<Result> SetExam(SetExamForCourseDto model)
    {
        var currentTerm = await termService.GetCurrentTerm();

        if (currentTerm == null){
            return new Result() { Message = "Term not found" };
        }

        if (currentTerm.ExamsEndTime <= model.ExamDate || model.ExamDate <= currentTerm.ExamsStartTime){
            return new Result()
            {
                Message = $"Exam date must be between {currentTerm.ExamsStartTime.ToString("yyyy-M-d dddd")} - {model.ExamDate.ToString("yyyy-M-d dddd")}",
            };
        }

        if (await courseRepository.IsExamExistInClass(model.ExamDate, GetTimeSlot(model.TimeSlotId), model.ClassroomId)){
            return new Result() { Message = "Course already taken for exam" };
        }


        var exam = new Exam()
        {
            ClassroomId = model.ClassroomId,
            CourseId = model.CourseId,
            TermId = currentTerm.Id,
            TimeSlot = GetTimeSlot(model.TimeSlotId),
            ExamDateTime = model.ExamDate,
        };

        var course = await courseRepository.GetCourseEntityById(model.CourseId);
        course.ExamIsSet = true;
        

        var updateResult = await courseRepository.UpdateCourse(course);

        if (!updateResult.Succeeded){
            return new Result()
            {
                Message = "Something went wrong",
            };
        }


        return await courseRepository.SetExam(exam);
    }

    public async Task<Result> RemoveCourse(int courseId)
    {
        return await courseRepository.RemoveCourse(courseId);
    }

    public async Task<Result> AddPrerequisiteToCourse(int courseId, int prerequisiteCourseId)
    {
        return await courseRepository.AddPrerequisiteToCourse(courseId, prerequisiteCourseId);
    }

    public async Task<Result> RemovePrerequisiteFromCourse(int courseId, int prerequisiteCourseId)
    {
        var course = await courseRepository.GetCourseEntityById(courseId);

        return await courseRepository.RemovePrerequisiteFromCourse(course, prerequisiteCourseId);
    }


    private static Result ValidateCourse(Course course)
    {
        var result = new Result();

        if (course.Unit > 3 || course.Unit < 1){
            result.Message = $"Course unit must be between 3 and 1 ";

            return result;
        }

        if (string.IsNullOrWhiteSpace(course.CourseName) || course.CourseName.Length < 3 || course.CourseName.Length > 40){
            result.Message = $"Course name must be between 3 and 40 characters ";

            return result;
        }

        if (string.IsNullOrWhiteSpace(course.Description) || course.Description.Length < 3 || course.Description.Length > 250){
            result.Message = $"Course description must be between 3 and 250 characters ";

            return result;
        }

        result.Succeeded = true;

        return result;
    }


    private TimeSlot GetTimeSlot(int timeSlotId)
    {
        switch (timeSlotId){
            case 1: return TimeSlot.First; break;

            case 2: return TimeSlot.Second; break;

            case 3: return TimeSlot.Third; break;

            case 4: return TimeSlot.Fourth; break;

            case 5: return TimeSlot.Fifth; break;

            case 6: return TimeSlot.Sixth; break;

            default: return TimeSlot.First;
        }
    }

}
