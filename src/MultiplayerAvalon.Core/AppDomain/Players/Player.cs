using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using MultiplayerAvalon.AppDomain.GameRoles;

namespace MultiplayerAvalon.AppDomain.Players
{
    [Table("Players")]
    public class Player : Entity<Guid>
    {
        [Required]
        [StringLength(64)]
        public string Name { get; set; }
        public GameRole RoleId {get; set;}
        public string RoleInfo { get; set; }
        public string RoleName { get; set; }
        public bool IsEvil { get; set; }
        public int Order { get; set; }
    }
}
