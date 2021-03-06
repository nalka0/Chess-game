using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static TyleType;

public class Tile : MonoBehaviour
{
    #region Properties
    public Vector2 Position;
    public TyleType Type;
    public bool IsEmpty { get { return GetComponentInChildren<Piece>() == null; } }
    public Piece ContainedPiece { get { return GetComponentInChildren<Piece>(); } }
    #endregion

    #region Unity Methods
    private void Update()
    {

    }
    #endregion

    #region Methods
    public void Highlight()
    {
        GameObject highlighter = Instantiate(GameObject.Find("highlighter"), transform);
        highlighter.transform.localPosition = new Vector3(-0.5f, 0.5f, 0.001f);
        highlighter.transform.Rotate(90, 0, 0);
    }
    #endregion
}