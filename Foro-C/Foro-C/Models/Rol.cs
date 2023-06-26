using Microsoft.AspNetCore.Identity;

namespace Foro_C.Models
{
    public class Rol : IdentityRole<int>
    {
        public Rol() : base()
        {

        }
        //public int Id { get; set; }

        public String Name
        {
            get { return base.Name; }
            set { base.Name = value; }
        }
    }
}
