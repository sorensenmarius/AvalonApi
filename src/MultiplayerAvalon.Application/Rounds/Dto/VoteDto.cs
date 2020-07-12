using System;
using System.Collections.Generic;
using System.Text;

namespace MultiplayerAvalon.Rounds.Dto
{
    public class VoteDto : GameAndPlayerIdDto
    {
        public bool Vote { get; set; }
    }
}
