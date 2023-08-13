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

namespace DayuuMod
{
    public sealed class DayuuDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(Dayuu);
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
               Index: 12003,
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
               Type: CardType.Skill,
               TargetType: TargetType.AllEnemies,
               Colors: new List<ManaColor>() { ManaColor.Black, ManaColor.Red, ManaColor.Green },
               IsXCost: true,
               Cost: new ManaGroup() { Black = 1, Red = 1, Green = 1 },
               UpgradedCost: null,
               MoneyCost: null,
               Damage: null,
               UpgradedDamage: null,
               Block: null,
               UpgradedBlock: null,
               Shield: null,
               UpgradedShield: null,
               Value1: 999,
               UpgradedValue1: 999,
               Value2: null,
               UpgradedValue2: null,
               Mana: null,
               UpgradedMana: new ManaGroup() { Philosophy = 1 },
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

               Keywords: Keyword.Exile,
               UpgradedKeywords: Keyword.Exile,
               EmptyDescription: false,
               RelativeKeyword: Keyword.Philosophy,
               UpgradedRelativeKeyword: Keyword.Philosophy,

               RelativeEffects: new List<string>() { },
               UpgradedRelativeEffects: new List<string>() { },
               RelativeCards: new List<string>() { },
               UpgradedRelativeCards: new List<string>() { },
               Owner: null,
               Unfinished: false,
               Illustrator: "MIO",
               SubIllustrator: new List<string>() { }
            );

            return cardConfig;
        }
    }
    [EntityLogic(typeof(DayuuDef))]
    public sealed class Dayuu : Card
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (SynergyAmountComplexMana(consumingMana, new ManaGroup() { White = 1, Blue = 1, Black = 1, Red = 1, Green = 1, Colorless = 1, Philosophy = 1}) > 0)
            {
                foreach (Unit unit in selector.GetUnits(base.Battle))
                if (unit.Hp <= base.Value1)
                {
                    yield return new ForceKillAction(base.Battle.Player, unit);
                }
            }
            yield break;
        }
        public override IEnumerable<BattleAction> OnDraw()
        {
            if (this.IsUpgraded)
            {
                yield return new GainManaAction(base.Mana);
            }
        }
    }
}
