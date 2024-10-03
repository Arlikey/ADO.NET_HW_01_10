using GameStudioClasses;
using GameStudioDbContextLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO.NET_HW_03_10
{
    internal class GameStudio
    {
        static void Main(string[] args)
        {
            using (GameStudioDbContext db = new GameStudioDbContext())
            {
                db.Games.AddRange([
                    new Game { Title = "The Witcher 3: Wild Hunt", Developer = "CD Projekt Red", Genre = Genre.RolePlaying, ReleaseDate = new DateTime(2015, 5, 19) },
                    new Game { Title = "Grand Theft Auto V", Developer = "Rockstar North", Genre = Genre.Action, ReleaseDate = new DateTime(2013, 9, 17) },
                    new Game { Title = "Minecraft", Developer = "Mojang Studios", Genre = Genre.Sandbox, ReleaseDate = new DateTime(2011, 11, 18) },
                    new Game { Title = "Dark Souls", Developer = "FromSoftware", Genre = Genre.Action, ReleaseDate = new DateTime(2011, 9, 22) },
                    new Game { Title = "Overwatch", Developer = "Blizzard Entertainment", Genre = Genre.Shooter, ReleaseDate = new DateTime(2016, 5, 24) }
                    ]);
                db.SaveChanges();

                var games = db.Games.ToList();

                foreach (var game in games)
                {
                    Console.WriteLine($"Id: {game.Id} | Title: {game.Title} | Developer: {game.Developer} | Genre: {game.Genre}");
                }

                Console.WriteLine("\n\nACTION GAMES\n\n");

                var actionGames = db.Games.Where(g => g.Genre == Genre.Action).ToList();

                foreach (var game in actionGames)
                {
                    Console.WriteLine($"Id: {game.Id} | Title: {game.Title} | Developer: {game.Developer} | Genre: {game.Genre}");
                }
            }
        }
    }
}
