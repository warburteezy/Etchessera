using System.Collections;
using UnityEngine;

public class AntiPawn : Chessman
{

	public override bool[,] PossibleMoves()
	{
		bool[,] r = new bool[8, 8];

		Chessman c, c2;

		int[] e = BoardManager.Instance.EnPassantMove;

		if (isWhite)
		{
			////// White team move //////

			// Diagonal left
			if (CurrentX != 0 && CurrentY != 7)
			{
				c = BoardManager.Instance.Chessmans[CurrentX - 1, CurrentY + 1];
				if (c == null)
					r[CurrentX - 1, CurrentY + 1] = true;
			}

			// Diagonal right
			if (CurrentX != 7 && CurrentY != 7)
			{

				c = BoardManager.Instance.Chessmans[CurrentX + 1, CurrentY + 1];
				if (c == null)
					r[CurrentX + 1, CurrentY + 1] = true;
			}

			// Middle
			if (CurrentY != 7)
			{
				if (e[0] == CurrentX && e[1] == CurrentY + 1)
					r[CurrentX, CurrentY + 1] = true;

				c = BoardManager.Instance.Chessmans[CurrentX, CurrentY + 1];
				if (c != null && !c.isWhite)
					r[CurrentX, CurrentY + 1] = true;
			}

			// Double move on first move
			//diagonal left 2
			if (CurrentX > 1 && !hasPreviouslyMoved)
			{
				c = BoardManager.Instance.Chessmans[CurrentX - 1, CurrentY + 1];
				c2 = BoardManager.Instance.Chessmans[CurrentX - 2, CurrentY + 2];
				if (c == null && c2 == null)
					r[CurrentX - 2, CurrentY + 2] = true;
			}
			//diagonal right 2
			if (CurrentX < 6 && !hasPreviouslyMoved)
			{
				c = BoardManager.Instance.Chessmans[CurrentX + 1, CurrentY + 1];
				c2 = BoardManager.Instance.Chessmans[CurrentX + 2, CurrentY + 2];
				if (c == null && c2 == null)
					r[CurrentX + 2, CurrentY + 2] = true;
			}

		}
		else
		{
			////// Black team move //////

			// Diagonal left
			if (CurrentX != 0 && CurrentY != 0)
			{
				
				c = BoardManager.Instance.Chessmans[CurrentX - 1, CurrentY - 1];
				if (c == null)
					r[CurrentX - 1, CurrentY - 1] = true;
			}

			// Diagonal right
			if (CurrentX != 7 && CurrentY != 0)
			{
				
				c = BoardManager.Instance.Chessmans[CurrentX + 1, CurrentY - 1];
				if (c == null )
					r[CurrentX + 1, CurrentY - 1] = true;
			}

			// Middle
			if (CurrentY != 0)
			{
				if (e[0] == CurrentX && e[1] == CurrentY - 1)
					r[CurrentX, CurrentY - 1] = true;

				c = BoardManager.Instance.Chessmans[CurrentX, CurrentY - 1];
				if  (c != null && c.isWhite)
					r[CurrentX, CurrentY - 1] = true;
			}
				

			// Double move on first move
			//diagonal right 2
			if (CurrentX > 1 && !hasPreviouslyMoved)
			{
				c = BoardManager.Instance.Chessmans[CurrentX - 1, CurrentY - 1];
				c2 = BoardManager.Instance.Chessmans[CurrentX - 2, CurrentY - 2];
				if (c == null && c2 == null)
					r[CurrentX - 2, CurrentY - 2] = true;
			}
			//diagonal left 2
			if (CurrentX < 6 && !hasPreviouslyMoved)
			{
				c = BoardManager.Instance.Chessmans[CurrentX + 1, CurrentY - 1];
				c2 = BoardManager.Instance.Chessmans[CurrentX + 2, CurrentY - 2];
				if (c == null && c2 == null)
					r[CurrentX + 2, CurrentY - 2] = true;
			}




		}

		return r;
	}
}
