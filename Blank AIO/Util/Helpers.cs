﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BlankAIO;
using EnsoulSharp;
using EnsoulSharp.SDK;
using SharpDX;
using Version = System.Version;
using HeroManager = EnsoulSharp.SDK.GameObjects;

namespace BlankAIO.Util
{
    internal static class Helpers
    {
        /// <summary>
        ///     ReversePosition
        /// </summary>
        /// <param name="positionMe"></param>
        /// <param name="positionEnemy"></param>
        /// <remarks>Credit to LXMedia1</remarks>
        /// <returns>Vector3</returns>
        public static Vector3 ReversePosition(Vector3 positionMe, Vector3 positionEnemy)
        {
            var x = positionMe.X - positionEnemy.X;
            var y = positionMe.Y - positionEnemy.Y;
            return new Vector3(positionMe.X + x, positionMe.Y + y, positionMe.Z);
        }

        public static void UpdateCheck()
        {
            Task.Factory.StartNew(
                () =>
                {
                    try
                    {
                        using (var c = new WebClient())
                        {
                            var rawVersion =
                                c.DownloadString(
                                    "https://raw.githubusercontent.com/h3h3/LeagueSharp/master/Support/Properties/AssemblyInfo.cs");
                            var match =
                                new Regex(
                                    @"\[assembly\: AssemblyVersion\(""(\d{1,})\.(\d{1,})\.(\d{1,})\.(\d{1,})""\)\]")
                                    .Match(rawVersion);

                            if (match.Success)
                            {
                                var gitVersion =
                                    new Version(
                                        string.Format(
                                            "{0}.{1}.{2}.{3}", match.Groups[1], match.Groups[2], match.Groups[3],
                                            match.Groups[4]));  
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                });
        }

        public static void PrintMessage(string message)
        {
            Chat.Print("<font color='#15C3AC'>BlankAIO:</font> <font color='#FFFFFF'>" + message + "</font>");
        }

        public static bool EnemyInRange(int numOfEnemy, float range)
        {
            return ObjectManager.Player.CountEnemyHeroesInRange((int) range) >= numOfEnemy;
        }

        public static List<AIHeroClient> AllyInRange(float range)
        {
            return
                HeroManager.AllyHeroes
                    .Where(
                        h =>
                            ObjectManager.Player.Distance(h.Position) < range && h.IsAlly && !h.IsMe && h.IsValid &&
                            !h.IsDead)
                    .OrderBy(h => ObjectManager.Player.Distance(h.Position))
                    .ToList();
        }

        public static AIHeroClient AllyBelowHp(int percentHp, float range)
        {
            foreach (var ally in HeroManager.AllyHeroes)
            {
                if (ally.IsMe)
                {
                    if (((ObjectManager.Player.Health/ObjectManager.Player.MaxHealth)*100) < percentHp)
                    {
                        return ally;
                    }
                }
                else if (ally.IsAlly)
                {
                    if (Vector3.Distance(ObjectManager.Player.Position, ally.Position) < range &&
                        ((ally.Health/ally.MaxHealth)*100) < percentHp)
                    {
                        return ally;
                    }
                }
            }

            return null;
        }
    }
}