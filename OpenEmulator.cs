using Playnite.SDK;
using Playnite.SDK.Events;
using Playnite.SDK.Models;
using Playnite.SDK.Plugins;
using System;
using System.Collections.Generic;
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
      /* TODO:
       - Make a hashset of the exes instead of the emulators, or in addition to
       - Display "Launch Ryujinx [ryujinx.exe]", one per exe in emulator installation folder
       - Make subcategories IF there are multiple exes for the emulator
      */

      ICollection<Emulator> emulators = GetEmulatorsOfGames(args.Games);

      foreach (Emulator emulator in emulators)
      {
        yield return new GameMenuItem
        {
          Description = "Open " + emulator.Name,

          Action = (gameArgs) =>
          {
            logger.Info("Launch " + emulator.Name);
            // emulator.BuiltInConfigId
            // emulator.AllProfiles.First().
            PlayniteApi.
          }
        };
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



    public override void OnGameInstalled(OnGameInstalledEventArgs args)
    {
      // Add code to be executed when game is finished installing.
    }

    public override void OnGameStarted(OnGameStartedEventArgs args)
    {
      // Add code to be executed when game is started running.
    }

    public override void OnGameStarting(OnGameStartingEventArgs args)
    {
      // Add code to be executed when game is preparing to be started.
    }

    public override void OnGameStopped(OnGameStoppedEventArgs args)
    {
      // Add code to be executed when game is preparing to be started.
    }

    public override void OnGameUninstalled(OnGameUninstalledEventArgs args)
    {
      // Add code to be executed when game is uninstalled.
    }

    public override void OnApplicationStarted(OnApplicationStartedEventArgs args)
    {
      // Add code to be executed when Playnite is initialized.
    }

    public override void OnApplicationStopped(OnApplicationStoppedEventArgs args)
    {
      // Add code to be executed when Playnite is shutting down.
    }

    public override void OnLibraryUpdated(OnLibraryUpdatedEventArgs args)
    {
      // Add code to be executed when library is updated.
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