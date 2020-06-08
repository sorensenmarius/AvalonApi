using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using MultiplayerAvalon.AppDomain.GameRoles;
using MultiplayerAvalon.AppDomain.Games;
using MultiplayerAvalon.AppDomain.Players;
using MultiplayerAvalon.AppDomain.Rounds;
using MultiplayerAvalon.Games.Dto;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultiplayerAvalon.Games
{
    public class GameAppService : MultiplayerAvalonAppServiceBase, IGameAppService
    {
        private readonly IRepository<Game, Guid> _gameRepository;
        public GameAppService(
            IRepository<Game, Guid> gameRepository
            )
        {
            _gameRepository = gameRepository;
        }
        public async Task<GameDto> CreateAsync()
        {
            Game g = new Game();
            await _gameRepository.InsertAsync(g);
            return ObjectMapper.Map<GameDto>(g);
        }
        public async Task<GameDto> GetAsync(Guid id)
        {
            Game g = await _gameRepository.GetAsync(id);
            return ObjectMapper.Map<GameDto>(g);
        }
        public async Task<GameDto> StartGame(Guid id, List<string> rollene, int minions)
        {
            List<Game> game = await _gameRepository.GetAllIncluding(game => game.Players).ToListAsync();
            Game g = game.Find(item => item.Id == id);
            Round r = await CreateNewRound();
            g.CurrentRound = r;
            g.CurrentPlayer = g.Players[0];
            g.Status = GameStatus.Playing;
            await AssertRoles(id, rollene, minions);
            await _gameRepository.UpdateAsync(g);
            return ObjectMapper.Map<GameDto>(g);
        }
        public async Task<GameDto> AssertRoles(Guid id, List<string> rollene,int minions)
        {
            List<Game> game = await _gameRepository.GetAllIncluding(game => game.Players).ToListAsync();
            Game g = game.Find(item => item.Id == id);
            while (rollene.Count() < g.Players.Count())
            {
                while (minions > 0) 
                {
                    rollene.Add("2");
                    minions--;
                }
                rollene.Add("1");
            }
            List<string> roles = ShuffleNew(rollene);
            List<GameRole> gameroles = new List<GameRole>();
            roles.ForEach(x => gameroles.Add((GameRole)Enum.Parse(typeof(GameRole), x.ToString())));
            int x = 0;
            foreach (Player player in g.Players)
            {
                player.Role = gameroles[x];
                x++;
            }
            await _gameRepository.UpdateAsync(g);
            return ObjectMapper.Map<GameDto>(g);
        }
        public async Task<Round> CreateNewRound()
        {
            Round r = new Round();
            r.CurrentTeam = new List<Player>();
            return r;
        }
        public List<Player> ShufflePlayers(List<Player> items)
        {
            return items.Distinct().OrderBy(x => System.Guid.NewGuid().ToString()).ToList();
        }
        public List<string> ShuffleNew(List<string> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                string value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            return list;
        }
    }
}
