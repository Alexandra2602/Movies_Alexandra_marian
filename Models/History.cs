namespace Movies_Alexandra_marian.Models
{
    public class History
    {
        public int HistoryID { get; set; }
        public int CustomerID { get; set; }
        public int MovieID { get; set; }

        public DateTime OrderDate { get; set; }
        public Customer? Customer { get; set; }
        public Movie? Movie { get; set; }
    }
}
