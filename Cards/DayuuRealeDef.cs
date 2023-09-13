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
using System.Security.Cryptography;
using LBoL.EntityLib.JadeBoxes;
using static DayuuMod.BepinexPlugin;
using UnityEngine.Playables;
using LBoL.EntityLib.Cards.Neutral.NoColor;
using LBoL.EntityLib.Cards.Character.Cirno.Friend;
using LBoL.EntityLib.Cards.Character.Reimu;
using LBoL.EntityLib.Cards.Character.Sakuya;
using LBoL.EntityLib.Cards.Neutral.MultiColor;
using LBoL.Presentation.UI.Panels;
using LBoL.Core.GapOptions;
using Mono.Cecil;
using DayuuMod;
using LBoL.Core.SaveData;
using UnityEngine.Assertions.Must;
using LBoL.Presentation;

namespace DayuuMod
{
    public sealed class DayuuRealeDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(DayuuReale);
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
               Rarity: Rarity.Rare,
               Type: CardType.Friend,
               TargetType: TargetType.Nobody,
               Colors: new List<ManaColor>() { ManaColor.Black },
               IsXCost: false,
               Cost: new ManaGroup() { Any = 3, Black = 2 },
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
               UpgradedValue2: null,
               Mana: null,
               UpgradedMana: null,
               Scry: null,
               UpgradedScry: null,
               ToolPlayableTimes: null,
               Loyalty: 1,
               UpgradedLoyalty: 1,
               PassiveCost: 2,
               UpgradedPassiveCost: 2,
               ActiveCost: 2,
               UpgradedActiveCost: 2,
               UltimateCost: -7,
               UpgradedUltimateCost: -7,

               Keywords: Keyword.None,
               UpgradedKeywords: Keyword.None,
               EmptyDescription: false,
               RelativeKeyword: Keyword.None,
               UpgradedRelativeKeyword: Keyword.None,

               RelativeEffects: new List<string>() { "Weak", "Fragil", "Vulnerable", "DayuuFriendSe" },
               UpgradedRelativeEffects: new List<string>() { "Weak", "Fragil", "Vulnerable", "DayuuFriendSe" },
               RelativeCards: new List<string>() { "DayuuFriend2+" },
               UpgradedRelativeCards: new List<string>() { "DayuuFriend2+" },
               Owner: null,
               Unfinished: false,
               Illustrator: "Reale雷耀",
               SubIllustrator: new List<string>() { "Cruezor" }
            );

            return cardConfig;
        }
    }
    [EntityLogic(typeof(DayuuRealeDef))]
    public sealed class DayuuReale : Card
    {
        public override bool OnDrawVisual
        {
            get
            {
                return false;
            }
        }
        protected override void OnEnterBattle(BattleController battle)
        {
            //base.ReactBattleEvent<GameEventArgs>(base.Battle.BattleStarted, new EventSequencedReactor<GameEventArgs>(this.OnBattleStarted));
            base.ReactBattleEvent<CardUsingEventArgs>(base.Battle.CardUsed, new EventSequencedReactor<CardUsingEventArgs>(this.OnCardUsed));
            //base.ReactBattleEvent<GameEventArgs>(base.Battle.BattleEnding, new EventSequencedReactor<GameEventArgs>(this.OnBattleEnding));
        }
        public override IEnumerable<BattleAction> OnDraw()
        {
            if (drawn == false)
            {
                if (!GameMaster.Instance.CurrentProfile.Name.Equals("Dayuu") && !this.IsUpgraded)
                {
                    drawn = true;
                    this.NotifyActivating();
                    yield return new ExileCardAction(this);
                }
                else
                {
                    drawn = true;
                    yield return new DiscardAction(this);
                    yield return new DrawManyCardAction(2);
                }
            }
            yield break;
        }
        /*private IEnumerable<BattleAction> OnBattleStarted(GameEventArgs args)
        {
            if (!GameMaster.Instance.CurrentProfile.Name.Equals("Dayuu") && !this.IsUpgraded)
            {
                this.NotifyActivating();
                yield return new ExileCardAction(this);
                yield break;
            }
        }*/
        private IEnumerable<BattleAction> OnCardUsed(CardUsingEventArgs args)
        {
            if ((base.Zone == CardZone.Hand) && base.Summoned && (base.Loyalty >= 7))
            {
                this.NotifyActivating();
                base.Loyalty += base.UltimateCost;
                base.UltimateUsed = true;
                Card friend = Library.CreateCard<DayuuFriend2>();
                friend.IsUpgraded = true;
                friend.Summon();
                yield return new AddCardsToHandAction(friend);
                yield return base.DebuffAction<Weak>(base.Battle.Player, 0, base.Value1, 0, 0, true, 0.2f);
                yield return base.DebuffAction<Fragil>(base.Battle.Player, 0, base.Value1, 0, 0, true, 0.2f);
                yield return base.DebuffAction<Vulnerable>(base.Battle.Player, 0, base.Value1, 0, 0, true, 0.2f);
                yield return new RemoveCardAction(this);
            }
            yield break;
        }
        public override IEnumerable<BattleAction> OnTurnStartedInHand()
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
                List<Card> list = base.Battle.RollCardsWithoutManaLimit(new CardWeightTable(RarityWeightTable.BattleCard, OwnerWeightTable.AllOnes, CardTypeWeightTable.CanBeLoot), base.Value1, (CardConfig config) => config.RelativeCards.Contains("DayuuExodia") && config.Id != base.Id).ToList<Card>();
                foreach (Card card in list)
                {
                    card.SetBaseCost(ManaGroup.Anys(card.ConfigCost.Amount));
                }
                yield return new AddCardsToHandAction(list);
                num = i;
            }
            if (this.Loyalty >= 7)
            {
                this.NotifyActivating();
                base.Loyalty += base.UltimateCost;
                base.UltimateUsed = true;
                Card friend2 = Library.CreateCard<DayuuFriend2>();
                friend2.IsUpgraded = true;
                friend2.Summon();
                yield return new AddCardsToHandAction(friend2);
                yield return base.DebuffAction<Weak>(base.Battle.Player, 0, base.Value1, 0, 0, true, 0.2f);
                yield return base.DebuffAction<Fragil>(base.Battle.Player, 0, base.Value1, 0, 0, true, 0.2f);
                yield return base.DebuffAction<Vulnerable>(base.Battle.Player, 0, base.Value1, 0, 0, true, 0.2f);
                yield return new RemoveCardAction(this);
            }
            yield break;
        }
        public override IEnumerable<BattleAction> SummonActions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            this.IsEthereal = false;
            foreach (BattleAction battleAction in base.SummonActions(selector, consumingMana, precondition))
            {
                yield return battleAction;
            }
            yield break;
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (precondition == null || ((MiniSelectCardInteraction)precondition).SelectedCard.FriendToken == FriendToken.Active)
            {
                base.Loyalty += base.ActiveCost;
                List<Card> list = base.Battle.RollCardsWithoutManaLimit(new CardWeightTable(RarityWeightTable.BattleCard, OwnerWeightTable.AllOnes, CardTypeWeightTable.CanBeLoot), base.Value1, (CardConfig config) => config.RelativeCards.Contains("DayuuExodia") && config.Id != base.Id).ToList<Card>();
                foreach (Card card in list)
                {
                    card.SetBaseCost(ManaGroup.Anys(card.ConfigCost.Amount));
                    card.IsUpgraded = true;
                }
                yield return new AddCardsToHandAction(list);
                yield return base.DebuffAction<Weak>(base.Battle.Player, 0, base.Value1, 0, 0, true, 0.2f);
                yield return base.DebuffAction<Fragil>(base.Battle.Player, 0, base.Value1, 0, 0, true, 0.2f);
            }
            else
            {
                base.Loyalty += base.UltimateCost;
                base.UltimateUsed = true;
                Card friend3 = Library.CreateCard<DayuuFriend2>();
                friend3.IsUpgraded = true;
                friend3.Summon();
                yield return new AddCardsToHandAction(friend3);
                yield return base.DebuffAction<Weak>(base.Battle.Player, 0, base.Value1, 0, 0, true, 0.2f);
                yield return base.DebuffAction<Fragil>(base.Battle.Player, 0, base.Value1, 0, 0, true, 0.2f);
                yield return base.DebuffAction<Vulnerable>(base.Battle.Player, 0, base.Value1, 0, 0, true, 0.2f);
            }
            yield break;
        }
        public override IEnumerable<BattleAction> AfterUseAction()
        {
            if (!base.Summoned || base.Battle.BattleShouldEnd)
            {
                yield break;
            }
            if (base.Loyalty <= 0 || base.UltimateUsed == true)
            {
                yield return new RemoveCardAction(this);
                yield break;
            }
            yield return new MoveCardAction(this, CardZone.Hand);
            yield break;
        }
        /*private IEnumerable<BattleAction> OnBattleEnding(GameEventArgs args)
        {
            drawn = false;
            yield break;
        }*/
        private bool drawn = false;
    }
}
