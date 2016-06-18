using System;
using System.Collections.Immutable;

namespace License.Model
{
    public class Authors
    {
        public virtual string Name { get; set; }
        public virtual int MemberId { get; set; }
    }

    public class Publications
    {
        public virtual int Id { get; set; }
        public virtual string Title { get; set; }
        public virtual ImmutableArray<Authors> Authors { get; set; }
        public virtual string Description { get; set; }
        public virtual DateTime PublicationDate { get; set; }
    }
}
