using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shoelace.Piece;
namespace Shoelace.Board
{
    public interface IBoard
    {
        public Vector2 GetWworldPositionForPieces(int x, int y);
        public Piece.Piece SpawnPiece(int x, int y, PieceType type);
        public IEnumerator Fill();
        public bool FillStep();
    }
}
