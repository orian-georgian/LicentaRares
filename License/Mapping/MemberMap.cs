using FluentNHibernate.Mapping;
using License.Model;

namespace License.Mapping
{
    public class MemberMap : ClassMap<Member>
    {
        public MemberMap()
        {
            Table("Members");
            Id(x => x.Id);
            Map(x => x.FullName);
            Map(x => x.Role);
            Map(x => x.MemberFunction);
            Map(x => x.PersonalPage);
            Map(x => x.Email);
            HasMany<Lecture>(x => x.Lectures).Inverse().Cascade.All();
            HasManyToMany<Project>(x => x.Projects)
                .Cascade.All()
                .Table("MembersProjects");
            HasManyToMany<Publication>(x => x.Publications)
               .Cascade.All()
               .Table("MembersPublications");
        }
    }
}