using System;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Server;
using PlayerRoles;

namespace RFF
{
    public class RoundFriendlyFire : Plugin<Config>
    {
        public override string Author => "MrDarvi";
        public override string Name => "RoundFriendlyFire";
        public override string Prefix => "RFF";
        public override Version Version => new Version(1, 0, 0);
        public override Version RequiredExiledVersion => new Version(9, 6, 1);

        public static RoundFriendlyFire Instance;
        private bool _ffEnabled;

        public override void OnEnabled()
        {
            Instance = this;
            Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnded;
            Exiled.Events.Handlers.Server.RestartingRound += OnRestartingRound;
            Log.Info($"{Name} v{Version} by {Author} uploaded!");
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnded;
            Exiled.Events.Handlers.Server.RestartingRound -= OnRestartingRound;
            Instance = null;
            base.OnDisabled();
        }

        private void OnRestartingRound()
        {
            if (_ffEnabled)
            {
                Server.FriendlyFire = false;
                _ffEnabled = false;
            }
        }

        private void OnRoundEnded(RoundEndedEventArgs ev)
        {
            try
            {
                // Проверяем, что остался только один тип игроков
                var aliveTeams = Player.List
                    .Where(p => p.IsAlive)
                    .Select(p => p.Role.Team)
                    .Distinct()
                    .Count();

                if (aliveTeams == 1)
                {
                    Server.FriendlyFire = true;
                    _ffEnabled = true;

                    // Отправляем сообщение всем игрокам
                    Map.Broadcast(
                        Config.MessageDuration,
                        Config.FFEnabledMessage,
                        Broadcast.BroadcastFlags.Normal
                    );

                    Log.Info("FriendlyFire is on! Only one team remains.");
                }
            }
            catch (Exception e)
            {
                Log.Error($"Error when processing the end of the round: {e}");
            }
        }
    }
}