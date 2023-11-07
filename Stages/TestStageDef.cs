﻿using HarmonyLib;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Randoms;
using LBoL.EntityLib.Exhibits.Shining;
using LBoL.EntityLib.Stages;
using LBoL.EntityLib.Stages.NormalStages;
using LBoL.Presentation.UI.Panels;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using UnityEngine;
using static DayuuMod.BepinexPlugin;

namespace DayuuMod.Stages
{
    /*[OverwriteVanilla]
    public sealed class battleAdvDef : StageTemplate
    {
        public override IdContainer GetId() => nameof(BattleAdvTest);


        public override StageConfig MakeConfig()
        {
            var config = StageConfig.FromId(GetId());
            config.Obj0 = NewBackgrounds.ghibli;
            config.Obj1 = NewBackgrounds.ghibli;
            config.Obj2 = NewBackgrounds.ghibli;
            config.Obj3 = NewBackgrounds.ghibli;
            config.Obj4 = NewBackgrounds.ghibli;
            return config;
        }
    }

    public sealed class NewStageExDef : StageTemplate
    {
        public override IdContainer GetId() => nameof(NewStageEx);


        public override StageConfig MakeConfig()
        {
            var config = DefaultConfig();
            config.Obj0 = NewBackgrounds.ghibli;
            config.Obj1 = NewBackgrounds.ghibli;
            config.Obj2 = NewBackgrounds.ghibli;
            config.Obj3 = NewBackgrounds.ghibli;
            config.Obj4 = NewBackgrounds.ghibli;
            return config;
        }

        [EntityLogic(typeof(NewStageExDef))]
        public sealed class NewStageEx : NormalStageBase
        {
            public NewStageEx()
            {
                Level = 1;
                CardUpgradedChance = 0f;
                IsSelectingBoss = true;
                EnemyPoolAct1 = new UniqueRandomPool<string>(true)
            {
                { "Sanyue", 1f },
                { "Aya", 1f },
                { "Rin", 1f }
            };
                EnemyPoolAct2 = new UniqueRandomPool<string>(true)
            {
                { "Sanyue", 1f },
                { "Aya", 1f },
                { "Rin", 1f }
            };
                EnemyPoolAct3 = new UniqueRandomPool<string>(true)
            {
                { "Sanyue", 1f },
                { "Aya", 1f },
                { "Rin", 1f }
            };
                EliteEnemyPool = new UniqueRandomPool<string>(true)
            {
                { "Sanyue", 1f },
                { "Aya", 1f },
                { "Rin", 1f }
            };
            }
        }
    }

    public sealed class AnotherNewStageExDef : StageTemplate
    {
        public override IdContainer GetId() => nameof(AnotherNewStage);


        public override StageConfig MakeConfig()
        {
            return DefaultConfig();
        }

        [EntityLogic(typeof(AnotherNewStageExDef))]
        public sealed class AnotherNewStage : NormalStageBase
        {
            public AnotherNewStage()
            {
                Level = 1;
                CardUpgradedChance = 0f;
                IsSelectingBoss = true;
                EnemyPoolAct1 = new UniqueRandomPool<string>(true)
            {
                { "11", 1f },
                { "11", 1f },
                { "11", 1f }
            };
                EnemyPoolAct2 = new UniqueRandomPool<string>(true)
            {
                { "11", 1f },
                { "11", 1f },
                { "11", 1f }
            };
                EnemyPoolAct3 = new UniqueRandomPool<string>(true)
            {
                { "11", 1f },
                { "11", 1f },
                { "11", 1f }
            };
                EliteEnemyPool = new UniqueRandomPool<string>(true)
            {
                { "11", 1f },
                { "11", 1f },
                { "11", 1f }
            };
            }
        }
    }
    */

    public static class NewBackgrounds
    {
        //public const string ghibli = "ghibli";
        //public const string ghibliDeez = "ghibliDeez";
        public const string HexagonBackground = "HexagonBackground";


        public static void AddNewBackgrounds()
        {
            //StageTemplate.AddEvironmentGameobject(StageTemplate.CreateSimpleEnvObject(ghibli, ResourceLoader.LoadSprite("ghibli.jpg", directorySource, ppu: 100)), managedByEnvironment: true);

            /*StageTemplate.AddEvironmentGameobject(StageTemplate.CreateSimpleEnvObject(ghibliDeez, ResourceLoader.LoadSprite("ghibliDeez.jpg", directorySource, ppu: 100)), managedByEnvironment: false,
                // makes it visible over regular bg
                adjustAfterSettingRoot: (self) =>
                {
                    self.transform.position += new Vector3(0, 0, -0.1f);
                    return self;
                });
            */
            StageTemplate.AddEvironmentGameobject(StageTemplate.CreateSimpleEnvObject(HexagonBackground, ResourceLoader.LoadSprite("HexagonBackground.png", directorySource, ppu: 100)), managedByEnvironment: false,
                // makes it visible over regular bg
                adjustAfterSettingRoot: (self) =>
                {
                    self.transform.position += new Vector3(0, 0, -0.1f);
                    return self;
                });

        }
    }

    /*
    public class StageExamples
    {

        static public void AddStages()
        {
            StageTemplate.ModifyStageList((list) =>
            {
                list.Insert(1, Library.CreateStage(new NewStageExDef().UniqueId));
                // more stages can be inserted
                return list;
            });

            // for test. AnotherNewStageExDef should be the first stage of the run
            StageTemplate.ModifyStageList((list) =>
            {
                list.Insert(1, Library.CreateStage(new AnotherNewStageExDef().UniqueId));
                return list;
            });

            StageTemplate.ModifyStageList((list) =>
            {
                list.Clear();
                list.Add(Library.CreateStage(typeof(BambooForest)).AsNormalFinal());
                list.Add(Library.CreateStage(typeof(FinalStage)).AsTrueEndFinal());

                return list;
            });

            StageTemplate.ModifyStage(nameof(BambooForest), (stage) =>
            {
                // can be new 
                stage.EnemyPoolAct1.Add(new BallsAndGirlDef().UniqueId, 1.5f);
                stage.EnemyPoolAct1.Add(new ThreeCrowsGroupDef().UniqueId, 1.5f);

                return stage;
            });

        }

    }
    */

}