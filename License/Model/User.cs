﻿
namespace License.Mapping
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public Members Members { get; set; }
        public string Role { get; set; }
        public string AuthToken { get; set; }

    }
}
