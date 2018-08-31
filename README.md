# Foundation

## Features

Contains some useful, baseline features:
- Common-use tweens for quick, one-off animations
- Event hubs for decoupling events from the responding GameObjects
- Singleton services for maintaining persistent state (deprecated)
- Popup stack

## Building

Visual Studio is the only IDE that is currently supported for building DLLs from source. Both the Debug and Release builds will output the DLLs into the Unity project's _Assets/TRUEStudios_ directory. There's also a convenience menu item for exporting the Unity package after changes have been made to the project template under: _Game_ -> _Export Package_. A convenience menu item has also been added for importing newer versions of the framework under _Game_ -> _Import Package_.

_NOTE_: _Import Package_ will only work in 3rd-party projects if the _Foundation_ framework's project is in the same directory as the 3rd-party project. Furthermore, the framework's project directory name must be named _Foundation_.
