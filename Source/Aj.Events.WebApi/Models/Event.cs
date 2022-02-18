using Aj.Events.WebApi.Models;
using System;
using System.Collections.Generic;

namespace Aj.Events.WebApi.Models
{
    public class Event
    {
        public string ShortDescription { get; set; }
        public string DetailedDescription { get; set; }
        public DateTime? DthEvent { get; set; }
        public DateTime? DthCreated { get; set; }
        public Source Source { get; set; }
        public Category Category { get; set; }
        public EventType EventType { get; set; }
        public string[] Tags { get; set; }
        public string Url { get; set; }

    }
}
