using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static PieceColor;
using static Player;

public class Piece : MonoBehaviour
{
    #region Properties
    public static Piece SelectedPiece
    {
        get { return _selectedPiece; }
        set
        {
            _selectedPiece = value;
            Highlighter.KillAll();
            foreach (Vector2 AvailableTile in value?.GetReachableTiles() ?? new List<Vector2>())
            {
                Board.Current.GetTileByPos(AvailableTile).Highlight();
            }
        }
    }

    public Tile ContainingTile
    {
        get { return transform.parent.GetComponent<Tile>(); }
        set
        {
            transform.SetParent(value.transform);
            transform.localPosition = new Vector3(-0.5f, 0.5f, 0);
            transform.localRotation = new Quaternion(0, 0, 0, 0);
        }
    }

    public PieceColor Color
    {
        get { return _color; }
        set
        {
            _color = value;
            GetComponent<MeshRenderer>().materials.ToList().ForEach(material => material.color = value == White ? UnityEngine.Color.white : UnityEngine.Color.black);
        }
    }

    public IMoveStrategy MovementType
    {
        get { return _movementType; }
        set { _movementType = value; }
    }
    #endregion

    #region Fields
    [SerializeField]
    [HideInInspector]
    private PieceColor _color;
    [SerializeField]
    [HideInInspector]
    private IMoveStrategy _movementType;
    private static Piece _selectedPiece;
    #endregion

    #region Unity Methods
    private void Awake() => SetMoveStrategy();

    private void OnMouseDown()
    {
        if (Color != None && Color == LocalPlayer.ControlledColor && LocalPlayer.Active)
        {
            SelectedPiece = SelectedPiece == this ? null : this;
        }
        else if (SelectedPiece.CanMove(ContainingTile.Position))
        {
            SelectedPiece.Move(ContainingTile);
        }
    }
    #endregion

    #region Methods
    private List<Vector2> GetReachableTiles() => MovementType.GetAvailableTiles(ContainingTile.Position, Color);

    public bool CanMove(Vector2 Destination) => GetReachableTiles().Contains(Destination);

    public bool UnsafeCanMove(Vector2 Destination) => MovementType.UnsafeGetAvailableTiles(ContainingTile.Position, Color).Contains(Destination);

    public void Move(Tile Destination)
    {
        if (CanMove(Destination.Position))
        {
            if (Destination.GetComponentInChildren<Piece>() != null)
            {
                DestroyImmediate(Destination.GetComponentInChildren<Piece>().gameObject);
            }
            Vector2 previousPosition = ContainingTile.Position;
            ContainingTile = Destination;
            SelectedPiece = null;
            MovementType.Move(previousPosition, Destination.Position, Color);
            if (Board.Current.KingInCheck(Color == White ? Black : White))
            {
                //here is the code that happens when a player is checked
                switch (Color)
                {
                    case Black:
                        WhitePlayer.CheckedOnce = true;
                        break;
                    case White:
                        BlackPlayer.CheckedOnce = true;
                        break;
                }
            }
        }
    }

    public void SetMoveStrategy()
    {
        if (gameObject.name.Contains(PieceName.Pawn.ToString()))
            MovementType = new PawnMoveStrategy();
        else if (gameObject.name.Contains(PieceName.Bishop.ToString()))
            MovementType = new BishopMoveStrategy();
        else if (gameObject.name.Contains(PieceName.Knight.ToString()))
            MovementType = new KnightMoveStrategy();
        else if (gameObject.name.Contains(PieceName.Rook.ToString()))
            MovementType = new RookMoveStrategy();
        else if (gameObject.name.Contains(PieceName.Queen.ToString()))
            MovementType = new QueenMoveStrategy();
        else if (gameObject.name.Contains(PieceName.King.ToString()))
            MovementType = new KingMoveStrategy();
    }

    public PieceName GetPieceName()
    {
        List<PieceName> AllNames = new List<PieceName>();
        foreach (PieceName element in typeof(PieceName).GetEnumValues())
        {
            if (gameObject.name.Contains(element.ToString()))
            {
                return element;
            }
        }
        return PieceName.Unknown;
    }
    #endregion
}