using Playnite.SDK;
using Playnite.SDK.Events;
using Playnite.SDK.Models;
using Playnite.SDK.Plugins;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace OpenEmulator
{
  public class OpenEmulator : GenericPlugin
  {
    private static readonly ILogger logger = LogManager.GetLogger();

    private OpenEmulatorSettingsViewModel settings { get; set; }

    public override Guid Id { get; } = Guid.Parse("031c26f0-0358-4503-aaf1-f8af851212bf");

    class EmulatorRecord : IEquatable<EmulatorRecord>
    {
      public string Name { get; set; }
      public string InstallDir { get; set; }

      public bool Equals(EmulatorRecord other)
      {
        return other != null &&
          Name == other.Name &&
          InstallDir == other.InstallDir;
      }

      public override int GetHashCode()
      {
        return HashCode.Combine(Name, InstallDir);
      }
    }

    public OpenEmulator(IPlayniteAPI api) : base(api)
    {
      settings = new OpenEmulatorSettingsViewModel(this);
      Properties = new GenericPluginProperties
      {
        HasSettings = true
      };
    }

    public override IEnumerable<GameMenuItem> GetGameMenuItems(GetGameMenuItemsArgs args)
    {
      string openString = ResourceProvider.GetString("LOCOpenEmulatorOpen");

      ICollection<EmulatorRecord> emulators = GetEmulatorsOfGames(args.Games);

      foreach (EmulatorRecord emulatorRecord in emulators)
      {
        string[] exeFiles = Directory.GetFiles(emulatorRecord.InstallDir, "*.exe");


        foreach (string exeFile in exeFiles)
        {
          yield return new GameMenuItem
          {
            MenuSection = exeFiles.Length > 1 ? $"{openString} {emulatorRecord.Name}" : "",

            Description = exeFiles.Length > 1 ? Path.GetFileName(exeFile) :
               $"{openString} {emulatorRecord.Name} [{Path.GetFileName(exeFile)}]",

            Action = (gameArgs) =>
            {
              ProcessStartInfo processInfo = new ProcessStartInfo
              {
                WorkingDirectory = emulatorRecord.InstallDir,
                FileName = exeFile
              };

              Process.Start(processInfo);
            }
          };
        }
      }
    }

    private ICollection<EmulatorRecord> GetEmulatorsOfGames(List<Game> games)
    {

      // Get all emulators for quick (?) lookup)
      Dictionary<System.Guid, Emulator> playniteEmulators = new Dictionary<Guid, Emulator>();

      foreach (Emulator emulator in PlayniteApi.Database.Emulators)
      {
        playniteEmulators.Add(emulator.Id, emulator);
      }


      HashSet<EmulatorRecord> emulators = new HashSet<EmulatorRecord>();

      // Go through all games & actions
      foreach (Game game in games)
      {
        foreach (GameAction action in game.GameActions)
        {
          if (action.Type != GameActionType.Emulator) break;
          Emulator emulator = playniteEmulators[action.EmulatorId];

          // Create record & add it
          EmulatorRecord record = new EmulatorRecord()
          {
            Name = emulator.Name,
            InstallDir = PlayniteApi.ExpandGameVariables(game, emulator.InstallDir)
          };

          emulators.Add(record);
        }
      }

      return emulators;
    }

    public override ISettings GetSettings(bool firstRunSettings)
    {
      return settings;
    }

    public override UserControl GetSettingsView(bool firstRunSettings)
    {
      return new OpenEmulatorSettingsView();
    }
  }
}