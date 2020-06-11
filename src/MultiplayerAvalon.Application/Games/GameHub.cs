using Microsoft.AspNetCore.SignalR;
using MultiplayerAvalon.AppDomain.Games;
using System;
using System.Threading.Tasks;

namespace MultiplayerAvalon.Games
{
    public class GameHub : Hub
    {
        public async Task SendMessage(Guid userId, string message)
        {
            System.Diagnostics.Debug.WriteLine(message);
            await Clients.All.SendAsync("RecieveMessage", userId, message);
        }

        /// <summary>
        /// Adds the player to the correct group and notifies other players that a player has been added
        /// </summary>
        /// <param name="gameId">Newly joined users id</param>
        /// <returns></returns>
        public async Task JoinGameGroup(Guid gameId)
        {
            string gId = gameId.ToString();
            await Groups.AddToGroupAsync(Context.ConnectionId, gId);
            await Clients.OthersInGroup(gId).SendAsync("GameUpdated");
        }
    }
}
