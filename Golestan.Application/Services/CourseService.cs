namespace Golestan.Application.Services;

using Domain.Entities;
using Domain.Enums;
using DTOs.Course;
using DTOs.Instructor;
using Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore;
using RepositoryInterfaces;
using Shared.Helpers;


public class CourseService(ICourseRepository courseRepository) : ICourseService {

    public async Task<List<CourseDto>> GetFacultyCourses(int facultyId)
    {
        try{
            return await courseRepository.GetFacultyCourses(facultyId);
        }
        catch (Exception e){
            throw new Exception(e.Message);
        }
    }

    public async Task<CourseDto> GetCourseById(int courseId)
    {
        try{
            return await courseRepository.GetCourseById(courseId);
        }
        catch (Exception e){
            throw new Exception(e.Message);
        }
    }

    public async Task<CourseInstructorDto> GetAvailableInsturctorsForCourse(int facultyId, int courseId)
    {
        try{
            return await courseRepository.GetAvailableInstructorsForCourse(facultyId, courseId);
        }
        catch (Exception e){
            throw new Exception(e.Message);
        }
    }

    public async Task<Dictionary<int, string>?> GetCourseInstructors(int courseId)
    {
        try{
            return await courseRepository.GetCourseInstructors(courseId);
        }
        catch (Exception e){
            throw new Exception(e.Message);
        }
    }

    public async Task<List<CourseDto>> GetAvailableCoursesForPrerequisite(int courseId)
    {
        return await courseRepository.GetAvailableCoursesForPrerequisite(courseId);
    }


    public async Task<List<Course>> GetAvailableCoursesForStudent(int studentId)
    {
        return await courseRepository.GetAvailableCoursesForStudent(studentId);
    }

    public async Task<Result> ApplyInstructorToCourse(CourseInstructorDto dto)
    {
        try{
            return await courseRepository.ApplyInstructorToCourse(dto.CourseId, dto.InstructorId);
        }
        catch (Exception e){
            throw new Exception(e.Message);
        }
    }

    public async Task<Result> AddCourse(AddCourseDto dto)
    {
        try{
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
        catch (Exception e){
            throw new Exception(e.Message);
        }
    }

    public async Task<Result> SetExam(SetExamForCourseDto dto)
    {
        // var currentTerm = await _termService.GetCurrentTerm();
        //
        // if (currentTerm == null){
        //     return new Result() { Message = "Term not found" };
        // }
        //
        // if (currentTerm.ExamsEndTime <= dto.ExamDateTime || dto.ExamDateTime <= currentTerm.ExamsStartTime){
        //     return new Result()
        //     {
        //         Message = $"Exam date must be between {currentTerm.ExamsStartTime.ToString("yyyy-M-d dddd")} - {dto.ExamDateTime.ToString("yyyy-M-d dddd")}",
        //     };
        // }

        if (await courseRepository.IsExamExistInClass(dto.ExamDateTime, GetTimeSlot(dto.ExamTimeSlotId), dto.ExamClassroomId)){
            return new Result() { Message = "Course already taken for exam" };
        }


        var exam = new Exam()
        {
            ClassroomId = dto.ExamClassroomId,
            CourseId = dto.CourseId,
            // TermId = currentTerm.Id,
            TimeSlot = GetTimeSlot(dto.ExamTimeSlotId),
            ExamDateTime = dto.ExamDateTime,
        };


        return await courseRepository.SetExam(exam);
    }

    public async Task<Result> RemoveCourse(int courseId)
    {
        try{
            return await courseRepository.RemoveCourse(courseId);
        }
        catch (Exception e){
            throw new Exception(e.Message);
        }
    }

    public async Task<Result> AddPrerequisiteToCourse(int courseId, int prerequisiteCourseId)
    {
        try{
            return await courseRepository.AddPrerequisiteToCourse(courseId, prerequisiteCourseId);
        }
        catch (Exception e){
            throw new Exception(e.Message);
        }
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
