using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shoelace.Board
{
    public interface IBoard
    {
        public Vector2 GetWworldPositionForPieces(int x, int y);
    }
}
