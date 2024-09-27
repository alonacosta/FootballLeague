namespace FootballLeague.Data.Entities
{
    public class Function : IEntity
    {
        public int Id { get; set; } 
        public string NamePosition { get; set; }
        User User { get; set; }
        Club Club { get; set; }
    }
}
