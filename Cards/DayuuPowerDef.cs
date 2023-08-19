using LBoL.ConfigData;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using System;
using System.Collections.Generic;
using System.Text;
using static DayuuMod.BepinexPlugin;
using UnityEngine;
using LBoL.Core;
using LBoL.Base;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Base.Extensions;
using System.Collections;
using LBoL.Presentation;
using LBoL.EntityLib.Cards.Neutral.Blue;
using HarmonyLib;
using LBoL.Core.StatusEffects;
using UnityEngine.Rendering;
using LBoL.Core.Units;
using LBoL.EntityLib.Exhibits.Shining;
using Mono.Cecil;
using JetBrains.Annotations;
using System.Linq;
using LBoL.EntityLib.StatusEffects.Neutral.Black;
using DayuuMod;

namespace DayuuMod
{
    public sealed class DayuuPowerDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(DayuuPower);
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
        // if some information is needed from this config it can be accessed by calling
        // CardConfig.FromId(new DayuuPowerDef().UniqueId) 
        public override CardConfig MakeConfig()
        {
            var cardConfig = new CardConfig(
                Index: sequenceTable.Next(typeof(CardConfig)),
                Id: "",
                Order: 10,
                AutoPerform: true,
                Perform: new string[0][],
                GunName: "Simple1",
                GunNameBurst: "Simple1",
                DebugLevel: 0,
                Revealable: false,
                IsPooled: true,
                HideMesuem: false,
                IsUpgradable: true,
                Rarity: Rarity.Uncommon,
                Type: CardType.Ability,
                TargetType: TargetType.Self,
                Colors: new List<ManaColor>() { ManaColor.Green },
                IsXCost: false,
                Cost: new ManaGroup() { Any = 2, Green = 1 },
                UpgradedCost: null,
                MoneyCost: null,
                Damage: null,
                UpgradedDamage: null,
                Block: null,
                UpgradedBlock: null,
                Shield: null,
                UpgradedShield: null,
                Value1: 1,
                UpgradedValue1: null,
                Value2: 2,
                UpgradedValue2: 4,
                Mana: new ManaGroup() { Any = 1 },
                UpgradedMana: null,
                Scry: null,
                UpgradedScry: null,
                ToolPlayableTimes: null,

                Loyalty: null,
                UpgradedLoyalty: null,
                PassiveCost: null,
                UpgradedPassiveCost: null,
                ActiveCost: null,
                UpgradedActiveCost: null,
                UltimateCost: null,
                UpgradedUltimateCost: null,

                Keywords: Keyword.None,
                UpgradedKeywords: Keyword.None,
                EmptyDescription: false,
                RelativeKeyword: Keyword.FriendCard,
                UpgradedRelativeKeyword: Keyword.FriendCard,

                RelativeEffects: new List<string>() { "DayuuExodiaSe" },
                UpgradedRelativeEffects: new List<string>() { "DayuuExodiaSe" },
                RelativeCards: new List<string>() { "DayuuFriend", "DayuuExodia" },
                UpgradedRelativeCards: new List<string>() { "DayuuFriend", "DayuuExodia" },
                Owner: null,
                Unfinished: false,
                Illustrator: "Roke",
                SubIllustrator: new List<string>() { "MIO" }
             );

            return cardConfig;
        }
        [EntityLogic(typeof(DayuuPowerDef))]
        public sealed class DayuuPower : Card
        {
            protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
            {
                yield return base.BuffAction<DayuuPowerSeDef.DayuuPowerSe>(base.Value2, 0, 0, 1, 0.2f);
                yield break;
            }
        }

    }
    public sealed class DayuuPowerSeDef : StatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(DayuuPowerSe);
        }

        public override LocalizationOption LoadLocalization()
        {
            var locFiles = new LocalizationFiles(embeddedSource);
            locFiles.AddLocaleFile(Locale.En, "Resources.StatusEffectsEn.yaml");
            return locFiles;
        }

        public override Sprite LoadSprite()
        {
            return ResourceLoader.LoadSprite("Resources.DayuuPowerSe.png", embeddedSource);
        }
        // if some information is needed from this config it can be accessed by calling
        // StatusEffectConfig.FromId(new DayuuPowerSeDefinition().UniqueId) 
        public override StatusEffectConfig MakeConfig()
        {
            var statusEffectConfig = new StatusEffectConfig(
                Id: "",
                Order: 10,
                Type: StatusEffectType.Positive,
                IsVerbose: false,
                IsStackable: true,
                StackActionTriggerLevel: null,
                HasLevel: true,
                LevelStackType: StackType.Add,
                HasDuration: false,
                DurationStackType: StackType.Add,
                DurationDecreaseTiming: DurationDecreaseTiming.Custom,
                HasCount: true,
                CountStackType: StackType.Keep,
                LimitStackType: StackType.Keep,
                ShowPlusByLimit: false,
                Keywords: Keyword.None,
                RelativeEffects: new List<string>() { },
                VFX: "Default",
                VFXloop: "Default",
                SFX: "Default"
            );
            return statusEffectConfig;
        }
        [EntityLogic(typeof(DayuuPowerSeDef))]
        public sealed class DayuuPowerSe : StatusEffect
        {
            protected override void OnAdded(Unit unit)
            {
                base.ReactOwnerEvent<UnitEventArgs>(base.Battle.Player.TurnStarted, new EventSequencedReactor<UnitEventArgs>(this.OnTurnStarted));
            }
            private IEnumerable<BattleAction> OnTurnStarted(UnitEventArgs args)
            {
                if (!base.Battle.BattleShouldEnd)
                {
                    List<Card> list = base.Battle.HandZone.Where((Card card) => (card.CardType == CardType.Friend) && card.Summoned && !(card is DayuuFriend)).ToList<Card>();
                    List<Card> list2 = base.Battle.HandZone.Where((Card card) => (card is DayuuFriend) && card.Summoned).ToList<Card>();
                    if (list.Count > 0 || list2.Count > 0)
                        base.NotifyActivating();
                    ManaGroup manaGroup = ManaGroup.Single(ManaColors.Colors.Sample(base.GameRun.BattleRng));
                    yield return new GainManaAction(manaGroup * (base.Count * list.Count));
                    yield return new GainManaAction(manaGroup * (base.Level * list2.Count));
                }
                yield break;
            }
        }
    }
}