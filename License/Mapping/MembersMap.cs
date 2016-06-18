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
            Map(x => x.Email);
            Map(x => x.Password);
            Map(x => x.AuthRole);
            Map(x => x.AuthToken);
            HasMany<Lectures>(x => x.Lectures).Inverse().Cascade.All();
            HasManyToMany<Projects>(x => x.Projects)
                .Cascade.All()
                .Table("MembersProjects");
            HasManyToMany<Publications>(x => x.Publications)
               .Cascade.All()
               .Table("MembersPublications");
        }
    }
}