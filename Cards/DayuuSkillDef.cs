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
using System.Linq;
using LBoL.Presentation.UI.Panels;
using static DayuuMod.BepinexPlugin;

namespace DayuuMod
{
    public sealed class DayuuSkillDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(DayuuSkill);
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
               Type: CardType.Skill,
               TargetType: TargetType.Self,
               Colors: new List<ManaColor>() { ManaColor.Red },
               IsXCost: false,
               Cost: new ManaGroup() { Any = 2, Red = 1 },
               UpgradedCost: null,
               MoneyCost: null,
               Damage: null,
               UpgradedDamage: null,
               Block: null,
               UpgradedBlock: null,
               Shield: null,
               UpgradedShield: null,
               Value1: 3,
               UpgradedValue1: 5,
               Value2: 2,
               UpgradedValue2: null,
               Mana: new ManaGroup() { Any = 0 },
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
               RelativeKeyword: Keyword.Misfortune,
               UpgradedRelativeKeyword: Keyword.Misfortune,

               RelativeEffects: new List<string>() { "TempFirepower", "DayuuExodiaSe" },
               UpgradedRelativeEffects: new List<string>() { "TempFirepower", "DayuuExodiaSe" },
               RelativeCards: new List<string>() { "DayuuExodia" },
               UpgradedRelativeCards: new List<string>() { "DayuuExodia" },
               Owner: null,
               Unfinished: false,
               Illustrator: "Moja4192",
               SubIllustrator: new List<string>() { "MIO" }
            );

            return cardConfig;
        }
    }
    [EntityLogic(typeof(DayuuSkillDef))]
    public sealed class DayuuSkill : Card
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return base.BuffAction<TempFirepower>(base.Value1, 0, 0, 0, 0.2f);
            if (!this.IsUpgraded)
            {
                foreach (EnemyUnit enemyUnit in base.Battle.AllAliveEnemies)
                {
                    yield return new ApplyStatusEffectAction<TempFirepower>(enemyUnit, base.Value1, null, null, null, 0.2f, true);
                }
            }
            else
            {
                foreach (EnemyUnit enemyUnit in base.Battle.AllAliveEnemies)
                {
                    yield return new ApplyStatusEffectAction<TempFirepower>(enemyUnit, base.Value2, null, null, null, 0.2f, true);
                }
            }
            List<Card> playable = null;
            DrawManyCardAction drawAction = new DrawManyCardAction(base.Value2);
            yield return drawAction;
            IReadOnlyList<Card> drawnCards = drawAction.DrawnCards;
            if (drawnCards != null && drawnCards.Count > 0)
            {
                //List<Card> negative = drawnCards.Where((Card card) => (card.CardType == CardType.Status) || (card.CardType == CardType.Misfortune)).ToList<Card>();
                //if (negative.Count > 0)
                //{
                //    yield return new ExileManyCardAction(negative);
                //}
                playable = drawnCards.Where((Card card) => (card.CardType == CardType.Attack) && !card.IsForbidden).ToList<Card>();
                foreach (Card card in playable)
                {
                    card.SetTurnCost(base.Mana);
                    //yield return new UseCardAction(card, selector, consumingMana);
                }
            }
            yield break;
        }
    }
}
