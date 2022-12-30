using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shoelace.Piece;
namespace Shoelace.Board
{
    public class Board : MonoBehaviour,IBoard
    {

        public int xDimension;
        public int yDimension;

        public Piece.Piece.PiecePrefab[] piecePrefabs;
        public GameObject backGroundPrefab;
        private Dictionary<PieceType, GameObject> pieceDict; // dictionary collection for piece prefab
        private IPiece[,] pieces;

       
        

        // Start is called before the first frame update
        void Start()
        {
            FillThePiecePrefabDictionary();
            MakeAGridBoard();
            PopulateTheGridBoardWithPieces();
        }
        private void FillThePiecePrefabDictionary()
        {
            pieceDict = new Dictionary<PieceType, GameObject>();
            for (int i = 0; i < piecePrefabs.Length; i++)
            {
                if (!pieceDict.ContainsKey(piecePrefabs[i].type))
                {
                    pieceDict.Add(piecePrefabs[i].type, piecePrefabs[i].prefab);
                }
            }
        }
        private void MakeAGridBoard()
        {
            for (int i = 0; i < xDimension; i++)
            {
                for (int j = 0; j < yDimension; j++)
                {
                    GameObject backGround = (GameObject)Instantiate(backGroundPrefab, GetWworldPositionForPieces(i,j), Quaternion.identity);
                    backGround.transform.parent = transform;
                }
            }
        }
        private void PopulateTheGridBoardWithPieces()
        {
            pieces = new Piece.Piece[xDimension, yDimension];
            for (int i = 0; i < xDimension; i++)
            {
                for (int j = 0; j < yDimension; j++)
                {
                    //GameObject newPiece = (GameObject)Instantiate(pieceDict[PieceType.Normal], Vector3.zero, Quaternion.identity);
                    //newPiece.name = "Piece(" + i + "," + j + ")";
                    //newPiece.transform.parent = transform;
                    //pieces[i, j] = newPiece.GetComponent<IPiece>();
                    //pieces[i, j].Init(i,j,this,PieceType.Normal);
                    //if(pieces[i,j].IsMovable())
                    //{
                    //    pieces[i, j].MovablePiece.Move(i, j);
                    //}
                    //if(pieces[i,j].IsColored())
                    //{
                    //    pieces[i, j].ColorPiece.SetColor((ColorType)Random.Range(0,pieces[i,j].ColorPiece.colorSprites.Length));
                    //}
                    SpawnPiece(i, j, PieceType.Empty);
                }
            }
        }
        public Vector2 GetWworldPositionForPieces(int x, int y)
        {
            return new Vector2(transform.position.x - xDimension/2.0f + x, transform.position.y - yDimension / 2.0f + y);
        }
        public Piece.Piece SpawnPiece(int x, int y, PieceType type)
        {
            GameObject newPiece = (GameObject)Instantiate(pieceDict[type], GetWworldPositionForPieces(x, y), Quaternion.identity);
            newPiece.transform.parent = transform;
            pieces[x, y] = newPiece.GetComponent<Piece.Piece>();
            pieces[x, y].Init(x, y, this, type);
            return (Piece.Piece)pieces[x, y];
        }
        public void Fill()
        {

        }
        public bool FillStep()
        {
            bool movedPiece = false;
            for (int j = yDimension-2; j >= 0; j--)
            {
                for (int i = 0; i < xDimension; i++)
                {
                    Piece.Piece piece = (Piece.Piece)pieces[i, j];
                    if(piece.IsMovable())
                    {
                        Piece.Piece pieceBelow = (Piece.Piece)pieces[i, j + 1];
                        if(pieceBelow.Type == PieceType.Empty)
                        {
                            piece.MovablePiece.Move(i, j + 1);
                            pieces[i, j + 1] = piece;
                            SpawnPiece(i, j, PieceType.Empty);
                            movedPiece = true;
                        }
                    }
                }
            }
            for (int i = 0; i < xDimension; i++)
            {
                Piece.Piece pieceBelow = (Piece.Piece)pieces[i, 0];
                if(pieceBelow.Type == PieceType.Empty)
                {
                    GameObject newPiece = (GameObject)Instantiate(pieceDict[PieceType.Normal], GetWworldPositionForPieces(i, -1), Quaternion.identity);
                    newPiece.transform.parent = transform;
                    pieces[i, 0] = newPiece.GetComponent<Piece.Piece>();
                    pieces[i, 0].Init(i, -1, this, PieceType.Normal);
                    pieces[i, 0].MovablePiece.Move(i, 0);
                    pieces[i, 0].ColorPiece.SetColor((ColorType)Random.Range(0, (int)ColorType.Count));
                    movedPiece = true;
                }
            }
            return movedPiece;
        }
    }
}
