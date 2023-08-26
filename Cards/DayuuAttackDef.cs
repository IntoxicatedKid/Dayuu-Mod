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

namespace DayuuMod
{
    public sealed class DayuuAttackDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(DayuuAttack);
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
               GunName: "YoumuKan",
               GunNameBurst: "YoumuKan",
               DebugLevel: 0,
               Revealable: false,
               IsPooled: true,
               HideMesuem: false,
               IsUpgradable: true,
               Rarity: Rarity.Rare,
               Type: CardType.Attack,
               TargetType: TargetType.RandomEnemy,
               Colors: new List<ManaColor>() { ManaColor.White },
               IsXCost: false,
               Cost: new ManaGroup() { Any = 2, White = 1 },
               UpgradedCost: null,
               MoneyCost: null,
               Damage: 13,
               UpgradedDamage: null,
               Block: null,
               UpgradedBlock: null,
               Shield: null,
               UpgradedShield: null,
               Value1: 1,
               UpgradedValue1: null,
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
               UpgradedKeywords: Keyword.Replenish,
               EmptyDescription: false,
               RelativeKeyword: Keyword.None,
               UpgradedRelativeKeyword: Keyword.Replenish,

               RelativeEffects: new List<string>() { "DayuuFriendSe" },
               UpgradedRelativeEffects: new List<string>() { "DayuuFriendSe" },
               RelativeCards: new List<string>() { "DayuuExodia" },
               UpgradedRelativeCards: new List<string>() { "DayuuExodia" },
               Owner: null,
               Unfinished: false,
               Illustrator: "Ogami Kazuki",
               SubIllustrator: new List<string>() { "MIO" }
            );

            return cardConfig;
        }
    }
    [EntityLogic(typeof(DayuuAttackDef))]
    public sealed class DayuuAttack : Card
    {
        public override bool OnDrawVisual
        {
            get
            {
                return false;
            }
        }
        public override bool OnDiscardVisual
        {
            get
            {
                return false;
            }
        }
        public override bool OnExileVisual
        {
            get
            {
                return false;
            }
        }
        public override bool OnMoveVisual
        {
            get
            {
                return false;
            }
        }
        public override IEnumerable<BattleAction> OnDraw()
        {
            return this.HandReactor();
        }
        public override IEnumerable<BattleAction> OnDiscard(CardZone srcZone)
        {
            if (base.Battle.BattleShouldEnd || srcZone != CardZone.Hand)
            {
                return null;
            }
            return this.DiscardHandReactor();
        }
        public override IEnumerable<BattleAction> OnExile(CardZone srcZone)
        {
            if (base.Battle.BattleShouldEnd || srcZone != CardZone.Hand)
            {
                return null;
            }
            return this.HandReactor();
        }
        public override IEnumerable<BattleAction> OnMove(CardZone srcZone, CardZone dstZone)
        {
            if (base.Battle.BattleShouldEnd || (srcZone == CardZone.Draw && dstZone == CardZone.Discard) || (srcZone == CardZone.Discard && dstZone == CardZone.Draw) || (srcZone == CardZone.Exile && dstZone == CardZone.Draw) || (srcZone == CardZone.Exile && dstZone == CardZone.Discard))
            {
                return null;
            }
            return this.HandReactor();
        }
        protected override void OnEnterBattle(BattleController battle)
        {
            if (base.Zone == CardZone.Hand)
            {
                base.React(new LazySequencedReactor(this.AddToHandReactor));
            }
        }
        private IEnumerable<BattleAction> AddToHandReactor()
        {
            base.NotifyActivating();
            List<DamageAction> list = new List<DamageAction>();
            foreach (BattleAction action in this.HandReactor())
            {
                yield return action;
                DamageAction damageAction = action as DamageAction;
                if (damageAction != null)
                {
                    list.Add(damageAction);
                }
            }
            if (list.NotEmpty<DamageAction>())
            {
                yield return new StatisticalTotalDamageAction(list);
            }
            yield break;
        }
        private IEnumerable<BattleAction> HandReactor()
        {
            if (base.Battle.BattleShouldEnd)
            {
                yield break;
            }
            EnemyUnit[] array = base.Battle.EnemyGroup.Alives.SampleManyOrAll(1, base.GameRun.BattleRng);
            foreach (EnemyUnit target in array)
            {
                if (target != null && target.IsAlive)
                {
                    yield return base.AttackAction(target);
                }
            }
            if (base.Battle.Player.HasStatusEffect<EvilTuiZhiSe>())
            {
                EvilTuiZhiSe statusEffect = base.Battle.Player.GetStatusEffect<EvilTuiZhiSe>();
                yield return statusEffect.TakeEffect();
            }
            yield break;
        }
        private IEnumerable<BattleAction> DiscardHandReactor()
        {
            if (base.Battle.BattleShouldEnd)
            {
                yield break;
            }
            EnemyUnit[] array = base.Battle.EnemyGroup.Alives.SampleManyOrAll(1, base.GameRun.BattleRng);
            foreach (EnemyUnit target in array)
            {
                if (target != null && target.IsAlive)
                {
                    yield return base.AttackAction(target);
                }
            }
            if (base.Battle.Player.HasStatusEffect<EvilTuiZhiSe>())
            {
                EvilTuiZhiSe statusEffect = base.Battle.Player.GetStatusEffect<EvilTuiZhiSe>();
                yield return statusEffect.TakeEffect();
            }
            yield return new MoveCardToDrawZoneAction(this, DrawZoneTarget.Top);
            yield break;
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return base.AttackAction(base.Battle.RandomAliveEnemy);
            yield break;
        }
        public override IEnumerable<BattleAction> AfterUseAction()
        {
            yield return new MoveCardToDrawZoneAction(this, DrawZoneTarget.Top);
            yield break;
        }
    }
}
