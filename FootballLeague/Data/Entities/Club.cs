﻿using System.ComponentModel.DataAnnotations;

namespace FootballLeague.Data.Entities
{
	public class Club : IEntity
	{
		public int Id { get; set; }

		[Required]
		[MaxLength(100)]
		public string Name { get; set; }

		[Display(Name = "Logo")]
		public string? ImageLogo { get; set; }

		[Required]
		public string Stadium { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "A capacidade deve ser um número positivo.")]
        public int Capacity { get; set; }

        [Display(Name = "Head Coach")]
        public string? HeadCoach { get; set; }
	}
}

