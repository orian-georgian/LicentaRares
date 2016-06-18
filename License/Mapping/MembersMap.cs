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
        }
    }
}
