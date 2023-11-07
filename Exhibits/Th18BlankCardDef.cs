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
using Mono.Cecil;
using LBoL.Core.StatusEffects;
using System.Linq;
using LBoL.EntityLib.Cards.Neutral.NoColor;
using LBoL.Core.Battle.Interactions;
using LBoL.Core.Randoms;
using LBoL.EntityLib.Cards.Character.Sakuya;
using LBoL.EntityLib.Cards.Other.Misfortune;
using static UnityEngine.TouchScreenKeyboard;
using JetBrains.Annotations;

namespace DayuuMod.Exhibits
{
    public sealed class Th18BlankCardDef : ExhibitTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(Th18BlankCard);
        }
        public override LocalizationOption LoadLocalization()
        {
            var locFiles = new LocalizationFiles(embeddedSource);
            locFiles.AddLocaleFile(Locale.En, "Resources.ExhibitsEn.yaml");
            return locFiles;
        }
        public override ExhibitSprites LoadSprite()
        {
            // embedded resource folders are separated by a dot
            var folder = "";
            var exhibitSprites = new ExhibitSprites();
            Func<string, Sprite> wrap = (s) => ResourceLoader.LoadSprite(folder + GetId() + s + ".png", embeddedSource);
            exhibitSprites.main = wrap("");
            return exhibitSprites;
        }
        public override ExhibitConfig MakeConfig()
        {
            var exhibitConfig = new ExhibitConfig(
                Index: sequenceTable.Next(typeof(ExhibitConfig)),
                Id: "",
                Order: 9,
                IsDebug: false,
                IsPooled: false,
                IsSentinel: false,
                Revealable: false,
                Appearance: AppearanceType.ShopOnly,
                Owner: "",
                LosableType: ExhibitLosableType.Losable,
                Rarity: Rarity.Common,
                Value1: null,
                Value2: null,
                Value3: null,
                Mana: null,
                BaseManaRequirement: null,
                BaseManaColor: null,
                BaseManaAmount: 0,
                HasCounter: false,
                InitialCounter: 0,
                Keywords: Keyword.None,
                RelativeEffects: new List<string>() { },

                RelativeCards: new List<string>() { }
            );
            return exhibitConfig;
        }
        [EntityLogic(typeof(Th18BlankCardDef))]
        [UsedImplicitly]
        public sealed class Th18BlankCard : Exhibit
        {
            protected override void OnEnterBattle()
            {
                ReactBattleEvent(Battle.Player.TurnStarted, new EventSequencedReactor<UnitEventArgs>(OnPlayerTurnStarted));
            }
            private IEnumerable<BattleAction> OnPlayerTurnStarted(GameEventArgs args)
            {
                if (Battle.Player.TurnCounter == 1)
                {
                    NotifyActivating();
                    List<Card> attackc = null;
                    List<Card> attacku = null;
                    List<Card> attackr = null;
                    List<Card> defensec = null;
                    List<Card> defenseu = null;
                    List<Card> defenser = null;
                    List<Card> skillc = null;
                    List<Card> skillu = null;
                    List<Card> skillr = null;
                    List<Card> abilityc = null;
                    List<Card> abilityu = null;
                    List<Card> abilityr = null;
                    List<Card> friendc = null;
                    List<Card> friendu = null;
                    List<Card> friendr = null;
                    //List<Card> statusc = null;
                    //List<Card> statusu = null;
                    //List<Card> statusr = null;
                    //List<Card> misfortunec = null;
                    //List<Card> misfortuneu = null;
                    //List<Card> misfortuner = null;
                    List<Card> hand = Battle.HandZone.ToList();
                    if (hand.Count > 0)
                    {
                        attackc = hand.Where((card) => card.CardType == CardType.Attack && card.Config.Rarity == Rarity.Common).ToList();
                        attacku = hand.Where((card) => card.CardType == CardType.Attack && card.Config.Rarity == Rarity.Uncommon).ToList();
                        attackr = hand.Where((card) => card.CardType == CardType.Attack && card.Config.Rarity == Rarity.Rare).ToList();
                        defensec = hand.Where((card) => card.CardType == CardType.Defense && card.Config.Rarity == Rarity.Common).ToList();
                        defenseu = hand.Where((card) => card.CardType == CardType.Defense && card.Config.Rarity == Rarity.Uncommon).ToList();
                        defenser = hand.Where((card) => card.CardType == CardType.Defense && card.Config.Rarity == Rarity.Rare).ToList();
                        skillc = hand.Where((card) => card.CardType == CardType.Skill && card.Config.Rarity == Rarity.Common).ToList();
                        skillu = hand.Where((card) => card.CardType == CardType.Skill && card.Config.Rarity == Rarity.Uncommon).ToList();
                        skillr = hand.Where((card) => card.CardType == CardType.Skill && card.Config.Rarity == Rarity.Rare).ToList();
                        abilityc = hand.Where((card) => card.CardType == CardType.Ability && card.Config.Rarity == Rarity.Common).ToList();
                        abilityu = hand.Where((card) => card.CardType == CardType.Ability && card.Config.Rarity == Rarity.Uncommon).ToList();
                        abilityr = hand.Where((card) => card.CardType == CardType.Ability && card.Config.Rarity == Rarity.Rare).ToList();
                        friendc = hand.Where((card) => card.CardType == CardType.Friend && card.Config.Rarity == Rarity.Common).ToList();
                        friendu = hand.Where((card) => card.CardType == CardType.Friend && card.Config.Rarity == Rarity.Uncommon).ToList();
                        friendr = hand.Where((card) => card.CardType == CardType.Friend && card.Config.Rarity == Rarity.Rare).ToList();
                        //statusc = hand.Where((Card card) => (card.CardType == CardType.Status) && (card.Config.Rarity == Rarity.Common)).ToList<Card>();
                        //statusu = hand.Where((Card card) => (card.CardType == CardType.Status) && (card.Config.Rarity == Rarity.Uncommon)).ToList<Card>();
                        //statusr = hand.Where((Card card) => (card.CardType == CardType.Status) && (card.Config.Rarity == Rarity.Rare)).ToList<Card>();
                        //misfortunec = hand.Where((Card card) => (card.CardType == CardType.Misfortune) && (card.Config.Rarity == Rarity.Common)).ToList<Card>();
                        //misfortuneu = hand.Where((Card card) => (card.CardType == CardType.Misfortune) && (card.Config.Rarity == Rarity.Uncommon)).ToList<Card>();
                        //misfortuner = hand.Where((Card card) => (card.CardType == CardType.Misfortune) && (card.Config.Rarity == Rarity.Rare)).ToList<Card>();
                        List<Card> list = new List<Card>();
                        yield return new ExileManyCardAction(hand);
                        foreach (Card card in attackc)
                        {
                            Card[] attackC = Battle.RollCards(new CardWeightTable(RarityWeightTable.OnlyCommon, OwnerWeightTable.Valid, CardTypeWeightTable.CanBeLoot), 1, (config) => config.Type == CardType.Attack);
                            if (card.IsUpgraded)
                            {
                                yield return new UpgradeCardsAction(attackC);
                            }
                            list.AddRange(attackC);
                        }
                        foreach (Card card in attacku)
                        {
                            Card[] attackU = Battle.RollCards(new CardWeightTable(RarityWeightTable.OnlyUncommon, OwnerWeightTable.Valid, CardTypeWeightTable.CanBeLoot), 1, (config) => config.Type == CardType.Attack);
                            if (card.IsUpgraded)
                            {
                                yield return new UpgradeCardsAction(attackU);
                            }
                            list.AddRange(attackU);
                        }
                        foreach (Card card in attackr)
                        {
                            Card[] attackR = Battle.RollCards(new CardWeightTable(RarityWeightTable.OnlyRare, OwnerWeightTable.Valid, CardTypeWeightTable.CanBeLoot), 1, (config) => config.Type == CardType.Attack);
                            if (card.IsUpgraded)
                            {
                                yield return new UpgradeCardsAction(attackR);
                            }
                            list.AddRange(attackR);
                        }
                        foreach (Card card in defensec)
                        {
                            Card[] defenseC = Battle.RollCards(new CardWeightTable(RarityWeightTable.OnlyCommon, OwnerWeightTable.Valid, CardTypeWeightTable.CanBeLoot), 1, (config) => config.Type == CardType.Defense);
                            if (card.IsUpgraded)
                            {
                                yield return new UpgradeCardsAction(defenseC);
                            }
                            list.AddRange(defenseC);
                        }
                        foreach (Card card in defenseu)
                        {
                            Card[] defenseU = Battle.RollCards(new CardWeightTable(RarityWeightTable.OnlyUncommon, OwnerWeightTable.Valid, CardTypeWeightTable.CanBeLoot), 1, (config) => config.Type == CardType.Defense);
                            if (card.IsUpgraded)
                            {
                                yield return new UpgradeCardsAction(defenseU);
                            }
                            list.AddRange(defenseU);
                        }
                        foreach (Card card in defenser)
                        {
                            Card[] defenseR = Battle.RollCards(new CardWeightTable(RarityWeightTable.OnlyRare, OwnerWeightTable.Valid, CardTypeWeightTable.CanBeLoot), 1, (config) => config.Type == CardType.Defense);
                            if (card.IsUpgraded)
                            {
                                yield return new UpgradeCardsAction(defenseR);
                            }
                            list.AddRange(defenseR);
                        }
                        foreach (Card card in skillc)
                        {
                            Card[] skillC = Battle.RollCards(new CardWeightTable(RarityWeightTable.OnlyCommon, OwnerWeightTable.Valid, CardTypeWeightTable.CanBeLoot), 1, (config) => config.Type == CardType.Skill);
                            if (card.IsUpgraded)
                            {
                                yield return new UpgradeCardsAction(skillC);
                            }
                            list.AddRange(skillC);
                        }
                        foreach (Card card in skillu)
                        {
                            Card[] skillU = Battle.RollCards(new CardWeightTable(RarityWeightTable.OnlyUncommon, OwnerWeightTable.Valid, CardTypeWeightTable.CanBeLoot), 1, (config) => config.Type == CardType.Skill);
                            if (card.IsUpgraded)
                            {
                                yield return new UpgradeCardsAction(skillU);
                            }
                            list.AddRange(skillU);
                        }
                        foreach (Card card in skillr)
                        {
                            Card[] skillR = Battle.RollCards(new CardWeightTable(RarityWeightTable.OnlyRare, OwnerWeightTable.Valid, CardTypeWeightTable.CanBeLoot), 1, (config) => config.Type == CardType.Skill);
                            if (card.IsUpgraded)
                            {
                                yield return new UpgradeCardsAction(skillR);
                            }
                            list.AddRange(skillR);
                        }
                        foreach (Card card in abilityc)
                        {
                            Card[] abilityC = Battle.RollCards(new CardWeightTable(RarityWeightTable.OnlyCommon, OwnerWeightTable.Valid, CardTypeWeightTable.CanBeLoot), 1, (config) => config.Type == CardType.Ability);
                            if (card.IsUpgraded)
                            {
                                yield return new UpgradeCardsAction(abilityC);
                            }
                            list.AddRange(abilityC);
                        }
                        foreach (Card card in abilityu)
                        {
                            Card[] abilityU = Battle.RollCards(new CardWeightTable(RarityWeightTable.OnlyUncommon, OwnerWeightTable.Valid, CardTypeWeightTable.CanBeLoot), 1, (config) => config.Type == CardType.Ability);
                            if (card.IsUpgraded)
                            {
                                yield return new UpgradeCardsAction(abilityU);
                            }
                            list.AddRange(abilityU);
                        }
                        foreach (Card card in abilityr)
                        {
                            Card[] abilityR = Battle.RollCards(new CardWeightTable(RarityWeightTable.OnlyRare, OwnerWeightTable.Valid, CardTypeWeightTable.CanBeLoot), 1, (config) => config.Type == CardType.Ability);
                            if (card.IsUpgraded)
                            {
                                yield return new UpgradeCardsAction(abilityR);
                            }
                            list.AddRange(abilityR);
                        }
                        foreach (Card card in friendc)
                        {
                            Card[] friendC = Battle.RollCards(new CardWeightTable(RarityWeightTable.OnlyCommon, OwnerWeightTable.Valid, CardTypeWeightTable.CanBeLoot), 1, (config) => config.Type == CardType.Friend);
                            if (card.IsUpgraded)
                            {
                                yield return new UpgradeCardsAction(friendC);
                            }
                            list.AddRange(friendC);
                        }
                        foreach (Card card in friendu)
                        {
                            Card[] friendU = Battle.RollCards(new CardWeightTable(RarityWeightTable.OnlyUncommon, OwnerWeightTable.Valid, CardTypeWeightTable.CanBeLoot), 1, (config) => config.Type == CardType.Friend);
                            if (card.IsUpgraded)
                            {
                                yield return new UpgradeCardsAction(friendU);
                            }
                            list.AddRange(friendU);
                        }
                        foreach (Card card in friendr)
                        {
                            Card[] friendR = Battle.RollCards(new CardWeightTable(RarityWeightTable.OnlyRare, OwnerWeightTable.Valid, CardTypeWeightTable.CanBeLoot), 1, (config) => config.Type == CardType.Friend);
                            if (card.IsUpgraded)
                            {
                                yield return new UpgradeCardsAction(friendR);
                            }
                            list.AddRange(friendR);
                        }
                        //foreach (Card card in statusc)
                        //{
                        //    Card[] statusC = base.Battle.RollCards(new CardWeightTable(RarityWeightTable.BattleCard, OwnerWeightTable.AllOnes, CardTypeWeightTable.AllOnes), 1, (CardConfig config) => config.Type == CardType.Status);
                        //    list.AddRange(statusC);
                        //}
                        //foreach (Card card in statusu)
                        //{
                        //    Card[] statusU = base.Battle.RollCards(new CardWeightTable(RarityWeightTable.BattleCard, OwnerWeightTable.AllOnes, CardTypeWeightTable.AllOnes), 1, (CardConfig config) => config.Type == CardType.Status);
                        //    list.AddRange(statusU);
                        //}
                        //foreach (Card card in statusr)
                        //{
                        //    Card[] statusR = base.Battle.RollCards(new CardWeightTable(RarityWeightTable.BattleCard, OwnerWeightTable.AllOnes, CardTypeWeightTable.AllOnes), 1, (CardConfig config) => config.Type == CardType.Status);
                        //    list.AddRange(statusR);
                        //}
                        //foreach (Card card in misfortunec)
                        //{
                        //    Card[] misfortuneC = base.Battle.RollCards(new CardWeightTable(RarityWeightTable.BattleCard, OwnerWeightTable.AllOnes, CardTypeWeightTable.AllOnes), 1, (CardConfig config) => config.Type == CardType.Misfortune);
                        //    list.AddRange(misfortuneC);
                        //}
                        //foreach (Card card in misfortuneu)
                        //{
                        //    Card[] misfortuneU = base.Battle.RollCards(new CardWeightTable(RarityWeightTable.BattleCard, OwnerWeightTable.AllOnes, CardTypeWeightTable.AllOnes), 1, (CardConfig config) => config.Type == CardType.Misfortune);
                        //    list.AddRange(misfortuneU);
                        //}
                        //foreach (Card card in misfortuner)
                        //{
                        //    Card[] misfortuneR = base.Battle.RollCards(new CardWeightTable(RarityWeightTable.BattleCard, OwnerWeightTable.AllOnes, CardTypeWeightTable.AllOnes), 1, (CardConfig config) => config.Type == CardType.Misfortune);
                        //    list.AddRange(misfortuneR);
                        //}
                        yield return new AddCardsToHandAction(list);
                    }
                }
                yield break;
            }
        }
    }
}