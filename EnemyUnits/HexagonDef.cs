using Cysharp.Threading.Tasks;
using DayuuMod.Stages;
using HarmonyLib;
using JetBrains.Annotations;
using LBoL.Base;
using LBoL.Base.Extensions;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.Stations;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using LBoL.EntityLib.Cards.Neutral.Red;
using LBoL.EntityLib.Cards.Other.Enemy;
using LBoL.EntityLib.EnemyUnits.Character;
using LBoL.EntityLib.Exhibits.Shining;
using LBoL.EntityLib.JadeBoxes;
using LBoL.EntityLib.Stages.NormalStages;
using LBoL.EntityLib.StatusEffects.Enemy;
using LBoL.Presentation;
using LBoL.Presentation.UI.Panels;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.ReflectionHelpers;
using LBoLEntitySideloader.Resource;
using LBoLEntitySideloader.Utils;
using Spine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using DayuuMod.JadeBoxes;
using UnityEngine;
using static MonoMod.Cil.RuntimeILReferenceBag.FastDelegateInvokers;
using static DayuuMod.BepinexPlugin;
using static UnityEngine.UI.GridLayoutGroup;


namespace DayuuMod.EnemyUnits
{
    public sealed class HexagonDef : EnemyUnitTemplate
    {
        public override IdContainer GetId() => nameof(Hexagon);
        public override LocalizationOption LoadLocalization()
        {
            AddBossNodeIcon("Hexagon", () => ResourceLoader.LoadSprite("Hexagon.png", directorySource));
            var locFiles = new LocalizationFiles(embeddedSource);
            locFiles.AddLocaleFile(Locale.En, "Resources.EnemyUnitsEn.yaml");
            return locFiles;
        }
        public override EnemyUnitConfig MakeConfig()
        {
            var enemyUnitConfig = new EnemyUnitConfig(
                Id: "Hexagon",
                RealName: true,
                OnlyLore: false,
                BaseManaColor: new ManaColor[] { ManaColor.Colorless },
                Order: 10,
                ModleName: null,
                NarrativeColor: "#ffffff",
                Type: EnemyType.Boss,
                IsPreludeOpponent: false,
                HpLength: null,
                MaxHpAdd: null,
                MaxHp: 9999,
                Damage1: int.MaxValue - 100,
                Damage2: int.MaxValue - 100,
                Damage3: int.MaxValue - 100,
                Damage4: int.MaxValue - 100,
                Power: 10,
                Defend: 9999,
                Count1: 30,
                Count2: 10,
                MaxHpHard: 9999,
                Damage1Hard: int.MaxValue - 100,
                Damage2Hard: int.MaxValue - 100,
                Damage3Hard: int.MaxValue - 100,
                Damage4Hard: int.MaxValue - 100,
                PowerHard: 15,
                DefendHard: 9999,
                Count1Hard: 25,
                Count2Hard: 15,
                MaxHpLunatic: 9999,
                Damage1Lunatic: int.MaxValue - 100,
                Damage2Lunatic: int.MaxValue - 100,
                Damage3Lunatic: int.MaxValue - 100,
                Damage4Lunatic: int.MaxValue - 100,
                PowerLunatic: 20,
                DefendLunatic: 9999,
                Count1Lunatic: 20,
                Count2Lunatic: 20,
                PowerLoot: new MinMax(100, 100),
                BluePointLoot: new MinMax(100, 100),
                Gun1: new string[] { "森罗" },
                Gun2: new string[] { "森罗" },
                Gun3: new string[] { "森罗" },
                Gun4: new string[] { "森罗" }
            );
            return enemyUnitConfig;
        }
    }
    [EntityLogic(typeof(HexagonDef))]
    public sealed class Hexagon : EnemyUnit
    {
        private static GameObject background = StageTemplate.TryGetEnvObject(NewBackgrounds.HexagonBackground);
        [HarmonyPatch(typeof(GameMaster), nameof(GameMaster.LeaveGameRun))]
        class GameMaster_LeaveGameRun_Patch
        {
            static void Postfix(GameMaster __instance)
            {
                BgmConfig.FromID(new HexagonBgm().UniqueId).LoopStart = 118.25f;
                BgmConfig.FromID(new HexagonBgm().UniqueId).LoopEnd = 236.4f;
                background.SetActive(false);
            }
        }
        [UsedImplicitly]
        public override string Name
        {
            get
            {
                return "Hexagon";
            }
        }
        private MoveType Next { get; set; }
        private string SpellGameOver
        {
            get
            {
                return "Game Over";
            }
        }
        private string SpellWonderful
        {
            get
            {
                return "Wonderful";
            }
        }
        protected override void OnEnterBattle(BattleController battle)
        {
            BgmConfig.FromID(new HexagonBgm().UniqueId).LoopStart = 0f;
            BgmConfig.FromID(new HexagonBgm().UniqueId).LoopEnd = 338f;
            if (GameMaster.Instance.CurrentGameRun.CurrentStation.Type != StationType.Boss)
            {
                AudioManager.PlayInLayer1("Hexagon");
            }
            background.SetActive(true);
            Next = MoveType.GameOver;
            ReactBattleEvent(Battle.BattleStarted, new System.Func<GameEventArgs, IEnumerable<BattleAction>>(OnBattleStarted));
            ReactBattleEvent(TurnStarted, new System.Func<GameEventArgs, IEnumerable<BattleAction>>(OnTurnStarted));
            HandleBattleEvent(Battle.Player.TurnEnded, delegate (UnitEventArgs _)
            {
                if (Hp <= 1)
                {
                    Next = MoveType.Wonderful;
                    UpdateTurnMoves();
                }
            });
        }
        private IEnumerable<BattleAction> OnBattleStarted(GameEventArgs arg)
        {
            //GameMaster.Instance.StartCoroutine(HexagonBackgroundOff(background));
            //yield return new CastBlockShieldAction(this, Defend, Defend, BlockShieldType.Normal, false);
            yield return new ApplyStatusEffectAction<LBoL.Core.StatusEffects.ExtraTurn>(Battle.Player, Count1, null, null, null, 0f, true);
            yield return new ApplyStatusEffectAction<HexagonSeDef.HexagonSe>(this, 0, null, 10, null, 0f, true);
            //GameMaster.Instance.StartCoroutine(Announce());
            yield break;
        }
        private IEnumerable<BattleAction> OnTurnStarted(GameEventArgs arg)
        {
            if (Hp <= 1)
            {
                Next = MoveType.Wonderful;
                UpdateTurnMoves();
            }
            else
            {
                Next = MoveType.GameOver;
                UpdateTurnMoves();
            }
            yield break;
        }
        protected override void OnLeaveBattle()
        {
            BgmConfig.FromID(new HexagonBgm().UniqueId).LoopStart = 118.25f;
            BgmConfig.FromID(new HexagonBgm().UniqueId).LoopEnd = 236.4f;
            background.SetActive(false);
        }
        /*protected override void OnLeaveGameRun()
        {
            BgmConfig.FromID(new HexagonBgm().UniqueId).LoopStart = 118.25f;
            BgmConfig.FromID(new HexagonBgm().UniqueId).LoopEnd = 236.4f;
            background.SetActive(false);
        }*/
        /*IEnumerator HexagonBackgroundOff(GameObject gameObject)
        {
            yield return new WaitForSeconds(5f);
            gameObject.SetActive(false);
        }*/
        /*private IEnumerator Announce()
        {
            yield return new WaitForSecondsRealtime(30f);
            yield return PerformAction.Chat(this, "Point", 1f, 0f, 0f, true);
            yield return new WaitForSecondsRealtime(30f);
            yield return PerformAction.Chat(this, "Line", 1f, 0f, 0f, true);
            yield return new WaitForSecondsRealtime(30f);
            yield return PerformAction.Chat(this, "Triangle", 1f, 0f, 0f, true);
            yield return new WaitForSecondsRealtime(30f);
            yield return PerformAction.Chat(this, "Square", 1f, 0f, 0f, true);
            yield return new WaitForSecondsRealtime(30f);
            yield return PerformAction.Chat(this, "Pentagon", 1f, 0f, 0f, true);
            yield return new WaitForSecondsRealtime(30f);
            yield return PerformAction.Chat(this, "Hexagon", 1f, 0f, 0f, true);
        }*/
        private IEnumerable<BattleAction> GameOver()
        {
            Battle.Player.ClearStatusEffects();
            foreach (BattleAction battleAction in AttackActions(null, Gun1, Damage1, 1, true, false))
            {
                yield return battleAction;
            }
            if (Battle.Player.IsAlive)
            {
                yield return new ForceKillAction(this, Battle.Player);
            }
        }
        private IEnumerable<BattleAction> Wonderful()
        {
            ClearStatusEffects();
            yield return PerformAction.Chat(this, "Wonderful", 1f, 0f, 0f, true);
            yield return new ForceKillAction(this, this);
        }
        protected override IEnumerable<IEnemyMove> GetTurnMoves()
        {
            switch (Next)
            {
                case MoveType.GameOver:
                    yield return new SimpleEnemyMove(Intention.SpellCard(SpellGameOver, Damage1, true), GameOver());
                    break;
                case MoveType.Wonderful:
                    yield return new SimpleEnemyMove(Intention.SpellCard(SpellWonderful, null, null, false), Wonderful());
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            yield break;
        }
        protected override void UpdateMoveCounters()
        {
            Next = MoveType.GameOver;
        }
        private enum MoveType
        {
            GameOver,
            Wonderful
        }
    }
    public sealed class HexagonUnitModelDef : UnitModelTemplate
    {
        public override IdContainer GetId() => nameof(Hexagon);
        public override LocalizationOption LoadLocalization()
        {
            var locFiles = new LocalizationFiles(embeddedSource);
            locFiles.AddLocaleFile(Locale.En, "UnitModelsEn.yaml");
            return locFiles;
        }

        public override ModelOption LoadModelOptions()
        {
            return new ModelOption(ResourceLoader.LoadSpriteAsync("Hexagon.png", directorySource, ppu: 565));
        }
        public override UniTask<Sprite> LoadSpellSprite() => ResourceLoader.LoadSpriteAsync("Hexagon.png", directorySource, ppu: 565);
        public override UnitModelConfig MakeConfig()
        {
            var unitModelConfig = new UnitModelConfig(
                Name: "Hexagon",
                Type: 0,
                EffectName: null,
                Offset: new Vector2(0.0f, 0.0f),
                Flip: true,
                Dielevel: 2,
                Box: new Vector2(0.8f, 1.8f),
                Shield: 1.2f,
                Block: 1.3f,
                Hp: new Vector2(0.0f, -1.3f),
                HpLength: 640,
                Info: new Vector2(0.0f, 1.2f),
                Select: new Vector2(1.6f, 2.0f),
                ShootStartTime: new float[] { 0.1f },
                ShootPoint: new Vector2[] { new Vector2(0.6f, 0.3f) },
                ShooterPoint: new Vector2[] { new Vector2(0.6f, 0.3f) },
                Hit: new Vector2(0.3f, 0.3f),
                HitRep: 0.1f,
                GuardRep: 0.1f,
                Chat: new Vector2(0.4f, 0.8f),
                ChatPortraitXY: new Vector2(-0.8f, -0.58f),
                ChatPortraitWH: new Vector2(0.6f, 0.5f),
                HasSpellPortrait: true,
                SpellPosition: new Vector2(400.0f, 0.0f),
                SpellScale: 0.9f,
                SpellColor: new Color32[] { new Color32(25, 25, 25, 255) }



            );
            return unitModelConfig;
        }
    }
    public sealed class HexagonSeDef : StatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(HexagonSe);
        }

        public override LocalizationOption LoadLocalization()
        {
            var locFiles = new LocalizationFiles(embeddedSource);
            locFiles.AddLocaleFile(Locale.En, "Resources.StatusEffectsEn.yaml");
            return locFiles;
        }

        public override Sprite LoadSprite()
        {
            return ResourceLoader.LoadSprite("Resources.HexagonSe.png", embeddedSource);
        }
        public override StatusEffectConfig MakeConfig()
        {
            var statusEffectConfig = new StatusEffectConfig(
                Id: "",
                Order: 99,
                Type: StatusEffectType.Special,
                IsVerbose: false,
                IsStackable: false,
                StackActionTriggerLevel: null,
                HasLevel: true,
                LevelStackType: StackType.Add,
                HasDuration: false,
                DurationStackType: StackType.Add,
                DurationDecreaseTiming: DurationDecreaseTiming.Custom,
                HasCount: true,
                CountStackType: StackType.Keep,
                LimitStackType: StackType.Keep,
                ShowPlusByLimit: false,
                Keywords: Keyword.None,
                RelativeEffects: new List<string>() { },
                VFX: "Default",
                VFXloop: "Default",
                SFX: "Default"
            );
            return statusEffectConfig;
        }


        [EntityLogic(typeof(HexagonSeDef))]
        public sealed class HexagonSe : StatusEffect
        {
            [UsedImplicitly]
            public int AccumulateDamage
            {
                get
                {
                    return (Owner as EnemyUnit).Count2 + DifficultyDamage;
                }
            }
            private int DifficultyDamage = 0;
            protected override void OnAdded(Unit unit)
            {
                HandleOwnerEvent(Battle.Player.BlockShieldGaining, delegate (BlockShieldEventArgs args)
                {
                    if (args.Block != 0f)
                    {
                        args.Block = Math.Max(Math.Min(Math.Min(250, args.Block), 250 - Battle.Player.Block), 0f);
                    }
                    if (args.Shield != 0f)
                    {
                        args.Shield = Math.Max(Math.Min(Math.Min(250, args.Shield), 250 - Battle.Player.Shield), 0f);
                    }
                    args.AddModifier(this);
                });
                HandleOwnerEvent(Battle.Player.BlockShieldGained, delegate (BlockShieldEventArgs args)
                {
                    if (Battle.Player.Block > 250)
                    {
                        Battle.Player.Block = 250;
                    }
                    if (Battle.Player.Shield > 250)
                    {
                        Battle.Player.Shield = 250;
                    }
                });
                HandleOwnerEvent(unit.DamageTaking, new GameEventHandler<DamageEventArgs>(OnDamageTaking));
                ReactOwnerEvent(Battle.CardUsed, new EventSequencedReactor<CardUsingEventArgs>(OnCardUsed));
                GameMaster.Instance.StartCoroutine(Difficulty());
                GameMaster.Instance.StartCoroutine(IncomingWall());
                GameMaster.Instance.StartCoroutine(LastWord());
                GameMaster.Instance.StartCoroutine(DeathSentence());
                DifficultyDamage = 0;
            }
            private void OnDamageTaking(DamageEventArgs args)
            {
                int num = args.DamageInfo.Damage.RoundToInt();
                if (num > 0)
                {
                    NotifyActivating();
                    args.DamageInfo = args.DamageInfo.ReduceActualDamageBy(num);
                    args.AddModifier(this);
                }
            }
            /*private void OnStatusEffectRemoving(StatusEffectEventArgs args)
            {
                if (args.Effect is HexagonSe)
                {
                    args.ForceCancelBecause(CancelCause.Reaction);
                    React(new DamageAction(Owner, Battle.Player, DamageInfo.Reaction(Level), "扩散结界", GunType.Single));
                    args.Effect.Count = 10;
                }
            }*/
            private IEnumerator IncomingWall()
            {
                while (Owner.Hp > 1)
                {
                    if (Count > 0)
                    {
                        Count--;
                    }
                    if (Count <= 0)
                    {
                        EnemyUnit enemyUnit = Owner as EnemyUnit;
                        Level += enemyUnit.Count2 + DifficultyDamage;
                        Count = 10;
                    }
                    yield return new WaitForSecondsRealtime(1f);
                }
            }
            private IEnumerator DeathSentence()
            {
                while (Owner.Hp > 1)
                {
                    if (Level >= 100)
                    {
                        if (Battle.Player.Hp < 1)
                        {
                            break;
                        }
                        else if (Battle.Player.Hp == 1)
                        {
                            GameMaster.Instance.CurrentGameRun.Battle.Player.ClearStatusEffects();
                            Battle.PlayerTurnShouldEnd = true;
                            Battle._isWaitingPlayerInput = false;
                            break;
                        }
                        else if (Battle.Player.Hp > 1)
                        {
                            GameMaster.Instance.CurrentGameRun.SetHpAndMaxHp(Battle.Player.Hp - 1, Battle.Player.MaxHp, false);
                        }
                    }
                    yield return new WaitForSecondsRealtime(Math.Max(1 - Level * 0.001f, 0.1f));
                }
            }
            private IEnumerator LastWord()
            {
                while (Owner.Hp > 1)
                {
                    GameMaster.Instance.CurrentGameRun.SetEnemyHpAndMaxHp(Math.Max(Owner.Hp - 3, 1), Owner.MaxHp, Owner as EnemyUnit, false);
                    yield return new WaitForSecondsRealtime(0.0836f);
                }
                if (Battle.Player.HasStatusEffect<LBoL.Core.StatusEffects.ExtraTurn>())
                {
                    Battle.Player.TryRemoveStatusEffect(Battle.Player.GetStatusEffect<LBoL.Core.StatusEffects.ExtraTurn>());
                }
                Battle.PlayerTurnShouldEnd = true;
                Battle._isWaitingPlayerInput = false;
            }
            private IEnumerator Difficulty()
            {
                yield return new WaitForSecondsRealtime(30f);
                DifficultyDamage += 5;
                yield return new WaitForSecondsRealtime(30f);
                DifficultyDamage += 5;
                yield return new WaitForSecondsRealtime(30f);
                DifficultyDamage += 5;
                yield return new WaitForSecondsRealtime(30f);
                DifficultyDamage += 5;
                yield return new WaitForSecondsRealtime(30f);
                DifficultyDamage += 5;
                yield return new WaitForSecondsRealtime(30f);
                DifficultyDamage += 5;
            }
            private IEnumerable<BattleAction> OnCardUsed(CardUsingEventArgs args)
            {
                if (Battle.Player.IsInTurn && Level > 0 && Owner.Hp > 1)
                {
                    NotifyActivating();
                    yield return new DamageAction(Owner, Battle.Player, DamageInfo.Reaction(Level), "扩散结界", GunType.Single);
                    Level = 0;
                }
                yield break;
            }
        }
    }
    public sealed class HexagonBgm : BgmTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(Hexagon);
        }

        public override UniTask<AudioClip> LoadAudioClipAsync()
        {
            return ResourceLoader.LoadAudioClip("Courtesy (Remastered) - Super Hexagon.ogg", AudioType.OGGVORBIS, directorySource);

        }

        public override BgmConfig MakeConfig()
        {
            var config = new BgmConfig(
                    ID: "",
                    No: sequenceTable.Next(typeof(BgmConfig)),
                    Show: true,
                    Name: "",
                    Folder: "",
                    Path: "",
                    LoopStart: (float?)118.25,
                    LoopEnd: (float?)236.4,
                    TrackName: "Courtesy",
                    Artist: "SiIvaGunner",
                    Original: "Courtesy",
                    Comment: ""
            );

            return config;
        }
    }
}