using System;
using System.Collections.Generic;

namespace License.Model
{
    public class Project
    {
        public virtual int Id { get; set; }
        public virtual string Title { get; set; }
        public virtual string Description { get; set; }
        public virtual DateTime StartDate { get; set; }
        public virtual DateTime EndDate { get; set; }
        public virtual IList<Member> Coordinators { get; set; }

        public Project()
        {
            Coordinators = new List<Member>();
        }
    }
}
