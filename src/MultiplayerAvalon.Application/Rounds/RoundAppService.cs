using Abp.Domain.Repositories;
using Microsoft.AspNetCore.SignalR;
using MultiplayerAvalon.AppDomain.Games;
using MultiplayerAvalon.AppDomain.Players;
using MultiplayerAvalon.AppDomain.Rounds;
using MultiplayerAvalon.Games;
using MultiplayerAvalon.Rounds.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MultiplayerAvalon.Rounds
{
    public class RoundAppService : MultiplayerAvalonAppServiceBase, IRoundAppService
    {
        private readonly IHubContext<GameHub> _gameHub;
        public RoundAppService(
            IHubContext<GameHub> gameHub
          )
        {
            _gameHub = gameHub;
        }

        public async Task SetTeam(SetTeamDto model)
        {
            Game g = GameStore.GetGame(model.GameId);
            List<Player> team = new List<Player>();
            g.CurrentRound.CurrentTeam.Clear();
            foreach (Guid playerId in model.CurrentTeam)
            {
                team.Add(g.GetPlayer(playerId));
            }
            g.CurrentRound.CurrentTeam = team;
            GameStore.AddOrUpdateGame(g);
            await _gameHub.Clients.Group(model.GameId.ToString()).SendAsync("UpdateHost");
        }
        public async Task<Round> VoteForTeam(VoteDto model)
        {
            Game g = GameStore.GetGame(model.GameId);
            g.CurrentRound.TeamVote(model.Vote);
            if (g.CurrentRound.TotalTeamVotes >= g.Players.Count)
            {
                await VoteForTeamResults(g);
            } else
            {
                GameStore.AddOrUpdateGame(g);
                await _gameHub.Clients.Group(model.GameId.ToString()).SendAsync("UpdateHost");
            }
            return g.CurrentRound;
        }
        public async Task<Round> ExpeditonVote(VoteDto model)
        {
            Game g = GameStore.GetGame(model.GameId);
            g.CurrentRound.ExpeditionVote(model.Vote);
            if (g.CurrentRound.TotalMissionVotes >= g.CurrentRound.CurrentTeam.Count)
            {
                await ExpeditionResults(g);
            }
            else
            {
                GameStore.AddOrUpdateGame(g);
                await _gameHub.Clients.Group(model.GameId.ToString()).SendAsync("UpdateHost");
            }
            return g.CurrentRound;
        }

        public async Task SetRoundStatus(SetRoundStatusDto model)
        {
            Game g = GameStore.GetGame(model.GameId);
            g.CurrentRound.Status = model.Status;
            if (model.Status == RoundStatus.SelectingTeam)
            {
                g.CurrentRound.VotesForTeam = 0;
                g.CurrentRound.VotesAgainstTeam = 0;
            }
            GameStore.AddOrUpdateGame(g);
            await _gameHub.Clients.Group(model.GameId.ToString()).SendAsync("UpdateAll");
        }
        
        private async Task VoteForTeamResults(Game g)
        {
            var Votesfor = g.CurrentRound.VotesForTeam;
            if (Votesfor <= g.Players.Count() / 2)
            {
                g.CurrentRound.Status = RoundStatus.TeamDenied;
                g.CurrentRound.FailedTeams++;
            } else
            {
                g.CurrentRound.Status = RoundStatus.TeamApproved;
            }
            g.Counter++;
            g.CurrentPlayer = g.Players[g.Counter % g.Players.Count()];
            GameStore.AddOrUpdateGame(g);
            await _gameHub.Clients.Group(g.Id.ToString()).SendAsync("UpdateAll");
        }
        private async Task ExpeditionResults(Game g)
        {
            if (g.CurrentRound.MissionVoteBad > 0)
            {
                g.CurrentRound.Status = RoundStatus.MissionFailed;
                g.PointsEvil++;
            }
            else
            {
                g.CurrentRound.Status = RoundStatus.MissionSuccess;
                g.PointsInnocent++;
            }
            GameStore.AddOrUpdateGame(g);
            await _gameHub.Clients.Group(g.Id.ToString()).SendAsync("UpdateAll");
        }
        public async Task SubmitTeam(GameAndPlayerIdDto model)
        {
            Game g = GameStore.GetGame(model.GameId);
            g.CurrentRound.Status = RoundStatus.VotingForTeam;
            GameStore.AddOrUpdateGame(g);
            await _gameHub.Clients.Group(model.GameId.ToString()).SendAsync("UpdateAll");
        }
        public HowManyPlayerHelper HowManyPlayers(int RoundNmr, int TotalPlayers)
        {
            bool DoubleRound = false;
            int[,] NmrPlayerBasedOnRound = new int[5,6] { { 2, 2, 2, 3, 3, 3 }, { 3, 3, 3, 4, 4, 4 }, { 2, 4, 3, 4, 4, 4 }, { 3, 3, 4, 5, 5, 5 }, { 3, 4, 4, 5, 5, 5 } };
            if(RoundNmr == 4 && TotalPlayers >= 7)
            {
                DoubleRound = true;
            }
            int HowMany = NmrPlayerBasedOnRound[RoundNmr-1, TotalPlayers-5];
            return new HowManyPlayerHelper(HowMany,DoubleRound);
        }
        public class HowManyPlayerHelper
        {
            public int HowMany;
            public bool DoubleRound;
            public HowManyPlayerHelper(int howMany, bool doubleRound)
            {
                HowMany = howMany;
                DoubleRound = doubleRound;
            }
        }

    }
}
