using Cysharp.Threading.Tasks.Triggers;
using HarmonyLib;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.JadeBoxes;
using LBoL.Core.Randoms;
using LBoL.Core.Stations;
using LBoL.Core.Units;
using LBoL.EntityLib.Adventures;
using LBoL.EntityLib.Stages.NormalStages;
using LBoL.Presentation;
using LBoL.Presentation.Effect;
using LBoL.Presentation.UI.Widgets;
using LBoL.Presentation.Units;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using DayuuMod.EnemyGroups;
using DayuuMod.EnemyUnits;
using DayuuMod.Exhibits;
using UnityEngine;

namespace DayuuMod.JadeBoxes
{
    public sealed class InterdimensionalObjectJadeBoxDef : JadeBoxTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(InterdimensionalObjectJadeBox);
        }

        public override LocalizationOption LoadLocalization()
        {
            return new DirectLocalization(new Dictionary<string, object>() {
                { "Name", "Interdimensional Object" },
                { "Description", "Hexagon will always appear."}
            });
        }
        public override JadeBoxConfig MakeConfig()
        {
            var config = DefaultConfig();
            return config;
        }
    }
    [EntityLogic(typeof(InterdimensionalObjectJadeBoxDef))]
    public sealed class InterdimensionalObjectJadeBox : JadeBox
    {
        [HarmonyPatch(typeof(FinalStage), nameof(FinalStage.InitBoss))]
        class FinalStage_InitBoss_Patch
        {
            static bool Prefix(FinalStage __instance, RandomGen rng)
            {
                if (GameMaster.Instance.CurrentGameRun != null && GameMaster.Instance.CurrentGameRun.JadeBox.Any((jb) => jb is InterdimensionalObjectJadeBox))
                {
                    __instance.BossPool = new RepeatableRandomPool<string> { { "Hexagon", 1f } };
                    __instance.Boss = Library.GetEnemyGroupEntry(__instance.BossPool.SampleOrDefault(rng));
                    return false;
                }
                return true;
            }
        }
    }
}
