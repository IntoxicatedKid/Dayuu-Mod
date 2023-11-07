using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.Adventures;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core;
using LBoL.Core.Cards;
using LBoL.EntityLib.StatusEffects.Sakuya;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using LBoL.Core.StatusEffects;
using LBoL.Core.Randoms;
using LBoL.Core.Units;
using LBoL.Base.Extensions;
using LBoL.EntityLib.Exhibits.Common;
using LBoL.EntityLib.Cards.Character.Marisa;
using LBoL.EntityLib.StatusEffects.Basic;
using LBoL.EntityLib.StatusEffects.Enemy;
using LBoL.EntityLib.StatusEffects.Neutral;
using LBoL.EntityLib.StatusEffects.Neutral.Black;
using LBoL.EntityLib.StatusEffects.Neutral.Blue;
using LBoL.EntityLib.StatusEffects.Neutral.Green;
using LBoL.EntityLib.StatusEffects.Neutral.MultiColor;
using LBoL.EntityLib.StatusEffects.Neutral.Red;
using LBoL.EntityLib.StatusEffects.Neutral.TwoColor;
using LBoL.EntityLib.StatusEffects.Neutral.White;
using LBoL.EntityLib.StatusEffects.Others;
using LBoL.EntityLib.Cards.Character.Sakuya;
using System.Linq;
using static UnityEngine.GraphicsBuffer;
using static DayuuMod.BepinexPlugin;
using Mono.Cecil;
using LBoL.Presentation;
using LBoL.EntityLib.Exhibits.Shining;
using HarmonyLib;
using UnityEngine;
using System.Numerics;
using LBoL.Core.Battle.Interactions;
using LBoL.EntityLib.StatusEffects.ExtraTurn.Partners;

namespace DayuuMod.Cards
{
    public sealed class DayuuReimuKoishiDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(DayuuReimuKoishi);
        }

        public override CardImages LoadCardImages()
        {
            var imgs = new CardImages(embeddedSource);
            imgs.AutoLoad(this, ".png", relativePath: "Resources.");
            return imgs;
        }

        public override LocalizationOption LoadLocalization()
        {
            var locFiles = new LocalizationFiles(embeddedSource);
            locFiles.AddLocaleFile(Locale.En, "Resources.CardsEn.yaml");
            return locFiles;
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = new CardConfig(
               Index: sequenceTable.Next(typeof(CardConfig)),
               Id: "",
               Order: 10,
               AutoPerform: true,
               Perform: new string[0][],
               GunName: new string[] { "T1RBlue1", "T2RBlue1", "T3RBlue1" }.OrderBy(x => UnityEngine.Random.Range(0, 3)).First(),
               GunNameBurst: new string[] { "T1RBlue1", "T2RBlue1", "T3RBlue1" }.OrderBy(x => UnityEngine.Random.Range(0, 3)).First(),
               DebugLevel: 0,
               Revealable: false,
               //IsPooled: GameMaster.Instance.CurrentGameRun != null && (GameMaster.Instance.CurrentGameRun.Player.HasExhibit<ReimuW>() || GameMaster.Instance.CurrentGameRun.Player.HasExhibit<ReimuR>()) && (GameMaster.Instance.CurrentGameRun.Player.HasExhibit<KoishiG>() || GameMaster.Instance.CurrentGameRun.Player.HasExhibit<KoishiB>()),
               IsPooled: true,
               HideMesuem: false,
               IsUpgradable: true,
               Rarity: Rarity.Rare,
               //Type: new List<CardType>() { CardType.Attack, CardType.Defense, CardType.Skill, CardType.Ability, CardType.Friend }.OrderBy(x => UnityEngine.Random.Range(0, 5)).First(),
               Type: CardType.Friend,
               TargetType: TargetType.Nobody,
               Colors: new List<ManaColor>() { ManaColor.White, ManaColor.Black, ManaColor.Red, ManaColor.Green },
               IsXCost: false,
               Cost: new ManaGroup() { White = 1, Black = 1, Red = 1, Green = 1 },
               UpgradedCost: null,
               MoneyCost: null,
               Damage: UnityEngine.Random.Range(18, 25),
               UpgradedDamage: UnityEngine.Random.Range(24, 31),
               Block: UnityEngine.Random.Range(18, 25),
               UpgradedBlock: UnityEngine.Random.Range(24, 31),
               Shield: UnityEngine.Random.Range(12, 19),
               UpgradedShield: UnityEngine.Random.Range(18, 25),
               Value1: UnityEngine.Random.Range(1, 3),
               UpgradedValue1: UnityEngine.Random.Range(3, 5),
               Value2: UnityEngine.Random.Range(2, 5),
               UpgradedValue2: UnityEngine.Random.Range(6, 9),
               Mana: new ManaGroup() { Philosophy = UnityEngine.Random.Range(1, 3) },
               UpgradedMana: new ManaGroup() { Philosophy = UnityEngine.Random.Range(3, 5) },
               Scry: UnityEngine.Random.Range(1, 3),
               UpgradedScry: UnityEngine.Random.Range(3, 5),
               ToolPlayableTimes: null,
               Loyalty: 1,
               UpgradedLoyalty: UnityEngine.Random.Range(1, 3),
               PassiveCost: 1,
               UpgradedPassiveCost: UnityEngine.Random.Range(1, 3),
               ActiveCost: -4,
               UpgradedActiveCost: UnityEngine.Random.Range(-4, -2),
               UltimateCost: -7,
               UpgradedUltimateCost: UnityEngine.Random.Range(-7, -5),

               Keywords: Keyword.None,
               UpgradedKeywords: Keyword.None,
               EmptyDescription: false,
               RelativeKeyword: Keyword.None,
               UpgradedRelativeKeyword: Keyword.None,

               RelativeEffects: new List<string>() { },
               UpgradedRelativeEffects: new List<string>() { },
               RelativeCards: new List<string>() { },
               UpgradedRelativeCards: new List<string>() { },
               Owner: null,
               Unfinished: false,
               Illustrator: "rinirim",
               SubIllustrator: new List<string>() { }
            );

            return cardConfig;
        }
    }
    [EntityLogic(typeof(DayuuReimuKoishiDef))]
    public sealed class DayuuReimuKoishi : Card
    {
        /*[HarmonyPatch(typeof(GameRunController), nameof(GameRunController.EnterStation))]
        class GameRunController_EnterStation_Patch
        {
            static void Postfix(GameRunController __instance)
            {
                if ((GameMaster.Instance.CurrentGameRun.Player.HasExhibit<ReimuW>() || GameMaster.Instance.CurrentGameRun.Player.HasExhibit<ReimuR>()) && (GameMaster.Instance.CurrentGameRun.Player.HasExhibit<KoishiG>() || GameMaster.Instance.CurrentGameRun.Player.HasExhibit<KoishiB>()))
                {
                    Library.CreateCard<DayuuReimuKoishi>().Config.IsPooled = true;
                }
                else
                {
                    Library.CreateCard<DayuuReimuKoishi>().Config.IsPooled = false;
                }
            }
        }*/
        /*private string DescriptionAttack
        {
            get
            {
                return LocalizeProperty("DescriptionAttack", true, true);
            }
        }
        private string DescriptionDefense
        {
            get
            {
                return LocalizeProperty("DescriptionDefense", true, true);
            }
        }
        private string DescriptionSkill
        {
            get
            {
                return LocalizeProperty("DescriptionSkill", true, true);
            }
        }
        private string DescriptionAbility
        {
            get
            {
                return LocalizeProperty("DescriptionAbility", true, true);
            }
        }
        private string DescriptionFriend
        {
            get
            {
                return LocalizeProperty("DescriptionFriend", true, true);
            }
        }
        protected override string GetBaseDescription()
        {
            switch (CardType)
            {
                case CardType.Attack:
                    return DescriptionAttack;
                case CardType.Defense:
                    return DescriptionDefense;
                case CardType.Skill:
                    return DescriptionSkill;
                case CardType.Ability:
                    return DescriptionAbility;
                case CardType.Friend:
                    return DescriptionFriend;
                default:
                    return base.GetBaseDescription();
            }
        }*/
        private new string ExtraDescription1
        {
            get
            {
                return GetBaseDescription();
            }
        }
        private new string ExtraDescription2
        {
            get
            {
                return GetBaseDescription();
            }
        }
        static Unit target = null;
        static List<Type> buffs1 = new List<Type>() { typeof(BurstUpgrade), typeof(LimitedDamage), typeof(WindGirl), typeof(AbsorbPower), typeof(AbsorbSpirit), typeof(Curiosity), typeof(EnemyDayaojing), typeof(FlatPeach), typeof(PowerByDefense), typeof(YonglinCardSe), typeof(HuiyeManaSe), typeof(BailianFireSe), typeof(SuperExtraTurn), typeof(PrismriverSe), typeof(YukariFriendSe), typeof(MeihongPowerSe), typeof(RainbowMarketSe), typeof(MoonWorldSe) };
        static List<Type> buffs2 = new List<Type>() { typeof(Firepower), typeof(Spirit), typeof(DroneBlock), typeof(RockHard), typeof(SeeFengshuiSe), typeof(DizangLoveSe), typeof(BlackPoisonSe), typeof(HekaHellRainSe), typeof(MeilingAbilitySe), typeof(RinDrawSe), typeof(SkyWaterSe), typeof(TodayYesterdaySe), typeof(YachieDefendSe), typeof(YonglinUpgradeSe) };
        static List<Type> buffs3 = new List<Type>() { typeof(Electric), typeof(LoveGirlDamageReduce), typeof(ShannvAbilitySe), typeof(RangziFanshuSe), typeof(MeihongFireSe), typeof(KokoroDanceSe), typeof(ModuoluoFireSe), typeof(ShirenKunchongSe) };
        static List<Type> tempBuffsP1 = new List<Type>() { typeof(Charging), typeof(Graze), typeof(TempFirepower), typeof(TempSpirit) };
        static List<Type> tempBuffsP2 = new List<Type>() { typeof(NextAttackUp), typeof(NextTurnGainBlock), typeof(NextTurnGainShield), typeof(Reflect), typeof(TempElectric), typeof(MaoyuBlock) };
        static List<Type> tempBuffsA1 = new List<Type>() { typeof(Burst), typeof(GuangxueMicai), typeof(TurnStartDontLoseBlock), typeof(Amulet), typeof(AmuletForCard), typeof(GrimoireStudySe), typeof(ZhenmiaowanAttackSe) };
        static List<Type> tempBuffsA2 = new List<Type>() { /*typeof(Charging),*/ typeof(Graze), typeof(TempFirepower), typeof(TempSpirit) };
        static List<Type> tempBuffsU1 = new List<Type>() { typeof(ExtraTurn), typeof(Invincible) };
        static List<Type> tempBuffsU2 = new List<Type>() { typeof(GuangxueMicai), typeof(TurnStartDontLoseBlock), typeof(Amulet), typeof(AmuletForCard), typeof(GrimoireStudySe), typeof(ZhenmiaowanAttackSe) };
        //static List<Type> debuffs1 = new List<Type>() { };
        static List<Type> debuffs2 = new List<Type>() { typeof(FirepowerNegative), /*typeof(SpiritNegative),*/ typeof(TiangouOrderSe) };
        static List<Type> debuffs3 = new List<Type>() { typeof(Drowning), typeof(LoveGirlDamageIncrease) };
        static List<Type> tempDebuffs1 = new List<Type>() { typeof(LunaClockSe)/*, typeof(CatchGoblinSe)*/ };
        static List<Type> tempDebuffs2 = new List<Type>() { /*typeof(Fragil),*/ typeof(LockedOn), typeof(TempFirepowerNegative), /*typeof(TempSpiritNegative),*/ typeof(Vulnerable), typeof(Weak) };
        static List<Type> tempDebuffs3 = new List<Type>() { typeof(Poison) };
        /*
        static int Rng1 = 0;
        static int Rng2 = 0;
        static int Rng3 = 0;
        static int Rng4 = 0;
        static int Rng5 = 0;
        static int Rng6 = 0;
        static int Rng7 = 0;
        static int Rng8 = 0;
        static int Rng9 = 0;
        static int Rng10 = 0;
        static int Rng11 = 0;
        static int Rng12 = 0;
        static int Rng13 = 0;
        static int Rng14 = 0;
        static int Rng15 = 0;
        static int Rng16 = 0;
        static int Rng17 = 0;
        static int Rng18 = 0;
        static int Rng19 = 0;
        static int Rng20 = 0;
        static int Rng21 = 0;
        static int Rng22 = 0;
        static int RngB1 = 0;
        static int RngB2 = 0;
        static int RngB3 = 0;
        static int RngP1 = 0;
        static int RngP2 = 0;
        static int RngA1 = 0;
        static int RngA2 = 0;
        static int RngU1 = 0;
        static int RngU2 = 0;
        static int RngD1 = 0;
        static int RngD2 = 0;
        static int RngD3 = 0;
        static int RngT1 = 0;
        static int RngT2 = 0;
        static int RngT3 = 0;
        */
        static int Rng1 = UnityEngine.Random.Range(0, 5);
        static int Rng2 = UnityEngine.Random.Range(0, 5);
        static int Rng3 = UnityEngine.Random.Range(0, 5);
        static int Rng4 = UnityEngine.Random.Range(0, 5);
        static int Rng5 = UnityEngine.Random.Range(0, 5);
        static int Rng6 = UnityEngine.Random.Range(0, 5);
        static int Rng7 = UnityEngine.Random.Range(0, 5);
        static int Rng8 = UnityEngine.Random.Range(0, 5);
        static int Rng9 = UnityEngine.Random.Range(0, 5);
        static int Rng10 = UnityEngine.Random.Range(0, 5);
        static int Rng11 = UnityEngine.Random.Range(0, 5);
        static int Rng12 = UnityEngine.Random.Range(0, 5);
        static int Rng13 = UnityEngine.Random.Range(0, 5);
        static int Rng14 = UnityEngine.Random.Range(0, 5);
        static int Rng15 = UnityEngine.Random.Range(0, 5);
        static int Rng16 = UnityEngine.Random.Range(0, 5);
        static int Rng17 = UnityEngine.Random.Range(0, 5);
        static int Rng18 = UnityEngine.Random.Range(0, 5);
        static int Rng19 = UnityEngine.Random.Range(0, 5);
        static int Rng20 = UnityEngine.Random.Range(0, 5);
        static int Rng21 = UnityEngine.Random.Range(0, 5);
        static int Rng22 = UnityEngine.Random.Range(0, 5);
        static int RngB1 = UnityEngine.Random.Range(0, buffs1.Count);
        static int RngB2 = UnityEngine.Random.Range(0, buffs2.Count);
        static int RngB3 = UnityEngine.Random.Range(0, buffs3.Count);
        static int RngP1 = UnityEngine.Random.Range(0, tempBuffsP1.Count);
        static int RngP2 = UnityEngine.Random.Range(0, tempBuffsP2.Count);
        static int RngA1 = UnityEngine.Random.Range(0, tempBuffsA1.Count);
        static int RngA2 = UnityEngine.Random.Range(0, tempBuffsA2.Count);
        static int RngU1 = UnityEngine.Random.Range(0, tempBuffsU1.Count);
        static int RngU2 = UnityEngine.Random.Range(0, tempBuffsU2.Count);
        //static int RngD1 = UnityEngine.Random.Range(0, debuffs1.Count);
        static int RngD2 = UnityEngine.Random.Range(0, debuffs2.Count);
        static int RngD3 = UnityEngine.Random.Range(0, debuffs3.Count);
        static int RngT1 = UnityEngine.Random.Range(0, tempDebuffs1.Count);
        static int RngT2 = UnityEngine.Random.Range(0, tempDebuffs2.Count);
        static int RngT3 = UnityEngine.Random.Range(0, tempDebuffs3.Count);
        public override void Initialize()
        {
            base.Initialize();
            /*
            Rng1 = UnityEngine.Random.Range(0, 5);
            Rng2 = UnityEngine.Random.Range(0, 5);
            Rng3 = UnityEngine.Random.Range(0, 5);
            Rng4 = UnityEngine.Random.Range(0, 5);
            Rng5 = UnityEngine.Random.Range(0, 5);
            Rng6 = UnityEngine.Random.Range(0, 5);
            Rng7 = UnityEngine.Random.Range(0, 5);
            Rng8 = UnityEngine.Random.Range(0, 5);
            Rng9 = UnityEngine.Random.Range(0, 5);
            Rng10 = UnityEngine.Random.Range(0, 5);
            Rng11 = UnityEngine.Random.Range(0, 5);
            Rng12 = UnityEngine.Random.Range(0, 5);
            Rng13 = UnityEngine.Random.Range(0, 5);
            Rng14 = UnityEngine.Random.Range(0, 5);
            Rng15 = UnityEngine.Random.Range(0, 5);
            Rng16 = UnityEngine.Random.Range(0, 5);
            Rng17 = UnityEngine.Random.Range(0, 5);
            Rng18 = UnityEngine.Random.Range(0, 5);
            Rng19 = UnityEngine.Random.Range(0, 5);
            Rng20 = UnityEngine.Random.Range(0, 5);
            Rng21 = UnityEngine.Random.Range(0, 5);
            Rng22 = UnityEngine.Random.Range(0, 5);
            RngB1 = UnityEngine.Random.Range(0, buffs1.Count);
            RngB2 = UnityEngine.Random.Range(0, buffs2.Count);
            RngB3 = UnityEngine.Random.Range(0, buffs3.Count);
            RngP1 = UnityEngine.Random.Range(0, tempBuffsP1.Count);
            RngP2 = UnityEngine.Random.Range(0, tempBuffsP2.Count);
            RngA1 = UnityEngine.Random.Range(0, tempBuffsA1.Count);
            RngA2 = UnityEngine.Random.Range(0, tempBuffsA2.Count);
            RngU1 = UnityEngine.Random.Range(0, tempBuffsU1.Count);
            RngU2 = UnityEngine.Random.Range(0, tempBuffsU2.Count);
            RngD1 = UnityEngine.Random.Range(0, debuffs1.Count);
            RngD2 = UnityEngine.Random.Range(0, debuffs2.Count);
            RngD3 = UnityEngine.Random.Range(0, debuffs3.Count);
            RngT1 = UnityEngine.Random.Range(0, tempDebuffs1.Count);
            RngT2 = UnityEngine.Random.Range(0, tempDebuffs2.Count);
            RngT3 = UnityEngine.Random.Range(0, tempDebuffs3.Count);
            */
        }
        public override IEnumerable<BattleAction> OnTurnStartedInHand()
        {
            if (CardType == CardType.Friend)
            {
                return GetPassiveActions();
            }
            return null;
        }
        /*public override IEnumerable<BattleAction> OnTurnEndingInHand()
        {
            if (CardType == CardType.Friend)
            {
                return GetPassiveActions();
            }
            return null;
        }*/
        public override IEnumerable<BattleAction> GetPassiveActions()
        {
            if (!Summoned || Battle.BattleShouldEnd)
            {
                yield break;
            }
            NotifyActivating();
            Loyalty += PassiveCost;
            int num;
            for (int i = 0; i < Battle.FriendPassiveTimes; i = num + 1)
            {
                yield return PerformAction.Sfx("FairySupport", 0f);
                target = Battle.RandomAliveEnemy;
                if (Battle.BattleShouldEnd)
                {
                    yield break;
                }
                if (Rng1 == 0)
                {
                    Loyalty += 1;
                }
                if (Rng1 > 0)
                {
                    switch (Rng2)
                    {
                        case 0:
                            yield return new DamageAction(Battle.Player, target, DamageInfo.Attack((Damage.Amount * 0.5).RoundToInt(), false), GunName, GunType.Single);
                            break;
                        case 1:
                            yield return new DamageAction(Battle.Player, target, DamageInfo.Attack((Damage.Amount * 0.5).RoundToInt(), false), GunName, GunType.Single);
                            foreach (BattleAction battleAction in ApplyTemporaryDebuff(target, 0.5f, true, false, false))
                            {
                                yield return battleAction;
                            }
                            break;
                        case 2:
                            yield return new CastBlockShieldAction(Battle.Player, (Block.Block * 0.5).RoundToInt(), 0, BlockShieldType.Direct, false);
                            break;
                        case 3:
                            yield return new CastBlockShieldAction(Battle.Player, (Block.Block * 0.5).RoundToInt(), 0, BlockShieldType.Direct, false);
                            foreach (BattleAction battleAction in ApplyTemporaryBuff(Battle.Player, 0.5f, true, false, false))
                            {
                                yield return battleAction;
                            }
                            break;
                        case 4:
                            yield return new CastBlockShieldAction(Battle.Player, Battle.Player, 0, (Shield.Shield * 0.5).RoundToInt(), BlockShieldType.Direct, false);
                            break;
                        default:
                            break;
                    }
                }
                if (Rng1 > 2)
                {
                    switch (Rng3)
                    {
                        case 0:
                            List<Card> list1 = Battle.HandZone.Where((card) => !card.IsPurified && card.Cost.HasTrivial).ToList();
                            if (list1.Count > 0)
                            {
                                Card card1 = list1.Sample(GameRun.BattleRng);
                                card1.NotifyActivating();
                                card1.IsPurified = true;
                            }
                            else
                            {
                                List<Card> list2 = Battle.HandZone.Where((card) => !card.IsPurified).ToList();
                                if (list2.Count > 0)
                                {
                                    Card card2 = list2.Sample(GameRun.BattleRng);
                                    card2.NotifyActivating();
                                    card2.IsPurified = true;
                                }
                            }
                            break;
                        case 1:
                            yield return new GainMoneyAction(Value2, SpecialSourceType.None);
                            break;
                        case 2:
                            yield return new GainPowerAction(Value2);
                            break;
                        case 3:
                            yield return new GainManaAction(new ManaGroup() { Philosophy = (Mana.Amount * 0.5).RoundToInt() });
                            break;
                        case 4:
                            yield return new DrawManyCardAction((Value1 * 0.5f).RoundToInt());
                            break;
                        default:
                            break;
                    }
                }
                num = i;
            }
            yield break;
        }
        public override IEnumerable<BattleAction> SummonActions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (Rng4 > 2)
            {
                switch (Rng5)
                {
                    case 0:
                        yield return new DamageAction(Battle.Player, Battle.AllAliveEnemies, DamageInfo.Attack((Damage.Amount * 0.75f).RoundToInt(), true), GunName, GunType.Single);
                        break;
                    case 1:
                        yield return PerformAction.Effect(Battle.Player, "DoubleDaJiejie", 0f, "Empty", 0f, PerformAction.EffectBehavior.PlayOneShot, 0f);
                        yield return new CastBlockShieldAction(Battle.Player, (Block.Block * 0.4).RoundToInt(), (Shield.Shield * 0.5).RoundToInt(), BlockShieldType.Direct, false);
                        break;
                    case 2:
                        yield return new UpgradeCardsAction(Battle.HandZone.Where((card) => card.CanUpgradeAndPositive));
                        break;
                    case 3:
                        yield return new DrawManyCardAction(Battle.MaxHand - Battle.HandZone.Count);
                        break;
                    case 4:
                        yield return PerformAction.Effect(Battle.Player, "ExtraTime", 0f, null, 0f, PerformAction.EffectBehavior.PlayOneShot, 0f);
                        yield return PerformAction.Sfx("ExtraTurnLaunch", 0f);
                        yield return PerformAction.Animation(Battle.Player, "spell", 1.6f, null, 0f, -1);
                        yield return new ApplyStatusEffectAction<ExtraTurn>(Battle.Player, 1, 0, 0, 0, 0.2f, true);
                        yield return new RequestEndPlayerTurnAction();
                        break;
                    default:
                        break;
                }
            }
            foreach (BattleAction battleAction in base.SummonActions(selector, consumingMana, precondition))
            {
                yield return battleAction;
            }
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (CardType == CardType.Friend)
            {
                if (precondition == null || ((MiniSelectCardInteraction)precondition).SelectedCard.FriendToken == FriendToken.Active)
                {
                    Loyalty += ActiveCost;
                    if (Rng6 > -1)
                    {
                        switch (Rng7)
                        {
                            case 0:
                                yield return new CastBlockShieldAction(Battle.Player, (Block.Block * 0.75f).RoundToInt(), 0, BlockShieldType.Direct, false);
                                yield return new DamageAction(Battle.Player, Battle.AllAliveEnemies, DamageInfo.Attack((Damage.Amount * 0.75f).RoundToInt(), true), GunName, GunType.Single);
                                break;
                            case 1:
                                foreach (BattleAction battleAction in ApplyBuff(Battle.Player, 1f, false, true, false))
                                {
                                    yield return battleAction;
                                }
                                break;
                            case 2:
                                foreach (BattleAction battleAction in ApplyTemporaryBuff(Battle.Player, 1f, true, true, false))
                                {
                                    yield return battleAction;
                                }
                                break;
                            case 3:
                                foreach (Unit enemyUnit in Battle.AllAliveEnemies)
                                {
                                    foreach (BattleAction battleAction in ApplyDebuff(enemyUnit, 1f, false, true, false))
                                    {
                                        yield return battleAction;
                                    }
                                }
                                break;
                            case 4:
                                foreach (Unit enemyUnit in Battle.AllAliveEnemies)
                                {
                                    foreach (BattleAction battleAction in ApplyTemporaryDebuff(enemyUnit, 1f, true, true, false))
                                    {
                                        yield return battleAction;
                                    }
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    if (Rng6 > 1)
                    {
                        switch (Rng8)
                        {
                            case 0:
                                List<Card> list = Battle.HandZone.Where((hand) => hand != this && hand.CardType != CardType.Tool && !hand.IsCopy).ToList();
                                if (list.Count <= 0)
                                {
                                    break;
                                }
                                SelectHandInteraction interaction = new SelectHandInteraction(0, 1, list)
                                {
                                    Source = this
                                };
                                yield return new InteractionAction(interaction, false);
                                Card origin = interaction.SelectedCards.FirstOrDefault();
                                List<Card> copied = new List<Card>();
                                for (int i = 0; i < 1; i++)
                                {
                                    Card card = origin.CloneBattleCard();
                                    card.SetTurnCost(Mana);
                                    card.IsExile = true;
                                    card.IsEthereal = true;
                                    copied.Add(card);
                                }
                                yield return new AddCardsToHandAction(copied);
                                if (origin.CardType == CardType.Ability || origin.IsExile)
                                {
                                    origin.IsCopy = true;
                                }
                                break;
                            case 1:
                                yield return new GainMoneyAction(Value2 * 2, SpecialSourceType.None);
                                yield return new GainPowerAction(Value2 * 2);
                                break;
                            case 2:
                                yield return new GainManaAction(new ManaGroup() { Philosophy = (Mana.Amount * 1f).RoundToInt() });
                                yield return new GainTurnManaAction(new ManaGroup() { Philosophy = (Mana.Amount * 1f).RoundToInt() });
                                break;
                            case 3:
                                yield return new ScryAction(new ScryInfo() { Count = Scry.Count * 2 });
                                yield return new DrawManyCardAction(Value1);
                                break;
                            case 4:
                                List<Card> getDrawZone = Battle.DrawZone.ToList();
                                if (getDrawZone.Count <= 0 || Battle.MaxHand <= Battle.HandZone.Count)
                                {
                                    break;
                                }
                                SelectHandInteraction interactionDrawZone = new SelectHandInteraction(0, 1, getDrawZone)
                                {
                                    Source = this
                                };
                                yield return new InteractionAction(interactionDrawZone, false);
                                Card cardDrawZone = interactionDrawZone.SelectedCards.FirstOrDefault();
                                yield return new MoveCardAction(cardDrawZone, CardZone.Hand);
                                break;
                            default:
                                break;
                        }
                    }
                }
                else
                {
                    Loyalty += UltimateCost;
                    UltimateUsed = true;
                    if (Rng9 > -1)
                    {
                        switch (Rng10)
                        {
                            case 0:
                                yield return PerformAction.Effect(Battle.Player, "DoubleDaJiejie", 0f, "Empty", 0f, PerformAction.EffectBehavior.PlayOneShot, 0f);
                                yield return new CastBlockShieldAction(Battle.Player, (Block.Block * 1f).RoundToInt(), (Shield.Shield * 1.25f).RoundToInt(), BlockShieldType.Direct, false);
                                yield return new DamageAction(Battle.Player, Battle.AllAliveEnemies, DamageInfo.Attack((Damage.Amount * 0.66f).RoundToInt(), true), GunName, GunType.Single);
                                yield return new DamageAction(Battle.Player, Battle.AllAliveEnemies, DamageInfo.Attack((Damage.Amount * 0.66f).RoundToInt(), true), GunName, GunType.Single);
                                yield return new DamageAction(Battle.Player, Battle.AllAliveEnemies, DamageInfo.Attack((Damage.Amount * 0.66f).RoundToInt(), true), GunName, GunType.Single);
                                break;
                            case 1:
                                foreach (BattleAction battleAction in ApplyBuff(Battle.Player, 1f, false, false, true))
                                {
                                    yield return battleAction;
                                }
                                break;
                            case 2:
                                foreach (BattleAction battleAction in ApplyTemporaryBuff(Battle.Player, 1f, true, true, true))
                                {
                                    yield return battleAction;
                                }
                                break;
                            case 3:
                                foreach (Unit enemyUnit in Battle.AllAliveEnemies)
                                {
                                    foreach (BattleAction battleAction in ApplyDebuff(enemyUnit, 1f, false, false, true))
                                    {
                                        yield return battleAction;
                                    }
                                }
                                break;
                            case 4:
                                foreach (Unit enemyUnit in Battle.AllAliveEnemies)
                                {
                                    foreach (BattleAction battleAction in ApplyTemporaryDebuff(enemyUnit, 1f, true, true, true))
                                    {
                                        yield return battleAction;
                                    }
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    if (Rng9 > 1)
                    {
                        switch (Rng11)
                        {
                            case 0:
                                yield return new CastBlockShieldAction(Battle.Player, (Block.Block * 1f).RoundToInt(), 0, BlockShieldType.Direct, false);
                                yield return new DamageAction(Battle.Player, Battle.AllAliveEnemies, DamageInfo.Attack((Damage.Amount * 0.5f).RoundToInt(), true), GunName, GunType.Single);
                                yield return new DamageAction(Battle.Player, Battle.AllAliveEnemies, DamageInfo.Attack((Damage.Amount * 0.5f).RoundToInt(), true), GunName, GunType.Single);
                                yield return new DamageAction(Battle.Player, Battle.AllAliveEnemies, DamageInfo.Attack((Damage.Amount * 0.5f).RoundToInt(), true), GunName, GunType.Single);
                                break;
                            case 1:
                                foreach (BattleAction battleAction in ApplyBuff(Battle.Player, 1f, false, false, true))
                                {
                                    yield return battleAction;
                                }
                                break;
                            case 2:
                                foreach (BattleAction battleAction in ApplyTemporaryBuff(Battle.Player, 1f, true, true, true))
                                {
                                    yield return battleAction;
                                }
                                break;
                            case 3:
                                foreach (Unit enemyUnit in Battle.AllAliveEnemies)
                                {
                                    foreach (BattleAction battleAction in ApplyDebuff(enemyUnit, 1f, false, false, true))
                                    {
                                        yield return battleAction;
                                    }
                                }
                                break;
                            case 4:
                                foreach (Unit enemyUnit in Battle.AllAliveEnemies)
                                {
                                    foreach (BattleAction battleAction in ApplyTemporaryDebuff(enemyUnit, 1f, true, true, true))
                                    {
                                        yield return battleAction;
                                    }
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    /*if (Rng9 > 3)
                    {
                        switch (Rng12)
                        {
                            case 0:
                                List<Card> list = Battle.HandZone.Where((Card hand) => hand != this && hand.CardType != CardType.Tool && !hand.IsCopy).ToList();
                                if (list.Count <= 0)
                                {
                                    break;
                                }
                                SelectHandInteraction interaction = new SelectHandInteraction(0, 1, list)
                                {
                                    Source = this
                                };
                                yield return new InteractionAction(interaction, false);
                                Card origin = interaction.SelectedCards.FirstOrDefault();
                                List<Card> copied = new List<Card>();
                                for (int i = 0; i < 1; i++)
                                {
                                    Card card = origin.CloneBattleCard();
                                    card.SetTurnCost(Mana);
                                    card.IsExile = true;
                                    card.IsEthereal = true;
                                    copied.Add(card);
                                }
                                yield return new AddCardsToHandAction(copied);
                                if (origin.CardType == CardType.Ability || origin.IsExile)
                                {
                                    origin.IsCopy = true;
                                }
                                break;
                            case 1:
                                yield return new GainMoneyAction(Value2 * 2, SpecialSourceType.None);
                                yield return new GainPowerAction(Value2 * 2);
                                break;
                            case 2:
                                yield return new GainManaAction(new ManaGroup() { Philosophy = (Mana.Amount * 1f).RoundToInt() });
                                yield return new GainTurnManaAction(new ManaGroup() { Philosophy = (Mana.Amount * 1f).RoundToInt() });
                                break;
                            case 3:
                                yield return new ScryAction(new ScryInfo() { Count = Scry.Count * 2 });
                                yield return new DrawManyCardAction(Value1);
                                break;
                            case 4:
                                List<Card> getDrawZone = Battle.DrawZone.ToList();
                                if (getDrawZone.Count <= 0 || Battle.MaxHand <= Battle.HandZone.Count)
                                {
                                    break;
                                }
                                SelectHandInteraction interactionDrawZone = new SelectHandInteraction(0, 1, getDrawZone)
                                {
                                    Source = this
                                };
                                yield return new InteractionAction(interactionDrawZone, false);
                                Card cardDrawZone = interactionDrawZone.SelectedCards.FirstOrDefault();
                                yield return new MoveCardAction(cardDrawZone, CardZone.Hand);
                                break;
                            default:
                                break;
                        }
                    }*/
                }
            }
        }
        private IEnumerable<BattleAction> ApplyBuff(Unit unit, float magnitude, bool passive, bool active, bool ultimate)
        {
            if (passive)
            {
                if (Rng13 > 3)
                {
                    Type type = buffs2.OrderBy(x => RngB2).First();
                    StatusEffectConfig config = StatusEffectConfig.FromId(type.Name);
                    yield return new ApplyStatusEffectAction(type, unit, config.HasLevel ? (Value1 * magnitude).ToInt() : 0, config.HasDuration ? (Value1 * magnitude).ToInt() : 0, 0, 0, 0.2f, true);
                }
                else if (Rng13 > 1)
                {
                    Type type = buffs3.OrderBy(x => RngB3).First();
                    StatusEffectConfig config = StatusEffectConfig.FromId(type.Name);
                    if (config.Id == "LoveGirlDamageReduce")
                    {
                        magnitude *= 2;
                    }
                    yield return new ApplyStatusEffectAction(type, unit, config.HasLevel ? (Value2 * magnitude).ToInt() : 0, config.HasDuration ? (Value2 * magnitude).ToInt() : 0, 0, 0, 0.2f, true);
                }
            }
            if (active)
            {
                if (Rng14 > 2)
                {
                    Type type = buffs2.OrderBy(x => RngB2).First();
                    StatusEffectConfig config = StatusEffectConfig.FromId(type.Name);
                    yield return new ApplyStatusEffectAction(type, unit, config.HasLevel ? (Value1 * magnitude).ToInt() : 0, config.HasDuration ? (Value1 * magnitude).ToInt() : 0, 0, 0, 0.2f, true);
                }
                else if (Rng14 > -1)
                {
                    Type type = buffs3.OrderBy(x => RngB3).First();
                    StatusEffectConfig config = StatusEffectConfig.FromId(type.Name);
                    if (config.Id == "LoveGirlDamageReduce")
                    {
                        magnitude *= 2;
                    }
                    yield return new ApplyStatusEffectAction(type, unit, config.HasLevel ? (Value2 * magnitude).ToInt() : 0, config.HasDuration ? (Value2 * magnitude).ToInt() : 0, 0, 0, 0.2f, true);
                }
            }
            if (ultimate)
            {
                Type type = buffs1.OrderBy(x => RngB1).First();
                StatusEffectConfig config = StatusEffectConfig.FromId(type.Name);
                if (config.Id == "LimitedDamage")
                {
                    magnitude *= IsUpgraded ? 5 : Value1 == 1 ? 25 : 15;
                }
                yield return new ApplyStatusEffectAction(type, unit, config.HasLevel ? (Value1 * magnitude).ToInt() : 0, config.HasDuration ? (Value1 * magnitude).ToInt() : 0, 0, 0, 0.2f, true);
                if (config.Id == "BurstUpgrade")
                {
                    yield return new ApplyStatusEffectAction(typeof(Burst), unit, config.HasLevel ? (Value1 * magnitude).ToInt() : 0, config.HasDuration ? (Value1 * magnitude).ToInt() : 0, 0, 0, 0.2f, true);
                }
            }
            yield break;
        }
        private IEnumerable<BattleAction> ApplyDebuff(Unit unit, float magnitude, bool passive, bool active, bool ultimate)
        {
            if (passive)
            {
                if (Rng15 > 3)
                {
                    Type type = debuffs2.OrderBy(x => RngD2).First();
                    StatusEffectConfig config = StatusEffectConfig.FromId(type.Name);
                    yield return new ApplyStatusEffectAction(type, unit, config.HasLevel ? (Value1 * magnitude).ToInt() : 0, config.HasDuration ? (Value1 * magnitude).ToInt() : 0, 0, 0, 0.2f, true);
                }
                else if (Rng15 > 1)
                {
                    Type type = debuffs3.OrderBy(x => RngD3).First();
                    StatusEffectConfig config = StatusEffectConfig.FromId(type.Name);
                    yield return new ApplyStatusEffectAction(type, unit, config.HasLevel ? (Value2 * magnitude).ToInt() : 0, config.HasDuration ? (Value2 * magnitude).ToInt() : 0, 0, 0, 0.2f, true);
                }
            }
            if (active)
            {
                if (Rng16 > 2)
                {
                    Type type = debuffs2.OrderBy(x => RngD2).First();
                    StatusEffectConfig config = StatusEffectConfig.FromId(type.Name);
                    yield return new ApplyStatusEffectAction(type, unit, config.HasLevel ? (Value1 * magnitude).ToInt() : 0, config.HasDuration ? (Value1 * magnitude).ToInt() : 0, 0, 0, 0.2f, true);
                }
                else if (Rng16 > -1)
                {
                    Type type = debuffs3.OrderBy(x => RngD3).First();
                    StatusEffectConfig config = StatusEffectConfig.FromId(type.Name);
                    magnitude *= 2.5f;
                    yield return new ApplyStatusEffectAction(type, unit, config.HasLevel ? (Value2 * magnitude).ToInt() : 0, config.HasDuration ? (Value2 * magnitude).ToInt() : 0, 0, 0, 0.2f, true);
                }
            }
            if (ultimate)
            {
                if (Rng17 > 2)
                {
                    Type type = debuffs2.OrderBy(x => RngD2).First();
                    StatusEffectConfig config = StatusEffectConfig.FromId(type.Name);
                    magnitude *= 2f;
                    yield return new ApplyStatusEffectAction(type, unit, config.HasLevel ? (Value1 * magnitude).ToInt() : 0, config.HasDuration ? (Value1 * magnitude).ToInt() : 0, 0, 0, 0.2f, true);
                }
                else if (Rng17 > -1)
                {
                    Type type = debuffs3.OrderBy(x => RngD3).First();
                    StatusEffectConfig config = StatusEffectConfig.FromId(type.Name);
                    magnitude *= 5f;
                    yield return new ApplyStatusEffectAction(type, unit, config.HasLevel ? (Value2 * magnitude).ToInt() : 0, config.HasDuration ? (Value2 * magnitude).ToInt() : 0, 0, 0, 0.2f, true);
                }
            }
            yield break;
        }
        private IEnumerable<BattleAction> ApplyTemporaryBuff(Unit unit, float magnitude, bool passive, bool active, bool ultimate)
        {
            if (passive)
            {
                if (Rng18 > 2)
                {
                    Type type = tempBuffsP1.OrderBy(x => RngP1).First();
                    StatusEffectConfig config = StatusEffectConfig.FromId(type.Name);
                    yield return new ApplyStatusEffectAction(type, unit, config.HasLevel ? (Value1 * magnitude).ToInt() : 0, config.HasDuration ? (Value1 * magnitude).ToInt() : 0, 0, 0, 0.2f, true);
                }
                else if (Rng18 > -1)
                {
                    Type type = tempBuffsP2.OrderBy(x => RngP2).First();
                    StatusEffectConfig config = StatusEffectConfig.FromId(type.Name);
                    if (config.Id == "Reflect")
                    {
                        magnitude *= 2;
                    }
                    yield return new ApplyStatusEffectAction(type, unit, config.HasLevel ? (Value2 * magnitude).ToInt() : 0, config.HasDuration ? (Value2 * magnitude).ToInt() : 0, 0, 0, 0.2f, true);
                }
            }
            if (active)
            {
                if (Rng19 > 2)
                {
                    Type type = tempBuffsA1.OrderBy(x => RngA1).First();
                    StatusEffectConfig config = StatusEffectConfig.FromId(type.Name);
                    magnitude *= 0.5f;
                    if (config.Id == "Amulet" || config.Id == "AmuletForCard")
                    {
                        yield return new RemoveAllNegativeStatusEffectAction(unit);
                    }
                    yield return new ApplyStatusEffectAction(type, unit, config.HasLevel ? (Value1 * magnitude).ToInt() : 0, config.HasDuration ? (Value1 * magnitude).ToInt() : 0, 0, config.Id == "ZhenmiaowanAttackSe" ? 1 : 0, 0.2f, true);
                }
                else if (Rng19 > -1)
                {
                    Type type = tempBuffsA2.OrderBy(x => RngA2).First();
                    StatusEffectConfig config = StatusEffectConfig.FromId(type.Name);
                    yield return new ApplyStatusEffectAction(type, unit, config.HasLevel ? (Value2 * magnitude).ToInt() : 0, config.HasDuration ? (Value2 * magnitude).ToInt() : 0, 0, 0, 0.2f, true);
                }
            }
            if (ultimate)
            {
                if (Rng20 > 2)
                {
                    Type type = tempBuffsU1.OrderBy(x => RngU1).First();
                    StatusEffectConfig config = StatusEffectConfig.FromId(type.Name);
                    magnitude *= 0.75f;
                    yield return new ApplyStatusEffectAction(type, unit, config.HasLevel ? (Value1 * magnitude).ToInt() : 0, config.HasDuration ? (Value1 * magnitude).ToInt() : 0, 0, 0, 0.2f, true);
                }
                else if (Rng20 > -1)
                {
                    Type type = tempBuffsU2.OrderBy(x => RngU2).First();
                    StatusEffectConfig config = StatusEffectConfig.FromId(type.Name);
                    magnitude *= 0.75f;
                    yield return new ApplyStatusEffectAction(type, unit, config.HasLevel ? (Value2 * magnitude).ToInt() : 0, config.HasDuration ? (Value2 * magnitude).ToInt() : 0, 0, config.Id == "ZhenmiaowanAttackSe" ? 1 : 0, 0.2f, true);
                }
            }
            yield break;
        }
        private IEnumerable<BattleAction> ApplyTemporaryDebuff(Unit unit, float magnitude, bool passive, bool active, bool ultimate)
        {
            if (passive)
            {
                if (Rng21 > 2)
                {
                    Type type = tempDebuffs2.OrderBy(x => RngT2).First();
                    StatusEffectConfig config = StatusEffectConfig.FromId(type.Name);
                    yield return new ApplyStatusEffectAction(type, unit, config.HasLevel ? (Value1 * magnitude).ToInt() : 0, config.HasDuration ? (Value1 * magnitude).ToInt() : 0, 0, 0, 0.2f, true);
                }
                else if (Rng21 > -1)
                {
                    Type type = tempDebuffs3.OrderBy(x => RngT3).First();
                    StatusEffectConfig config = StatusEffectConfig.FromId(type.Name);
                    magnitude *= 1.5f;
                    yield return new ApplyStatusEffectAction(type, unit, config.HasLevel ? (Value2 * magnitude).ToInt() : 0, config.HasDuration ? (Value2 * magnitude).ToInt() : 0, 0, 0, 0.2f, true);
                }
            }
            if (active)
            {
                if (Rng22 > 2)
                {
                    Type type = tempDebuffs2.OrderBy(x => RngT2).First();
                    StatusEffectConfig config = StatusEffectConfig.FromId(type.Name);
                    yield return new ApplyStatusEffectAction(type, unit, config.HasLevel ? (Value1 * magnitude).ToInt() : 0, config.HasDuration ? (Value1 * magnitude).ToInt() : 0, 0, 0, 0.2f, true);
                }
                else if (Rng22 > -1)
                {
                    Type type = tempDebuffs3.OrderBy(x => RngT3).First();
                    StatusEffectConfig config = StatusEffectConfig.FromId(type.Name);
                    magnitude *= 2f;
                    yield return new ApplyStatusEffectAction(type, unit, config.HasLevel ? (Value2 * magnitude).ToInt() : 0, config.HasDuration ? (Value2 * magnitude).ToInt() : 0, 0, 0, 0.2f, true);
                }
            }
            if (ultimate)
            {
                Type type = tempDebuffs1.OrderBy(x => RngT1).First();
                StatusEffectConfig config = StatusEffectConfig.FromId(type.Name);
                yield return new ApplyStatusEffectAction(type, unit, config.HasLevel ? (Value1 * magnitude).ToInt() : 0, config.HasDuration ? (Value1 * magnitude).ToInt() : 0, 0, 0, 0.2f, true);
            }
            yield break;
        }
    }
}
