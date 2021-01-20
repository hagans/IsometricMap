using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class Cell
{    
    /// <summary>
    /// Creates an empty <see cref="Cell"/> at the specified <see cref="Tilemap"/> position;
    /// </summary>
    /// <param name="map"></param>
    /// <param name="position"></param>
    public Cell(Map map, Vector3Int position)
    {        
        Map = map;
        Position = position;
    }

    /// <summary>
    /// <see cref="IPlaceable"/>s placed at this instance.
    /// </summary>
    public ICollection<IPlaceable> Placeables { get; } = new List<IPlaceable>();

    /// <summary>
    /// <see cref="Map"/> of which this instance is part.
    /// </summary>
    public Map Map { get; }

    /// <summary>
    /// Position of the <see cref="Cell"/> on the <see cref="Tilemap"/>.
    /// </summary>
    public Vector3Int Position { get; }

    /// <summary>
    /// Indicates if the <see cref="Cell"/> can accept more <see cref="IPlaceable"/>s.
    /// </summary>
    public bool IsEmpty => !Placeables.Any(placeable => placeable.FillsUpCell);

    /// <summary>
    /// Tile representing this cell.
    /// </summary>
    public TileBase Tile
    {
        get => Map.Tilemap.GetTile(Position);
        set => Map.Tilemap.SetTile(Position, value);
    }

    /// <summary>
    /// World position of the <see cref="Cell"/>, any transform placed at this position will be above the cell.
    /// </summary>
    public Vector3 WorldPosition => Map.WorldPositionOfCell(this);

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
    /// Iterable that loops through all adjacent <see cref="Cell"/>s.
    /// </summary>
    /// <returns><see cref="IEnumerable{T}"/> containing the specified <see cref="Cell"/>s.</returns>
    public IEnumerable<Cell> Neighbours()
    {
        if (Map.TryGetCell(Position + Vector3Int.right, out var cell)) yield return cell;
        if (Map.TryGetCell(Position + Vector3Int.left, out cell)) yield return cell;
        if (Map.TryGetCell(Position + Vector3Int.down, out cell)) yield return cell;
        if (Map.TryGetCell(Position + Vector3Int.up, out cell)) yield return cell;

        if (Position.y % 2 == 0)
        {
            if (Map.TryGetCell(Position + new Vector3Int(-1,1,0), out cell)) yield return cell;
            if (Map.TryGetCell(Position + new Vector3Int(-1, -1, 0), out cell)) yield return cell;
        }
        else
        {            
            if (Map.TryGetCell(Position + new Vector3Int(1, 1, 0), out cell)) yield return cell;            
            if (Map.TryGetCell(Position + new Vector3Int(1, -1, 0), out cell)) yield return cell;
        }
    }

    /// <summary>
    /// Distance between this <see cref="Cell"/> and the specified one.
    /// </summary>
    public int Distance(Cell to) => Distance(this, to);
}
