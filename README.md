# YTD Texture Analyzer

## Overview
YTD Texture Analyzer is a command-line tool for analyzing and modifying texture dictionary (YTD) files used in Grand Theft Auto V. It allows users to extract texture details such as compression type, resolution, and mipmap levels. Additionally, it includes an option to fix `script_rt` textures by converting them to the correct format (`A8R8G8B8`) and setting the appropriate mipmap levels.

## Features
- Analyzes YTD files and prints texture information.
- Displays texture name, compression format, resolution, mipmaps, and file size.
- Option to fix `script_rt` textures automatically.
- Supports Windows, Linux, and macOS.
- Standalone executable (no .NET runtime required).

## Installation

### Windows
Download the prebuilt executable or compile it yourself:
```sh
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o bin/win-x64
```

### Linux
```sh
dotnet publish -c Release -r linux-x64 --self-contained true -p:PublishSingleFile=true -o bin/linux-x64
```

### macOS
```sh
dotnet publish -c Release -r osx-x64 --self-contained true -p:PublishSingleFile=true -o bin/osx-x64
```

## Usage
Run the program from the command line:
```sh
YtdTextureAnalyzer <ytd_file> [--fix-script-rt]
```

### Options
```
-h, --help          Show this help message
--fix-script-rt     Fix script_rt textures by converting them to A8R8G8B8 format with 1 mipmap
```

### Why Use `--fix-script-rt`?
Using the `--fix-script-rt` option corrects improperly processed `script_rt` textures. This can help prevent warnings and performance issues like:

```
[   5097609] [b2699_GTAProce] ResourcePlacementThr/ ^3Warning: Texture script_rt_dials_race (in txd alEMSalamo.ytd) was set to a compressed texture format, but 'script_rt' textures should always be uncompressed.
[   5097609] [b2699_GTAProce] ResourcePlacementThr/ This file was likely processed by a bad tool. To improve load performance and reduce the risk of it crashing, fix or update the tool used.^7
```

### Example
To analyze a YTD file:
```sh
YtdTextureAnalyzer my_textures.ytd
```

To analyze and fix `script_rt` textures:
```sh
YtdTextureAnalyzer my_textures.ytd --fix-script-rt
```

## Build Instructions
1. Install [.NET SDK](https://dotnet.microsoft.com/download)
2. Clone this repository:
```sh
git clone https://github.com/yourusername/YtdTextureAnalyzer.git
cd YtdTextureAnalyzer
```
3. Build for your platform:
```sh
dotnet build
```
4. Run the program:
```sh
dotnet run <ytd_file>
```

## License
This project is open-source and licensed under the MIT License.

## Contributions
Feel free to submit issues and pull requests to improve the tool!
