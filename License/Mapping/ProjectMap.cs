using FluentNHibernate.Mapping;
using License.Model;

namespace License.Mapping
{
    public class ProjectMap : ClassMap<Project>
    {
        public ProjectMap()
        {
            Table("Projects");
            Id(x => x.Id);
            Map(x => x.Title);
            Map(x => x.Description);
            Map(x => x.StartDate);
            Map(x => x.EndDate);
            HasManyToMany<Member>(x => x.Coordinators)
                           .Cascade.All()
                           .Inverse()
                           .Table("MembersProjects");
        }
    }
}
