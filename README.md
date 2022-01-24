# Herbhiker
A C# bot for Wotlk 3.3.5 build 12340 which gather herbs. Only in flyable zones (Outland or Northrend).

# How it works
It requires a pre-definied path of points which is stored in a seperate text file. The path is loaded into the bot and then it will fly point to point. While follwing the path it reads the nearby herbs from minimap and searches for a matching GUID in the object manager list. If a matching GUID is found then we read its X, Y and Z coordinates and fly towards it for pickup.

The bot is meant to fly under the map, which is possible through noclip. When moving towards a herb it will move directly under it and then proceed to fly above ground. While above ground noclip is turned off and then we start to get grounded. While (hopefully) grounded we write loot action to CTM. If we take any damage during this process we abort and blacklist the herb GUID for the future. If we still failed to loot it (maybe we was not grounded) it will fly down and try again. If very unlucky this can become a loop. Otherwise when pickup succeeded we go back down and follow path again.

## Generator
The app also have functionality for creating a path by flying in-game and manually placing points by hitting `LSHIFT + E`. This path can then be saved to text file.
