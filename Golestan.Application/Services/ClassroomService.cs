namespace Golestan.Application.Services;

using Domain.Entities;
using DTOs.Classroom;
using DTOs.Section;
using Interfaces;
using Mapster;
using Microsoft.EntityFrameworkCore;
using RepositoryInterfaces;
using Shared.Helpers;


public class ClassroomService(IClassroomRepository classroomRepository, ISectionRepository sectionRepository) : IClassroomService {

    public async Task<List<ClassroomDto>> GetFacultyClassrooms(int facultyId)
    {
        try{
            var result = await classroomRepository.GetFacultyClassrooms(facultyId);
            var model = result.Adapt<List<ClassroomDto>>();


            return model;
        }
        catch (Exception e){
            throw new Exception(e.Message);
        }
    }

    public async Task<ClassroomDto> GetClassroomById(int classroomId)
    {
        try{
            var result = await classroomRepository.GetClassroomById(classroomId);

            var sections = await sectionRepository.GetClassroomSections(classroomId);

            var model = result.Adapt<ClassroomDto>();

            model.Sections = sections.Adapt<List<SectionDto>>();


            return model;
        }
        catch (Exception e){
            throw new Exception(e.Message);
        }
    }

    public async Task<Result> AddClassroom(AddClassroomDto dto)
    {
        try{
            var classroom = new Classroom()
            {
                ClassNumber = dto.ClassNumber,
                Capacity = dto.Capacity,
                FacultyId = dto.FacultyId,
            };


            return await classroomRepository.AddClassroom(classroom);
        }
        catch (Exception e){
            throw new Exception(e.Message);
        }
    }

    public async Task<Result> RemoveClassroom(int classroomId)
    {
        try{
            return await classroomRepository.RemoveClassroom(classroomId);
        }
        catch (Exception e){
            throw new Exception(e.Message);
        }
    }

    public async Task<bool> VerifyClassroomNumber(string classNumber, int facultyId)
    {
        return await classroomRepository.VerifyClassroomNumber(classNumber, facultyId);
    }

}
