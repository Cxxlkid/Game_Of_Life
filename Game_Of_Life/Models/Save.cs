namespace Game_Of_Life.Models;

public class Save
{
    private int _gridSize;

    public int X
    {
        get => _gridSize;
        set => _gridSize = value;
    }
    
    public Save(int gridSize)
    {
        _gridSize = gridSize;
    }
}