using MultiplayerAvalon.AppDomain.Rounds;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiplayerAvalon.Rounds.Dto
{
    public class SetRoundStatusDto
    {
        public Guid GameId { get; set; }
        public RoundStatus Status { get; set; }
    }
}
