
namespace License.Model
{
    public class Lecture
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string Line { get; set; }
        public virtual string Laboratory { get; set; }
        public virtual string Year { get; set; }
        public virtual Member Teacher { get; set; }

        public Lecture()
        {
            Teacher = new Member();
        }
    }
}
