using BepInEx;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilla;

namespace GorillaQOL
{
    [ModdedGamemode]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        private GameObject objectToEnableDisable;

        bool inRoom;
        bool isObjectEnabled = true;
        bool keyi;

        void OnEnable()
        {
            HarmonyPatches.ApplyHarmonyPatches();
            Utilla.Events.GameInitialized += OnGameInitialized;
        }

        void OnDisable()
        {
            HarmonyPatches.RemoveHarmonyPatches();
            Utilla.Events.GameInitialized -= OnGameInitialized;
        }

        void OnGameInitialized(object sender, EventArgs e)
        {
            GameObject.Find("Level/lower level/mirror (1)").SetActive(true);
            GameObject.Find("Level/lower level/mirror (1)/board").SetActive(true);
            GameObject.Find("Level/lower level/mirror (1)/stand").SetActive(true);
            objectToEnableDisable = GameObject.Find("Global/Third Person Camera");
            if (objectToEnableDisable == null)
            {
                Debug.LogError("they probably changed the g to an uncapital one or just changed the name of the camera, pls dm me if broken because yes -brokenstone");
            }
        }

        void Update()
        {
            if (Keyboard.current.iKey.isPressed)
            {
                if(!keyi)
                {
                    isObjectEnabled = !isObjectEnabled;
                    if (objectToEnableDisable != null)
                    {
                        objectToEnableDisable.SetActive(isObjectEnabled);
                    }
                }
                keyi = true;
            }
            else
            {
                keyi = false;
            }
        }

        [ModdedGamemodeJoin]
        public void OnJoin(string gamemode)
        {
            inRoom = true;
            
            GameObject.Find("Level/forest/ForestObjects/slide/Slide").GetComponent<GorillaSurfaceOverride>().overrideIndex = 61;
        }

        [ModdedGamemodeLeave]
        public void OnLeave(string gamemode)
        {
            inRoom = false;
            GameObject.Find("Level/forest/ForestObjects/slide/Slide").GetComponent<GorillaSurfaceOverride>().overrideIndex = 18;
        }
    }
}
