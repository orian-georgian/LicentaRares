using FluentNHibernate.Mapping;
using License.Model;

namespace License.Mapping
{
    public class PublicationMap : ClassMap<Publication>
    {
        public PublicationMap()
        {
            Table("Publications");
            Id(x => x.Id);
            Map(x => x.Title);
            Map(x => x.Description);
            Map(x => x.PublicationDate);
            HasManyToMany<Member>(x => x.Authors)
                           .Inverse()
                           .Table("MembersPublications");
        }
    }
}
