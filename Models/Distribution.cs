using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Movies_Alexandra_marian.Models
{
    public class Distribution
    {
        public int ID { get; set; }
        [Required]
        [Display(Name = "Distribution Name")]
        [StringLength(50)]
        public string DistributionName { get; set; }

        [StringLength(70)]
        public string Adress { get; set; }
        public ICollection<DistributedMovie>? DistributedMovies { get; set; }
    }
}
