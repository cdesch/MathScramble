// ResultController.cs
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
	public class ResultController : IStateController
	{
		private String classname = "ResultController";
		private Cube mCube;
		private Color bgColor = Color.White;
		private CubeWrapper mWrapper;
		public int result;
		
		public ResultController (Cube cube)
		{
			Log.Debug (classname + " Init");
			mCube = cube;
			mWrapper = (CubeWrapper)cube.userData;
			
			//Event listeners
			mCube.NeighborAddEvent += OnNeighborAdd;
			mCube.NeighborRemoveEvent += OnNeighborRemove;
			
			
			
		}
		
		public void OnSetup (string trainsitionId)
		{
			Log.Debug (classname + " OnSetup");
			result = 0;
			mCube.Paint ();
		}
		
		public void OnTick (float dt)
		{
			Log.Debug (classname + " OnTick");
			
			if (mWrapper.mNeedDraw) {
				mWrapper.mNeedDraw = false;
				
				Cube[] connected = CubeHelper.FindConnected (mCube);		//Get all the connected cubes
				//Log.Debug (" num cubes connected: {0}", connected.Length);
			
				int resultnum = 0;
			
				Log.Debug ("Result Connected: " + connected.Length);
			
				//Loop through each cube
				foreach (Cube neighbor in connected) {
					
					CubeWrapper neighborWrapper = (CubeWrapper)neighbor.userData;
					
					//Skip any other cube other than an Operand cube
					//Find the total of the num
					if (neighborWrapper.mCubeStateMachine.Current == Constants.OperandState) {
						
						resultnum += neighborWrapper.mValue;
			
						
					}
				
				}
				
				Log.Debug ("Result: " + result.ToString ());
				//Only change it if the result is different
				if (resultnum != result) {
					result = resultnum;
					OnPaint (mWrapper.mNeedDraw);
				}
				
				mWrapper.mNeedDraw = false;
			
			}
			
		}
		
		//Event Listeners
		//On Neightbored add event. Show Operation on the hint cube
		private void OnNeighborAdd (Cube cube1, Cube.Side side1, Cube cube2, Cube.Side side2)
		{
			
			//Log.Debug ("-- cube Neighbored{0}", this.mCube.UniqueId);
			/*
			Cube[] connected = CubeHelper.FindConnected (cube1);		//Get all the connected cubes
			Log.Debug (" num cubes connected: {0}", connected.Length);
			
			result = 0;
			
			
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
			*/
			//mWrapper.mNeedDraw = true;	
			
			
		}
		
		//On on Neighbore removed event. return to default state
		private void OnNeighborRemove (Cube cube1, Cube.Side side1, Cube cube2, Cube.Side side2)
		{
			//Do Nothing
			
			Cube[] connected = CubeHelper.FindConnected (mCube);
			if (connected.Length == 1) {
				//Draw the default state
				
				//mWrapper.mValue = result;
				//mCube.FillScreen (bgColor);
				//Set the draw flag
				//mWrapper.mNeedDraw = true;	
			}
			
		}
		
		public void OnPaint (bool canvasDirty)
		{
			//Log.Debug ("{0} OnPaint: {1}", classname, this.mCube.UniqueId);
			//Paint Background
			Log.Debug (classname + " OnPaint");
			
			
			mCube.FillScreen (bgColor);
			
			Cube[] connected = CubeHelper.FindConnected (mCube);		//Get all the connected cubes
			
			if (connected.Length == 1) {
				StringPainter painter = new StringPainter (mCube, "=? ");	
				painter = null;	//Free the resource
			} else {
				StringPainter painter = new StringPainter (mCube, "=" + result.ToString ());	
				painter = null;	//Free the resource
			}
			
			/*
			if (result != 99) {
				StringPainter painter = new StringPainter (mCube, "= " + result.ToString ());	
				painter = null;	//Free the resource
			} else {
				StringPainter painter = new StringPainter (mCube, "= ?");	
				painter = null;	//Free the resource
			}*/
		
			
			mCube.Paint ();
			
		}
		
		public void OnDispose ()
		{
			Log.Debug (classname + " OnDispose");
		}
		
	}
}

