namespace Movies_Alexandra_marian.Models
{
    public class Director
    {
        public int DirectorID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ICollection<Movie>? Movies { get; set; }
    }
}
