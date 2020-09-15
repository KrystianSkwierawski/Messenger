using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Model
{
    public class Message
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string Time { get; set; }
        public string ApplicationUserId { get; set; }

        [ForeignKey("RelationShip")]
        public int RelationShipId { get; set; }
        public RelationShip RelationShip { get; set; }
    }
}
