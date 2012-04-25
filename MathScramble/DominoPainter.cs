// DominoPainter.cs
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
using Sifteo;

namespace MathScramble
{
	public class DominoPainter
	{
		
		private Cube mCube;
		private Color mRectColor = Constants.BlueColor;
		private int x, y;	//Center of Drawing for sided hints //Starting point
		private int xVal, yVal;
		private int xOffset, yOffset;
		private int h = Constants.kSidedCubeSide;
		private int w = Constants.kSidedCubeSide;
		private int separatorWidth = 2;
		
		/*
		 * Consturctor
		 */
		public DominoPainter (Cube cube, int number)
		{
			Log.Debug ("DominoPainter {0}", number);
			mCube = cube;
			
			switch (number) {
			case 1:
				one ();
				break;
			case 2:
				two ();
				break;
			case 3:
				three ();
				break;
			case 4:
				four ();
				break;
			case 5:
				five ();
				break;
			case 6:
				six ();
				break;
			case 7:
				seven ();
				break;
			case 8:
				eight ();
				break;
			case 9:	
				nine ();
				break;
			default:
				
				if (number == 0) {
					//Do Nothing
				} else if (number > 9) {
					tilePaint2 (number);
				}
				
				break;
				
			}				
		}
		
		/*
		 * Constructor with Side
		 */ 
		public DominoPainter (Cube cube, int number, Cube.Side side)
		{
			Log.Debug ("DominoPainter {0}", number);
			mCube = cube;
			
			Cube.Side mSide = side;
			

			
			//Left Side
			if (mSide == Cube.Side.LEFT) {
				x = Constants.kSidedWallPadding;
				y = Cube.SCREEN_HEIGHT / 2;
				
				//Negative
				xOffset = (((Constants.kSidedCubeSide * 1) + (Constants.kSidedWallPadding * 1)) * -1);
				yOffset = 0;
				
			} else if (mSide == Cube.Side.RIGHT) {
				//Right side
				x = Cube.SCREEN_WIDTH - Constants.kSidedWallPadding - Constants.kSidedCubeSide;
				y = Cube.SCREEN_HEIGHT / 2;
				
				//Positive
				xOffset = (Constants.kSidedCubeSide + (Constants.kSidedWallPadding * 1));
				yOffset = 0;
				
			} else if (mSide == Cube.Side.TOP) {
				x = Constants.kSidedWallPadding;
				y = Cube.SCREEN_HEIGHT / 2;
				
				//Negative
				xOffset = (((Constants.kSidedCubeSide * 1) + (Constants.kSidedWallPadding * 1)) * -1);
				yOffset = 0;
				
				//mSide = Cube.Side.Left ();
				
				//Top Side
				//x = Cube.SCREEN_WIDTH / 2;
				//y = Constants.kSidedWallPadding;
				
			} else if (mSide == Cube.Side.BOTTOM) {
				//Force it
				//Right side
				x = Cube.SCREEN_WIDTH - Constants.kSidedWallPadding - Constants.kSidedCubeSide;
				y = Cube.SCREEN_HEIGHT / 2;
				
				//Positive
				xOffset = (Constants.kSidedCubeSide + (Constants.kSidedWallPadding * 1));
				yOffset = 0;
				
				//mSide = Cube.Side.RIGHT;
				
				//Bottom Side
				//x = Cube.SCREEN_WIDTH / 2;
				//y = Cube.SCREEN_HEIGHT - Constants.kSidedWallPadding - Constants.kSidedCubeSide;
			}
			
			x = 59;
			y = 64;
			
			
			switch (number) {
			case 1:
				one (mSide);
				break;
			case 2:
				two (mSide);
				break;
			case 3:
				three (mSide);
				break;
			case 4:
				four (mSide);
				break;
			case 5:
				five (mSide);
				break;
			case 6:
				six (mSide);
				break;
			case 7:
				seven (mSide);
				break;
			case 8:
				eight (mSide);
				break;
			case 9:
				nine (mSide);
				break;
			default:
				Log.Debug ("Error");
				break;
				
			}				
		}
		
		/*
		 * Desconstructor
		 */
		~DominoPainter ()
		{
			//Cleanup
		}
		
		private void one ()
		{
			
			mCube.FillRect (mRectColor, 54, 54, 20, 20);
			//mCube.Paint ();
		
		}
		
		private void two ()
		{
			
			mCube.FillRect (mRectColor, 10, 10, 20, 20);
			mCube.FillRect (mRectColor, 98, 98, 20, 20);
			//mCube.Paint ();
		}
		
		private void three ()
		{
			
			mCube.FillRect (mRectColor, 10, 10, 20, 20);
			mCube.FillRect (mRectColor, 54, 54, 20, 20);
			mCube.FillRect (mRectColor, 98, 98, 20, 20);
			//mCube.Paint ();
		}
		
		private void four ()
		{
			
			mCube.FillRect (mRectColor, 10, 10, 20, 20);
			mCube.FillRect (mRectColor, 10, 98, 20, 20);
			mCube.FillRect (mRectColor, 98, 98, 20, 20);
			mCube.FillRect (mRectColor, 98, 10, 20, 20);
		}
		
		private void five ()
		{
			
			mCube.FillRect (mRectColor, 10, 10, 20, 20);
			mCube.FillRect (mRectColor, 10, 98, 20, 20);
			mCube.FillRect (mRectColor, 54, 54, 20, 20);
			mCube.FillRect (mRectColor, 98, 98, 20, 20);
			mCube.FillRect (mRectColor, 98, 10, 20, 20);
		}
		
		private void six ()
		{
			
			mCube.FillRect (mRectColor, 10, 10, 20, 20);
			mCube.FillRect (mRectColor, 10, 98, 20, 20);
			mCube.FillRect (mRectColor, 10, 54, 20, 20);
			mCube.FillRect (mRectColor, 98, 98, 20, 20);
			mCube.FillRect (mRectColor, 98, 10, 20, 20);
			mCube.FillRect (mRectColor, 98, 54, 20, 20);
		}
		
		private void seven ()
		{
			
			mCube.FillRect (mRectColor, 10, 10, 20, 20);
			mCube.FillRect (mRectColor, 10, 98, 20, 20);
			mCube.FillRect (mRectColor, 10, 54, 20, 20);
			mCube.FillRect (mRectColor, 98, 98, 20, 20);
			mCube.FillRect (mRectColor, 98, 10, 20, 20);
			mCube.FillRect (mRectColor, 98, 54, 20, 20);
			mCube.FillRect (mRectColor, 54, 10, 20, 20);
		}
		
		private void eight ()
		{
			
			mCube.FillRect (mRectColor, 10, 10, 20, 20);
			mCube.FillRect (mRectColor, 10, 98, 20, 20);
			mCube.FillRect (mRectColor, 10, 54, 20, 20);
			mCube.FillRect (mRectColor, 98, 98, 20, 20);
			mCube.FillRect (mRectColor, 98, 10, 20, 20);
			mCube.FillRect (mRectColor, 98, 54, 20, 20);
			mCube.FillRect (mRectColor, 54, 10, 20, 20);
			mCube.FillRect (mRectColor, 54, 98, 20, 20);

		}
		
		private void nine ()
		{
			
			mCube.FillRect (mRectColor, 10, 10, 20, 20);
			mCube.FillRect (mRectColor, 10, 98, 20, 20);
			mCube.FillRect (mRectColor, 10, 54, 20, 20);
			mCube.FillRect (mRectColor, 98, 98, 20, 20);
			mCube.FillRect (mRectColor, 98, 10, 20, 20);
			mCube.FillRect (mRectColor, 98, 54, 20, 20);
			mCube.FillRect (mRectColor, 54, 10, 20, 20);
			mCube.FillRect (mRectColor, 54, 98, 20, 20);
			mCube.FillRect (mRectColor, 54, 54, 20, 20);
		}
		
		private void tilePaint (int num)
		{
			Log.Debug ("TilePaint");
			int numPainted = 0;
			
			int xCoord = 5;
			int yCoord = 5;
			for (int i  = 0; i < 6; i++) {
				
				for (int j = 0; j < 6; j++) {
					
					mCube.FillRect (mRectColor, xCoord, yCoord, h + 5, w + 5);	
					xCoord += 20;
					numPainted ++;
					
					if (numPainted == num) {
						return;
					}
						
				}
				xCoord = 5;
				yCoord += 20;
				
			}
				
		}
		
		private void tilePaint2 (int num)
		{
			//Check highest number
			if (num > 36) {
				return;
			}
			
			Log.Debug ("TilePaint2");
			
			int xCoord = 5;
			int yCoord = 5;
			
			//If divisible by 4 evenly
			if (num % 3 == 0) {
				Log.Debug ("Div 3");
				int div = num / 3;
				DrawVerticleSeparator ();
				DrawHorizontalSeparator ();
				paintDie (div, xCoord, yCoord);
				paintDie (div, xCoord + 60, yCoord);
				paintDie (div, xCoord, yCoord + 60);
				
			} else if (num % 4 == 0) {
				Log.Debug ("Div 4");
				int div = num / 4;
				DrawVerticleSeparator ();
				DrawHorizontalSeparator ();
				paintDie (div, xCoord, yCoord);
				paintDie (div, xCoord + 60, yCoord);
				paintDie (div, xCoord, yCoord + 60);
				paintDie (div, xCoord + 60, yCoord + 60);
				
			} else {
				
				tilePaint (num);
			}
		}
		
		private void paintDie (int num, int xCoord, int yCoord)
		{
			Log.Debug ("die paint 3 " + num.ToString ());
			
			int numPainted = 0;
			
			int myX = xCoord;
			int myY = yCoord;
			
			for (int i  = 0; i < 3; i++) {
				
				for (int j = 0; j < 3; j++) {
					
					mCube.FillRect (mRectColor, myX, myY, h + 5, w + 5);	
					myX += 20;
					numPainted ++;
					
					if (numPainted == num) {
						return;
					}
						
				}
				myX = xCoord;
				myY += 20;
				
			}
		}
		
		/*
		 * Overloaded with sides
		 */ 
		
		private void one (Cube.Side side)
		{
				
			mCube.FillRect (mRectColor, x + xOffset, y, h, w);
				

			
			
			
			//mCube.Paint ();
		
		}
		
		private void two (Cube.Side side)
		{
			
			if (Cube.Side.LEFT == side || side == Cube.Side.TOP) {
				
				mCube.FillRect (mRectColor, x + (xOffset * 3), y - 15, h, w);
				mCube.FillRect (mRectColor, x + (xOffset * 1), y + 15, h, w);
	
			} else {
				mCube.FillRect (mRectColor, x + (xOffset * 3), y + 15, h, w);
				mCube.FillRect (mRectColor, x + (xOffset * 1), y - 15, h, w);
	
			}
			
			
		
			//mCube.FillRect (mRectColor, x + (xOffset * 2), y - 15, h, w);
			//mCube.FillRect (mRectColor, x + (xOffset * 2), y + 30, h, w);
			//mCube.FillRect (mRectColor, x + (xOffset * 2), y - 30, h, w);
			
			/*
			mCube.FillRect (mRectColor, x + 15, y, h, w);
			mCube.FillRect (mRectColor, x - 15, y, h, w);
			mCube.FillRect (mRectColor, x + 30, y, h, w);
			mCube.FillRect (mRectColor, x - 30, y, h, w);
			*/
			//mCube.Paint ();
		}
		
		private void three (Cube.Side side)
		{
			if (Cube.Side.LEFT == side || side == Cube.Side.TOP) {
				
				mCube.FillRect (mRectColor, x + (xOffset * 2), y, h, w);
				mCube.FillRect (mRectColor, x + (xOffset * 3), y + 15, h, w);
				mCube.FillRect (mRectColor, x + (xOffset * 1), y - 15, h, w);
	
			} else {
				mCube.FillRect (mRectColor, x + (xOffset * 2), y, h, w);
				mCube.FillRect (mRectColor, x + (xOffset * 1), y + 15, h, w);
				mCube.FillRect (mRectColor, x + (xOffset * 3), y - 15, h, w);
	
			}
			
		}
		
		private void four (Cube.Side side)
		{
			
			mCube.FillRect (mRectColor, x + (xOffset * 1), y + 15, h, w);
			mCube.FillRect (mRectColor, x + (xOffset * 3), y + 15, h, w);
			mCube.FillRect (mRectColor, x + (xOffset * 3), y - 15, h, w);
			mCube.FillRect (mRectColor, x + (xOffset * 1), y - 15, h, w);
			
		}
		
		private void five (Cube.Side side)
		{
			
			mCube.FillRect (mRectColor, x + (xOffset * 2), y, h, w);
			mCube.FillRect (mRectColor, x + (xOffset * 1), y + 15, h, w);
			mCube.FillRect (mRectColor, x + (xOffset * 1), y - 15, h, w);
			mCube.FillRect (mRectColor, x + (xOffset * 3), y + 15, h, w);
			mCube.FillRect (mRectColor, x + (xOffset * 3), y - 15, h, w);
			//mCube.FillRect (mRectColor, x + (xOffset * 3), y + 30, h, w);
			//mCube.FillRect (mRectColor, x + (xOffset * 3), y - 30, h, w);
			
		}
		
		private void six (Cube.Side side)
		{
			
			mCube.FillRect (mRectColor, x + (xOffset * 1), y, h, w);
			mCube.FillRect (mRectColor, x + (xOffset * 1), y + 15, h, w);
			mCube.FillRect (mRectColor, x + (xOffset * 1), y - 15, h, w);
			mCube.FillRect (mRectColor, x + (xOffset * 3), y, h, w);
			mCube.FillRect (mRectColor, x + (xOffset * 3), y + 15, h, w);
			mCube.FillRect (mRectColor, x + (xOffset * 3), y - 15, h, w);

		}
		
		private void seven (Cube.Side side)
		{
			
			mCube.FillRect (mRectColor, x + (xOffset * 1), y, h, w);
			mCube.FillRect (mRectColor, x + (xOffset * 1), y + 15, h, w);
			mCube.FillRect (mRectColor, x + (xOffset * 1), y - 15, h, w);
			mCube.FillRect (mRectColor, x + (xOffset * 3), y, h, w);
			mCube.FillRect (mRectColor, x + (xOffset * 3), y - 15, h, w);
			mCube.FillRect (mRectColor, x + (xOffset * 3), y + 15, h, w);
			mCube.FillRect (mRectColor, x + (xOffset * 2), y - 15, h, w);
		}
		
		private void eight (Cube.Side side)
		{
			
			mCube.FillRect (mRectColor, x + (xOffset * 1), y, h, w);
			mCube.FillRect (mRectColor, x + (xOffset * 1), y + 15, h, w);
			mCube.FillRect (mRectColor, x + (xOffset * 1), y - 15, h, w);
			mCube.FillRect (mRectColor, x + (xOffset * 2), y + 15, h, w);
			mCube.FillRect (mRectColor, x + (xOffset * 3), y, h, w);
			mCube.FillRect (mRectColor, x + (xOffset * 3), y + 15, h, w);
			mCube.FillRect (mRectColor, x + (xOffset * 3), y - 15, h, w);
			mCube.FillRect (mRectColor, x + (xOffset * 2), y - 15, h, w);
			//mCube.FillRect (mRectColor, x + (xOffset * 4), y + 30, h, w);
			
		}
		
		private void nine (Cube.Side side)
		{
			
			mCube.FillRect (mRectColor, x + (xOffset * 1), y, h, w);
			mCube.FillRect (mRectColor, x + (xOffset * 1), y + 15, h, w);
			mCube.FillRect (mRectColor, x + (xOffset * 1), y - 15, h, w);
			mCube.FillRect (mRectColor, x + (xOffset * 2), y, h, w);
			mCube.FillRect (mRectColor, x + (xOffset * 2), y - 15, h, w);
			mCube.FillRect (mRectColor, x + (xOffset * 2), y + 15, h, w);
			mCube.FillRect (mRectColor, x + (xOffset * 3), y, h, w);
			mCube.FillRect (mRectColor, x + (xOffset * 3), y - 15, h, w);
			mCube.FillRect (mRectColor, x + (xOffset * 3), y + 15, h, w);
			
		}
		
		private void pyramind (int num)
		{
			
			int xPos = Cube.SCREEN_WIDTH / 2;//x;
			int yPos = Cube.SCREEN_WIDTH / 2;//y;
			
			
			xPos += x;
			yPos += y;
			
			int level = 1;
			
			for (int i = 0; i < num; i++) {
				mCube.FillRect (mRectColor, xPos, yPos, h, w);
				
				//Increment the next level of he pyramind
				if (i == level) {
					level += level;
				}
				
				
			}
			
		}
		
		private void DrawVerticleSeparator ()
		{
			int width = separatorWidth;
			mCube.FillRect (Constants.RedColor, (Cube.SCREEN_WIDTH / 2) - width - 1, 0, width, Cube.SCREEN_MAX_Y);
			
		}
		
		private void DrawHorizontalSeparator ()
		{
			int width = separatorWidth;
			mCube.FillRect (Constants.RedColor, 0, (Cube.SCREEN_WIDTH / 2) - width - 1, Cube.SCREEN_MAX_X, width);
			
		}
		
	}
	
}

