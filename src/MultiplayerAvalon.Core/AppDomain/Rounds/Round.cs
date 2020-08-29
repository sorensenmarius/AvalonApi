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
        public Round(int roundNumber, int totalPlayerCount)
        {
            CurrentTeam = new List<Player>();
            Status = RoundStatus.SelectingTeam;
            CreationTime = DateTime.Now;
            RequiredPlayers = HowManyPlayers(roundNumber, totalPlayerCount).HowMany;
        }
        [Required]
        public int FailedTeams { get; set; }
        public List<Player> CurrentTeam { get; set; }
        public string TeamString { get; set; }
        public DateTime CreationTime { get; set; }
        public RoundStatus Status { get; set; }
        public int VotesForTeam { get; set; }
        public int VotesAgainstTeam { get; set; }
        public int MissionVoteGood { get; set; }
        public int MissionVoteBad { get; set; }
        public int RequiredPlayers { get; set; }
        public string TeamString { get; set; }

        private HowManyPlayerHelper HowManyPlayers(int RoundNmr, int TotalPlayers)
        {
            bool DoubleRound = false;
            int[,] NmrPlayerBasedOnRound = new int[5, 6] { { 2, 2, 2, 3, 3, 3 }, { 3, 3, 3, 4, 4, 4 }, { 2, 4, 3, 4, 4, 4 }, { 3, 3, 4, 5, 5, 5 }, { 3, 4, 4, 5, 5, 5 } };
            if (RoundNmr == 4 && TotalPlayers >= 7)
            {
                DoubleRound = true;
            }
            int HowMany = NmrPlayerBasedOnRound[RoundNmr - 1, TotalPlayers - 5];
            return new HowManyPlayerHelper(HowMany, DoubleRound);
        }

        public class HowManyPlayerHelper
        {
            public int HowMany;
            public bool DoubleRound;
            public HowManyPlayerHelper(int howMany, bool doubleRound)
            {
                HowMany = howMany;
                DoubleRound = doubleRound;
            }
        }
    }
}