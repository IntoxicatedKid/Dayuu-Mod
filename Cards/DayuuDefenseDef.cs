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
using static DayuuMod.BepinexPlugin;
using System.Linq;
using LBoL.Presentation.UI.Panels;
using Mono.Cecil;

namespace DayuuMod
{
    public sealed class DayuuDefenseDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(DayuuDefense);
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
               GunName: "Simple1",
               GunNameBurst: "Simple1",
               DebugLevel: 0,
               Revealable: false,
               IsPooled: true,
               HideMesuem: false,
               IsUpgradable: true,
               Rarity: Rarity.Uncommon,
               Type: CardType.Defense,
               TargetType: TargetType.Self,
               Colors: new List<ManaColor>() { ManaColor.Blue },
               IsXCost: false,
               Cost: new ManaGroup() { Any = 2, Blue = 1 },
               UpgradedCost: null,
               MoneyCost: null,
               Damage: null,
               UpgradedDamage: null,
               Block: 20,
               UpgradedBlock: 26,
               Shield: null,
               UpgradedShield: null,
               Value1: 2,
               UpgradedValue1: 1,
               Value2: null,
               UpgradedValue2: null,
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
               RelativeKeyword: Keyword.Block,
               UpgradedRelativeKeyword: Keyword.Block,

               RelativeEffects: new List<string>() { "DayuuFriendSe" },
               UpgradedRelativeEffects: new List<string>() { "DayuuFriendSe" },
               RelativeCards: new List<string>() { "DayuuExodia" },
               UpgradedRelativeCards: new List<string>() { "DayuuExodia" },
               Owner: null,
               Unfinished: false,
               Illustrator: "Fun Bo",
               SubIllustrator: new List<string>() { "MIO" }
            );

            return cardConfig;
        }
    }
    [EntityLogic(typeof(DayuuDefenseDef))]
    public sealed class DayuuDefense : Card
    {
        public override bool DiscardCard
        {
            get
            {
                return true;
            }
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return base.SacrificeAction(base.Value1);
            yield return base.DefenseAction(true);
            if (base.Battle.DrawZone.Count > 0)
            {
                Card drawzone = base.Battle.DrawZone.First<Card>();
                yield return new MoveCardAction(drawzone, CardZone.Hand);
                if ((drawzone.CardType == CardType.Attack) && (drawzone.Zone == CardZone.Hand))
                {
                    yield return new DiscardAction(drawzone);
                }
            }
            if (base.Battle.DiscardZone.Count > 0)
            {
                Card discardzone = base.Battle.DiscardZone.Last<Card>();
                yield return new MoveCardAction(discardzone, CardZone.Hand);
                if ((discardzone.CardType == CardType.Attack) && (discardzone.Zone == CardZone.Hand))
                {
                    yield return new DiscardAction(discardzone);
                }
            }
            yield break;
        }
    }
}
