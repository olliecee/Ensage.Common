﻿namespace Ensage.Common
{
    using System.Linq;

    using Ensage.Common.Extensions;

    /// <summary>
    ///     Class used to find targets based on conditions
    /// </summary>
    public class TargetSelector
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Find enemy hero that takes least hits to kill
        /// </summary>
        /// <param name="source">Source hero</param>
        /// <returns></returns>
        public static Hero BestAutoAttackTarget(Hero source)
        {
            var attackRange = source.GetAttackRange();
            var enemyHeroes =
                ObjectMgr.GetEntities<Hero>()
                    .Where(
                        x =>
                        x.Team == source.GetEnemyTeam() && !x.IsIllusion && x.IsAlive && x.IsVisible
                        && x.Distance2D(source) <= (attackRange + x.HullRadius / 2));
            var aaDmg = source.MinimumDamage + source.BonusDamage;
            Hero bestTarget = null;
            var lastHitsToKill = 0f;
            foreach (var enemyHero in enemyHeroes)
            {
                var takenDmg = enemyHero.DamageTaken(aaDmg, DamageType.Physical, source, false);
                var hitsToKill = enemyHero.Health / takenDmg;
                if (bestTarget != null && !(lastHitsToKill < hitsToKill))
                {
                    continue;
                }
                bestTarget = enemyHero;
                lastHitsToKill = hitsToKill;
            }
            return bestTarget;
        }

        /// <summary>
        ///     Finds target closest to mouse in specified range
        /// </summary>
        /// <param name="source"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        public static Hero ClosestToMouse(Hero source, float range = 1000)
        {
            var mousePosition = Game.MousePosition;
            var enemyHeroes =
                ObjectMgr.GetEntities<Hero>()
                    .Where(
                        x =>
                        x.Team == source.GetEnemyTeam() && !x.IsIllusion && x.IsAlive && x.IsVisible
                        && x.Distance2D(mousePosition) <= range);
            Hero closestHero = null;
            foreach (var enemyHero in enemyHeroes)
            {
                if (closestHero == null || closestHero.Distance2D(mousePosition) > enemyHero.Distance2D(mousePosition))
                {
                    closestHero = enemyHero;
                }
            }
            return closestHero;
        }

        #endregion
    }
}