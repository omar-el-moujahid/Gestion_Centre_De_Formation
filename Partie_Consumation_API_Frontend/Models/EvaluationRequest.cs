namespace Partie_Consumation_API_Frontend.Models
{
    public class EvaluationRequest
    {
        public int ParticipantId { get; set; }
        public int FormationId { get; set; }
        public int Rating { get; set; }
        public string Review { get; set; }
    }
}
