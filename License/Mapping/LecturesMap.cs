using FluentNHibernate.Mapping;
using License.Model;

namespace License.Mapping
{
    public class LecturesMap : ClassMap<Lectures>
    {
        public LecturesMap()
        {
            Id(x => x.Id);
            Map(x => x.Name);
            Map(x => x.Line);
            Map(x => x.Laboratory);
            References<Members>(x => x.Teacher);
        }
    }
}
