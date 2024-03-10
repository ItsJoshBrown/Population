using System.Collections.Generic;
using Newtonsoft.Json;
using Oxide.Core;
using Oxide.Core.Configuration;
using Oxide.Core.Libraries.Covalence;

namespace Oxide.Plugins
{
    [Info("Population", "Jdam", "1.0.0")]
    [Description("Population plugin displays the current in-game time, count of online players, and sleepers when someone types /pop.")]

    public class Population : CovalencePlugin
    {
        private Configuration config;
        private readonly HashSet<string> registeredCommands = new HashSet<string>();

        private class Configuration
        {
            public CommandSettings CommandSettings { get; set; } = new CommandSettings();
            public DisplaySettings DisplaySettings { get; set; } = new DisplaySettings();
        }

        private class CommandSettings
        {
            public List<string> ChatCommands { get; set; } = new List<string> { "pop", "population" };
        }

        private class DisplaySettings
        {
            public DisplayedInformation DisplayedInformation { get; set; } = new DisplayedInformation();
            public Colors Colors { get; set; } = new Colors();
        }

        private class DisplayedInformation
        {
            public bool CurrentTime { get; set; } = true;
            public bool PlayersOnline { get; set; } = true;
            public bool Sleepers { get; set; } = true;
        }

        private class Colors
        {
            public string Error { get; set; } = "#ED2939";
            public string Normal { get; set; } = "#ADD8E6CC";
        }

        protected override void LoadDefaultConfig()
        {
            config = Config.ReadObject<Configuration>() ?? new Configuration();

            // Ensure default commands are added if not present
            foreach (var command in GetDefaultCommands())
            {
                if (!config.CommandSettings.ChatCommands.Contains(command))
                {
                    config.CommandSettings.ChatCommands.Add(command);
                }
            }

            SaveConfig();
        }

        private List<string> GetDefaultCommands()
        {
            // Define your default commands here
            return new List<string> { "pop", "population" };
        }

        private void LoadConfigData()
        {
            config = Config.ReadObject<Configuration>() ?? new Configuration();
        }

        private void SaveConfig()
        {
            Config.WriteObject(config, true);
        }

        private void Init()
        {
            LoadConfigData();
            RegisterCommands();
            RegisterPermissions();
        }

        private void RegisterCommands()
        {
            foreach (var command in config.CommandSettings.ChatCommands)
            {
                if (!registeredCommands.Contains(command))
                {
                    AddCovalenceCommand(command, "PopCommand");
                    registeredCommands.Add(command);
                }
            }
        }

        private void RegisterPermissions()
        {
            permission.RegisterPermission("Population.use", this);
            permission.RegisterPermission("Population.all", this);
            permission.RegisterPermission("Population.time", this);
            permission.RegisterPermission("Population.online", this);
            permission.RegisterPermission("Population.sleepers", this);
        }

        private void PopCommand(IPlayer player, string command, string[] args)
        {
            if (!player.HasPermission("Population.use"))
            {
                player.Reply($"<color={config.DisplaySettings.Colors.Error}>You don't have permission to use this command.</color>");
                return;
            }

            var message = ConstructMessage(player);

            if (string.IsNullOrEmpty(message))
            {
                player.Reply($"<color={config.DisplaySettings.Colors.Normal}>No information to display.</color>");
            }
            else
            {
                player.Reply(message.Trim());
            }
        }

        private string ConstructMessage(IPlayer player)
        {
            var message = "";

            if (player.HasPermission("Population.all"))
            {
                if (config.DisplaySettings.DisplayedInformation.CurrentTime)
                    message += $"<color={config.DisplaySettings.Colors.Normal}>Current Time:</color> {TOD_Sky.Instance.Cycle.DateTime:HH:mm:ss} | ";

                if (config.DisplaySettings.DisplayedInformation.PlayersOnline)
                    message += $"<color={config.DisplaySettings.Colors.Normal}>Players Online:</color> {BasePlayer.activePlayerList.Count} | ";

                if (config.DisplaySettings.DisplayedInformation.Sleepers)
                    message += $"<color={config.DisplaySettings.Colors.Normal}>Sleepers:</color> {CountSleepingPlayers()} ";
            }
            else
            {
                if (config.DisplaySettings.DisplayedInformation.CurrentTime && player.HasPermission("Population.time"))
                    message += $"<color={config.DisplaySettings.Colors.Normal}>Current Time:</color> {TOD_Sky.Instance.Cycle.DateTime:HH:mm:ss} | ";

                if (config.DisplaySettings.DisplayedInformation.PlayersOnline && player.HasPermission("Population.online"))
                    message += $"<color={config.DisplaySettings.Colors.Normal}>Players Online:</color> {BasePlayer.activePlayerList.Count} | ";

                if (config.DisplaySettings.DisplayedInformation.Sleepers && player.HasPermission("Population.sleepers"))
                    message += $"<color={config.DisplaySettings.Colors.Normal}>Sleepers:</color> {CountSleepingPlayers()} ";
            }

        // Trim the trailing "|" character if the message is not empty
        if (!string.IsNullOrEmpty(message))
        {
            message = message.TrimEnd(' ', '|');
        }

        return message;
}

        private int CountSleepingPlayers()
        {
            int sleepingPlayers = 0;
            foreach (var playerObject in BasePlayer.sleepingPlayerList)
            {
                if (playerObject is BasePlayer)
                    sleepingPlayers++;
            }
            return sleepingPlayers;
        }
    }
}
