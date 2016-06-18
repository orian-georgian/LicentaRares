namespace License.Model
{
    public class Members
    {
        public virtual int Id { get; set; }
        public virtual string FullName { get; set; }
        public virtual string Role { get; set; }
        public virtual string MemberFunction { get; set; }
        public virtual string PersonalPage { get; set; }

        public Members(string fullName, string role, string function, string page)
        {
            this.FullName = fullName;
            this.Role = role;
            this.MemberFunction = function;
            this.PersonalPage = page;
        }

        public Members() { }
    }
}
