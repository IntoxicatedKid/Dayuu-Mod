using DayuuMod.EnemyUnits;
using HarmonyLib;
using LBoL.Base;
using LBoL.ConfigData;
using LBoL.EntityLib.EnemyUnits.Character;
using LBoL.EntityLib.EnemyUnits.Normal;
using LBoL.EntityLib.EnemyUnits.Normal.Ravens;
using LBoL.EntityLib.Stages.NormalStages;
using LBoL.Presentation;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;
using UnityEngine;

namespace DayuuMod.EnemyGroups
{
    public sealed class HexagonEnemyGroupDef : EnemyGroupTemplate
    {
        public override IdContainer GetId() => "Hexagon";
        public override EnemyGroupConfig MakeConfig()
        {
            var config = new EnemyGroupConfig(
                Id: "",
                Name: "Hexagon",
                FormationName: VanillaFormations.Single,
                Enemies: new List<string>() { nameof(Hexagon) },
                EnemyType: EnemyType.Boss,
                DebutTime: 1f,
                RollBossExhibit: false,
                PlayerRoot: new Vector2(-4f, 0.5f),
                PreBattleDialogName: "",
                PostBattleDialogName: ""
            );
            return config;
        }
        [HarmonyPatch(typeof(FinalStage), nameof(FinalStage.InitBoss))]
        class FinalStage_InitBoss_Patch
        {
            static void Prefix(FinalStage __instance)
            {
                __instance.BossPool.Add("Hexagon", 0.11f);
            }
        }
    }
}