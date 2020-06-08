using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using MultiplayerAvalon.AppDomain.Players;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace MultiplayerAvalon.AppDomain.Rounds
{
    [Table("Rounds")]

    public class Round : Entity<Guid>, IHasCreationTime
    {
        [Required]
        public int FailedTeams { get; set; }
        public List<Player> Players { get; set; }
        public List<Player> CurrentTeam { get; set; }
        public DateTime CreationTime { get; set; }
        public RoundStatus Status { get; set; }
        public int VotesForTeam { get; set; }
        public int TeamExpVote { get; set; }
        public int ExpFailureVote { get; set; }
        //public List<string> Votes {get;set;} // For the time being votes can be added in string such as: Votes for {VoteUp} and votes against {VoteDown}
        //public List<List<VoteResults>> ExpeditionVote { get; set; }
    }
}