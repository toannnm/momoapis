﻿using Domain.Enums;

namespace Application.Models.DocumentModels
{
    public class FilterDocumentModel
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Content { get; set; }
        public DocumentStatusEnum? DocumentStatus { get; set; }
        public PriorityEnum? Priority { get; set; }
    }
}