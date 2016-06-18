
namespace License.Model
{
    public class Users
    {
        public virtual int Id { get; set; }
        public virtual string UserName { get; set; }
        public virtual string Password { get; set; }
        public virtual string Role { get; set; }
        public virtual string AuthToken { get; set; }
        public virtual Members Member { get; set; }
    }
}
