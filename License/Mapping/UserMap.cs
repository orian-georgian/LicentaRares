using FluentNHibernate.Mapping;
using License.Model;

namespace License.Mapping
{
    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Table("Users");
            Id(x => x.Id);
            Map(x => x.Email);
            Map(x => x.Password);
            Map(x => x.Name);
            Map(x => x.AuthRole);
            Map(x => x.AuthToken);

        }
    }
}
