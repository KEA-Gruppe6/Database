﻿using Database_project.Core.MongoDB.Entities;

namespace Database_project.Core.MongoDB.RequestDTOs;

public class MongoDBAirlineDTO
{
    public string AirlineName { get; set; }

    public ICollection<MongoDBPlane> Planes { get; set; } = [];
}