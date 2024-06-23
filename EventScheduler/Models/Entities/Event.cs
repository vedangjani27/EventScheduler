using EventScheduler.Helpers;
using System.ComponentModel.DataAnnotations;

namespace EventScheduler.Models.Entities
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }

        [Required]
        public string EventTitle { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 5)]
        [Display(Name = "Event Description")]
        public string EventDescription { get; set; }

        [Required]
        public Enums.Priority Priority { get; set; }
    }

}
