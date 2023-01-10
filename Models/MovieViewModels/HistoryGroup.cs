using System.ComponentModel.DataAnnotations;

namespace Movies_Alexandra_marian.Models.MovieViewModels
{
    public class HistoryGroup
    {
        [DataType(DataType.Date)]
        public DateTime? OrderDate { get; set; }
        public int MovieCount { get; set; }
    }
}
