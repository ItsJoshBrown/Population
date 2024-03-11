# Population

## Overview

The Population plugin is designed to work with [uMod/Oxide](https://umod.org/games/rust) for the Rust game.

This will provide players with essential information about the current in-game time, the number of online players, and the count of sleeping players. This information is accessible via the `/pop` command. 

<img src="https://github.com/ItsJoshBrown/Population/raw/main/img/Population-Screenshot.png" alt="Population Screenshot" width="auto" height="100">


## Features

- Displays the current in-game time
- Shows the count of online players
- Provides information about sleeping players
- Supports permission-based access to different types of information
- Customizable display settings for colors and displayed information

## Installation

1. **Download the `Population.cs` file from the repository.**
2. **Place the downloaded file in your Rust server's root directory under `oxide/plugins`.**
3. **The Plugin should load automatically if not youc an run `o.load Population`. if all else fails you should be able to get it loaded after you restart your Rust server to load the plugin.**

## Permissions

- `Population.use`: Allows players to use the `/pop` command.
- `Population.all`: Grants access to all displayed information.
- `Population.time`: Grants access to view the current time.
- `Population.online`: Grants access to view the number of online players.
- `Population.sleepers`: Grants access to view the count of sleeping players.

## Usage

- Use the `/pop` `/Population` command to display the relevant messages.
- Depending on the permissions granted, players will see different subsets of information, you can allow for All or however many you would like.

## Configuration

The plugin configuration is stored in JSON format and can be found in the `Population.json` file within the `oxide/config` directory. Here's an example configuration snippet:

```json
{
  "CommandSettings": {
    "ChatCommands": [
      "pop",
      "population"
    ]
  },
  "DisplaySettings": {
    "DisplayedInformation": {
      "CurrentTime": true,
      "PlayersOnline": true,
      "Sleepers": true
    },
    "Colors": {
      "Error": "#ED2939",
      "Normal": "#ADD8E6CC"
    }
  }
}
```

## Explanation

**CommandSettings**: Allows customization of the chat commands used to trigger the population information display.

- **ChatCommands**: An array of strings representing the chat commands that can trigger the population display. The default is `/Pop` and `/population`. This can be customized wiht your own command just go to `oxide/plugins/Population.json` and replace the default value with your own.

**DisplaySettings**: Allows customization of the displayed information and colors.

- **DisplayedInformation**: Specifies which information to display.
  - **CurrentTime**: Whether to display the current in-game time. `(true/false)`
  - **PlayersOnline**: Whether to display the count of online players. `(true/false)`
  - **Sleepers**: Whether to display the count of sleeping players. `(true/false)`

### Important 
If the `DisplaySettings` are set to false, it will bypass the permissions and not allow that specific message to populate.

I primarly put this in place so Server-Admins have the ability to disable a specific message, if it's not something they wish to share when someone is using the plugin.


## Support

For any issues or questions regarding the Population plugin, please feel free to reach out.

## License

This is released under the MIT License. See the LICENSE file for more details.
