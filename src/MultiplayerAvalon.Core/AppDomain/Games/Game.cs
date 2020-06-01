using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using MultiplayerAvalon.AppDomain.Players;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MultiplayerAvalon.AppDomain.Games
{
    [Table("Games")]
    public class Game : Entity<Guid>, IHasCreationTime
    {
        [Required]
        public int JoinCode { get; set; }
        public List<Player> Players { get; set; }
        public DateTime CreationTime { get; set; }
        public GameStatus Status { get; set; }

        public Game()
        {
            Random generator = new Random();
            JoinCode = generator.Next(99999, 999999);
            Players = new List<Player>();
            CreationTime = DateTime.Now;
            Status = GameStatus.WaitingForPlayers;
        }
    }
}
