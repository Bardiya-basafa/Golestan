﻿namespace Golestan.Domain.Entities;

using Enums;


public class Student {

    public int Id { get; set; }

    public string AppUserId { get; set; }// foreign key to AppUser 

    public AppUser AppUser { get; set; }

    public string FullName { get; set; }

    public string StudentNumber { get; set; }

    public DateTime EnteredDate { get; set; }

    public int FacultyId { get; set; }


    public Faculty Faculty { get; set; }


    public ICollection<Section> Sections { get; set; } = new HashSet<Section>();

}
