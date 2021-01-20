using UnityEngine;

public class PlaceableBehaviour : MonoBehaviour, IPlaceable
{
    [SerializeField, Tooltip("Indicates if the " + nameof(Cell) + " can accept other " + nameof(IPlaceable) + "s while containing this instance.")]
    bool _fillsUpCell = false;

    [SerializeField, Tooltip("The max distance this " + nameof(PlaceableBehaviour) + " can be displaced in a movement.")] 
    int _maxDistance = int.MaxValue;


    public Cell CurrentCell { get; private set; }    

    public Map CurrentMap => CurrentCell.Map;

    public bool FillsUpCell => _fillsUpCell;


    public bool TryToPlace(Cell cell)
    {
        if (cell.IsEmpty)
        {
            CurrentCell?.Placeables.Remove(this);
            CurrentCell = cell;
            cell.Placeables.Add(this);
            transform.position = cell.WorldPosition;
            return true;
        }

        return false;
    }    

    public bool TryToMove(Cell cell)
    {
        if (CurrentCell == null) 
            throw new System.InvalidOperationException(nameof(IPlaceable) + "must be placed in a cell in order to move it. Use " + nameof(TryToPlace) + " instead of.");

        if (CurrentCell.Distance(cell) <= _maxDistance)
            return TryToPlace(cell);

        return false;
    }
}
