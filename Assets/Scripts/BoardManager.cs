using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;







public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance { get; set; }
    private bool[,] allowedMoves { get; set; }

    private const float TILE_SIZE = 1.0f;
    private const float TILE_OFFSET = 0.5f;

    private int selectionX = -1;
    private int selectionY = -1;

    public List<GameObject> chessmanPrefabs;
    private List<GameObject> activeChessman;

    private Quaternion whiteOrientation = Quaternion.Euler(0, 270, 0);
    private Quaternion blackOrientation = Quaternion.Euler(0, 90, 0);

    public Chessman[,] Chessmans { get; set; }
    private Chessman selectedChessman;

    public bool isWhiteTurn = true;

    public SettingsManagerScript gameSettingsManager;

    private Material previousMat;
    public Material selectedMat;


    public int[] EnPassantMove { set; get; }
	public int[] EnPassantEndingCoordinates {set; get;}

    // Use this for initialization
    void Start()
    {
        Instance = this;
        //SpawnAllChessmans();
        SpawnAllChessmansFromPresets();
        EnPassantMove = new int[2] { -1, -1 };
		EnPassantEndingCoordinates = new int[2] { -1, -1 };

    }

    // Update is called once per frame
    void Update()
    {
        UpdateSelection();

        if (Input.GetMouseButtonDown(0))
        {
            if (selectionX >= 0 && selectionY >= 0)
            {
                if (selectedChessman == null)
                {
                    // Select the chessman
                    SelectChessman(selectionX, selectionY);
                }
                else
                {
                    // Move the chessman
                    MoveChessman(selectionX, selectionY);
                }
            }
        }

	


        if (Input.GetKey("escape"))
            Application.Quit();
    }





    private void SelectChessman(int x, int y)
    {
        if (Chessmans[x, y] == null) return;

        if (Chessmans[x, y].isWhite != isWhiteTurn) return;

        bool hasAtLeastOneMove = false;

        allowedMoves = Chessmans[x, y].PossibleMoves();
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (allowedMoves[i, j])
                {
                    hasAtLeastOneMove = true;
                    i = 8;
                    break;
                }
            }
        }

        if (!hasAtLeastOneMove)
            return;

        selectedChessman = Chessmans[x, y];
        previousMat = selectedChessman.GetComponent<MeshRenderer>().material;
        selectedMat.mainTexture = previousMat.mainTexture;
        selectedChessman.GetComponent<MeshRenderer>().material = selectedMat;

        BoardHighlights.Instance.HighLightAllowedMoves(allowedMoves);
    }

    private void MoveChessman(int x, int y)
    {
        if (allowedMoves[x, y])
        {
            Chessman c = Chessmans[x, y];

            if (c != null && c.isWhite != isWhiteTurn)
            {
                // Capture a piece

                if (c.GetType() == typeof(King))
                {
                    // End the game
                    EndGame();
                    return;
                }

                activeChessman.Remove(c.gameObject);
                Destroy(c.gameObject);
            }

            //enpassant capture
            if (selectedChessman.GetType() == typeof(Pawn) )
            {
                if (x == EnPassantMove[0] && y == EnPassantMove[1] && selectedChessman.CurrentX != x)
                {

                    c = Chessmans[EnPassantEndingCoordinates[0], EnPassantEndingCoordinates[1]];

                    activeChessman.Remove(c.gameObject);
                    Destroy(c.gameObject);
                }

            }
            //enpassant capture
            if (selectedChessman.GetType() == typeof(AntiPawn))
            {
                if (x == EnPassantMove[0] && y == EnPassantMove[1] && selectedChessman.CurrentX == x)
                {

                    c = Chessmans[EnPassantEndingCoordinates[0], EnPassantEndingCoordinates[1]];

                    activeChessman.Remove(c.gameObject);
                    Destroy(c.gameObject);
                }


            }
            
            EnPassantMove[0] = -1;
            EnPassantMove[1] = -1;
			EnPassantEndingCoordinates [0] = -1;
			EnPassantEndingCoordinates [1] = -1;
            if (selectedChessman.GetType() == typeof(Pawn))
            {
                if(y == 7) // White Promotion
                {
                    activeChessman.Remove(selectedChessman.gameObject);
                    Destroy(selectedChessman.gameObject);
                    SpawnChessman(1, x, y, true);
                    selectedChessman = Chessmans[x, y];
                }
                else if (y == 0) // Black Promotion
                {
                    activeChessman.Remove(selectedChessman.gameObject);
                    Destroy(selectedChessman.gameObject);
                    SpawnChessman(7, x, y, false);
                    selectedChessman = Chessmans[x, y];
                }
              
				if (y - selectedChessman.CurrentY == 2) {
					EnPassantMove [1] = y - 1;
					EnPassantMove[0] = x;
					EnPassantEndingCoordinates[0] = x;
					EnPassantEndingCoordinates[1] = y;
				} else if (selectedChessman.CurrentY - y == 2) {
					EnPassantMove [1] = y + 1;
					EnPassantMove[0] = x;
					EnPassantEndingCoordinates[0] = x;
					EnPassantEndingCoordinates[1] = y;
				}

					
            }
			if (selectedChessman.GetType() == typeof(AntiPawn))
			{
				if(y == 7) // White Promotion
				{
					activeChessman.Remove(selectedChessman.gameObject);
					Destroy(selectedChessman.gameObject);
					SpawnChessman(1, x, y, true); //queen
					selectedChessman = Chessmans[x, y];
				}
				else if (y == 0) // Black Promotion
				{
					activeChessman.Remove(selectedChessman.gameObject);
					Destroy(selectedChessman.gameObject);
					SpawnChessman(7, x, y, false); //queen
					selectedChessman = Chessmans[x, y];
				}
			
				if (y - selectedChessman.CurrentY == 2) {
					EnPassantMove [1] = y - 1;
					EnPassantMove[0] = (x + selectedChessman.CurrentX) / 2;
					EnPassantEndingCoordinates[0] = x;
					EnPassantEndingCoordinates[1] = y;
				} else if (selectedChessman.CurrentY - y == 2) {
					EnPassantMove [1] = y + 1;
					EnPassantMove[0] = (x + selectedChessman.CurrentX) / 2;
					EnPassantEndingCoordinates[0] = x;
					EnPassantEndingCoordinates[1] = y;
				}


			}
            if (selectedChessman.GetType() == typeof(King))
            {
                //Castling
              if(selectedChessman.CurrentX - x > 1)
                {
                    Chessman associatedRook = null;
                    bool foundRook = false;
                    int rookX = x -1;  
                    while(!foundRook && rookX > -1)
                    {
                        associatedRook = Chessmans[rookX, y];
                        if(associatedRook != null)
                        {
                            foundRook = true;
                        }
                        else
                        {
                            rookX--;
                        }
                    }
                    if (foundRook)
                    {
                        Destroy(associatedRook.gameObject);
                        SpawnChessman(chessmanIndexForCharacterString('R', selectedChessman.isWhite), (selectedChessman.CurrentX + x) / 2, y, selectedChessman.isWhite);
                        Chessman spawnedRook = Chessmans[(selectedChessman.CurrentX + x) / 2, y];
                        spawnedRook.hasPreviouslyMoved = true;
                    }
 

                }
                else if(x - selectedChessman.CurrentX > 1)
                {

                    Chessman associatedRook = null;
                    bool foundRook = false;
                    int rookX = x + 1;
                    while (!foundRook && rookX < 8)
                    {
                        associatedRook = Chessmans[rookX, y];
                        if (associatedRook != null)
                        {
                            foundRook = true;
                        }
                        else
                        {
                            rookX++;
                        }
                    }
                    if (foundRook)
                    {
                        Destroy(associatedRook.gameObject);
                        SpawnChessman(chessmanIndexForCharacterString('R', selectedChessman.isWhite), (selectedChessman.CurrentX + x) / 2, y, selectedChessman.isWhite);
                        Chessman spawnedRook = Chessmans[(selectedChessman.CurrentX + x) / 2, y];
                        spawnedRook.hasPreviouslyMoved = true;
                    }

                }


            }





            Chessmans[selectedChessman.CurrentX, selectedChessman.CurrentY] = null;
            selectedChessman.transform.position = GetTileCenter(x, y);
            selectedChessman.SetPosition(x, y);
            Chessmans[x, y] = selectedChessman;
			selectedChessman.hasPreviouslyMoved = true;
			isWhiteTurn = !isWhiteTurn;

        }

        selectedChessman.GetComponent<MeshRenderer>().material = previousMat;

        BoardHighlights.Instance.HideHighlights();
        selectedChessman = null;
    }

    private void UpdateSelection()
    {
        if (!Camera.main) return;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 50.0f, LayerMask.GetMask("ChessPlane")))
        {
            selectionX = (int)hit.point.x;
            selectionY = (int)hit.point.z;
        }
        else
        {
            selectionX = -1;
            selectionY = -1;
        }
    }

    private void SpawnChessman(int index, int x, int y, bool isWhite)
    {
        Debug.Log("SpawnChessman at index:" + index + "     x:" + x + "     y:" + y);
        if (index < 0 || x < 0 || y < 0 || x > 7 || y > 7 )
        {                      
            return;  //incorrect value
        }

        Vector3 position = GetTileCenter(x, y);
        GameObject go;

        if (isWhite)
        {
            go = Instantiate(chessmanPrefabs[index], position, whiteOrientation) as GameObject;
        }
        else
        {
            go = Instantiate(chessmanPrefabs[index], position, blackOrientation) as GameObject;
        }

        go.transform.SetParent(transform);
        Chessmans[x, y] = go.GetComponent<Chessman>();
        Chessmans[x, y].SetPosition(x, y);
        activeChessman.Add(go);
    }

    private Vector3 GetTileCenter(int x, int y)
    {
        Vector3 origin = Vector3.zero;
        origin.x += (TILE_SIZE * x) + TILE_OFFSET;
        origin.z += (TILE_SIZE * y) + TILE_OFFSET;

        return origin;
    }

    private void SpawnAllChessmans()  //old
    {
        activeChessman = new List<GameObject>();
        Chessmans = new Chessman[8, 8];
        /////// White ///////
        // King
        SpawnChessman(0, 3, 0, true);
        // Queen
        SpawnChessman(1, 4, 0, true);
        // Rooks
        SpawnChessman(2, 0, 0, true);
        SpawnChessman(2, 7, 0, true);
        // Bishops
        SpawnChessman(3, 2, 0, true);
        SpawnChessman(14, 5, 0, true);
        // Knights
        SpawnChessman(4, 1, 0, true);
		//SpawnChessman(4, 6, 0, true);
        SpawnChessman(12, 6, 0, true);//unicorn
        // Pawns
        for (int i = 0; i < 4; i++)
        {
            SpawnChessman(5, i, 1, true);
        }
		for (int i = 4; i < 8; i++)
		{
			SpawnChessman(16, i, 1, true);
		}
		SpawnChessman(5, 1, 2, true);
		SpawnChessman(16, 3, 2, true);
        /////// Black ///////
        // King
        SpawnChessman(6, 4, 7, false);
        // Queen
        SpawnChessman(7, 3, 7, false);
        // Rooks
        SpawnChessman(8, 0, 7, false);
        SpawnChessman(8, 7, 7, false);
        // Bishops
        SpawnChessman(9, 2, 7, false);
        SpawnChessman(15, 5, 7, false);
        // Knights
        SpawnChessman(10, 1, 7, false);
		//SpawnChessman(10, 6, 7, false);
        SpawnChessman(13, 6, 7, false);//unicorn
        // Pawns
        for (int i = 0; i < 4; i++)
        {
            SpawnChessman(11, i, 6, false);
        }
		for (int i = 4; i < 8; i++)
		{
			SpawnChessman(17, i, 6, false);
		}
		SpawnChessman(11, 1, 5, false);
		SpawnChessman(17, 3, 5, false);
    }

    private void SpawnAllChessmansFromPresets()
    {
        activeChessman = new List<GameObject>();
        Chessmans = new Chessman[8, 8];

        string whiteArmyString = gameSettingsManager.getWhiteArmyPreset().Replace(System.Environment.NewLine, ""); //remove newlines
        whiteArmyString = whiteArmyString.Replace("\n", "");
        whiteArmyString = whiteArmyString.Replace("\r", "");
        string blackArmyString = gameSettingsManager.getBlackArmyPreset().Replace(System.Environment.NewLine, ""); //remove newlines
        blackArmyString = blackArmyString.Replace("\n", "");
        blackArmyString = blackArmyString.Replace("\r", "");

        //main loop for spawning pieces
        for (int i = 0; i < whiteArmyString.Length; i++)
        {
            SpawnChessman(chessmanIndexForCharacterString(whiteArmyString[i],true), i % 8, 2 - (i/8), true);
        }

        for (int i = 0; i < blackArmyString.Length; i++)
        {
            SpawnChessman(chessmanIndexForCharacterString(blackArmyString[i], false), 7 - (i % 8), 5 + (i / 8), false);
        }


        //Bad user input handling
        //backup king near the middle of the board just in case
        if (whiteArmyString.ToUpper().IndexOf("K") < 0)
        {
            SpawnChessman(chessmanIndexForCharacterString('K', true), 2, 3, true);
        }
        if (blackArmyString.ToUpper().IndexOf("K") < 0)
        {
            SpawnChessman(chessmanIndexForCharacterString('K', false), 5, 4, true);
        }







    }

    private int chessmanIndexForCharacterString(char character, bool whitePieceColor)
    {
        string characterString = Char.ToString(character).ToUpper();//ensure upper case
        if (whitePieceColor)
        {
            if (characterString.Equals("K"))
            {
                return 0;
            }
            if (characterString.Equals("Q"))
            {
                return 1;
            }
            if (characterString.Equals("R"))
            {
                return 2;
            }
            if (characterString.Equals("B"))
            {
                return 3;
            }
            if (characterString.Equals("N"))
            {
                return 4;
            }
            if (characterString.Equals("P"))
            {
                return 5;
            }
            if (characterString.Equals("A"))//AntiPawn
            {
                return 16;
            }
            if (characterString.Equals("U"))//Unicorn
            {
                return 12;
            }
            if (characterString.Equals("C"))//Cardinal
            {
                return 14;
            }

        }
        else
        {
            if (characterString.Equals("K"))
            {
                return 6;
            }
            if (characterString.Equals("Q"))
            {
                return 7;
            }
            if (characterString.Equals("R"))
            {
                return 8;
            }
            if (characterString.Equals("B"))
            {
                return 9;
            }
            if (characterString.Equals("N"))
            {
                return 10;
            }
            if (characterString.Equals("P"))
            {
                return 11;
            }
            if (characterString.Equals("A"))//AntiPawn
            {
                return 17;
            }
            if (characterString.Equals("U"))//Unicorn
            {
                return 13;
            }
            if (characterString.Equals("C"))//Cardinal
            {
                return 15;
            }


        }
       

        //not found, then don't spawn a piece
        return -1;
       

    }



    private void EndGame()
    {
        if (isWhiteTurn)
            Debug.Log("White wins");
        else
            Debug.Log("Black wins");

        foreach (GameObject go in activeChessman)
        {
            Destroy(go);
        }

        isWhiteTurn = true;
        BoardHighlights.Instance.HideHighlights();
        //SpawnAllChessmans();
        SpawnAllChessmansFromPresets();
    }

	public bool getWhiteTurn(){
		return isWhiteTurn;
	}
}


