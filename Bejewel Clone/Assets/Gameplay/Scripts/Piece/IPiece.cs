using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shoelace.Board;
namespace Shoelace.Piece
{
    public interface IPiece 
    {
        public int X
        {
            get;
            set;
        }
        public int Y
        {
            get;
            set;
        }
        public abstract void Init(int _x, int _y, IBoard _board, PieceType _type);
        public bool IsMovable();
        public bool IsColored();
        public bool IsClearable();
        public MovablePiece MovablePiece
        {
            get;
        }
        public ColorPiece ColorPiece
        {
            get;
        }
    }
}
