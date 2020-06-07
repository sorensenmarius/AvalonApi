using Abp.AutoMapper;
using MultiplayerAvalon.AppDomain.Games;
using MultiplayerAvalon.AppDomain.Players;
using MultiplayerAvalon.AppDomain.Rounds;
using System;
using System.ComponentModel.DataAnnotations;

namespace MultiplayerAvalon.Rounds.Dto
{
    [AutoMapFrom(typeof(Round))]
    public class RoundDto
    {
        [Required]
        public string RoundCode { get; set; }
        public Player[] Players { get; set; }
        public Player[] CurrentlyChoosen { get; set; }
        public RoundStatus Status { get; set; }
        public int WhosTurn { get; set; }
    }
}