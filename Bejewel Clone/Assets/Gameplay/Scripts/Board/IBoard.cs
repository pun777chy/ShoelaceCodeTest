using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shoelace.Piece;
namespace Shoelace.Board
{
    public interface IBoard
    {
        public Vector2 GetWworldPositionForPieces(int x, int y);
        public Piece.MainPiece SpawnPiece(int x, int y, PieceType type);
        public IEnumerator Fill();
        public bool FillStep();
        public bool IsAdjacent(Piece.MainPiece piece1, Piece.MainPiece piece);
        public void PressPiece(MainPiece piece);
        public void EnterPiece(MainPiece piece);
        public void ReleasePiece();
    }
}
