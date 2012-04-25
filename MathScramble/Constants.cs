// Constants.cs
// 
// Copyright (c) 2012 Montclair State University
// 
// Contributors:  Christopher Desch <cdesch@gmail.com>
//				  Dr. Jerry Fails 	<failsj@mail.montclair.edu>
// 
// Permission is hereby granted, free of charge, to any person obtaining 
// a copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense, 
// and/or sell copies of the Software, and to permit persons to whom the 
// Software is furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in 
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF 
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR 
// ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION 
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sifteo.Util;
using Sifteo;

namespace MathScramble
{
	public static class Constants
	{
		
		//
		public static readonly int kMinCubes = 6;	// Min cubes required to play
		
		//Ints
		public static readonly int kOperandMin = 0;
		public static readonly int kOperandMax = 9;
		public static readonly int kOperatorMin = 1;
		public static readonly int kOperatorMax = 1; //4;
		
		//strings
		public static readonly string FLASH_STARTING_MESSAGE = "Preparing flash file for upload...";
		
		//Colors
		public static readonly Color transparentColor = new Color (72, 255, 170);
		public static readonly Color LightBlue = new  Color (36, 182, 255);
		public static readonly Color RedColor = new  Color (255, 0, 0);
		public static readonly Color BlueColor = new  Color (0, 0, 255);
		public static readonly Color GreenColor = new  Color (0, 255, 0);
		
		//Game States
		public static readonly string LAUNCH = "STARTUP";
		public static readonly string TitleState = "Title";
		public static readonly string MenuState = "Menu";
		public static readonly string GameState = "Game";
		public static readonly string GameBegin = "GameBegin";
		public static readonly string GameEndState = "GameEnd";
		

		//Transition // Prefix with 't'
		public static readonly string tTitleToMenu = "TitleToMenu";
		public static readonly string tMenuToGameBegin = "MenuToGameBegin";
		public static readonly string tGameBeginToMenu = "GameBeginToMenu";
		public static readonly string tGameBeginToGame = "GameBeginToGame";
		public static readonly string tGameToGameEnd = "GameToGameEnd";
		public static readonly string tGameEndToMenu = "GameEndToMenu";
		
		//Cube States
		public static readonly string HintState = "HintState";
		public static readonly string OperandState = "OperandState";
		public static readonly string OperatorState = "OperatorState";
		public static readonly string ResultState = "ResultState";
		
		//Cube transitions
		public static readonly string tTitleToHintState = "tTitleToHintState";
		public static readonly string tTitleToOperandState = "tTitleToOperandState";
		public static readonly string tTitleToOperatorState = "tTitleToOperatorState";
		public static readonly string tTitleToResultState = "tTitleToResultState";
		public static readonly string tOperandToOperatorState = "tOperandToOperatorState";
		public static readonly string tOperatorToOperandState = "tOperatorToOperandState";
		
		
		//Operator Constants
		public static readonly string opQuestion = "?";
		public static readonly string opAdd = "+";
		public static readonly string opSub = "-";
		public static readonly string opMul = "x";
		public static readonly string opDiv = "/";
		public static readonly string opEquals = "=";
		
		//Hint Cube Sided Domino Coordinates
		public static readonly int kSidedWallPadding = 5;
		public static readonly int kSidedCubeSide = 10;
		
		
	}
}

