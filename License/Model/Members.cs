using System.Collections.Generic;

namespace License.Model
{
    public class Members
    {
        public virtual int Id { get; set; }
        public virtual string FullName { get; set; }
        public virtual string Role { get; set; }
        public virtual string MemberFunction { get; set; }
        public virtual string PersonalPage { get; set; }
        public virtual Users User { get; set; }
        public virtual IList<Lectures> Lectures { get; set; }
        public virtual IList<Projects> Projects { get; set; }
        public virtual IList<Publications> Publications { get; set; }

        public Members(string fullName, string role, string function, string page)
        {
            this.FullName = fullName;
            this.Role = role;
            this.MemberFunction = function;
            this.PersonalPage = page;
            this.Lectures = new List<Lectures>();
            this.Projects = new List<Projects>();
            this.Publications = new List<Publications>();
        }

        public Members()
        {
            Lectures = new List<Lectures>();
            Projects = new List<Projects>();
            Publications = new List<Publications>();
        }
    }
}
