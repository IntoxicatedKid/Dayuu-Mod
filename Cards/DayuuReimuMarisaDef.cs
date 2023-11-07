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
using LBoL.EntityLib.Cards.Character.Marisa;
using LBoL.EntityLib.StatusEffects.Cirno;
using LBoL.EntityLib.StatusEffects.Marisa;
using LBoL.EntityLib.StatusEffects.Others;
using LBoL.EntityLib.StatusEffects.Reimu;
using LBoL.EntityLib.Cards.Character.Sakuya;
using System.Linq;
using static UnityEngine.GraphicsBuffer;
using static DayuuMod.BepinexPlugin;
using Mono.Cecil;
using LBoL.Presentation;
using LBoL.EntityLib.Exhibits.Shining;
using HarmonyLib;
using UnityEngine;

namespace DayuuMod.Cards
{
    public sealed class DayuuReimuMarisaDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(DayuuReimuMarisa);
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
               //IsPooled: GameMaster.Instance.CurrentGameRun != null && (GameMaster.Instance.CurrentGameRun.Player.HasExhibit<ReimuW>() || GameMaster.Instance.CurrentGameRun.Player.HasExhibit<ReimuR>()) && (GameMaster.Instance.CurrentGameRun.Player.HasExhibit<MarisaB>() || GameMaster.Instance.CurrentGameRun.Player.HasExhibit<MarisaR>()),
               IsPooled: true,
               HideMesuem: false,
               IsUpgradable: true,
               Rarity: Rarity.Rare,
               Type: CardType.Attack,
               TargetType: TargetType.AllEnemies,
               Colors: new List<ManaColor>() { ManaColor.White, ManaColor.Black, ManaColor.Red },
               IsXCost: false,
               Cost: new ManaGroup() { White = 1, Black = 1, Red = 2 },
               UpgradedCost: null,
               MoneyCost: null,
               Damage: 17,
               UpgradedDamage: null,
               Block: null,
               UpgradedBlock: null,
               Shield: null,
               UpgradedShield: null,
               Value1: 0,
               UpgradedValue1: 1,
               Value2: 3,
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

               Keywords: Keyword.Accuracy | Keyword.Exile,
               UpgradedKeywords: Keyword.Accuracy | Keyword.Exile,
               EmptyDescription: false,
               RelativeKeyword: Keyword.None,
               UpgradedRelativeKeyword: Keyword.None,

               RelativeEffects: new List<string>() { },
               UpgradedRelativeEffects: new List<string>() { },
               RelativeCards: new List<string>() { },
               UpgradedRelativeCards: new List<string>() { },
               Owner: null,
               Unfinished: false,
               Illustrator: "Dayuu",
               SubIllustrator: new List<string>() { "fuente" }
            );

            return cardConfig;
        }
    }
    [EntityLogic(typeof(DayuuReimuMarisaDef))]
    public sealed class DayuuReimuMarisa : Card
    {
        /*[HarmonyPatch(typeof(GameRunController), nameof(GameRunController.EnterStation))]
        class GameRunController_EnterStation_Patch
        {
            static void Postfix(GameRunController __instance)
            {
                Card card = Library.CreateCard<DayuuReimuMarisa>();
                card.Initialize();
            }
        }*/
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (Value1 > 0)
            {
                foreach (BattleAction battleAction in DebuffAction<Vulnerable>(selector.GetUnits(Battle), 0, Value1, 0, 0, true, 0.2f))
                {
                    yield return battleAction;
                }
            }
            /*for (int i = 0; i < Battle.AllAliveEnemies.Count(); i++)
            {
                yield return PerformAction.Gun(Battle.Player, selector.GetUnits(Battle)[i], "ReimuSpell1", 0.01f);
            }*/
            yield return PerformAction.Gun(Battle.Player, selector.GetUnits(Battle).FirstOrDefault(), "ReimuSpell1", 0.01f);
            yield return PerformAction.Gun(Battle.Player, selector.GetUnits(Battle).FirstOrDefault(), "MasterSpark", 0.01f);
            yield return PerformAction.Spell(Battle.Player, "DayuuFantasySpark");
            yield return PerformAction.Gun(Battle.Player, selector.GetUnits(Battle).FirstOrDefault(), "Instant", 1f);
            for (int i = 0; i < Value2; i++)
            {
                yield return new DamageAction(Battle.Player, Battle.AllAliveEnemies, Damage, "Instant", GunType.Single);
            }
        }
    }
    public sealed class DayuuFantasySparkDef : UltimateSkillTemplate
    {
        public override IdContainer GetId() => nameof(DayuuFantasySpark);

        public override LocalizationOption LoadLocalization()
        {
            var locFiles = new LocalizationFiles(embeddedSource);
            locFiles.AddLocaleFile(Locale.En, "UltimateSkillsEn.yaml");
            return locFiles;
        }

        public override Sprite LoadSprite()
        {
            return null;
            //return ResourceLoader.LoadSprite("AyaUltG.png", embeddedSource);
        }

        public override UltimateSkillConfig MakeConfig()
        {
            var config = new UltimateSkillConfig(
                Id: "",
                Order: 10,
                PowerCost: 100,
                PowerPerLevel: 100,
                MaxPowerLevel: 2,
                RepeatableType: UsRepeatableType.OncePerTurn,
                Damage: 0,
                Value1: 0,
                Value2: 0,
                Keywords: Keyword.Accuracy,
                RelativeEffects: new List<string>() { },
                RelativeCards: new List<string>() { }
                );
            return config;
        }
    }

    [EntityLogic(typeof(DayuuFantasySparkDef))]
    public sealed class DayuuFantasySpark : UltimateSkill
    {
        public DayuuFantasySpark()
        {
            TargetType = TargetType.SingleEnemy;
            GunName = "Simple1";
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector)
        {
            yield return new DamageAction(Owner, selector.GetEnemy(Battle), Damage, GunName, GunType.Single);
        }
    }
}
