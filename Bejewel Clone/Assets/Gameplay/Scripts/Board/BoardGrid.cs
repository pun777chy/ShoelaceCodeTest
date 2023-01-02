using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shoelace.Piece;
namespace Shoelace.Board
{
    public class BoardGrid : MonoBehaviour,IBoard
    {
        public int xDimension;
        public int yDimension;
        public float fillTime;
        public MainPiece.PiecePrefab[] piecePrefabs;
        public GameObject backGroundPrefab;
        private Dictionary<PieceType, GameObject> pieceDict; // dictionary collection for piece prefab
        private MainPiece[,] pieces;
        private bool inverse;

        private MainPiece pressedPiece;
        private MainPiece enteredPiece;

        // A list of all the matches that have been found on the board.
      
        public List<MainPiece> neighborsList;
        // Start is called before the first frame update
        void Start()
        {
            FillThePiecePrefabDictionary();
            MakeAGridBoard();
            PopulateTheGridBoardWithPieces();
            StartCoroutine(Fill());
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
            pieces = new MainPiece[xDimension, yDimension];
            for (int i = 0; i < xDimension; i++)
            {
                for (int j = 0; j < yDimension; j++)
                {
                    SpawnPiece(i, j, PieceType.Empty);
                }
            }
        }
        public Vector2 GetWworldPositionForPieces(int x, int y)
        {
           // return new Vector2(transform.position.x - xDimension/2.0f + x, transform.position.y - yDimension / 2.0f + y);
            return new Vector2(transform.position.x + xDimension / 2.0f - x, transform.position.y + yDimension / 2.0f - y);
        }
        public MainPiece SpawnPiece(int x, int y, PieceType type)
        {
            GameObject newPiece = (GameObject)Instantiate(pieceDict[type], GetWworldPositionForPieces(x, y), Quaternion.identity);
            newPiece.transform.parent = transform;
            pieces[x, y] = newPiece.GetComponent<MainPiece>();
            pieces[x, y].Init(x, y, this, type);
            return pieces[x, y];
        }
        public IEnumerator Fill()
        {
            bool needsRefill = true;
            while (needsRefill)
            {
                yield return new WaitForSeconds(fillTime);
                while (FillStep())
                {
                    inverse = !inverse;
                    yield return new WaitForSeconds(fillTime);
                }
                needsRefill = ClearAllValidMatches();
            }
        }
        public bool FillStep()
        {
            bool movedPiece = false;
            for (int j = yDimension-2; j >= 0; j--)
            {
                for (int loopX = 0; loopX < xDimension; loopX++)
                {
                    int i = loopX;
                    if(inverse)
                    {
                        i = xDimension - 1 - loopX;

                    }
                    MainPiece piece = pieces[i, j];
                    if(piece.IsMovable())
                    {
                        MainPiece pieceBelow = pieces[i, j + 1];
                        if(pieceBelow.Type == PieceType.Empty)
                        {
                            Destroy(pieceBelow.gameObject);
                            piece.MovablePiece.Move(i, j + 1,fillTime);
                          
                            pieces[i, j + 1] = piece;
                            SpawnPiece(i, j, PieceType.Empty);
                            movedPiece = true;
                        }
                        else
                        {
                            for (int diag = -1; diag <= 1 ; diag++)
                            {
                                if(diag != 0)
                                {
                                    int diagX = xDimension + diag;
                                    if(!inverse)
                                    {
                                        diagX = i - diag;
                                    }
                                    if(diagX>=0 && diagX < xDimension)
                                    {
                                        MainPiece diagonalPiece = pieces[diagX, j + 1];
                                        if(diagonalPiece.Type == PieceType.Empty)
                                        {
                                            bool hasPieceAbove = true;
                                            for (int aboveY = j; aboveY >= 0; aboveY--)
                                            {
                                                MainPiece pieceAbove = pieces[diagX,aboveY];
                                                if(pieceAbove.IsMovable())
                                                {
                                                    break;
                                                }
                                                else if(!pieceAbove.IsMovable() && pieceAbove.Type != PieceType.Empty)
                                                {
                                                    hasPieceAbove = false;
                                                    break;
                                                }
                                            }
                                            if(!hasPieceAbove)
                                            {
                                                
                                                Destroy(diagonalPiece.gameObject);
                                                piece.MovablePiece.Move(diagX, j + 1, fillTime);
                                               
                                                pieces[diagX, j + 1] = piece;
                                                SpawnPiece(i, j, PieceType.Empty);
                                                movedPiece = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < xDimension; i++)
            {
                MainPiece pieceBelow = pieces[i, 0];
                if(pieceBelow.Type == PieceType.Empty)
                {
                    Destroy(pieceBelow.gameObject);
                    GameObject newPiece = (GameObject)Instantiate(pieceDict[PieceType.Normal], GetWworldPositionForPieces(i, -1), Quaternion.identity);
                    newPiece.transform.parent = transform;
                    newPiece.name = PieceType.Normal + "(" + i + "," + 0 + ")";
                    pieces[i, 0] = newPiece.GetComponent<MainPiece>();
                    pieces[i, 0].Init(i, -1, this, PieceType.Normal);
                    pieces[i, 0].MovablePiece.Move(i, 0,fillTime);
                    pieces[i, 0].ColorPiece.SetColor((ColorType)Random.Range(0, (int)ColorType.Count));
                    movedPiece = true;
                }
            }
            return movedPiece;
        }
        public bool IsAdjacent(MainPiece piece1, MainPiece piece2)
        {
            return (piece1.X == piece2.X && (int)Mathf.Abs(piece1.Y - piece2.Y) == 1)
                || (piece1.Y == piece2.Y && (int)Mathf.Abs(piece1.X - piece2.X) == 1);
        }
        public void SwapPieces(MainPiece piece1, MainPiece piece2)
        {
            if(piece1.IsMovable() && piece2.IsMovable())
            {
                pieces[piece1.X, piece2.Y] = piece2;
                pieces[piece2.X, piece1.Y] = piece1;
            
                int piece1X = piece1.X;
                int piece1Y = piece1.Y;

                piece1.MovablePiece.Move(piece2.X, piece2.Y, fillTime);
                piece2.MovablePiece.Move(piece1X, piece1Y, fillTime);
                if (ConductSearch(piece1) != null)
                {
                     ClearAllValidMatches();
                     StartCoroutine(Fill());    
                  }
                else
                {
                  StartCoroutine(SwapItBack( piece1, piece2));
               
                }

            }
           
        }
        IEnumerator SwapItBack(MainPiece piece1, MainPiece piece2)
        {
                yield return new WaitForSeconds(0.5f); // time can be adjusted later this is just for test
                pieces[piece1.X, piece1.Y] = piece1;
                pieces[piece2.X, piece2.Y] = piece2;
                if (piece1.IsMovable() && piece2.IsMovable())
                {
                     int piece1X = piece1.X;
                     int piece1Y = piece1.Y;
                     piece1.MovablePiece.Move(piece2.X, piece2.Y, fillTime);
                     piece2.MovablePiece.Move(piece1X, piece1Y, fillTime);
                }
                yield break;
        }
        public void PressPiece(MainPiece piece)
        {
            Debug.Log(piece.X+", "+piece.Y);
        //    ConductSearch(enteredPiece);
            pressedPiece = piece;
        }
        public void EnterPiece(MainPiece piece)
        {
            enteredPiece = piece;
            
        }
        public void ReleasePiece()
        {
            if(IsAdjacent(pressedPiece,enteredPiece))
            {
                SwapPieces(pressedPiece, enteredPiece);
            }
        }
      
        public bool ClearAllValidMatches()
        {
            bool needsRefill = false;
        
                List<MainPiece> match = neighborsList;
                if (match != null)
                {
                    for (int j = 0; j < match.Count; j++)
                    {
                       if(ClearPiece(match,match[j].X, match[j].Y))
                        {
                            needsRefill = true;
                        }  
                    }
            }
           
            return needsRefill;
        }
        public bool ClearPiece(List<MainPiece> match, int x, int y )
        {
            if(pieces[x, y].IsClearable() && !pieces[x, y].ClearablePiece.IsBeingCleared)
            {
                pieces[x, y].ClearablePiece.Clear();
                match.Remove(pieces[x, y]);
                SpawnPiece(x, y, PieceType.Empty);
                return true;
            }
            return false;

        }
        public List<MainPiece> ConductSearch(MainPiece piece)
        {
            neighborsList.Clear();
            if (GetConnectedNeighbors(piece.X, piece.Y, piece.ColorPiece.Color)!=null)
            {
                for (int i = 0; i < neighborsList.Count; i++)
                {
                    GetConnectedNeighbors(neighborsList[i].X, neighborsList[i].Y, neighborsList[i].ColorPiece.Color);
                }
            }
            if (neighborsList.Count > 2)
                return neighborsList;
            
            return null;
        }
        
        public List<MainPiece> GetConnectedNeighbors(int x, int y, ColorType color)
        {
            AddItem(neighborsList,pieces[x,y],color);
            if( x == 0 && y == 0)
            {
                AddItem(neighborsList, (pieces[x+1,y]),color);
                AddItem(neighborsList, (pieces[x, y+1]), color);
            }
            else if(x > 0 && y == 0)
            {
                if(x < xDimension-1)
                {
                    AddItem(neighborsList, (pieces[x+1,y]), color);
                }
                AddItem(neighborsList, (pieces[x-1,y]), color);
                AddItem(neighborsList, (pieces[x, y+1]), color);
            }
            else if(x == 0 && y>0)
            {
                AddItem(neighborsList, (pieces[x, y - 1]), color);
                AddItem(neighborsList, (pieces[x + 1, y]), color);
                if(y < yDimension - 1)
                {
                    AddItem(neighborsList, (pieces[x, y + 1]), color);
                }
            }
            else if(x > 0 && y > 0)
            {
                AddItem(neighborsList, (pieces[x, y - 1]), color);
                if (x < xDimension - 1)
                {
                    AddItem(neighborsList, (pieces[x + 1, y]), color);
                }
                AddItem(neighborsList, (pieces[x - 1, y]), color);
                if (y < yDimension - 1)
                {
                    AddItem(neighborsList, (pieces[x, y + 1]), color);
                }
            }
              
            return neighborsList;
           
        }
        public void AddItem(List<MainPiece> list, MainPiece item,ColorType color)
        {
            if (item.IsColored())
            {
                if (!list.Contains(item) && item.ColorPiece.Color == color)
                {
                    list.Add(item);
                }
            }
        }
    }
  
    
}
