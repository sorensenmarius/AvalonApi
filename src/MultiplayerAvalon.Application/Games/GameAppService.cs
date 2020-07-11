using Abp.Domain.Repositories;
using Microsoft.AspNetCore.SignalR;
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
        private readonly IRepository<Player, Guid> _playerRepository;
        private readonly IHubContext<GameHub> _gameHub;
        private int NmrEvils = 0;
        public GameAppService(
            IRepository<Game, Guid> gameRepository, 
            IRepository<Player, Guid> playerRepository,
            IHubContext<GameHub> gameHub
            )
        {
            _gameRepository = gameRepository;
            _playerRepository = playerRepository;
            _gameHub = gameHub;
        }
        public async Task<GameDto> CreateAsync()
        {
            Game g = new Game();
            await _gameRepository.InsertAsync(g);
            return ObjectMapper.Map<GameDto>(g);
        }
        /// <summary>
        /// Gets a game by ID, populated with players
        /// </summary>
        /// TODO - Refactor this to a stored procedure in the DB
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<GameDto> GetAsync(Guid id)
        {
            Game g = await _gameRepository.GetAll()
                .Include("Players")
                .Include("CurrentPlayer")
                .Include("CurrentRound.CurrentTeam")
                .FirstOrDefaultAsync(p => p.Id == id);
            return ObjectMapper.Map<GameDto>(g);
        }
        public async Task<GameDto> StartGame(StartGameDto model)
        {
            List<Game> game = await _gameRepository.GetAllIncluding(game => game.Players).ToListAsync();
            Game g = game.Find(item => item.Id == model.Id);
            Round r = await CreateNewRound();
            g.CurrentRound = r;
            g.CurrentPlayer = g.Players[0];
            g.Status = GameStatus.Playing;
            int evils = GetHowManyEvils(g.Players.Count());
            await AssertRoles(model.Id, model.Roles, evils-model.Minions);
            await _gameRepository.UpdateAsync(g);
            await _gameHub.Clients.Group(g.Id.ToString()).SendAsync("GameUpdated");
            return ObjectMapper.Map<GameDto>(g);
        }
        public async Task<GameDto> GameEnd(Guid id)
        {
            List<Game> game = await _gameRepository.GetAllIncluding(game => game.Players).ToListAsync();
            Game g = game.Find(item => item.Id == id);
            if(g.PointsEvil == 3)
            {
                
                g.Status = GameStatus.EvilWin;
            }
            else if(g.PointsInnocent == 3)
            {
                bool AssassinGame = false;
                g.Players.ForEach(x =>
                {
                    if (x.RoleId == GameRole.Assassin)
                    {
                        AssassinGame = true;
                    }
                });
                if (AssassinGame) g.Status = GameStatus.AssassinTurn;
                else g.Status = GameStatus.GoodWin;
            }
            return ObjectMapper.Map<GameDto>(g);
        }
        public async Task Assassinate(Guid GameId, Guid PlayerId)
        {
            List<Game> game = await _gameRepository.GetAllIncluding(game => game.Players).ToListAsync();
            Game g = game.Find(item => item.Id == GameId);
            Player p = await _playerRepository.GetAsync(PlayerId);
            if (p.RoleId == GameRole.Merlin) g.Status = GameStatus.EvilWin;
        }
        public async Task<GameDto> AssertRoles(Guid id, List<string> rollene,int minions)
        {
            List<Game> game = await _gameRepository.GetAllIncluding(game => game.Players).ToListAsync();
            Game g = game.Find(item => item.Id == id);
            while (rollene.Count() < g.Players.Count())
            {
                while (minions > 0) 
                {
                    rollene.Add("4");
                    minions--;
                    System.Diagnostics.Debug.WriteLine("Adding minion");
                }
                if(rollene.Count() < g.Players.Count())
                {
                    rollene.Add("1");
                    System.Diagnostics.Debug.WriteLine("Adding ¨Servant");
                } 
            }
            List<string> roles = ShuffleNew(rollene);
            List<GameRole> gameroles = new List<GameRole>();
            roles.ForEach(x => gameroles.Add((GameRole)Enum.Parse(typeof(GameRole), x.ToString())));
            int x = 0;
            foreach (Player player in g.Players)
            {
                player.RoleId = gameroles[x];
                player.RoleName = gameroles[x].ToString();
                if (player.RoleId == GameRole.Minion || player.RoleId == GameRole.Assassin || player.RoleId == GameRole.Mordred || player.RoleId == GameRole.Morgana || player.RoleId == GameRole.Oberon) player.IsEvil = true;
                else player.IsEvil = false;
                x++;
            }
            g.Players.ForEach(async x => x.RoleInfo = await ReturnInfo(g,x.RoleId,x));
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
        public async Task<string> ReturnInfo(Game game, GameRole PlayerRole,Player p)
        {
            string RoleInfo = "Your role is: " + PlayerRole.ToString();
            switch (PlayerRole)
            {
                case GameRole.NotYetChosen:
                    RoleInfo += ". We are sorry";
                    break;
                case GameRole.Servant:
                    RoleInfo += ". You win if you can complete 3 Missions.";
                    break;
                case GameRole.Minion:
                    RoleInfo += ". The Evil players are: " + whoIsEvil(game);
                    break;
                case GameRole.Merlin:
                    RoleInfo += ". The Evil players are: ";
                    game.Players.ForEach(x =>
                    {
                        if (x.IsEvil && x.RoleId!=GameRole.Mordred) RoleInfo += x.Name + "   ";
                    });
                    break;
                case GameRole.Percival:
                    RoleInfo += ". Protect: ";
                    game.Players.ForEach(x =>
                    {
                        if (x.RoleId==GameRole.Merlin||x.RoleId==GameRole.Morgana) RoleInfo += x.Name + "   ";
                    });
                    break;
                case GameRole.Mordred:
                    RoleInfo += ". The Evil players are: " + whoIsEvil(game);
                    break;
                case GameRole.Morgana:
                    RoleInfo += "Percival thinks you can be Merlin. The evil players are: " + whoIsEvil(game);
                    break;
                case GameRole.Oberon:
                    RoleInfo += ". You are Evil.";
                    break;
                case GameRole.Assassin:
                    RoleInfo += ". If you figure out who Merlin is you win! " + whoIsEvil(game);
                    break;
            }
            return RoleInfo;
        }
        public string whoIsEvil(Game g)
        {
            string RoleInfo = "";
            g.Players.ForEach(x =>
            {
                if (x.IsEvil && x.RoleId != GameRole.Oberon) RoleInfo += x.RoleId.ToString() + ": " + x.Name + "    ";
            });
            return RoleInfo;
        }
        public int GetHowManyEvils(int HowManyPlayers)
        {
            int[,] HowManyGood_Evils = new int[2, 6] { { 3, 4, 4, 5, 6, 6 }, { 2, 2, 3, 3, 3, 4 } };
            return HowManyGood_Evils[1, HowManyPlayers-5];
        }
    }
}
