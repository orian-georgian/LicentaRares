using System.Collections.Generic;

namespace License.Model
{
    public class Lecture
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Line { get; set; }
        public virtual string Laboratory { get; set; }
        public virtual string Year { get; set; }
        public virtual IList<Member> Teachers { get; set; }

        public Lecture()
        {
            Teachers = new List<Member>();
        }
    }
}
