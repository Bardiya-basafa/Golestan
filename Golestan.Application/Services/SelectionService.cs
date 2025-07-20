namespace Golestan.Application.Services;

using DTOs.Section;
using DTOs.Selection;
using Interfaces;
using RepositoryInterfaces;
using Shared.Helpers;


public class SelectionService(ISelectionRepository selectionRepository, IStudentRepository studentRepository, ISectionRepository sectionRepository, ITermService termService, ICourseService courseService, ISectionService sectionService) : ISelectionService {

    public async Task<SelectionDto> GetSelectionDto(string studentId)
    {
        var selectionDto = new SelectionDto();
        selectionDto.AvailableSections = await GetAvailableSectionsForSelection(studentId);
        selectionDto.SelectedSections = await GetSelectedSections(studentId);

        return selectionDto;
    }

    public async Task<List<SectionDto>?> GetAvailableSectionsForSelection(string studentId)
    {
        var currentTerm = await termService.GetCurrentTermEntity();


        var currentDate = DateTime.UtcNow;

        if (currentDate > currentTerm.SectionSelectionEndTime){
            return null;
        }

        var availableCourses = await courseService.GetAvailableCoursesForStudent(studentId);

        var student = await studentRepository.GetStudentEntityById(studentId);


        return await selectionRepository.GetAvailableSections(availableCourses, student);
    }

    public async Task<SelectionInfoDto> GetSelectionInfo(string studentId)
    {
        var model = new SelectionInfoDto();
        var student = await studentRepository.GetStudentDtoById(studentId);
        model.FullName = student.FullName;
        model.StudentNumber = student.StudentNumber;
        var currentTerm = await termService.GetCurrentTerm();

        if (currentTerm == null){
            throw new NullReferenceException("Current Term is null");
        }

        model.Term = currentTerm;
        var selectedSections = await GetSelectedSections(studentId);

        model.TotalUnit = selectedSections
            .Select(s => s.Course)
            .Sum(c => c.Unit);

        var examResults = await studentRepository.GetAllExamResults(studentId);

        if (examResults.Count == 0){
            model.Gpa = -1;
        }
        else{
            model.Gpa = examResults.Sum(e => e.Score) / examResults.Count;
        }


        return model;
    }

    public async Task<List<SectionDto>> GetSelectedSections(string studentId)
    {
        return await selectionRepository.GetSelectedSections(studentId);
    }

    public async Task<Result> SelectSection(string studentId, int sectionId)
    {
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

        var student = await studentRepository.GetStudentEntityById(studentId);
        var course = await sectionRepository.GetCourseBySectionId(sectionId);

        if (student.Sections.Any(s => s.Course.Id == course.Id)){
            result.Message = "You have already selected a section from this course";

            return result;
        }

        if (await selectionRepository.IsExamTimeTaken(studentId, section.Course.Exam.ExamDateTime, section.Course.Exam.TimeSlot)){
            result.Message = "There is an exam in the section time exam";

            return result;
        }


        var studentTotalUnit = student.Sections.Sum(s => s.Course.Unit);

        if (studentTotalUnit > 24){
            result.Message = "Something went wrong";

            return result;
        }

        if (section.Course.Unit + studentTotalUnit > 24){
            result.Message = "You can not select more than 24 units";

            return result;
        }

        return await sectionService.AddStudentsToSection([student.Id], sectionId);
    }

    public async Task<Result> UnselectSection(string studentId, int sectionId)
    {
        var student = await studentRepository.GetStudentDtoById(studentId);
        return await sectionService.RemoveStudentFromSection(student.Id, sectionId);
    }

}
