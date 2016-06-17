
namespace License.Mapping
{
    public class Lecture
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Line { get; set; }
        public string Laboratory { get; set; }
        public string Year { get; set; }
        public Members Teacher { get; set; }
    }
}
