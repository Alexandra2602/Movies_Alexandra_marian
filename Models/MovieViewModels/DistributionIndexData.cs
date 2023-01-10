using System.Security.Policy;

namespace Movies_Alexandra_marian.Models.MovieViewModels
{
    public class DistributionIndexData
    {
        public IEnumerable<Distribution> Distributions { get; set; }
        public IEnumerable<Movie> Movies { get; set; }
        public IEnumerable<History> Histories { get; set; }
    }
}
