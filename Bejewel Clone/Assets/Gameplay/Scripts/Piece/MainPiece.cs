using Shoelace.Board;
using UnityEngine;
namespace Shoelace.Piece
{
    public enum PieceType
    {
        Empty,
        Normal,
        Brick,
        Count,
    }
    public class MainPiece : MonoBehaviour,IPiece
    {
        [System.Serializable]
        public struct PiecePrefab
        {
            public PieceType type;
            public GameObject prefab;
        }
        private int x;
        private int y;
        public int X
        {
            get
            {
                return x;
            }
            set
            {
                if(IsMovable())
                {
                    x = value;
                }
            }
        }
        public int Y
        {
            get
            {
                return y;
            }
            set
            {
                if(IsMovable())
                {
                    y = value;
                }
            }
            
        }
        private PieceType type;
        public PieceType Type
        {
            get { return type; }
        }

        private IBoard board;
        public IBoard boardRef
        {
            get
            {
                return board;
            }
        }
        private MovablePiece movablePiece;
        public MovablePiece MovablePiece
        {
            get
            {
                return movablePiece;
            }
        }
        private ColorPiece colorPiece;
        public ColorPiece ColorPiece
        {
            get
            {
                return colorPiece;
            }
        }
        private ClearablePiece clearablePiece;
        public ClearablePiece ClearablePiece
        {
            get
            {
                return clearablePiece;
            }
        }
        private void Awake()
        {
            movablePiece = GetComponent<MovablePiece>();
            colorPiece = GetComponent<ColorPiece>();
            clearablePiece = GetComponent<ClearablePiece>();
        }
    

        public void Init(int _x, int _y, IBoard _board, PieceType _type)
        {
           
            x = _x;
            y = _y;
            board = _board;
            type = _type;
            
        }
        private void OnMouseEnter()
        {
            boardRef.EnterPiece(this);
        }
        private void OnMouseDown()
        {
            boardRef.PressPiece(this);
        }
        private void OnMouseUp()
        {
            boardRef.ReleasePiece();
        }
        public bool IsMovable()
        {
            return movablePiece != null;
        }
        public bool IsColored()
        {
            return colorPiece != null;
        }
        public bool IsClearable()
        {
            return clearablePiece != null;
        }
    }
}
