using Shoelace.Board;
using Shoelace.Piece;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shoelace.Piece
{
    public class MovablePiece : MonoBehaviour
    {
        private Piece piece;
        private void Awake()
        {
            piece = GetComponent<Piece>();
        }

        public void Move(int newX, int newY)
        {
            piece.X = newX;
            piece.Y = newY;
            piece.transform.localPosition = piece.boardRef.GetWworldPositionForPieces(newX,newY);
        }
    }
}
