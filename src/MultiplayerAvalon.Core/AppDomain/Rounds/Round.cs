using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using MultiplayerAvalon.AppDomain.Players;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace MultiplayerAvalon.AppDomain.Rounds
{
    [Table("Rounds")]
    public class Round : Entity<Guid>, IHasCreationTime
    {
        public Round()
        {
            CurrentTeam = new List<Player>();
            Status = RoundStatus.SelectingTeam;
            CreationTime = DateTime.Now;
        }
        [Required]
        public int FailedTeams { get; set; }
        public List<Player> CurrentTeam { get; set; }
        public DateTime CreationTime { get; set; }
        public RoundStatus Status { get; set; }
        public int VotesForTeam { get; set; }
        public int VotesAgainstTeam { get; set; }
        public int MissionVoteGood { get; set; }
        public int MissionVoteBad { get; set; }
    }
}