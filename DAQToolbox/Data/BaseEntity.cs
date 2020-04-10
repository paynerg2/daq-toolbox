using System;

namespace DAQToolbox.Data
{
    public class BaseEntity : IEntity
    {
        public Guid Id { get; set; }
    }
}