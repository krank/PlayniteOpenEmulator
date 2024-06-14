﻿using Playnite.SDK;
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

    public void Test(GetGameMenuItemsArgs args)
    {

    }

    public override IEnumerable<GameMenuItem> GetGameMenuItems(GetGameMenuItemsArgs args)
    {
      if (args.Games.Count == 1)
      {
        yield return new GameMenuItem
        {
          Description = "Open emulator",

          Action = (args2) =>
          {
            // use args.Games to get list of games attached to the menu source
            Console.WriteLine("Invoked from game menu item!");
            logger.Info("Games!");
          }
        };
      }
      // else yield return null;
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