using System;
using System.Collections.Generic;
using System.Text;

namespace MultiplayerAvalon.Rounds.Dto
{
    public class GameAndPlayerIdDto
    {
        public Guid PlayerId { get; set; }
        public Guid GameId { get; set; }
    }
}
