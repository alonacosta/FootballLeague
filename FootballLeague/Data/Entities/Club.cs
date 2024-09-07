using System.ComponentModel.DataAnnotations;

namespace FootballLeague.Data.Entities
{
	public class Club : IEntity
	{
		public int Id { get; set; }
		public string Name { get; set; }

		[Display(Name = "Logo")]
		public string ImageLogo { get; set; }
		public string Stadium { get; set; }
		public string Trainer { get; set; }
	}
}

