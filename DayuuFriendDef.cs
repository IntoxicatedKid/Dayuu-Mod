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
using LBoL.EntityLib.StatusEffects.Cirno;
using LBoL.Core.Battle.Interactions;
using LBoL.EntityLib.StatusEffects.Others;
using System.Linq;
using UnityEngine;

namespace DayuuMod
{
    public sealed class DayuuFriendDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(DayuuFriend);
        }

        public override CardImages LoadCardImages()
        {
            var imgs = new CardImages(BepinexPlugin.embeddedSource);
            imgs.AutoLoad(this, extension: ".png");
            return imgs;
        }

        public override LocalizationOption LoadLocalization()
        {
            var loc = new GlobalLocalization(BepinexPlugin.embeddedSource);
            loc.LocalizationFiles.AddLocaleFile(LBoL.Core.Locale.En, "CardsEn.yaml");
            return loc;
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = new CardConfig(
               Index: 12004,
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
               Rarity: Rarity.Rare,
               Type: CardType.Friend,
               TargetType: TargetType.Nobody,
               Colors: new List<ManaColor>() { ManaColor.Black, ManaColor.Red, ManaColor.Green },
               IsXCost: false,
               Cost: new ManaGroup() { Any = 2, Black = 1, Red = 1, Green = 1 },
               UpgradedCost: null,
               MoneyCost: null,
               Damage: null,
               UpgradedDamage: null,
               Block: null,
               UpgradedBlock: null,
               Shield: null,
               UpgradedShield: null,
               Value1: 2,
               UpgradedValue1: 2,
               Value2: 6,
               UpgradedValue2: 6,
               Mana: null,
               UpgradedMana: null,
               Scry: null,
               UpgradedScry: null,
               ToolPlayableTimes: null,
               Loyalty: 9,
               UpgradedLoyalty: 9,
               PassiveCost: -1,
               UpgradedPassiveCost: 0,
               ActiveCost: -3,
               UpgradedActiveCost: -3,
               UltimateCost: -6,
               UpgradedUltimateCost: -6,

               Keywords: Keyword.None,
               UpgradedKeywords: Keyword.None,
               EmptyDescription: false,
               RelativeKeyword: Keyword.Power,
               UpgradedRelativeKeyword: Keyword.Power,

               RelativeEffects: new List<string>() { "TempFirepowerNegative", "FirepowerNegative", "Weak", "Vulnerable" },
               UpgradedRelativeEffects: new List<string>() { "TempFirepowerNegative", "FirepowerNegative", "Weak", "Vulnerable" },
               RelativeCards: new List<string>() { },
               UpgradedRelativeCards: new List<string>() { },
               Owner: null,
               Unfinished: false,
               Illustrator: "Liz Triangle",
               SubIllustrator: new List<string>() { }
            );

            return cardConfig;
        }
    }
    [EntityLogic(typeof(DayuuFriendDef))]
    public sealed class DayuuFriend : Card
    {
        public override IEnumerable<BattleAction> OnTurnEndingInHand()
        {
            return this.GetPassiveActions();
        }
        public override IEnumerable<BattleAction> GetPassiveActions()
        {
            if (!base.Summoned || base.Battle.BattleShouldEnd)
            {
                yield break;
            }
            base.NotifyActivating();
            base.Loyalty += base.PassiveCost;
            int num;
            for (int i = 0; i < base.Battle.FriendPassiveTimes; i = num + 1)
            {
                if (base.Battle.BattleShouldEnd)
                {
                    yield break;
                }
                foreach (BattleAction battleAction in base.DebuffAction<TempFirepowerNegative>(base.Battle.AllAliveEnemies, base.Value1, 0, 0, 0, true, 0.2f))
                {
                    yield return battleAction;
                }
                foreach (BattleAction battleAction2 in base.DebuffAction<TempFirepowerNegative>(base.Battle.AllAliveEnemies, base.Value1, 0, 0, 0, true, 0.2f))
                {
                    yield return battleAction2;
                }
                if (base.Loyalty <= 0)
                {
                    yield return new RemoveCardAction(this);
                    yield break;
                }
                num = i;
            }
            yield break;
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (precondition == null || ((MiniSelectCardInteraction)precondition).SelectedCard.FriendToken == FriendToken.Active)
            {
                base.Loyalty += base.ActiveCost;
                yield return PerformAction.Effect(base.Battle.Player, "Wave1s", 0f, "BirdSing", 0f, PerformAction.EffectBehavior.PlayOneShot, 0f);
                foreach (BattleAction battleAction in base.DebuffAction<FirepowerNegative>(base.Battle.AllAliveEnemies, base.Value1, 0, 0, 0, true, 0.2f))
                {
                    yield return battleAction;
                }
                foreach (BattleAction battleAction2 in base.DebuffAction<Weak>(base.Battle.AllAliveEnemies, 0, base.Value1, 0, 0, true, 0.2f))
                {
                    yield return battleAction2;
                }
                if (!this.IsUpgraded)
                {
                    yield return new GainPowerAction(10);
                }
                else
                {
                    yield return new GainPowerAction(15);
                }
            }
            else
            {
                base.Loyalty += base.UltimateCost;
                base.UltimateUsed = true;
                yield return PerformAction.Sfx("BirdSing", 0f);
                yield return PerformAction.Effect(base.Battle.Player, "JunkoNightmare", 0f, "JunkoNightmare", 0f, PerformAction.EffectBehavior.PlayOneShot, 0f);
                foreach (EnemyUnit enemyUnit in base.Battle.AllAliveEnemies)
                {
                    if (enemyUnit.Hp <= (enemyUnit.MaxHp) / 3)
                    {
                        yield return new ForceKillAction(base.Battle.Player, enemyUnit);
                    }
                }
                foreach (BattleAction battleAction in base.DebuffAction<FirepowerNegative>(base.Battle.AllAliveEnemies, base.Value2, 0, 0, 0, true, 0.2f))
                {
                    yield return battleAction;
                }
                foreach (BattleAction battleAction2 in base.DebuffAction<Weak>(base.Battle.AllAliveEnemies, 0, base.Value2, 0, 0, true, 0.2f))
                {
                    yield return battleAction2;
                }
                foreach (BattleAction battleAction3 in base.DebuffAction<Vulnerable>(base.Battle.AllAliveEnemies, 0, base.Value2, 0, 0, true, 0.2f))
                {
                    yield return battleAction3;
                }
                if (!this.IsUpgraded)
                {
                    yield return new GainPowerAction(20);
                }
                else
                {
                    yield return new GainPowerAction(30);
                }
            }
            yield break;
        }
        public override IEnumerable<BattleAction> AfterUseAction()
        {
            if (!base.Summoned)
            {
                yield break;
            }
            if (base.Loyalty <= 0)
            {
                yield return new RemoveCardAction(this);
                yield break;
            }
            yield return new MoveCardAction(this, CardZone.Hand);
            yield break;
        }
    }
}
