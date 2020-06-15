using MultiplayerAvalon.AppDomain.Games;
using MultiplayerAvalon.AppDomain.Players;
using System;
using System.Collections.Generic;
using System.Text;

namespace MultiplayerAvalon.Players.Dto
{
    public class GamePlayerDto 
    {
        public Game Game { get; set; }
        public Player Player { get; set; }
    }
}
