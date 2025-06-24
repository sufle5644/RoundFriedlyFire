using System.ComponentModel;
using Exiled.API.Interfaces;

namespace RFF
{
    public class Config : IConfig
    {
        [Description("Is the plugin enabled")]
        public bool IsEnabled { get; set; } = true;

        [Description("Debugging mode")]
        public bool Debug { get; set; } = false;

        [Description("A message when turning on FriendlyFire")]
        public string FFEnabledMessage { get; set; } = "<color=#FFFFFF><b>DAMAGE TO YOUR INCLUDED</b></color>";

        [Description("Message duration (seconds)")]
        public ushort MessageDuration { get; set; } = 10;
    }
}