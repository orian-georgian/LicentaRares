using System;
using System.Collections.Immutable;
namespace License.Mapping
{
    public class Project
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ImmutableArray<Members> Coordinators { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
