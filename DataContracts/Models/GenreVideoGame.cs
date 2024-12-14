using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataContracts.Models
{
    public class GenreVideoGame
    {
        public int GenresId { get; set; }

        public int VideoGamesId { get; set; }
    }
}
