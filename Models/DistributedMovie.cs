using System.Security.Policy;

namespace Movies_Alexandra_marian.Models
{
    public class DistributedMovie
    {
        public int DistributionID { get; set; }
        public int MovieID { get; set; }
        public Distribution Distribution { get; set; }
        public Movie Movie { get; set; }
    }
}
