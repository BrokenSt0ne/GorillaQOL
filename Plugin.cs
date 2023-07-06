using BepInEx;
using System;
using System.Reflection;
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
        private GameObject thirdPersonCamera;
        private GameObject waterBalloonBucketAsset;
        private GameObject waterBalloonBucket;

        bool inRoom;
        bool isObjectEnabled = true;
        bool keyPressI;

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
            ImplementWaterBalloons();
            thirdPersonCamera = GameObject.Find("Global/Third Person Camera");
            if (thirdPersonCamera == null)
            {
                Debug.LogError("they probably changed the g to an uncapital one or just changed the name of the camera, pls dm me if broken because yes -brokenstone");
            }
        }

        void ImplementWaterBalloons()
        {
            var Path = Assembly.GetExecutingAssembly().GetManifestResourceStream("GorillaWantWaterBalloons.Resources.waterballoonbucket");
            var Bundle = AssetBundle.LoadFromStream(Path);
            waterBalloonBucketAsset = Bundle.LoadAsset("WaterBalloonBucket") as GameObject;

            waterBalloonBucket = Instantiate(waterBalloonBucketAsset);
            waterBalloonBucket.transform.position = new Vector3(-64.4638f, 2.43f, -58.8753f);

            GameObject Child0 = waterBalloonBucket.transform.GetChild(0).gameObject;
            GameObject Child1 = waterBalloonBucket.transform.GetChild(1).gameObject;
            GameObject Child2 = waterBalloonBucket.transform.GetChild(2).gameObject;

            GorillaSurfaceOverride Override0 = Child0.AddComponent<GorillaSurfaceOverride>();
            Override0.extraVelMultiplier = 1;
            Override0.extraVelMaxMultiplier = 1;
            Override0.overrideIndex = 81;

            GorillaSurfaceOverride Override1 = Child1.AddComponent<GorillaSurfaceOverride>();
            Override1.extraVelMultiplier = 1;
            Override1.extraVelMaxMultiplier = 1;
            Override1.overrideIndex = 32;

            GorillaSurfaceOverride Override2 = Child2.AddComponent<GorillaSurfaceOverride>();
            Override2.extraVelMultiplier = 1;
            Override2.extraVelMaxMultiplier = 1;
            Override2.overrideIndex = 82;

            GameObject LeftSnowball = GameObject.Find("Global/Local VRRig/Local Gorilla Player/Holdables/SnowballLeftAnchor/LMACE.");
            GameObject RightSnowball = GameObject.Find("Global/Local VRRig/Local Gorilla Player/Holdables/SnowballRightAnchor/LMACF.");
            GameObject LeftWaterBalloon = GameObject.Find("Global/Local VRRig/Local Gorilla Player/Holdables/WaterBalloonLeftAnchor/LMAEX.");

            LeftSnowball.GetComponent<SnowballThrowable>().projectilePrefab = LeftWaterBalloon.GetComponent<SnowballThrowable>().projectilePrefab;
            RightSnowball.GetComponent<SnowballThrowable>().projectilePrefab = LeftWaterBalloon.GetComponent<SnowballThrowable>().projectilePrefab;
        }

        void Update()
        {
            if (Keyboard.current.iKey.isPressed)
            {
                if(!keyPressI)
                {
                    isObjectEnabled = !isObjectEnabled;
                    if (thirdPersonCamera != null)
                    {
                        thirdPersonCamera.SetActive(isObjectEnabled);
                    }
                }
                keyPressI = true;
            }
            else
            {
                keyPressI = false;
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
