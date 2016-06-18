using FluentNHibernate.Mapping;
using License.Model;

namespace License.Mapping
{
    public class ProjectsMap : ClassMap<Projects>
    {
        public ProjectsMap()
        {
            Id(x => x.Id);
            //Map(x => x.Title);
            //Map(x => x.CoordinatorsIds);
            //Map(x => x.Description);
            //Map(x => x.StartDate);
            //Map(x => x.EndDate);
        }
    }
}
