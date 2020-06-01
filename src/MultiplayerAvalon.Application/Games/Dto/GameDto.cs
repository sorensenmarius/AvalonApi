using Abp.AutoMapper;
using MultiplayerAvalon.AppDomain.Games;
using MultiplayerAvalon.AppDomain.Players;
using System;
using System.ComponentModel.DataAnnotations;

namespace MultiplayerAvalon.Games.Dto
{
    [AutoMapFrom(typeof(Game))]
    public class GameDto
    {
        [Required]
        public string JoinCode { get; set; }
        public Player[] Players { get; set; }
        public GameStatus Status { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
