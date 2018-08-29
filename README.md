# Foundation
-

## Features

Contains some useful, baseline features:
- Common-use tweens for quick, one-off animations
- Event hubs for decoupling events from the responding GameObjects
- Singleton services for maintaining persistent state
- Popup stack

## Building

Visual Studio is the only IDE that is currently supported.

The *.csproj files for the DLLs needed to be modified with wildcard activated:

```
<ItemGroup>
  <Compile Include="Scripts\**\*.cs" />
</ItemGroup>
```

This only appears to work if the source scripts are copied into the project's directory first though.

There's a _Pre-Build_ script that will automatically do this.
