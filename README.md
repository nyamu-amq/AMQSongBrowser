# AMQ Song Browser

A lightweight and fast desktop application designed to search and play anime songs used in the game **Anime Music Quiz**.

![Screenshot](https://i.imgur.com/08y8HP7.jpeg)

## Getting Started

### Prerequisites

* **OS:** Windows 10 or later
* **.NET Desktop Runtime 10.0.x** (or higher) may be required.
  * Download the official runtime from the [Microsoft .NET Download Page](https://dotnet.microsoft.com/en-us/download/dotnet/10.0).

### Installation & Run

1. Download the latest ZIP file from the [Releases](https://github.com/nyamu-amq/AMQSongBrowser/releases) page.
2. Extract the archive to your desired location.
3. Run the `AMQSongBrowser.exe` file to start the application.

### Building from Source

1. Clone or download the entire repository.
2. Open the solution file (`.slnx`) using **Visual Studio 2026**.
3. Build and run the project.

## Usage

* On the first launch, the list will be empty. Click the `Update Cache` button to fetch the latest song data from AMQ.
* From the next launch, the application automatically loads the locally saved cache file on startup.
* Type keywords to instantly filter songs by title, artist, anime title, or types.
* Select any song in the list to view its detailed information in a tooltip.
* Double-click a song or press `Enter` to add to playlist the selected song and play immediately.
* Right-click songs in the lists to open a context menu with available actions
* Use the Music Player window to manage queued songs
* The playlist is automatically saved and restored between application launches
* Drag and drop an AMQ exported JSON file onto the playlist to import songs.
* By default, importing a JSON file replaces the current playlist. Hold Ctrl while dropping the file to append the imported songs to the existing playlist instead.

## Thanks to

* [Egerod](https://animemusicquiz.com/) and the AMQ mod team for the game and database.
* [xSardine](https://anisongdb.com/) for the database.
