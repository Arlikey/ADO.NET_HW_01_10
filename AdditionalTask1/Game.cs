namespace GameStudioClasses
{
    public class Game
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Developer { get; set; }
        public Genre Genre { get; set; }
        public DateTime ReleaseDate { get; set; }
    }

    public enum Genre
    {
        Action,
        Adventure,
        RolePlaying,
        Strategy,
        Simulation,
        Sports,
        Puzzle,
        Horror,
        Racing,
        Fighting,
        Shooter,
        Platformer,
        Sandbox
    }
}
