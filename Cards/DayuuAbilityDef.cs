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

namespace DayuuMod.Cards
{
    public sealed class DayuuAbilityDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(DayuuAbility);
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
        // CardConfig.FromId(new DayuuAbilityDef().UniqueId) 
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
               Value2: 1,
               UpgradedValue2: 2,
               Mana: null,
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
               RelativeKeyword: Keyword.None,
               UpgradedRelativeKeyword: Keyword.None,

               RelativeEffects: new List<string>() { "DayuuFriendSe" },
               UpgradedRelativeEffects: new List<string>() { "DayuuFriendSe" },
               RelativeCards: new List<string>() { "DayuuFriend", "DayuuExodia" },
               UpgradedRelativeCards: new List<string>() { "DayuuFriend", "DayuuExodia" },
               Owner: null,
               Unfinished: false,
               Illustrator: "Roke",
               SubIllustrator: new List<string>() { "MIO" }
            );

            return cardConfig;
        }
    }
    [EntityLogic(typeof(DayuuAbilityDef))]
    public sealed class DayuuAbility : Card
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return BuffAction<DayuuAbilitySeDef.DayuuAbilitySe>(Value2, 0, 0, Value1, 0.2f);
            yield break;
        }
    }
    public sealed class DayuuAbilitySeDef : StatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(DayuuAbilitySe);
        }

        public override LocalizationOption LoadLocalization()
        {
            var locFiles = new LocalizationFiles(embeddedSource);
            locFiles.AddLocaleFile(Locale.En, "Resources.StatusEffectsEn.yaml");
            return locFiles;
        }

        public override Sprite LoadSprite()
        {
            return ResourceLoader.LoadSprite("Resources.DayuuAbilitySe.png", embeddedSource);
        }
        // if some information is needed from this config it can be accessed by calling
        // StatusEffectConfig.FromId(new DayuuAbilitySeDefinition().UniqueId) 
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
                CountStackType: StackType.Add,
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
        [EntityLogic(typeof(DayuuAbilitySeDef))]
        public sealed class DayuuAbilitySe : StatusEffect
        {
            protected override void OnAdded(Unit unit)
            {
                ReactOwnerEvent(Battle.Player.TurnStarted, new EventSequencedReactor<UnitEventArgs>(OnTurnStarted));
            }
            private IEnumerable<BattleAction> OnTurnStarted(UnitEventArgs args)
            {
                if (!Battle.BattleShouldEnd)
                {
                    NotifyActivating();
                    List<Card> list = Battle.HandZone.Where((card) => card is DayuuAttack || card is DayuuDefense || card is DayuuSkill || card is DayuuAbility || card is DayuuFriend || card is DayuuFriend2 || card.CardType == CardType.Friend).ToList();
                    ManaGroup manaGroup = ManaGroup.Empty;
                    for (int i = 0; i < Count; i++)
                    {
                        manaGroup += ManaGroup.Single(ManaColors.Colors.Sample(GameRun.BattleRng));
                    }
                    if (list.Count > 0)
                    {
                        for (int i = 0; i < Level * list.Count; i++)
                        {
                            manaGroup += ManaGroup.Single(ManaColors.Colors.Sample(GameRun.BattleRng));
                        }
                    }
                    yield return new GainManaAction(manaGroup);
                }
                yield break;
            }
        }
    }
}