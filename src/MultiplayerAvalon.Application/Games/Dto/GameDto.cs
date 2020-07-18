using Abp.AutoMapper;
using MultiplayerAvalon.AppDomain.Games;
using MultiplayerAvalon.AppDomain.Players;
using MultiplayerAvalon.AppDomain.Rounds;
using System;
using System.ComponentModel.DataAnnotations;

namespace MultiplayerAvalon.Games.Dto
{
    [AutoMapFrom(typeof(Game))]
    public class GameDto
    {
        public Guid Id { get; set; }
        public string JoinCode { get; set; }
        public Player[] Players { get; set; }
        public GameStatus Status { get; set; }
        public DateTime CreationTime { get; set; }
        public Round CurrentRound { get; set; }
        public Player CurrentPlayer { get; set; }
        public int PointsInnocent { get; set; }
        public int PointsEvil { get; set; }
        public int Counter { get; set; }
        public Round[] PreviousRounds { get; set; }
    }
}
