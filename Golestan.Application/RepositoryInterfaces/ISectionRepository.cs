namespace Golestan.Application.RepositoryInterfaces;

using Domain.Entities;


public interface ISectionRepository {

    Task<List<Section>> GetClassroomSections(int classroomId);

}
