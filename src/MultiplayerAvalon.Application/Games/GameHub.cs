using Microsoft.AspNetCore.SignalR;
using MultiplayerAvalon.AppDomain.Games;
using System;
using System.Threading.Tasks;

namespace MultiplayerAvalon.Games
{
    public class GameHub : Hub
    {
        /// <summary>
        /// Adds the player to the correct group and notifies other players that a player has been added
        /// </summary>
        /// <param name="gameId">Newly joined users id</param>
        /// <returns></returns>
        public async Task JoinAllGroup(Guid gameId)
        {
            string gId = gameId.ToString();
            await Groups.AddToGroupAsync(Context.ConnectionId, gId);
            await Clients.Group(gId).SendAsync("UpdateAll");
        }

        public async Task JoinHostGroup(Guid gameId)
        {
            string gId = gameId.ToString();
            await Groups.AddToGroupAsync(Context.ConnectionId, "H-" + gId);
        }
    }
}
