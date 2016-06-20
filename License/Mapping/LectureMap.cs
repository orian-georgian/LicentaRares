using FluentNHibernate.Mapping;
using License.Model;

namespace License.Mapping
{
    public class LectureMap : ClassMap<Lecture>
    {
        public LectureMap()
        {
            Table("Lectures");
            Id(x => x.Id);
            Map(x => x.Name);
            Map(x => x.Line);
            Map(x => x.Laboratory);
            Map(x => x.Year);
            References<Member>(x => x.Teacher);
        }
    }
}
