using FluentNHibernate.Mapping;
using License.Model;

namespace License.Mapping
{
    public class UsersMap : ClassMap<Users>
    {
        public UsersMap()
        {
            Id(x => x.Id);
            Map(x => x.UserName);
            Map(x => x.Password);
            Map(x => x.Role);
            Map(x => x.AuthToken);
            References(x => x.Member).Unique().Cascade.All();
        }
    }
}
