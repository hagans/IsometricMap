using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map : MonoBehaviour
{
    [SerializeField] Tilemap _tilemap;
    List<Cell> _cells = new List<Cell>();


    /// <summary>
    /// <see cref="Tilemap"/> which contains all the graphics.
    /// </summary>
    public Tilemap Tilemap => _tilemap;


    /// <summary>
    /// Defines whether the instance contains a <see cref="Cell"/> at the specified position.
    /// </summary>
    /// <param name="position">Position of the searched <see cref="Cell"/>.</param>
    /// <returns><see langword="true"/> if <see cref="Cell"/> exists, otherwise <see langword="false"/>.</returns>
    public bool HasCellAt(Vector3 position) => _tilemap.GetTile(_tilemap.WorldToCell(position));

    /// <summary>
    /// Gets the <see cref="Cell"/> at the specified world coordinates.
    /// </summary>
    /// <param name="position">World coordinates of the desired <see cref="Cell"/>.</param>
    /// <returns>The specified <see cref="Cell"/>, if it exists, otherwhise <see cref="null"/>.</returns>
    public Cell GetCellAtWorldPosition(Vector3 position)
    {
        var cellPosition = _tilemap.WorldToCell(position);

        if (_tilemap.GetTile(cellPosition))
            return GetCell(cellPosition);

        return null;
    }

    /// <summary>
    /// Gets the <see cref="Cell"/> at the current mouse position.
    /// </summary>
    /// <returns>The specified <see cref="Cell"/> if it exists, otherwhise <see langword="null"/>.</returns>
    public Cell GetCellAtMousePosition() => GetCellAtWorldPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));

    /// <summary>
    /// Gets the world coordinates of the specified <see cref="Cell"/>.
    /// </summary>
    public Vector3 WorldPositionOfCell(Cell cell) => _tilemap.CellToWorld(cell.Position);
    
    Cell GetCell(Vector3Int position)
    {
        for (int i = 0; i < _cells.Count; i++)
        {
            if (_cells[i].Position == position)
                return _cells[i];
        }

        var newCell = new Cell(this, position);
        _cells.Add(newCell);
        return newCell;        
    }
}



public class Cell
{
    Map _map;

        
    public Cell(Map map, Vector3Int position)
    {
        _map = map;
        Position = position;
    }
    
    
    /// <summary>
    /// Position of the <see cref="Cell"/> on the <see cref="Tilemap"/>.
    /// </summary>
    public Vector3Int Position { get; }    

    /// <summary>
    /// Tile representing this cell.
    /// </summary>
    public TileBase Tile
    {
        get => _map.Tilemap.GetTile(Position);
        set => _map.Tilemap.SetTile(Position, value);
    }

    /// <summary>
    /// World position of the <see cref="Cell"/>, any transform placed at this position will be above the cell.
    /// </summary>
    public Vector3 WorldPosition => _map.WorldPositionOfCell(this);
    
    
    /// <summary>
    /// Calculates the number of cells between a and b.
    /// </summary>
    public static int Distance(Cell a, Cell b)
    {
        int deltaX = Mathf.Abs(a.Position.x - b.Position.x);
        int deltaY = Mathf.Abs(a.Position.y - b.Position.y);

        if ((a.Position.x > b.Position.x) ^ a.Position.y % 2 != 0)
            deltaX = Mathf.Max(0, deltaX - (deltaY + 1) / 2);
        else
            deltaX = Mathf.Max(0, deltaX - deltaY / 2);
        return deltaX + deltaY;
    }

    /// <summary>
    /// Distance between this <see cref="Cell"/> and the specified one.
    /// </summary>
    public int Distance(Cell to) => Distance(this, to);
}
