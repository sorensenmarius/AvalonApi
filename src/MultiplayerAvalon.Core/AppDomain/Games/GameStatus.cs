﻿namespace MultiplayerAvalon.AppDomain.Games
{
    public enum GameStatus
    {
        WaitingForPlayers = 0,
        Playing = 1,
        AssassinTurn=2,
        GoodWin = 3,
        EvilWin = 4,
        Ended = -1
    }
}
