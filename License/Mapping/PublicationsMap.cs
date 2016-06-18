using FluentNHibernate.Mapping;
using License.Model;

namespace License.Mapping
{
    public class PublicationsMap : ClassMap<Publications>
    {
        public PublicationsMap()
        {
            Id(x => x.Id);
            Map(x => x.Title);
            Map(x => x.Description);
            Map(x => x.PublicationDate);
            HasManyToMany<Members>(x => x.Authors)
                           .Cascade.All()
                           .Inverse()
                           .Table("MembersPublications");
        }
    }
}
