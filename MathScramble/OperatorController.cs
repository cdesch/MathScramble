// OperatorController.cs
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
	public class OperatorController : IStateController
	{
	
		private String classname = "OperatorController";
		private Cube mCube;
		private Color bgColor = Color.White;
		private CubeWrapper mWrapper;
		
		public OperatorController (Cube cube)
		{
			Log.Debug (classname + " Init");
			mCube = cube;
			mWrapper = (CubeWrapper)cube.userData;
			
			mCube.NeighborAddEvent += OnNeighborAdd;
			mCube.NeighborRemoveEvent += OnNeighborRemove;
		}
		
		public void OnSetup (string trainsitionId)
		{
			Log.Debug (classname + " OnSetup");
		}
		
		public void OnTick (float dt)
		{
			Log.Debug (classname + " OnTick");
			
			if (mWrapper.mNeedDraw) {
				
				OnPaint (mWrapper.mNeedDraw);
			}
			
		}
		
		//Event Listeners
		//On Neightbored add event. Show Operation on the hint cube
		private void OnNeighborAdd (Cube cube1, Cube.Side side1, Cube cube2, Cube.Side side2)
		{
			
			//Log.Debug ("-- cube Neighbored{0}", this.mCube.UniqueId);
			
			Cube[] connected = CubeHelper.FindConnected (cube1);		//Get all the connected cubes
			Log.Debug (" num cubes connected: {0}", connected.Length);
			//Loop through each cube
			foreach (Cube neighbor in connected) {
					
				CubeWrapper neighborWrapper = (CubeWrapper)neighbor.userData;
					
				//Skip any other cube other than an Operand cube
				if (neighborWrapper.mCubeStateMachine.Current == Constants.HintState) {
					//Mark the hint cube to be updated
					neighborWrapper.mNeedDraw = true;
				}
			}
			
		}
		
		//On on Neighbore removed event. return to default state
		private void OnNeighborRemove (Cube cube1, Cube.Side side1, Cube cube2, Cube.Side side2)
		{
			//Do Nothing
			/*
			Cube[] connected = CubeHelper.FindConnected (mCube);
			if (connected.Length == 1) {
				//Draw the default state
				mCube.FillScreen (bgColor);
				//Set the draw flag
				mWrapper.mNeedDraw = true;	
			}
			 */
		}
		
		public void OnPaint (bool canvasDirty)
		{
			Log.Debug ("{0} OnPaint: {1}", classname, this.mCube.UniqueId);
			//Paint Background
		
			String operatorString = Constants.opQuestion;
			if (mWrapper.mValue == 1) {
				//Add
				operatorString = Constants.opAdd;
			} else if (mWrapper.mValue == 2) {
				//Subtract
				operatorString = Constants.opSub;
			} else if (mWrapper.mValue == 3) {
				//Multiply
				operatorString = Constants.opMul;
			} else if (mWrapper.mValue == 4) {
				//Divide
				operatorString = Constants.opDiv;
			}
			
			mCube.FillScreen (bgColor);
			StringPainter painter = new StringPainter (mCube, operatorString, mWrapper.mColor, mWrapper.mSize);
			painter = null;
			
			mCube.Paint ();
		
		}
		
		public void OnDispose ()
		{
			Log.Debug (classname + " OnDispose");
		}
		

		/*
		 *  Generate the Operator Value
		 */

		public int GenerateValue ()
		{
			// + = 1
			// - = 2
			// * = 3
			// / = 4
			
			//Generate new Random Number
			return RandomNumber (1, 4);
		
		}
		
		private int RandomNumber (int min, int max)
		{
			Random random = new Random ();
			return random.Next (min, max); 
		}
		
	}
}

