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
using LBoL.Core.Units;
using LBoL.EntityLib.Cards.Character.Cirno.Friend;
using LBoL.EntityLib.Cards.Character.Reimu;
using LBoL.EntityLib.Cards.Neutral.MultiColor;
using static DayuuMod.DayuuAbilityDef;
using LBoL.Presentation.UI.Panels;
using UnityEngine.InputSystem.Controls;
using DayuuMod;

namespace DayuuMod
{
    public sealed class SmallSparrowDef : ExhibitTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(SmallSparrow);
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

            Func<string, Sprite> wrap = (s) => ResourceLoader.LoadSprite((folder + GetId() + s + ".png"), embeddedSource);

            exhibitSprites.main = wrap("");

            // loads many custom sprites for futher use
            //exhibitSprites.customSprites.Add("none", wrap("_none"));
            //exhibitSprites.customSprites.Add("luna", wrap("_luna"));
            //exhibitSprites.customSprites.Add("-luna", wrap("_-luna"));
            //exhibitSprites.customSprites.Add("star", wrap("_star"));
            //exhibitSprites.customSprites.Add("-star", wrap("_-star"));
            //exhibitSprites.customSprites.Add("sunny", wrap("_sunny"));
            //exhibitSprites.customSprites.Add("-sunny", wrap("_-sunny"));


            return exhibitSprites;
        }



        public override ExhibitConfig MakeConfig()
        {
            var exhibitConfig = new ExhibitConfig(
                Index: 0,
                Id: "",
                Order: 10,
                IsDebug: false,
                IsPooled: true,
                IsSentinel: false,
                Revealable: false,
                Appearance: AppearanceType.Anywhere,
                Owner: "",
                LosableType: ExhibitLosableType.Losable,
                Rarity: Rarity.Common,
                Value1: 5,
                Value2: null,
                Value3: null,
                Mana: new ManaGroup() { Colorless = 2 },
                BaseManaRequirement: null,
                BaseManaColor: null,
                BaseManaAmount: 0,
                HasCounter: false,
                InitialCounter: 0,
                Keywords: Keyword.None,
                RelativeEffects: new List<string>() { },
                // example of referring to UniqueId of an entity without calling MakeConfig
                RelativeCards: new List<string>() { }
            );

            return exhibitConfig;
        }

        [EntityLogic(typeof(SmallSparrowDef))]
        public sealed class SmallSparrow : Exhibit
        {
            // Changes the icon according to last three cards played 
            // Sunny = attack, Star = defense, Luna = skill
            // this is where the keys of custom sprites are used
            //public override string OverrideIconName
            //{
            //    get
            //    {
            //        if (Battle == null)
            //            return Id;
            //
            //        if (triggered)
            //            return Id;
            //
            //        if (cardTracker.Empty())
            //            return Id + "none";
            //        if (cardTracker.Count == 1)
            //        {
            //            if (cardTracker.Contains(CardType.Attack))
            //                return Id + "sunny";
            //            if (cardTracker.Contains(CardType.Defense))
            //                return Id + "star";
            //            if (cardTracker.Contains(CardType.Skill))
            //                return Id + "luna";
            //        }
            //        if (cardTracker.Count == 2)
            //        {
            //            if (!cardTracker.Contains(CardType.Attack))
            //                return Id + "-sunny";
            //            if (!cardTracker.Contains(CardType.Defense))
            //                return Id + "-star";
            //            if (!cardTracker.Contains(CardType.Skill))
            //                return Id + "-luna";
            //        }
            //
            //
            //        return Id;
            //    }
            //
            //}
            protected override IEnumerator SpecialGain(PlayerUnit player)
            {
                this.OnGain(player);
                List<Card> array = new List<Card> { Library.CreateCard<DayuuAttack>(), Library.CreateCard<DayuuDefense>(), Library.CreateCard<DayuuSkill>(), Library.CreateCard<DayuuAbility>(), Library.CreateCard<DayuuFriend>() };
                if (array.Count != 0)
                {
                    base.GameRun.UpgradeNewDeckCardOnFlags(array);
                    MiniSelectCardInteraction interaction = new MiniSelectCardInteraction(array, true, true, true)
                    {
                        Source = this
                    };
                    yield return base.GameRun.InteractionViewer.View(interaction);
                    Card selectedCard = interaction.SelectedCard;
                    if (selectedCard != null)
                    {
                        base.GameRun.AddDeckCard(selectedCard, true, new VisualSourceData
                        {
                            SourceType = VisualSourceType.CardSelect
                        });
                    }
                    interaction = null;
                }
                yield break;
            }
            protected override void OnEnterBattle()
            {
                base.ReactBattleEvent<CardEventArgs>(base.Battle.CardDrawn, new EventSequencedReactor<CardEventArgs>(this.OnCardDrawn));
            }
            private IEnumerable<BattleAction> OnCardDrawn(CardEventArgs args)
            {
                Card card = args.Card;
                if (card is DayuuAttack || card is DayuuDefense || card is DayuuSkill || card is DayuuAbility || card is DayuuFriend)
                {
                    base.NotifyActivating();
                    yield return new GainManaAction(card.ConfigCostAnyToColorless(true) - this.Mana);
                }
                yield break;
            }
        }
    }
}