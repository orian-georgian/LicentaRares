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
        public virtual string Email { get; set; }
        public virtual string Password { get; set; }
        public virtual string AuthRole { get; set; }
        public virtual string AuthToken { get; set; }
        public virtual IList<Lectures> Lectures { get; set; }
        public virtual IList<Projects> Projects { get; set; }
        public virtual IList<Publications> Publications { get; set; }

        public Members(string fullName, string role, string function, string page, string email, string password, string authRole, string authToken,
            List<Lectures> lectures, List<Projects> projects, List<Publications> publications)
        {
            this.FullName = fullName;
            this.Role = role;
            this.MemberFunction = function;
            this.PersonalPage = page;
            this.Email = email;
            this.Password = password;
            this.AuthRole = AuthRole;
            this.AuthToken = authToken;
            this.Lectures = lectures;
            this.Projects = projects;
            this.Publications = publications;
        }

        public Members()
        {
            Lectures = new List<Lectures>();
            Projects = new List<Projects>();
            Publications = new List<Publications>();
        }

        //public Members MapToMember(object obj)
        //{
        //    this.Id = obj.;
        //    this.FullName = obj[1];
        //    this.Role = obj[2];
        //    this.MemberFunction = obj[3];
        //    this.PersonalPage = obj[4];
        //    this.Email = obj[5];
        //    this.Password = obj[6];
        //    this.AuthRole = obj[7];
        //    this.AuthToken = obj[9];

        //    return this;
        //}
    }
}
