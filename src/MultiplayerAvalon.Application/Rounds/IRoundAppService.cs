using MultiplayerAvalon.AppDomain.Players;
using MultiplayerAvalon.Games.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerAvalon.Rounds
{
    public interface IRoundAppService
    {
        public Task VoteForTeam(Guid GameId, Guid PlayerId, bool Vote);
        public Task ExpeditonVote(Guid GameId, Guid PlayerId, bool Vote);
        public Task VoteForTeamResults(Guid GameId);
        public Task ExpeditionResults(Guid GameId);
        public Task AddPlayerToTeam(Guid PlayerId, Guid GameId);
        public Task RemovePlayerFromTeam(Guid PlayerId, Guid GameId);
    }
}
