using Playnite.SDK;
using Playnite.SDK.Events;
using Playnite.SDK.Models;
using Playnite.SDK.Plugins;
using System;
using System.Collections.Generic;
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
      string OpenString = ResourceProvider.GetString("LOCOpenEmulatorOpen");

      ICollection<Emulator> emulators = GetEmulatorsOfGames(args.Games);

      foreach (Emulator emulator in emulators)
      {
        string[] exeFiles = Directory.GetFiles(emulator.InstallDir, "*.exe");

        foreach (string exeFile in exeFiles)
        {
          yield return new GameMenuItem
          {
            MenuSection = exeFiles.Length > 1 ? $"{OpenString} {emulator.Name}" : "",

            Description = exeFiles.Length > 1 ? Path.GetFileName(exeFile) :
               $"{OpenString} {emulator.Name} [{Path.GetFileName(exeFile)}]",

            Action = (gameArgs) =>
            {
              Process.Start(exeFile);
            }
          };
        }
      }
    }

    private ICollection<Emulator> GetEmulatorsOfGames(List<Game> games)
    {
      HashSet<System.Guid> emulatorGuids = new HashSet<Guid>();

      foreach (Game game in games)
      {
        foreach (GameAction action in game.GameActions)
        {
          if (action.Type != GameActionType.Emulator) break;
          emulatorGuids.Add(action.EmulatorId);
        }
      }

      HashSet<Emulator> emulators = new HashSet<Emulator>();

      foreach (Emulator emulator in PlayniteApi.Database.Emulators)
      {
        if (emulatorGuids.Contains(emulator.Id))
        {
          emulators.Add(emulator);
          emulatorGuids.Remove(emulator.Id);
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