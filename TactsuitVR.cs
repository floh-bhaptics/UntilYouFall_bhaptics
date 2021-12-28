using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Bhaptics.Tact;
using MelonLoader;
using UnityEngine;

namespace TactsuitUntilYouFall
{
    public class TactsuitVR
    {
        public bool suitDisabled = true;

        public bool rightArmCollisionDisabled = false;
        public bool leftArmCollisionDisabled = false;

        public bool leftHandClimbing = false;
        public bool rightHandClimbing = false;

        public bool leftHandPowerPuncher = false;
        public bool rightHandPowerPuncher = false;

        public bool leftHandGravityPlateActivated = false;
        public bool rightHandGravityPlateActivated = false;
        public bool bothHandsGravityPlateActivated = false;

        public bool leftHandBoardGun = false;
        public bool rightHandBoardGun = false;

        public Vector3 lastFiredHandPosition = Vector3.zeroVector;
        public string lastFiredGunName = "";


        public TactsuitVR()
        {
            FillFeedbackList();
        }

        public void LOG(string logStr)
        {
            MelonLogger.Msg(logStr);
        }

        private void FillFeedbackList()
        {
            feedbackMap.Clear();
            feedbackMap.Add(FeedbackType.HeartBeat, new Feedback(FeedbackType.HeartBeat, "HeartBeat", 0));
            feedbackMap.Add(FeedbackType.DashForward, new Feedback(FeedbackType.DashForward, "DashForward", 0));
            feedbackMap.Add(FeedbackType.SummonWeapon_R, new Feedback(FeedbackType.SummonWeapon_R, "SummonWeapon_R", 0));
            feedbackMap.Add(FeedbackType.SummonWeapon_L, new Feedback(FeedbackType.SummonWeapon_L, "SummonWeapon_L", 0));
            feedbackMap.Add(FeedbackType.Block_L, new Feedback(FeedbackType.Block_L, "Block_L", 0));
            feedbackMap.Add(FeedbackType.Block_R, new Feedback(FeedbackType.Block_R, "Block_R", 0));
            feedbackMap.Add(FeedbackType.CrushCrystalHands_L, new Feedback(FeedbackType.CrushCrystalHands_L, "CrushCrystalHands_L", 0));
            feedbackMap.Add(FeedbackType.CrushCrystalHands_R, new Feedback(FeedbackType.CrushCrystalHands_R, "CrushCrystalHands_R", 0));
            feedbackMap.Add(FeedbackType.SummonWeaponHands_R, new Feedback(FeedbackType.SummonWeaponHands_R, "SummonWeaponHands_R", 0));
            feedbackMap.Add(FeedbackType.SummonWeaponHands_L, new Feedback(FeedbackType.SummonWeaponHands_L, "SummonWeaponHands_L", 0));
            feedbackMap.Add(FeedbackType.BlockHands_L, new Feedback(FeedbackType.BlockHands_L, "BlockHands_L", 0));
            feedbackMap.Add(FeedbackType.BlockHands_R, new Feedback(FeedbackType.BlockHands_R, "BlockHands_R", 0));
            feedbackMap.Add(FeedbackType.CrushCrystal_L, new Feedback(FeedbackType.CrushCrystal_L, "CrushCrystal_L", 0));
            feedbackMap.Add(FeedbackType.CrushCrystal_R, new Feedback(FeedbackType.CrushCrystal_R, "CrushCrystal_R", 0));
            feedbackMap.Add(FeedbackType.HitByHammer, new Feedback(FeedbackType.HitByHammer, "HitByHammer", 0));
            feedbackMap.Add(FeedbackType.KnockBack, new Feedback(FeedbackType.KnockBack, "KnockBack", 0));
            feedbackMap.Add(FeedbackType.BlockVest_L, new Feedback(FeedbackType.BlockVest_L, "BlockVest_L", 0));
            feedbackMap.Add(FeedbackType.BlockVest_R, new Feedback(FeedbackType.BlockVest_R, "BlockVest_R", 0));
            feedbackMap.Add(FeedbackType.RiseAgain, new Feedback(FeedbackType.RiseAgain, "RiseAgain", 0));
            feedbackMap.Add(FeedbackType.SlashDefault, new Feedback(FeedbackType.SlashDefault, "SlashDefault", 0));
            feedbackMap.Add(FeedbackType.CrystalCrushed_L, new Feedback(FeedbackType.CrystalCrushed_L, "CrystalCrushed_L", 0));
            feedbackMap.Add(FeedbackType.CrystalCrushed_R, new Feedback(FeedbackType.CrystalCrushed_R, "CrystalCrushed_R", 0));
            feedbackMap.Add(FeedbackType.GroundAttack, new Feedback(FeedbackType.GroundAttack, "GroundAttack", 0));
            feedbackMap.Add(FeedbackType.Heal, new Feedback(FeedbackType.Heal, "Heal", 0));
            feedbackMap.Add(FeedbackType.ActivateSuper_L, new Feedback(FeedbackType.ActivateSuper_L, "ActivateSuper_L", 0));
            feedbackMap.Add(FeedbackType.ActivateSuper_R, new Feedback(FeedbackType.ActivateSuper_R, "ActivateSuper_R", 0));
            feedbackMap.Add(FeedbackType.Premonition, new Feedback(FeedbackType.Premonition, "Premonition", 0));
            feedbackMap.Add(FeedbackType.Bulwark, new Feedback(FeedbackType.Bulwark, "Bulwark", 0));
        }

        public bool systemInitialized = false;
        //public MelonLoader.bHaptics hapticPlayer;
        public HapticPlayer hapticPlayer;

        Dictionary<FeedbackType, Feedback> feedbackMap = new Dictionary<FeedbackType, Feedback>();

        [Flags]
        public enum FeedbackType
        {
            DashForward,
            HeartBeat,
            SummonWeapon_R,
            SummonWeapon_L,
            SummonWeaponHands_L,
            SummonWeaponHands_R,
            Block_L,
            Block_R,
            BlockHands_L,
            BlockHands_R,
            CrushCrystal_L,
            CrushCrystal_R,
            CrushCrystalHands_L,
            CrushCrystalHands_R,
            HitByHammer,
            KnockBack,
            BlockVest_L,
            BlockVest_R,
            RiseAgain,
            SlashDefault,
            CrystalCrushed_L,
            CrystalCrushed_R,
            GroundAttack,
            Heal,
            ActivateSuper_L,
            ActivateSuper_R,
            Premonition,
            Bulwark,
            NoFeedback
        }

        struct Feedback
        {
            public Feedback(FeedbackType _feedbackType, string _prefix, int _feedbackFileCount)
            {
                feedbackType = _feedbackType;
                prefix = _prefix;
                feedbackFileCount = _feedbackFileCount;
            }
            public FeedbackType feedbackType;
            public string prefix;
            public int feedbackFileCount;
        };

        void SimpleTactFileRegister(string key, string filename)
        {
            hapticPlayer.RegisterTactFileStr(key, filename);
        }

        void TactFileRegister(string fullname, Feedback feedback)
        {
            if (feedbackMap.ContainsKey(feedback.feedbackType))
            {
                Feedback f = feedbackMap[feedback.feedbackType];
                f.feedbackFileCount += 1;
                feedbackMap[feedback.feedbackType] = f;

                string tactFileStr = File.ReadAllText(fullname);

                hapticPlayer.RegisterTactFileStr(feedback.prefix, tactFileStr);
                // hapticPlayer.RegisterTactFileStr(feedback.prefix + (feedbackMap[feedback.feedbackType].feedbackFileCount).ToString(), tactFileStr);
            }
        }

        void RegisterFeedbackFiles()
        {
            string configPath = Directory.GetCurrentDirectory() + "\\Mods\\bHaptics";

            DirectoryInfo d = new DirectoryInfo(configPath);
            FileInfo[] Files = d.GetFiles("*.tact", SearchOption.AllDirectories);

            for (int i = 0; i < Files.Length; i++)
            {
                string filename = Files[i].Name;
                string fullName = Files[i].FullName;
                string prefix = Path.GetFileNameWithoutExtension(filename);
                if (filename == "." || filename == "..")
                    continue;

                foreach (var element in feedbackMap)
                {
                    if (filename.StartsWith(element.Value.prefix))
                    {
                        TactFileRegister(fullName, element.Value);
                        LOG("Tact file registered: " + filename);
                        break;
                    }
                    /*else
                    {
                        LOG("Trying manual register: " + prefix + " " + filename);
                        string tactFileStr = File.ReadAllText(fullName);
                        hapticPlayer.RegisterTactFileStr(prefix, tactFileStr);
                    }*/
                }
            }

            foreach (FeedbackType f in Enum.GetValues(typeof(FeedbackType)))
            {
                if (f == FeedbackType.NoFeedback)
                    continue;

                if (!feedbackMap.ContainsKey(f) || feedbackMap[f].feedbackFileCount == 0)
                {
                    LOG("Tact file NOT found: " + f.ToString());
                }
            }
        }

        public void CreateSystem()
        {
            if (!systemInitialized)
            {
                hapticPlayer = new Bhaptics.Tact.HapticPlayer("mod_untilyoufall", "mod_untilyoufall");

                if (hapticPlayer != null)
                {
                    RegisterFeedbackFiles();
                    systemInitialized = true;
                    LOG("Tactsuit system initialized.");
                }
            }
        }

        bool IsPlayingKeyAll(bool reflected, string prefix, int feedbackFileCount)
        {
            for (int i = 1; i <= feedbackFileCount; i++)
            {
                string key = (reflected ? "Reflected_" : "") + prefix + i.ToString();
                if (hapticPlayer.IsPlaying(key))
                {
                    return true;
                }
            }
            return false;
        }


        public void ProvideHapticFeedbackThread(float locationAngle, float locationHeight, FeedbackType effect, float intensityMultiplier, bool waitToPlay, bool reflected, float duration = 1.0f)
        {
            if (suitDisabled)
                return;

            if (intensityMultiplier < 0.001f)
                return;

            if (!systemInitialized || hapticPlayer == null)
                CreateSystem();

            if (hapticPlayer != null)
            {
                if (feedbackMap.ContainsKey(effect))
                {
                    if (feedbackMap[effect].feedbackFileCount > 0)
                    {
                        if (waitToPlay)
                        {
                            if (IsPlayingKeyAll(reflected, feedbackMap[effect].prefix, feedbackMap[effect].feedbackFileCount))
                            {
                                return;
                            }
                        }

                        string key = (reflected ? "Reflected_" : "") + feedbackMap[effect].prefix + (RandomNumber.Between(1, feedbackMap[effect].feedbackFileCount)).ToString();

                        if (locationHeight < -0.5f)
                            locationHeight = -0.5f;
                        else if (locationHeight > 0.5f)
                            locationHeight = 0.5f;

                        Bhaptics.Tact.RotationOption RotOption = new RotationOption(locationAngle, locationHeight);

                        Bhaptics.Tact.ScaleOption scaleOption = new ScaleOption(intensityMultiplier, duration);

                        
                        hapticPlayer.SubmitRegisteredVestRotation(key, key, RotOption, scaleOption);
                        LOG("===> Submitted Feedback: " + key + " Intensity: " + intensityMultiplier + " Height: " + locationHeight + " Angle: " + locationAngle);
                    }
                }
            }
        }
        
        public void HeartBeat()
        {
            string key_long = "HeartBeat";
            Bhaptics.Tact.ScaleOption scaleOption = new ScaleOption(1.0f, 1.0f);
            hapticPlayer.SubmitRegistered(key_long, scaleOption);
        }

        public void ProvideHapticFeedback(float locationAngle, float locationHeight, FeedbackType effect, bool waitToPlay, float intensity, FeedbackType secondEffect, bool reflected, float duration = 1.0f)
        {
            if (effect != FeedbackType.NoFeedback)
            {
                float intensityMultiplier = intensity;
                if (intensityMultiplier > 0.01f)
                {
                    Thread thread = new Thread(() => ProvideHapticFeedbackThread(locationAngle, locationHeight, effect, intensityMultiplier, waitToPlay, reflected, duration));
                    thread.Start();
                    if (secondEffect != FeedbackType.NoFeedback)
                    {
                        Thread thread2 = new Thread(() => ProvideHapticFeedbackThread(locationAngle, locationHeight, secondEffect, intensityMultiplier, waitToPlay, reflected, duration));
                        thread2.Start();
                    }
                }
            }
        }

        public void SimpleFeedback(FeedbackType effect)
        {
            if (effect != FeedbackType.NoFeedback)
            {
                if (suitDisabled)
                    return;

                if (!systemInitialized || hapticPlayer == null)
                    CreateSystem();

                if (hapticPlayer != null)
                {
                    if (feedbackMap.ContainsKey(effect))
                    {
                        if (feedbackMap[effect].feedbackFileCount > 0)
                        {
                            string key = feedbackMap[effect].prefix + (RandomNumber.Between(1, feedbackMap[effect].feedbackFileCount)).ToString();
                            hapticPlayer.SubmitRegistered(key);
                        }

                    }
                }
            }
        }

        public void FileFeedback(String key, float intensity = 1.0f, float duration = 1.0f)
        {
            Bhaptics.Tact.ScaleOption scaleOption = new ScaleOption(intensity, duration);
            hapticPlayer.SubmitRegistered(key, scaleOption);
        }

        public void StopHapticFeedback(FeedbackType effect)
        {
            if (hapticPlayer != null)
            {
                if (feedbackMap.ContainsKey(effect))
                {
                    if (feedbackMap[effect].feedbackFileCount > 0)
                    {
                        for (int i = 1; i <= feedbackMap[effect].feedbackFileCount; i++)
                        {
                            string key = feedbackMap[effect].prefix + i.ToString();
                            hapticPlayer.TurnOff(key);
                        }
                    }
                }
            }
        }

        public void PauseHapticFeedBack(FeedbackType effect)
        {
            for (int i = 1; i <= feedbackMap[effect].feedbackFileCount; i++)
            {
                hapticPlayer.TurnOff(feedbackMap[effect].prefix + i.ToString());
            }
        }

    }
}
