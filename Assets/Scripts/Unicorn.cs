using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unicorn : Chessman
{
	public override bool[,] PossibleMoves()
	{
		bool[,] r = new bool[8, 8];

		//knight subset
		// Up left
		Move(CurrentX - 1, CurrentY + 2, ref r);

		// Up right
		Move(CurrentX + 1, CurrentY + 2, ref r);

		// Down left
		Move(CurrentX - 1, CurrentY - 2, ref r);

		// Down right
		Move(CurrentX + 1, CurrentY - 2, ref r);


		// Left Down
		Move(CurrentX - 2, CurrentY - 1, ref r);

		// Right Down
		Move(CurrentX + 2, CurrentY - 1, ref r);

		// Left Up
		Move(CurrentX - 2, CurrentY + 1, ref r);

		// Right Up
		Move(CurrentX + 2, CurrentY + 1, ref r);



		//Bishop subset

		// Right
		int i, j;

		// Top left
		i = CurrentX;
		j = CurrentY;
		while (true)
		{
			i--;
			j++;
			if (i < 0 || j >= 8) break;

			if (Move(i, j, ref r)) break;
		}

		// Top right
		i = CurrentX;
		j = CurrentY;
		while (true)
		{
			i++;
			j++;
			if (i >= 8 || j >= 8) break;

			if (Move(i, j, ref r)) break;
		}

		// Down left
		i = CurrentX;
		j = CurrentY;
		while (true)
		{
			i--;
			j--;
			if (i < 0 || j < 0) break;

			if (Move(i, j, ref r)) break;
		}

		// Down right
		i = CurrentX;
		j = CurrentY;
		while (true)
		{
			i++;
			j--;
			if (i >= 8 || j < 0) break;

			if (Move(i, j, ref r)) break;
		}

		return r;
	}

}
