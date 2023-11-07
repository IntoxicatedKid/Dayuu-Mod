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
using LBoL.Presentation.UI.Panels;
using UnityEngine.InputSystem.Controls;
using JetBrains.Annotations;
using DayuuMod.Cards;

namespace DayuuMod.Exhibits
{
    public sealed class DayuuSmallSparrowDef : ExhibitTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(DayuuSmallSparrow);
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
        [EntityLogic(typeof(DayuuSmallSparrowDef))]
        [UsedImplicitly]
        public sealed class DayuuSmallSparrow : Exhibit
        {
            protected override IEnumerator SpecialGain(PlayerUnit player)
            {
                OnGain(player);
                List<Card> array = new List<Card> { Library.CreateCard<DayuuAttack>(), Library.CreateCard<DayuuDefense>(), Library.CreateCard<DayuuSkill>(), Library.CreateCard<DayuuAbility>(), Library.CreateCard<DayuuFriend>() };
                if (array.Count != 0)
                {
                    GameRun.UpgradeNewDeckCardOnFlags(array);
                    MiniSelectCardInteraction interaction = new MiniSelectCardInteraction(array, true, true, true)
                    {
                        Source = this
                    };
                    yield return GameRun.InteractionViewer.View(interaction);
                    Card selectedCard = interaction.SelectedCard;
                    if (selectedCard != null)
                    {
                        GameRun.AddDeckCard(selectedCard, true, new VisualSourceData
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
                ReactBattleEvent(Battle.CardDrawn, new EventSequencedReactor<CardEventArgs>(OnCardDrawn));
            }
            private IEnumerable<BattleAction> OnCardDrawn(CardEventArgs args)
            {
                Card card = args.Card;
                if (card is DayuuAttack || card is DayuuDefense || card is DayuuSkill || card is DayuuAbility || card is DayuuFriend)
                {
                    NotifyActivating();
                    yield return new GainManaAction(card.ConfigCostAnyToColorless(true) - Mana);
                }
                yield break;
            }
        }
    }
}