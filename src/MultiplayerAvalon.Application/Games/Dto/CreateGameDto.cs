using Abp.AutoMapper;
using MultiplayerAvalon.AppDomain.Games;
using System;

namespace MultiplayerAvalon.Games.Dto
{
    [AutoMapFrom(typeof(Game))]
    public class CreateGameDto
    {
        public DateTime CreationTime {get; set;}
    }
}
