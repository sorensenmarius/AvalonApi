using Abp.AutoMapper;
using MultiplayerAvalon.AppDomain.Games;
using MultiplayerAvalon.AppDomain.Players;
using System;
using System.ComponentModel.DataAnnotations;

namespace MultiplayerAvalon.Players.Dto
{
    [AutoMapFrom(typeof(Player))]
    public class CreatePlayerDto
    {
        [Required]
        [StringLength(64)]
        public string Name { get; set; }
        public int JoinCode { get; set; }
    }
}
