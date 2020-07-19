using System;
using System.Collections.Generic;
using System.Text;

namespace MultiplayerAvalon.Games.Dto
{
    public class StartGameDto
    {
        public Guid Id { get; set; }
        public List<string> Roles { get; set; }
    }
}
