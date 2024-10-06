using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace FootballLeague.Data.Entities
{
    public class Function : IEntity
    {
        public int Id { get; set; } 
        public string NamePosition { get; set; }
       
    }
}
