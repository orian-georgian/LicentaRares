using FluentNHibernate.Mapping;
using License.Model;

namespace License.Mapping
{
    public class MembersMap : ClassMap<Members>
    {
        public MembersMap()
        {
            Id(x => x.Id);
            Map(x => x.FullName);
            Map(x => x.Role);
            Map(x => x.MemberFunction);
            Map(x => x.PersonalPage);
            HasOne(x => x.User).Cascade.All().PropertyRef("Member");
            HasMany<Lectures>(x => x.Lectures).Inverse().AsBag();
            HasManyToMany<Projects>(x => x.Projects)
                .Cascade.All()
                .Table("MembersProjects");
            HasManyToMany<Publications>(x => x.Publications)
               .Cascade.All()
               .Table("MembersPublications");
        }
    }
}