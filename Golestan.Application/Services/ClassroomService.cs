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
        var result = await classroomRepository.GetFacultyClassrooms(facultyId);
        var model = result.Adapt<List<ClassroomDto>>();


        return model;
    }

    public async Task<ClassroomDto> GetClassroomById(int classroomId)
    {
        return await classroomRepository.GetClassroomDtoById(classroomId);
    }

    public async Task<Result> AddClassroom(AddClassroomDto dto)
    {
        var classroom = new Classroom()
        {
            ClassNumber = dto.ClassNumber,
            Capacity = dto.Capacity,
            FacultyId = dto.FacultyId,
        };


        return await classroomRepository.AddClassroom(classroom);
    }

    public async Task<Result> RemoveClassroom(int classroomId)
    {
        return await classroomRepository.RemoveClassroom(classroomId);
    }

    public async Task<bool> VerifyClassroomNumber(string classNumber, int facultyId)
    {
        return await classroomRepository.VerifyClassroomNumber(classNumber, facultyId);
    }

}
