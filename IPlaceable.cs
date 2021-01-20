public interface IPlaceable
{
    /// <summary>
    /// Indicates if the <see cref="Cell"/> can accept other <see cref="IPlaceable"/> while containing this instance.
    /// </summary>
    bool FillsUpCell { get; }

    /// <summary>
    /// Places the instance in the specified cell, if possible.
    /// </summary>
    /// <param name="cell"><see cref="Cell"/> where the placeable will be placed.</param>
    /// <returns><see langword="true"/> if the instance was placed, otherwise <see langword="false"/>.</returns>
    bool TryToPlace(Cell cell);

    /// <summary>
    /// Moves the instance in the specified cell, if possible.
    /// </summary>
    /// <param name="cell"><see cref="Cell"/> where the placeable will be moved.</param>
    /// <returns><see langword="true"/> if the instance was placed, otherwise <see langword="false"/>.</returns>
    bool TryToMove(Cell cell);
}
