using System;
using System.Collections.Generic;
using System.Text;

namespace MultiplayerAvalon.AppDomain.Games
{
    public sealed class GameStore
    {
        private static readonly GameStore instance = new GameStore();
        public Dictionary<Guid, Game> Games;
        static GameStore()
        {
        }

        private GameStore()
        {
            Games = new Dictionary<Guid, Game>();
        }

        public static GameStore Instance
        {
            get
            {
                return instance;
            }
        }

        public static void AddOrUpdateGame(Game g)
        {
            instance.Games[g.Id] = g;
        }

        public static void DeleteGame(Guid id)
        {
            instance.Games.Remove(id);
        }
    }
}
