using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Piece;
using static Player;

public class KnightMoveStrategy : IMoveStrategy
{
    #region IMoveStrategy
    public List<Vector2> GetAvailableTiles(Vector2 Origin, PieceColor MovingColor)
    {
        List<Vector2> ret = new List<Vector2>();
        List<Vector2> ToCheck = new List<Vector2>()
        {
            new Vector2(-2, 1),
            new Vector2(-2, -1),
            new Vector2(-1 , 2),
            new Vector2(-1, -2),
            new Vector2(1, 2),
            new Vector2(1, -2),
            new Vector2(2, 1),
            new Vector2(2, -1)
        };
        Tile CurrentTile;
        foreach (Vector2 element in ToCheck)
            if ((CurrentTile = Board.Current.GetTileByPos(Origin + element)) != null && CurrentTile.ContainedPiece?.Color != MovingColor)
                ret.AddIfNotChecking(CurrentTile.Position, Origin, MovingColor);
        return ret;
    }

    public void Move(Vector2 Origin, Vector2 Destination, PieceColor MovingColor) { }

    public List<Vector2> UnsafeGetAvailableTiles(Vector2 Origin, PieceColor MovingColor)
    {
        List<Vector2> ret = new List<Vector2>();
        List<Vector2> ToCheck = new List<Vector2>()
        {
            new Vector2(-2, 1),
            new Vector2(-2, -1),
            new Vector2(-1 , 2),
            new Vector2(-1, -2),
            new Vector2(1, 2),
            new Vector2(1, -2),
            new Vector2(2, 1),
            new Vector2(2, -1)
        };
        Tile CurrentTile;
        foreach (Vector2 element in ToCheck)
            if ((CurrentTile = Board.Current.GetTileByPos(Origin + element)) != null && CurrentTile.ContainedPiece?.Color != MovingColor)
                ret.Add(CurrentTile.Position);
        return ret;
    }
    #endregion
}
