using System.ComponentModel.DataAnnotations.Schema;

namespace Movies_Alexandra_marian.Models
{
    public class Movie
    {
        public int ID { get; set; }
        public string Title { get; set; }

        

        public int? DirectorID { get; set; }
        public Director? Director{ get; set; }

        public ICollection<History>? Histories { get; set; }

        public ICollection<DistributedMovie>? DistributedMovies { get; set; }
    }
}