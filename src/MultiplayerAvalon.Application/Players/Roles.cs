using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using MultiplayerAvalon.AppDomain.Games;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MultiplayerAvalon.Players
{ 
    public class Roles : MultiplayerAvalonAppServiceBase
    {
        public string Name { get; set; }
        public bool IsEvil { get; set; }
        public string RoleInfo { get; set; }
        public void Action(Game g) {}
    }
}
