using FluentNHibernate.Mapping;
using License.Model;

namespace License.Mapping
{
    public class AuthorsMap : ClassMap<Authors>
    {
        public AuthorsMap()
        {
            Id(x => x.Id);
            //Map(x => x.Name);
            //Map(x => x.MemberId);
        }
    }

    public class PublicationsMap : ClassMap<Publications>
    {
        public PublicationsMap()
        {
            Id(x => x.Id);
            //Map(x => x.Title);
            //Map(x => x.AuthorsIds);
            //Map(x => x.Description);
            //Map(x => x.PublicationDate);
        }
    }
}
