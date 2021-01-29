namespace NetCoreAPI_Template_v2.DTOs.Fight
{
    public class AttackResultDto
    {
        public string AttackerName { get; set; }
        public int AttackHP { get; set; }
        public string OpponentName { get; set; }
        public int OpponentHP { get; set; }
        public int Damage { get; set; }
        public string AttackResultMessage { get; set; }
    }
}