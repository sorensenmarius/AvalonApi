using System.Collections.Generic;

namespace MultiplayerAvalon.Authentication.External
{
    public interface IExternalAuthConfiguration
    {
        List<ExternalLoginProviderInfo> Providers { get; }
    }
}
