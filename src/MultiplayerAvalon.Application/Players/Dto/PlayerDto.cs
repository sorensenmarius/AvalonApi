using Abp.AutoMapper;
using MultiplayerAvalon.AppDomain.GameRoles;
using MultiplayerAvalon.AppDomain.Players;
using System;
using System.ComponentModel.DataAnnotations;

namespace MultiplayerAvalon.Players.Dto
{
    [AutoMapFrom(typeof(Player))]
    public class PlayerDto
    {
        [Required]
        public string Name { get; set; }
        public GameRole Role { get; set; }
        public string RoleInfo { get; set; }
        public string RoleName { get; set; }
        public bool IsEvil { get; set; }
    }
}
