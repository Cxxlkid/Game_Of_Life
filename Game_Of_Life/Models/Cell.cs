namespace Game_Of_Life.Models
{
    public class Cell
    {
        public bool IsAlive { get; set; }

        public Cell()
        {
            IsAlive = false; // Initial state is dead
        }
    }
}