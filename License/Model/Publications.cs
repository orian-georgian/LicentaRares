using System;
using System.Collections.Generic;

namespace License.Model
{
    public class Publications
    {
        public virtual int Id { get; set; }
        public virtual string Title { get; set; }
        public virtual string Description { get; set; }
        public virtual DateTime PublicationDate { get; set; }
        public virtual IList<Members> Authors { get; set; }

        public Publications()
        {
            Authors = new List<Members>();
        }
    }
}
