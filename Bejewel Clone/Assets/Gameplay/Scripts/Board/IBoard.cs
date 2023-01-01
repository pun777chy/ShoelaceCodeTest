using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shoelace.Piece;
namespace Shoelace.Board
{
    public interface IBoard
    {
        public Vector2 GetWworldPositionForPieces(int x, int y);
        public MainPiece SpawnPiece(int x, int y, PieceType type);
        public void SwapPieces(MainPiece piece1, MainPiece piece2);
        public List<MainPiece> GetMatch(MainPiece piece, int newX, int newY);
        public IEnumerator Fill();
        public bool FillStep();
        public bool IsAdjacent(Piece.MainPiece piece1, Piece.MainPiece piece);
        public void PressPiece(MainPiece piece);
        public void EnterPiece(MainPiece piece);
        public void ReleasePiece();
        public bool ClearPiece(List<MainPiece> match,int x, int y);
        public bool ClearAllValidMatches();
    }
}
