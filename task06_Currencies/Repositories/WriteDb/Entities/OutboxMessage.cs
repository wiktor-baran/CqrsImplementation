using System.ComponentModel.DataAnnotations;

namespace task06_Currencies.Repositories.WriteDb.Entities
{
    public class OutboxMessage
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public required DateTimeOffset OccurredOn { get; set; }

        [Required]
        public required string Type { get; set; }

        [Required]
        public required string Content { get; set; }

        public DateTimeOffset? ProcessedOn { get; set; }
    }

}
