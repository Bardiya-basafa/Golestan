namespace Golestan.Domain.Entities;

public class AppMessage {

    public int Id { get; set; }

    public string Content { get; set; }

    public Guid AppUserId { get; set; }

    public AppUser AppUser { get; set; }

    public bool? IsSuccess { get; set; }

}
