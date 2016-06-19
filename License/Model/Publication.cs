using System;
using System.Collections.Generic;

namespace License.Model
{
    public class Publication
    {
        public virtual int Id { get; set; }
        public virtual string Title { get; set; }
        public virtual string Description { get; set; }
        public virtual DateTime PublicationDate { get; set; }
        public virtual IList<Member> Authors { get; set; }

        public Publication()
        {
            Authors = new List<Member>();
        }
    }
}
