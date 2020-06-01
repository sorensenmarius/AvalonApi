using System;
using System.Collections.Generic;
using System.Text;

namespace MultiplayerAvalon.GameRoles
{
    public interface IGameRole
    {
        public string Name { get; set; }
        public bool isEvil { get; set; }

    }
}
