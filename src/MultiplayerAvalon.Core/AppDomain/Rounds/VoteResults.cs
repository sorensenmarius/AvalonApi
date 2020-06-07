using System;
using System.Collections.Generic;
using System.Text;

namespace MultiplayerAvalon.AppDomain.Rounds
{
    public class VoteResults
    {
        public Guid VoteId { get; set; }
        public Guid RoundId { get; set; }
        public Guid PlayerId { get; set; }
        public bool Vote { get; set; }
    }
}
