namespace Golestan.Application.Services;

using DTOs.Section;
using DTOs.Selection;
using Interfaces;
using RepositoryInterfaces;
using Shared.Helpers;


public class SelectionService(ISelectionRepository selectionRepository, IStudentRepository studentRepository, ISectionRepository sectionRepository, ITermService termService, ICourseService courseService, ISectionService sectionService) : ISelectionService {

    public async Task<SelectionDto> SelectionTermDetails()
    {
        var currentTerm = await termService.GetCurrentTermEntity();

        var model = new SelectionDto();

        if (currentTerm == null){
            model.Result.Message = "There is no current term right now";

            return model;
        }

        var currentDate = DateTime.UtcNow;

        if (currentDate > currentTerm.SectionSelectionEndTime){
            model.Result.Message = "The current selection has ended and no available selection";

            return model;
        }

        model.StartDate = currentTerm.SectionSelectionStartTime;
        model.EndDate = currentTerm.SectionSelectionEndTime;
        model.Term = currentTerm.TermIdentifier;
        model.Result.Succeeded = true;

        return model;
    }

    public async Task<List<SectionDto>?> GetAvailableSectionsForSelection(int studentId)
    {
        try{
            var currentTerm = await termService.GetCurrentTermEntity();

            if (currentTerm == null){
                throw new Exception("There is no current term right now");
            }

            var currentDate = DateTime.UtcNow;

            if (currentTerm.SectionSelectionStartTime > currentDate || currentDate > currentTerm.SectionSelectionEndTime){
                return null;
            }

            var availableCourses = await courseService.GetAvailableCoursesForStudent(studentId);

            var student = await studentRepository.GetStudentEntityById(studentId);


            return await selectionRepository.GetAvailableSections(availableCourses, student);
        }

        catch (Exception e){
            throw new Exception(e.Message);
        }
    }

    public async Task<List<SectionDto>> GetSelectedSections(int studentId)
    {
        try{
            return await selectionRepository.GetSelectedSections(studentId);
        }

        catch (Exception e){
            throw new Exception(e.Message);
        }
    }

    public async Task<Result> SelectSection(int studentId, int sectionId)
    {
        try{
            var result = new Result();

            var section = await sectionRepository.GetSectionEntityById(sectionId);
            var remainCapacity = section.Classroom.Capacity - section.Students.Count;


            if (remainCapacity == 0){
                result.Message = "Section is full and cannot be selected";

                return result;
            }


            if (await selectionRepository.IsStudentTimeTaken(studentId, section)){
                result.Message = "There is another section in this time";

                return result;
            }

            if (await selectionRepository.IsExamTimeTaken(studentId, section.Course.Exam.ExamDateTime, section.Course.Exam.TimeSlot)){
                result.Message = "There is an exam in the section time exam";

                return result;
            }


            var student = await studentRepository.GetStudentEntityById(studentId);


            var studentTotalUnit = student.Sections.Sum(s => s.Course.Unit);

            if (studentTotalUnit > 24){
                result.Message = "Something went wrong";

                return result;
            }

            if (section.Course.Unit + studentTotalUnit > 24){
                result.Message = "You can not select more than 24 units";

                return result;
            }

            return await sectionService.AddStudentsToSection([studentId], sectionId);
        }

        catch (Exception e){
            throw new Exception(e.Message);
        }
    }

    public async Task<Result> UnselectSection(int studentId, int sectionId)
    {
        try{
            var result = new Result();

            return await sectionService.RemoveStudentFromSection(studentId, sectionId);
        }

        catch (Exception e){
            throw new Exception(e.Message);
        }
    }

}
