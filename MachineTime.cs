using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MelonLoader;
using ScheduleOne.ObjectScripts;
using UnityEngine;
using HarmonyLib;
using Newtonsoft.Json;
using System.IO;
using MelonLoader.TinyJSON;

namespace FasterMixing
{
    public class MachineTime : MelonMod
    {

        public class Settings
        {
            public int MixTimePerItem { get; set; }
            public bool InstantMixing { get; set; }

        }

        public static Settings Modsettings;

        public override void OnInitializeMelon()
        {
            LoadSettings();
            MelonLogger.Msg("Faster Mixing loaded!");
            var harmony = new HarmonyLib.Harmony("zlatan.FasterMixing");
            harmony.PatchAll();

        }
        private void LoadSettings()
        {
            try
            {
                string settingsPath = Path.Combine(Directory.modDirectory, "configFasterMixing.json");
                if (!File.Exists(settingsPath))
                {
                    var defaultSettings = new Settings { MixTimePerItem = 1, InstantMixing = false };
                    File.WriteAllText(settingsPath, JsonConvert.SerializeObject(defaultSettings, Formatting.Indented));
                    MelonLogger.Msg("No settings file found. Created one with default settings.");
                }

                string json = File.ReadAllText(settingsPath);
                Modsettings = JsonConvert.DeserializeObject<Settings>(json);
            }
            catch (Exception e)
            {
                MelonLogger.Msg($"Error loading the settings from json fiel: {e}");
            }
        }

        [HarmonyPatch(typeof(MixingStation), "GetMixTimeForCurrentOperation")]
        class Patching
        {

            static void Postfix(ref int __result, MixingStation __instance)
            {
                if (__instance.CurrentMixOperation == null)
                {
                    return;
                }
                if (MachineTime.Modsettings.InstantMixing)
                {
                    __result = 1;
                }
                else
                {
                    __result = MachineTime.Modsettings.MixTimePerItem * __instance.CurrentMixOperation.Quantity;
                    MelonLogger.Msg($"Changed mix time to {__result}.");
                }

            }
        }

    }
}
