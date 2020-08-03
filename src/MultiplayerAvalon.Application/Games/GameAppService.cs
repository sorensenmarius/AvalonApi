using Abp.Domain.Repositories;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using MultiplayerAvalon.AppDomain.GameRoles;
using MultiplayerAvalon.AppDomain.Games;
using MultiplayerAvalon.AppDomain.Players;
using MultiplayerAvalon.AppDomain.Rounds;
using MultiplayerAvalon.Games.Dto;
using MultiplayerAvalon.Rounds.Dto;
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
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<GameDto> GetAsync(Guid id)
        {
            Game g = await _gameRepository.GetAll()
                .Include("Players")
                .Include("CurrentPlayer")
                .Include("CurrentRound.CurrentTeam")
                .Include("PreviousRounds")
                .FirstOrDefaultAsync(p => p.Id == id);
            return ObjectMapper.Map<GameDto>(g);
        }
        public async Task<GameDto> StartGame(StartGameDto model)
        {
            Game g = _gameRepository.GetAll().Include("Players").FirstOrDefault(game => game.Id == model.Id);
            for (int i = 0; i < g.Players.Count; i++)
            {
                g.Players[i].Order = i;
            }
            g.CurrentRound = new Round(1, g.Players.Count);
            g.CurrentPlayer = g.Players[0];
            g.Status = GameStatus.Playing;
            int evils = GetHowManyEvils(g.Players.Count());
            await AssertRoles(model.Id, model.Roles);
            await _gameRepository.UpdateAsync(g);
            await _gameHub.Clients.Group(g.Id.ToString()).SendAsync("UpdateAll");
            return ObjectMapper.Map<GameDto>(g);
        }
        public async Task GameEnd(Guid id)
        {
            Game g = _gameRepository.GetAll().Include("Players").Include("PreviousRounds").FirstOrDefault(game => game.Id == id);
            g.Status = GameStatus.Ended;
            if(g.PointsEvil < 3)
            {
                bool AssassinGame = false;
                g.Players.ForEach(x =>
                {
                    if (x.RoleId == GameRole.Assassin)
                    {
                        AssassinGame = true;
                    }
                });
                if(AssassinGame)
                {
                    g.Status = GameStatus.AssassinTurn;
                }
            }
            await _gameHub.Clients.Group(g.Id.ToString()).SendAsync("UpdateAll");
        }
        public async Task Assassinate(GameAndPlayerIdDto model)
        {
            Game g = _gameRepository.GetAll().Include("Players").Include("PreviousRounds").FirstOrDefault(game => game.Id == model.GameId);
            Player p = await _playerRepository.GetAsync(model.PlayerId);
            if (p.RoleId == GameRole.Merlin) g.PointsEvil += 100;
            g.Status = GameStatus.Ended;
            await _gameRepository.UpdateAsync(g);
            await _gameHub.Clients.Group(g.Id.ToString()).SendAsync("UpdateAll");
        }
        public async Task<GameDto> AssertRoles(Guid id, List<string> rollene)
        {
            Game g = _gameRepository.GetAll().Include("Players").FirstOrDefault(game => game.Id == id);
            var roles = rollene.Select(int.Parse).ToList();
            roles.ForEach(role =>
            {
                var random = new Random();
                // Random player that is not yet assigned a role
                var notYetChosenPlayers = g.Players.FindAll(p => p.RoleId == GameRole.NotYetChosen);
                var randomPlayer = notYetChosenPlayers[random.Next(notYetChosenPlayers.Count)];
                randomPlayer.RoleId = (GameRole)role;
                if (role >= 4)
                {
                    randomPlayer.IsEvil = true;
                }
                g.Players[g.Players.FindIndex(p => p.Id == randomPlayer.Id)] = randomPlayer;
            });
            var numberOfGood = roles.FindAll(r => r <= 3).Count;
            var numberOfEvil = roles.Count - numberOfGood;
            var numberOfMinions = GetHowManyEvils(g.Players.Count) - numberOfEvil;
            for(int i = 0; i < numberOfMinions; i++)
            {
                var random = new Random();
                // Random player that is not yet assigned a role
                var notYetChosenPlayers = g.Players.FindAll(p => p.RoleId == GameRole.NotYetChosen);
                var randomPlayer = notYetChosenPlayers[random.Next(notYetChosenPlayers.Count)];
                randomPlayer.RoleId = GameRole.Minion;
                randomPlayer.IsEvil = true;
                g.Players[g.Players.FindIndex(p => p.Id == randomPlayer.Id)] = randomPlayer;
            }
            g.Players.ForEach(p =>
            {
                if (p.RoleId == GameRole.NotYetChosen)
                {
                    p.RoleId = GameRole.Servant;
                }
                p.RoleName = p.RoleId.ToString();
                p.RoleInfo = ReturnInfo(g, p.RoleId, p);
            });
            await _gameRepository.UpdateAsync(g);
            return ObjectMapper.Map<GameDto>(g);
        }
        public async Task NextRound(GameAndPlayerIdDto model)
        {
            Game g = _gameRepository.GetAll().Include("CurrentRound").Include("Players").Include("PreviousRounds").FirstOrDefault(game => game.Id == model.GameId);
            if(g.PointsEvil >= 3 || g.PointsInnocent >= 3)
            {
                await GameEnd(model.GameId);
            } else
            {
                g.PreviousRounds.Add(g.CurrentRound);
                g.CurrentRound = new Round(g.PreviousRounds.Count, g.Players.Count);
            }
            await _gameRepository.UpdateAsync(g);
            await _gameHub.Clients.Group(g.Id.ToString()).SendAsync("UpdateAll");
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

        public async Task RemovePlayer(Guid GameId, Guid PlayerId)
        {
            Game g = _gameRepository.GetAll().Include("Players").FirstOrDefault(game => game.Id == GameId);
            await _playerRepository.DeleteAsync(PlayerId);
            g.Players.RemoveAll(p => p.Id == PlayerId);
            await _gameRepository.UpdateAsync(g);
            await _gameHub.Clients.Group(GameId.ToString()).SendAsync("UpdateAll");
        }

        public string ReturnInfo(Game game, GameRole PlayerRole, Player p)
        {
            string RoleInfo = "";
            switch (PlayerRole)
            {
                case GameRole.NotYetChosen:
                    RoleInfo += "We are sorry";
                    break;
                case GameRole.Servant:
                    RoleInfo += "You win if you can complete 3 Missions.";
                    break;
                case GameRole.Minion:
                    RoleInfo += "The Evil players are: " + whoIsEvil(game);
                    break;
                case GameRole.Merlin:
                    RoleInfo += "The Evil players are: ";
                    game.Players.ForEach(x =>
                    {
                        if (x.IsEvil && x.RoleId!=GameRole.Mordred) RoleInfo += x.Name + "   ";
                    });
                    break;
                case GameRole.Percival:
                    RoleInfo += "Merlin is: ";
                    game.Players.ForEach(x =>
                    {
                        if (x.RoleId==GameRole.Merlin||x.RoleId==GameRole.Morgana) RoleInfo += x.Name + " or ";
                    });
                    RoleInfo = RoleInfo.Substring(0, RoleInfo.Length - 4);
                    break;
                case GameRole.Mordred:
                    RoleInfo += "The evil players are: " + whoIsEvil(game);
                    break;
                case GameRole.Morgana:
                    RoleInfo += "Percival sees you as Merlin.|The evil players are: " + whoIsEvil(game);
                    break;
                case GameRole.Oberon:
                    RoleInfo += "You are Evil.";
                    break;
                case GameRole.Assassin:
                    RoleInfo += "If you figure out who Merlin is you win!|The evil players are: " + whoIsEvil(game);
                    break;
            }
            return RoleInfo;
        }
        public string whoIsEvil(Game g)
        {
            string RoleInfo = "";
            g.Players.ForEach(x =>
            {
                if (x.IsEvil && x.RoleId != GameRole.Oberon) RoleInfo += x.Name + " & ";
            });
            RoleInfo = RoleInfo.Substring(0, RoleInfo.Length - 3);
            return RoleInfo;
        }
        public int GetHowManyEvils(int HowManyPlayers)
        {
            if (HowManyPlayers < 5) HowManyPlayers = 5;
            int[,] HowManyGood_Evils = new int[2, 6] { { 3, 4, 4, 5, 6, 6 }, { 2, 2, 3, 3, 3, 4 } };
            return HowManyGood_Evils[1, HowManyPlayers-5];
        }
    }
}
