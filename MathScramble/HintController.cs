// HintState.cs
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
using Sifteo.Util;
using Sifteo;
using System.Collections.Generic;

namespace MathScramble
{
	public class HintController : IStateController
	{
		String classname = "HintController";
		Cube mCube;
		CubeWrapper mWrapper;
		Color bgColor = Color.White;
		Cube lastNeighbor;
		
		public HintController (Cube cube)
		{
			Log.Debug (classname + " Init");
			mCube = cube;
			mWrapper = (CubeWrapper)cube.userData;
			
			lastNeighbor = null;
			
			mCube.NeighborAddEvent += OnNeighborAdd;
			mCube.NeighborRemoveEvent += OnNeighborRemove;
		
		}
		
		public void OnSetup (string trainsitionId)
		{
			Log.Debug (classname + " OnSetup");
			mCube.FillScreen (bgColor);
			StringPainter painter = new StringPainter (mCube, "Hint", System.Drawing.Color.White, 24);
			mCube.Paint ();
		}
		
		public void OnTick (float dt)
		{
			Log.Debug (classname + " OnTick");
			if (mWrapper.mNeedDraw) {
				
				OnPaint (mWrapper.mNeedDraw);
			}
		}
		
		//FIXME: Needs to be reworked such that the Event handles only mark the cube has dirty and the cube then detects and redraws itself.
		
		//Event Listeners
		//On Neightbored add event. Show Operation on the hint cube
		private void OnNeighborAdd (Cube cube1, Cube.Side side1, Cube cube2, Cube.Side side2)
		{
			
			//Set the draw flag
			mWrapper.mNeedDraw = true;	
			
			
			lastNeighbor = cube2;	
		
			
			//Log.Debug ("-- cube Neighbored{0}", this.mCube.UniqueId);
		
			
		}
		
		//On on Neighbore removed event. return to default state
		private void OnNeighborRemove (Cube cube1, Cube.Side side1, Cube cube2, Cube.Side side2)
		{
			//Set the draw flag
			mWrapper.mNeedDraw = true;	
			
			Cube[] connected = CubeHelper.FindConnected (mCube);
			if (connected.Length == 1) {
				//Draw the default state
				mCube.FillScreen (bgColor);
				StringPainter painter = new StringPainter (mCube, "Hint", System.Drawing.Color.Blue, 24);
				painter = null;	//Free the resource
				
				lastNeighbor = null;
			}
			
		}
		
		public void OnPaint (bool canvasDirty)
		{			
		
			//Log.Debug ("{0} OnPaint: {1}", classname, this.mCube.UniqueId);
			if (canvasDirty) {
				
				Cube[] connected = CubeHelper.FindConnected (mCube);		//Get all the connected cubes
				
				//Check to make sure the last neighboor is still directly connected to the cube
				if (!mCube.Neighbors.Contains (lastNeighbor)) {
					//Find a new neighbor
					if (mCube.Neighbors.Top != null) {
						lastNeighbor = mCube.Neighbors.Top;
					} else if (mCube.Neighbors.Bottom != null) {
						lastNeighbor = mCube.Neighbors.Bottom;
					} else if (mCube.Neighbors.Right != null) {
						lastNeighbor = mCube.Neighbors.Right;
					} else if (mCube.Neighbors.Left != null) {
						lastNeighbor = mCube.Neighbors.Left;
					} else {
						Log.Debug ("EXCEPTION!");
						return;
					}
				}
				
				if (connected.Length >= 2) {	
				
					CubeWrapper wrapper = (CubeWrapper)lastNeighbor.userData;
				
					//Draw Domino Operand
					if (wrapper.mCubeStateMachine.Current == Constants.OperandState) {
						DrawOperand (wrapper);
					} else if (wrapper.mCubeStateMachine.Current == Constants.OperatorState) {
						//Draw Domino Operator Split
						DrawOperator (lastNeighbor);	
					} else if (wrapper.mCubeStateMachine.Current == Constants.ResultState) {
						DrawResult (wrapper);
					}
				
					//Draw Domino Operator
					mWrapper.mNeedDraw = true;
				
				} else if (connected.Length > 3) {
					//just another cube, ignore it and do nothing
				}

				mCube.Paint ();
			}
			
		}
		
		/*
		 * Drawing of the different hints
		 */ 
		public void DrawOperand (CubeWrapper wrapper)
		{
			Log.Debug ("-- Hint Controller Draw Operand");
			//Paint Background
			mCube.FillScreen (bgColor);
			PaintDomino (wrapper);

		
		}
		
		public void DrawOperator (Cube cube)
		{
			mCube.FillScreen (bgColor);
			
			Log.Debug ("Draw Operator");
			Cube[] connected = CubeHelper.FindConnected (cube);
			if (connected.Length > 2) {
				
				//Loop through each cube
				foreach (Cube neighbor in connected) {
					
					CubeWrapper neighborWrapper = (CubeWrapper)neighbor.userData;
					
					//Skip any other cube other than an Operand cube
					if (neighborWrapper.mCubeStateMachine.Current == Constants.OperandState) {
						
						//Determine the side it is on
						//DrawDiagnals ();
						DrawVerticleSeparator ();
						//Log.Debug ("side of Operator: " + cube.Neighbors.SideOf (neighbor).ToString () + " num: " + neighborWrapper.mValue.ToString ());
						CubeWrapper wrapper = (CubeWrapper)neighbor.userData;
						//Paint that side of the cube //TODO: get the relative side to the hint cube. 
						DominoPainter painter = new DominoPainter (mCube, wrapper.mValue, cube.Neighbors.SideOf (neighbor));
						painter = null;
						
					} else {
						//Do nothing
					}
					
				}
				
				//Draw Left Side
				
				
				
				//Draw Right side
				
				
			}
			
		}
		
		public void DrawResult (CubeWrapper wrapper)
		{
			
			Cube[] connected = CubeHelper.FindConnected (mCube);		//Get all the connected cubes
			Log.Debug (" num cubes connected: {0}", connected.Length);
			
			int result = 0;
			
			
			//Loop through each cube
			foreach (Cube neighbor in connected) {
					
				CubeWrapper neighborWrapper = (CubeWrapper)neighbor.userData;
					
				//Skip any other cube other than an Operand cube
				if (neighborWrapper.mCubeStateMachine.Current == Constants.OperandState) {
					//Mark the hint cube to be updated
					result += neighborWrapper.mValue;
					mWrapper.mNeedDraw = true;	
				}
				
			}
			
			//Paint Background
			mCube.FillScreen (bgColor);
			PaintDomino (wrapper, result);
			
		}
		
		public void PaintDomino (CubeWrapper wrapper)
		{
			DominoPainter painter = new DominoPainter (mCube, wrapper.mValue);
			painter = null;
		}
		
		public void PaintDomino (CubeWrapper wrapper, int operandVal)
		{
			DominoPainter painter = new DominoPainter (mCube, operandVal);
			painter = null;
		}
		
		
		//Cut cube into quarters
		private void DrawDiagnals ()
		{
			//From top left to bottom right
			for (int i = 0; i< Cube.SCREEN_WIDTH; i += 2) {
				mCube.FillRect (Constants.RedColor, i, i, 2, 2);
			}
			
			int x = 126;
			for (int i = 0; i< Cube.SCREEN_WIDTH; i += 2) {
				mCube.FillRect (Constants.RedColor, x, i, 2, 2);
				x -= 2;
			}
			
		}
		
		private void DrawVerticleSeparator ()
		{
			int width = 4;
			mCube.FillRect (Constants.RedColor, (Cube.SCREEN_WIDTH / 2) - width, 0, width, Cube.SCREEN_MAX_Y);
			
		}
		
		private void DrawHorizontalSeparator ()
		{
			int width = 4;
			mCube.FillRect (Constants.RedColor, 0, (Cube.SCREEN_WIDTH / 2) - width, Cube.SCREEN_MAX_X, width);
			
		}
		
		public void OnDispose ()
		{
			Log.Debug (classname + " OnDispose");
		}
	}
}

