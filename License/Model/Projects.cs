using System;
using System.Collections.Immutable;

namespace License.Model
{
    public class Projects
    {
        public virtual int Id { get; set; }
        public virtual string Title { get; set; }
        public virtual ImmutableArray<Members> Coordinators { get; set; }
        public virtual string Description { get; set; }
        public virtual DateTime StartDate { get; set; }
        public virtual DateTime EndDate { get; set; }
    }
}
