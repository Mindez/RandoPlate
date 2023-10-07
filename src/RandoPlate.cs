using Kitchen;
using Kitchen.Modules;
using KitchenMods;

using KitchenLib;
using KitchenLib.Event;
using KitchenLib.Logging;
using KitchenLib.Preferences;

using UnityEngine;


/*
 * [ERROR] [KitchenLib] Exception while handling event PreferenceMenu_PauseMenu_CreateSubmenusEvent:
System.MissingFieldException: Field 'KitchenLib.Event.PreferenceMenu_CreateSubmenusArgs`1<Kitchen.PauseMenuAction>.Menus' not found.
  at KitchenLib.Utils.EventUtils.InvokeEvent[T] (System.String name, System.Collections.Generic.IEnumerable`1[T] handlers, System.Object sender, T args) [0x00023] in <9d169105632641b58df104076ab3e56b>:0 
 */

namespace RandoPlate {
    // --- Menu Definitions ---
    public class RandoMenu<T> : KLMenu<T> {
        public RandoMenu(Transform container, ModuleList module_list) : base(container, module_list) { }

        private Option<bool> testBool =
            new Option<bool> (
                new List<bool> { true, false },
                RandoPlate.randoPreferenceManager.GetPreference<PreferenceBool>("TestBool").Get(),
                new List<string> { "Enabled", "Disabled" }
            );

        public override void Setup(int player_id) {
            //base.Setup(player_id);
            New<SpacerElement>(true);
            AddLabel("Test Label Please Ignore");
            AddSelect<bool>(testBool);
            testBool.OnChanged += delegate (object? _, bool result) {
                RandoPlate.randoPreferenceManager.GetPreference<PreferenceBool>("TestBool").Set(result);
            };
            New<SpacerElement>(true);

            AddButton(base.Localisation["MENU_BACK_SETTINGS"], delegate (int i) {
                RandoPlate.randoPreferenceManager.Save();
                this.RequestPreviousMenu();
            }, 0, 1f, 0.2f);
        }
    }


    public class RandoPlate : GenericSystemBase, IModSystem
    {
        private const string MOD_ID = "randoplate";
        private const string MOD_NAME = "RandoPlate";
        //private const string MOD_AUTHOR = "Mindez";
        //private const string MOD_VERSION = "0.0.1";
        //private const string MOD_COMPATIBLE_VERSIONS = ">=1.7.0";

        private static KitchenLogger logger = new KitchenLogger(MOD_NAME);
        public static PreferenceManager randoPreferenceManager = new PreferenceManager(MOD_ID);

        protected override void Initialise() {
            base.Initialise();
            logger.LogInfo("Initialising...");

            // --- Create PreferenceManager for menu settings
            randoPreferenceManager.RegisterPreference(new PreferenceBool("TestBool", false));
            randoPreferenceManager.Load();

            // --- Register & Add Menu To Pause Menu ---
            ModsPreferencesMenu<PauseMenuAction>.RegisterMenu(MOD_NAME, typeof(RandoMenu<PauseMenuAction>), typeof(PauseMenuAction));
            Events.PreferenceMenu_PauseMenu_CreateSubmenusEvent += (object? s, PreferenceMenu_CreateSubmenusArgs<PauseMenuAction> args) => {
                args.Menus.Add(typeof(RandoMenu<PauseMenuAction>), new RandoMenu<PauseMenuAction>(args.Container, args.Module_list));
            };

            // --- Register & Add Menu to Main Menu ---
            ModsPreferencesMenu<MainMenuAction>.RegisterMenu(MOD_NAME, typeof(RandoMenu<MainMenuAction>), typeof(MainMenuAction));
            Events.PreferenceMenu_MainMenu_CreateSubmenusEvent += (s, args) => {
                args.Menus.Add(typeof(RandoMenu<MainMenuAction>), new RandoMenu<MainMenuAction>(args.Container, args.Module_list));
            };

            logger.LogInfo("Initialisation Complete!");
        }

        protected override void OnUpdate() {
        }
    }
}