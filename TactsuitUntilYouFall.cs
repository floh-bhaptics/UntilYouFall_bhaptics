using System;
using MelonLoader;
using HarmonyLib;
using System.Threading;

using Il2CppSG.Claymore.Movement.Dash;
using Il2CppSG.Claymore.Combat.Blocking;
using Il2CppSG.Claymore.Interaction;
using Il2CppSG.Claymore.Armaments;
using Il2CppSG.Claymore.Armaments.Abilities;
using Il2CppSG.Claymore.Player;
using Il2CppSG.Claymore.Combat.EnemyAttacks;
using Il2CppSG.Claymore.Entities;
using Il2CppSG.Claymore.Armory;

namespace TactsuitUntilYouFall
{
    public class TactsuitUntilYouFall : MelonMod
    {
        public static TactsuitVR tactsuitVr;
        private static String ActiveHand = "PlayerHandRight";
        private static ManualResetEvent mrse = new ManualResetEvent(false);
        private static bool BulwarkActive = false;
        private static bool handsConnected = true;

        private static void HeartBeatFunc()
        {
            while (true)
            {
                mrse.WaitOne();
                tactsuitVr.FileFeedback("HeartBeat");
                Thread.Sleep(1000);
            }
        }

        public override void OnUpdate()
        {
        }


        public override void OnInitializeMelon()
        {
            //Preferences.Setup();

            tactsuitVr = new TactsuitVR();
            tactsuitVr.CreateSystem();

            //Create hooks here:

            //var harmony = new Harmony.HarmonyInstance("TactsuitUntilYouFallHarmony");
            //HarmonyInstance. Create("TactsuitUntilYouFallHarmony").PatchAll();
            //harmony.PatchAll();


            tactsuitVr.LOG("Events created.");
            try
            {
                TactsuitUntilYouFall.tactsuitVr.HeartBeat();
                tactsuitVr.LOG("Startup heartbeat.");
            }
            catch (Exception e) { tactsuitVr.LOG("Heartbeat failed. " + e.ToString()); }
            Thread thread = new Thread(HeartBeatFunc);
            thread.Start();
            //handsConnected = TactsuitUntilYouFall.tactsuitVr.HandsConnected();

        }


        [HarmonyPatch(typeof(PlayerDash), "OnDashForward")]
        public class DashForward
        {
            [HarmonyPostfix]
            public static void Postfix(PlayerDash __instance)
            {
                //tactsuitVr.LOG("OnDashForward.");
                try
                {
                    //TactsuitVR.FeedbackType feedback = TactsuitVR.FeedbackType.DashForward;
                    //TactsuitUntilYouFall.tactsuitVr.SimpleFeedback(feedback);
                    TactsuitUntilYouFall.tactsuitVr.FileFeedback("DashForward");
                }
                catch(Exception e) { tactsuitVr.LOG("Feedback failed. " + e.ToString()); }
            }
        }

        [HarmonyPatch(typeof(Armament), "InitSummon")]
        public class InitSummon
        {
            [HarmonyPostfix]
            public static void Postfix(Armament __instance, PlayerHand summoningHand)
            {
                if (summoningHand.name == "PlayerHandLeft")
                {
                    // tactsuitVr.LOG("MeleeWeapon.Summon Left");
                    TactsuitUntilYouFall.tactsuitVr.FileFeedback("SummonWeapon_L");
                    if (handsConnected) { TactsuitUntilYouFall.tactsuitVr.FileFeedback("SummonWeaponHands_L"); }
                }
                else
                {
                    // tactsuitVr.LOG("MeleeWeapon.Summon Right");
                    TactsuitUntilYouFall.tactsuitVr.FileFeedback("SummonWeapon_R");
                    if (handsConnected) { TactsuitUntilYouFall.tactsuitVr.FileFeedback("SummonWeaponHands_R"); }
                }
            }
        }


        [HarmonyPatch(typeof(MeleeWeapon), "GetForceRating", new Type[] { typeof(Il2CppSG.Claymore.HitSystem.HitData.HitQualityType) })]
        public class HitForce
        {
            [HarmonyPostfix]
            public static void Postfix(MeleeWeapon __instance, float __result)
            {

                if (__result > 1.0)
                {
                    float intensity = __result / 10.0f + 0.02f;
                    if (__instance.armament.BoundHand == __instance.HoldingPlayer.LeftHand)
                    {
                        TactsuitUntilYouFall.tactsuitVr.FileFeedback("Block_L", intensity);
                        if (handsConnected) { TactsuitUntilYouFall.tactsuitVr.FileFeedback("BlockHands_L", intensity); }
                        if (__instance.armament.isHeldInTwoHands)
                        {
                            TactsuitUntilYouFall.tactsuitVr.FileFeedback("Block_R", intensity * 0.5f);
                            if (handsConnected) { TactsuitUntilYouFall.tactsuitVr.FileFeedback("BlockHands_R", intensity * 0.5f); }
                        }
                    }
                        else
                    {
                        TactsuitUntilYouFall.tactsuitVr.FileFeedback("Block_R", intensity);
                        if (handsConnected) { TactsuitUntilYouFall.tactsuitVr.FileFeedback("BlockHands_R", intensity); }
                        if (__instance.armament.isHeldInTwoHands)
                        {
                            TactsuitUntilYouFall.tactsuitVr.FileFeedback("Block_L", intensity * 0.5f);
                            if (handsConnected) { TactsuitUntilYouFall.tactsuitVr.FileFeedback("BlockHands_L", intensity * 0.5f); }
                        }
                    }
                    // tactsuitVr.LOG("GetForceRating");
                    // tactsuitVr.LOG(__result.ToString());

                }
            }
        }

        [HarmonyPatch(typeof(ArmamentAbilityUser), "OnSuperActivated")]
        public class SuperActivated
        {
            [HarmonyPostfix]
            public static void Postfix(ArmamentAbilityUser __instance)
            {
                // tactsuitVr.LOG("ArmamentAbilityUser");
                if (__instance.HoldingPlayer.LeftHand == __instance.armament.BoundHand)
                {
                    TactsuitUntilYouFall.tactsuitVr.FileFeedback("CrushCrystal_L");
                    TactsuitUntilYouFall.tactsuitVr.FileFeedback("ActivateSuper_L");
                    if (handsConnected) { TactsuitUntilYouFall.tactsuitVr.FileFeedback("CrushCrystalHands_L"); }
                } else
                {
                    TactsuitUntilYouFall.tactsuitVr.FileFeedback("CrushCrystal_R");
                    TactsuitUntilYouFall.tactsuitVr.FileFeedback("ActivateSuper_R");
                    if (handsConnected) { TactsuitUntilYouFall.tactsuitVr.FileFeedback("CrushCrystalHands_R"); }
                }
            }
        }

        [HarmonyPatch(typeof(PlayerHealth), "OnHealthChanged")]
        public class HealthChanged
        {
            [HarmonyPostfix]
            public static void Postfix(PlayerHealth __instance, float hp)
            {
                BulwarkActive = false;
                // tactsuitVr.LOG(hp.ToString());
                if (hp >= 1.0f)
                {
                    mrse.Reset();
                }
                if(__instance.IsDead)
                {
                    mrse.Reset();
                }
            }
        }

        [HarmonyPatch(typeof(PlayerDefense), "OnAttackHit")]
        public class OnAttackHit
        {
            [HarmonyPostfix]
            public static void Postfix(PlayerDefense __instance, BlockTimingData blockTiming)
            {
                // tactsuitVr.LOG("PlayerDefense.OnAttackHit");
                if (__instance.health.IsOnDeathsDoor)
                {
                    mrse.Set();
                }
                if (__instance.health.IsDead)
                {
                    mrse.Reset();
                }
                if (BulwarkActive)
                {
                    // tactsuitVr.LOG("Bulwark active");
                    TactsuitUntilYouFall.tactsuitVr.FileFeedback("Bulwark");
                }
                else if (blockTiming.IsDodgePremonition)
                {
                    TactsuitUntilYouFall.tactsuitVr.FileFeedback("HitByHammer");
                }
                else
                {
                    TactsuitUntilYouFall.tactsuitVr.FileFeedback("SlashDefault");
                }
            }
        }

        [HarmonyPatch(typeof(PlayerDefense), "OnAttackBlocked")]
        public class OnAttackBlocked
        {
            [HarmonyPostfix]
            public static void Postfix(PlayerDefense __instance, AttackBlocker blocker)
            {
                // tactsuitVr.LOG("PlayerDefense.OnAttackBlocked");
                if (blocker == blocker.HoldingPlayer.leftBlocker)
                {
                    TactsuitUntilYouFall.tactsuitVr.FileFeedback("Block_L");
                    TactsuitUntilYouFall.tactsuitVr.FileFeedback("BlockVest_L");
                    if (handsConnected) { TactsuitUntilYouFall.tactsuitVr.FileFeedback("BlockHands_L"); }
                    if (blocker.armament.isHeldInTwoHands)
                    {
                        TactsuitUntilYouFall.tactsuitVr.FileFeedback("Block_R", 0.5f);
                        TactsuitUntilYouFall.tactsuitVr.FileFeedback("BlockVest_R", 0.5f);
                        if (handsConnected) { TactsuitUntilYouFall.tactsuitVr.FileFeedback("BlockHands_R", 0.5f); }
                    }
                }
                else
                {
                    TactsuitUntilYouFall.tactsuitVr.FileFeedback("Block_R");
                    TactsuitUntilYouFall.tactsuitVr.FileFeedback("BlockVest_R");
                    if (handsConnected) { TactsuitUntilYouFall.tactsuitVr.FileFeedback("BlockHands_R"); }
                    if (blocker.armament.isHeldInTwoHands)
                    {
                        TactsuitUntilYouFall.tactsuitVr.FileFeedback("Block_L", 0.5f);
                        TactsuitUntilYouFall.tactsuitVr.FileFeedback("BlockVest_L", 0.5f);
                        if (handsConnected) { TactsuitUntilYouFall.tactsuitVr.FileFeedback("BlockHands_L", 0.5f); }
                    }
                }

            }
        }

        [HarmonyPatch(typeof(PlayerDefense), "KnockbackPlayer")]
        public class Knockback
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                // tactsuitVr.LOG("PlayerDefense.KnockBackPlayer");
                TactsuitUntilYouFall.tactsuitVr.FileFeedback("KnockBack");
            }
        }

        [HarmonyPatch(typeof(CrushInteractable), "OnCrushStart")]
        public class CrushStart
        {
            [HarmonyPostfix]
            public static void Postfix(CrushInteractable __instance)
            {
                if (__instance.crushingHand.name == "PlayerHandLeft")
                {
                    //tactsuitVr.LOG("CrushInteractable.OnCrushStart Left");
                    TactsuitUntilYouFall.tactsuitVr.FileFeedback("CrushCrystal_L");
                    if (handsConnected) { TactsuitUntilYouFall.tactsuitVr.FileFeedback("CrushCrystalHands_L"); }
                    ActiveHand = "PlayerHandLeft";
                } else
                {
                    //tactsuitVr.LOG("CrushInteractable.OnCrushStart Right");
                    TactsuitUntilYouFall.tactsuitVr.FileFeedback("CrushCrystal_R");
                    if (handsConnected) { TactsuitUntilYouFall.tactsuitVr.FileFeedback("CrushCrystalHands_R"); }
                    ActiveHand = "PlayerHandRight";
                }
            }
        }

        [HarmonyPatch(typeof(CrushInteractable), "OnCancelCrush")]
        public class CrushCancel
        {
            [HarmonyPostfix]
            public static void Postfix(CrushInteractable __instance)
            {
                //tactsuitVr.LOG("CrushInteractable.OnCancelCrush");
                if (__instance.crushingHand.name == "PlayerHandLeft")
                {
                    TactsuitUntilYouFall.tactsuitVr.StopHapticFeedback(TactsuitVR.FeedbackType.CrushCrystal_L);
                    TactsuitUntilYouFall.tactsuitVr.StopHapticFeedback(TactsuitVR.FeedbackType.CrushCrystalHands_L);
                }
                else
                {
                    TactsuitUntilYouFall.tactsuitVr.StopHapticFeedback(TactsuitVR.FeedbackType.CrushCrystal_R);
                    TactsuitUntilYouFall.tactsuitVr.StopHapticFeedback(TactsuitVR.FeedbackType.CrushCrystalHands_R);
                }
            }
        }

        [HarmonyPatch(typeof(CrushInteractable), "FinishCrush")]
        public class CrushComplete
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                if (ActiveHand == "PlayerHandLeft")
                {
                    //tactsuitVr.LOG("CrushInteractable.OnCrushStart Left");
                    TactsuitUntilYouFall.tactsuitVr.FileFeedback("CrystalCrushed_L");
                }
                else
                {
                    //tactsuitVr.LOG("CrushInteractable.OnCrushStart Right");
                    TactsuitUntilYouFall.tactsuitVr.FileFeedback("CrystalCrushed_R");
                }
            }
        }

        [HarmonyPatch(typeof(GroundAttackPlayable), "Raise")]
        public class GroundAttack
        {
            [HarmonyPostfix]
            public static void Postfix(GroundAttackPlayable __instance)
            {
                tactsuitVr.LOG("GroundAttackPlayable.Raise");
                TactsuitUntilYouFall.tactsuitVr.FileFeedback("GroundAttack");
            }
        }

        [HarmonyPatch(typeof(PlayerHealth), "Restore")]
        public class RestoreHealth
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                // tactsuitVr.LOG("PlayerHealth.Restore");
                TactsuitUntilYouFall.tactsuitVr.FileFeedback("Heal");
            }
        }

        [HarmonyPatch(typeof(WeaponRackSlot), "OnEnable")]
        public class WeaponRack
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                TactsuitUntilYouFall.tactsuitVr.FileFeedback("RiseAgain");
            }
        }

        [HarmonyPatch(typeof(BulwarkAbility), "OnActivationSuccess")]
        public class ActivateBulwark
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                // tactsuitVr.LOG("Activate Bulwark");
                BulwarkActive = true;
            }
        }

        /*
                [HarmonyPatch(typeof(BlockPremonitionInstanced), "StartPremonition")]
                public class StartBlockPremonition
                {
                    [HarmonyPostfix]
                    public static void Postfix(float premonitionDuration)
                    {
                        // tactsuitVr.LOG("StartBlockPremonition");
                        TactsuitUntilYouFall.tactsuitVr.FileFeedback("Premonition");
                    }
                }

                [HarmonyPatch(typeof(BlockPremonitionInstanced), "SetIsSucceeding")]
                public class StopBlockPremonition
                {
                    [HarmonyPostfix]
                    public static void Postfix()
                    {
                        TactsuitUntilYouFall.tactsuitVr.StopHapticFeedback(TactsuitVR.FeedbackType.Premonition);
                    }
                }

                [HarmonyPatch(typeof(DodgePremonitionInstanced), "StartPremonition")]
                public class StartDodgePremonition
                {
                    [HarmonyPostfix]
                    public static void Postfix(float premonitionDuration)
                    {
                        // tactsuitVr.LOG("StartBlockPremonition");
                        TactsuitUntilYouFall.tactsuitVr.FileFeedback("Premonition");
                    }
                }

                [HarmonyPatch(typeof(DodgePremonitionInstanced), "SetIsSucceeding")]
                public class StopDodgePremonition
                {
                    [HarmonyPostfix]
                    public static void Postfix()
                    {
                        TactsuitUntilYouFall.tactsuitVr.StopHapticFeedback(TactsuitVR.FeedbackType.Premonition);
                    }
                }
        */
    }
}
