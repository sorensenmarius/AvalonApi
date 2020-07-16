using System;
using System.Collections.Generic;
using System.Text;

namespace MultiplayerAvalon.Rounds.Dto
{
    public class SetTeamDto
    {
        public Guid GameId { get; set; }
        public List<Guid> CurrentTeam { get; set; }
    }
}
