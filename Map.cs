using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map : MonoBehaviour
{
    [SerializeField]
    Tilemap _tilemap = null;

    List<Cell> _cells = new List<Cell>();


    /// <summary>
    /// <see cref="Tilemap"/> which contains all the graphics.
    /// </summary>
    public Tilemap Tilemap => _tilemap;

    public bool HasCellAt(Vector3Int position) => Tilemap.GetTile(position);

    /// <summary>
    /// Defines if the instance contains a <see cref="Cell"/> at the specified world position.
    /// </summary>
    /// <param name="position">Position of the searched <see cref="Cell"/>.</param>
    /// <returns><see langword="true"/> if <see cref="Cell"/> exists, otherwise <see langword="false"/>.</returns>
    public bool HasCellAtWorldPosition(Vector3 position) => HasCellAt(Tilemap.WorldToCell(position));

    /// <summary>
    /// Gets the <see cref="Cell"/> at the specified <see cref="UnityEngine.Tilemaps.Tilemap"/> position.
    /// </summary>
    public Cell GetCell(Vector3Int position)
    {
        for (int i = 0; i < _cells.Count; i++)
        {
            if (_cells[i].Position == position)
                return _cells[i];
        }
        if (HasCellAt(position))
        {
            var newCell = new Cell(this, position);
            _cells.Add(newCell);
            return newCell;
        }

        throw new System.NullReferenceException("No existent " + nameof(Cell) + " at the specified position.");
    }

    /// <summary>
    /// Gets the cell at the specified position.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="cell"></param>
    /// <returns></returns>
    public bool TryGetCell(Vector3Int position, out Cell cell)
    {
        for (int i = 0; i < _cells.Count; i++)
        {
            if (_cells[i].Position == position)
            {
                cell = _cells[i];
                return true;
            }                
        }
        if (HasCellAt(position))
        {
            cell = new Cell(this, position);
            _cells.Add(cell);
            return true;
        }

        cell = default;
        return false;
    }

    /// <summary>
    /// Gets the <see cref="Cell"/> at the specified world coordinates.
    /// </summary>
    /// <param name="position">World coordinates of the desired <see cref="Cell"/>.</param>
    /// <returns>The specified <see cref="Cell"/>, if it exists, otherwhise <see cref="null"/>.</returns>
    public Cell GetCellAtWorldPosition(Vector3 position) => GetCell(Tilemap.WorldToCell(position));

    public bool TryGetCellAtWorldPosition(Vector3 position, out Cell cell) => TryGetCell(Tilemap.WorldToCell(position), out cell);

    /// <summary>
    /// Gets the <see cref="Cell"/> at the current mouse position.
    /// </summary>
    /// <returns>The specified <see cref="Cell"/> if it exists, otherwhise <see langword="null"/>.</returns>
    public Cell GetCellAtMousePosition() => GetCellAtWorldPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition));

    public bool TryGetCellAtMousePosition(out Cell cell) => TryGetCellAtWorldPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition), out cell);

    /// <summary>
    /// Gets the world coordinates of the specified <see cref="Cell"/>.
    /// </summary>
    public Vector3 WorldPositionOfCell(Cell cell) => Tilemap.CellToWorld(cell.Position);
}
