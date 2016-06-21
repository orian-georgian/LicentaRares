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
            Map(x => x.MemberPhoto);
            HasManyToMany<Lecture>(x => x.Lectures)
                .Cascade.All()
                .Table("MembersLectures");
            HasManyToMany<Project>(x => x.Projects)
                .Cascade.All()
                .Table("MembersProjects");
            HasManyToMany<Publication>(x => x.Publications)
               .Cascade.All()
               .Table("MembersPublications");
        }
    }
}