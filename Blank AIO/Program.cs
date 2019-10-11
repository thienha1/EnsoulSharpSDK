using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using EnsoulSharp;
using EnsoulSharp.SDK;
using EnsoulSharp.SDK.MenuUI;
using SharpDX;
using BlankAIO.Champions;

namespace BlankAIO
{
    internal class Program
    {
        public static AIHeroClient player;
        public static string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

        private static void Main(string[] args)
        {
            GameEvent.OnGameLoad += OnGameLoad;
        }

        private static void OnGameLoad()
        {
            try
            {
                player = ObjectManager.Player;
                if (player.CharacterName == "Leona")
                {
                    Leona.OnLoad();
                }
                else if (player.CharacterName == "Pyke")
                {
                    Pyke.OnLoad();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Failed To load: " + e);
            }
        }
    }
}
