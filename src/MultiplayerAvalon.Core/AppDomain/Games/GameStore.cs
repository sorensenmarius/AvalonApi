using System;
using System.Collections.Generic;
using System.Linq;

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

        public static Game GetGame(Guid id)
        {
            instance.Games.TryGetValue(id, out Game g);
            return g;
        }

        public static void DeleteGame(Guid id)
        {
            instance.Games.Remove(id);
        }

        public static Game GetGameByJoinCode(int joinCode)
        {
            Game g = instance.Games.Where(keyValuePair => keyValuePair.Value.JoinCode == joinCode).FirstOrDefault().Value;
            if (g == default(Game))
                throw new KeyNotFoundException($"Game with Join Code {joinCode} does not exist");
            return g;
        }
    }
}
