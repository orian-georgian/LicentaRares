using System;
using System.Collections.Immutable;

namespace License.Mapping
{
    public struct Authors
    {
        public string Name { get; set; }
        public int MemberId { get; set; }
    }
    public class Publication
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ImmutableArray<Authors> Authors { get; set; }
        public string Description { get; set; }
        public DateTime PublicationDate { get; set; }
    }
}
