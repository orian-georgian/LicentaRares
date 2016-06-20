using System.Collections.Generic;

namespace License.Model
{
    public class Member
    {
        public virtual int Id { get; set; }
        public virtual string FullName { get; set; }
        public virtual string Role { get; set; }
        public virtual string MemberFunction { get; set; }
        public virtual string PersonalPage { get; set; }
        public virtual string Email { get; set; }
        public virtual byte[] MemberPhoto { get; set; }
        public virtual IList<Lecture> Lectures { get; set; }
        public virtual IList<Project> Projects { get; set; }
        public virtual IList<Publication> Publications { get; set; }

        public Member(string fullName, string role, string function, string page, string email,
            List<Lecture> lectures, List<Project> projects, List<Publication> publications)
        {
            this.FullName = fullName;
            this.Role = role;
            this.MemberFunction = function;
            this.PersonalPage = page;
            this.Email = email;
            this.Lectures = lectures;
            this.Projects = projects;
            this.Publications = publications;
        }

        public Member()
        {
            Lectures = new List<Lecture>();
            Projects = new List<Project>();
            Publications = new List<Publication>();
        }
    }
}
