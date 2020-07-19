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
        public Player[] CurrentTeam { get; set; }
        public RoundStatus Status { get; set; }
        public int FailedTeams { get; set; }
        public int VotesForTeam { get; set; }
        public int VotesAgainstTeam { get; set; }
        public int MissionVoteGood { get; set; }
        public int MissionVoteBad { get; set; }
        public int RequiredPlayers { get; set; }
    }
}