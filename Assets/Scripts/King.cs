using System.Collections;
using UnityEngine;

public class King : Chessman
{
    public override bool[,] PossibleMoves()
    {
        bool[,] r = new bool[8, 8];

        Move(CurrentX + 1, CurrentY, ref r); // up
        Move(CurrentX - 1, CurrentY, ref r); // down
        Move(CurrentX, CurrentY - 1, ref r); // left
        Move(CurrentX, CurrentY + 1, ref r); // right
        Move(CurrentX + 1, CurrentY - 1, ref r); // up left
        Move(CurrentX - 1, CurrentY - 1, ref r); // down left
        Move(CurrentX + 1, CurrentY + 1, ref r); // up right
        Move(CurrentX - 1, CurrentY + 1, ref r); // down right

        //Castling
        if (!hasPreviouslyMoved)
        {
 
 
            //to the positive side of the board
            if(CurrentX < 5)
            {
                Chessman c = BoardManager.Instance.Chessmans[CurrentX + 1, CurrentY];
                Chessman c2 = BoardManager.Instance.Chessmans[CurrentX + 2, CurrentY];
                if (c == null && c2 == null)
                {
                    Chessman potentialRook = null;
                    int potentialRookX = CurrentX + 3;
                    bool foundRook = false;
                    bool stillValidForCastling = true;//empty space or rook that has not moved
                    while (!foundRook && potentialRookX < 8 && stillValidForCastling)
                    {
                        potentialRook = BoardManager.Instance.Chessmans[potentialRookX, CurrentY];
                        if(potentialRook == null)
                        {
                            potentialRookX++;
                        }
                        else if (potentialRook.GetType() == typeof(Rook))
                        {
                            if (potentialRook.hasPreviouslyMoved)
                            {
                                stillValidForCastling = false;
                            }
                            else
                            {
                                //success
                                foundRook = true;
                            }
                        }
                        else
                        {
                            stillValidForCastling = false;
                        }
                    }
                    if (foundRook)
                    {
                        Move(CurrentX + 2, CurrentY, ref r);
                    }
                }
            }
            //towards the negative side of the board
            if (CurrentX > 2)
            {
                Chessman c = BoardManager.Instance.Chessmans[CurrentX - 1, CurrentY];
                Chessman c2 = BoardManager.Instance.Chessmans[CurrentX - 2, CurrentY];
                if (c == null && c2 == null)
                {
                    Chessman potentialRook = null;
                    int potentialRookX = CurrentX - 3;
                    bool foundRook = false;
                    bool stillValidForCastling = true;//empty space or rook that has not moved
                    while (!foundRook && potentialRookX > -1 && stillValidForCastling)
                    {
                        potentialRook = BoardManager.Instance.Chessmans[potentialRookX, CurrentY];
                        if (potentialRook == null)
                        {
                            potentialRookX--;
                        }
                        else if (potentialRook.GetType() == typeof(Rook))
                        {
                            if (potentialRook.hasPreviouslyMoved)
                            {
                                stillValidForCastling = false;
                            }
                            else
                            {
                                //success
                                foundRook = true;
                            }
                        }
                        else
                        {
                            stillValidForCastling = false;
                        }
                    }
                    if (foundRook)
                    {
                        Move(CurrentX - 2, CurrentY, ref r);
                    }



                }
            }




           
        }




        return r;
    }



}
