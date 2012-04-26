// MathScramble.cs
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
using System.Collections;
using System.Collections.Generic;
using Sifteo;
using Sifteo.Util;

namespace MathScramble
{
	public class MathScramble : BaseApp
	{
		
		private bool debuggingMode = true;
		public List<CubeWrapper> mWrappers = new List<CubeWrapper> (0);
		//public List<OperandCube> operandWrappers = new List<OperandCube> (0);
		//public List<OperatorCube> operatorWrappers = new List<OperatorCube> (0);
		public bool mNeedCheck;
		private Sound mMusic;
		private int lastIndex;
		
		//State Machine
		public StateMachine sm;
		
		//State MachineControllers
		private TitleController 	titleController;
		private MenuController 		menuController;
		private GameBeginController gameBeginController;
		private GameController 		gameController;
		private GameEndController 	gameEndController;
		public CubeRole[] cubeStates;
		
		//Set 
		override public int FrameRate {
			get { return 20; }
		}

		// called during intitialization, before the game has started to run
		override public void Setup ()
		{
			Log.Debug ("Setup()");
			
			//Initially, check all the cubes
			mNeedCheck = true;
			
			System.Version myVersion = System.Reflection.Assembly.GetExecutingAssembly ().GetName ().Version;		
			Log.Debug (myVersion.ToString ());
			

			//Init Cube roles
			cubeStates = new CubeRole[6];
			cubeStates [0] = new CubeRole (Constants.HintState, Constants.tTitleToHintState, -1, -1, System.Drawing.Color.Black);
			cubeStates [1] = new CubeRole (Constants.OperandState, Constants.tTitleToOperandState, Constants.kOperandMin, Constants.kOperandMax, System.Drawing.Color.Orange);
			cubeStates [2] = new CubeRole (Constants.OperatorState, Constants.tTitleToOperatorState, Constants.kOperatorMin, Constants.kOperatorMax, System.Drawing.Color.Black);
			cubeStates [3] = new CubeRole (Constants.OperandState, Constants.tTitleToOperandState, Constants.kOperandMin, Constants.kOperandMax, System.Drawing.Color.Red);
			cubeStates [4] = new CubeRole (Constants.ResultState, Constants.tTitleToResultState, 5, 5, System.Drawing.Color.Blue);
			cubeStates [5] = new CubeRole (Constants.OperandState, Constants.tTitleToOperandState, Constants.kOperandMin, Constants.kOperandMax, System.Drawing.Color.Green);

			// Loop through all the cubes and set them up.
			lastIndex = 1;
			foreach (Cube cube in CubeSet) {
				
				CubeWrapper wrapper = new CubeWrapper (this, cube, lastIndex);
				lastIndex += 1;
				mWrappers.Add (wrapper);
			}
			
			//Event Listeners
			this.PauseEvent += OnPause;
			this.UnpauseEvent += OnUnpause;
			CubeSet.NewCubeEvent += OnNewCube;
			CubeSet.LostCubeEvent += OnLostCube;
			CubeSet.NeighborAddEvent += OnNeighborAdd;
			CubeSet.NeighborRemoveEvent += OnNeighborRemove;
			
		}

		// When Siftrunner pauses, the game is responsible for drawing a message to
		// the cubes' display to notify the user.
		private void OnPause ()
		{
			Log.Debug ("Pause!");
			foreach (Cube cube in CubeSet) {
				cube.FillScreen (Color.Black);
				cube.FillRect (Color.White, 55, 55, 6, 18);
				cube.FillRect (Color.White, 67, 55, 6, 18);
				cube.Paint ();
			}
    
		}
		
		// ### Unpause ###
		// When Siftrunner unpauses, we need to repaint everything that was
		// previously covered by the pause message.
		//
		// In this case, we can just set the mNeedCheck flag, and we'll repaint on
		// the next Tick.
		private void OnUnpause ()
		{
			Log.Debug ("Unpause.");
			mNeedCheck = true;
		}
	
		// ### New Cube ###
		// When a new cube connects while the game is running, we need to create a
		// wrapper for it so that it is included in gameplay.
		//
		// If a new cube is added while the game is paused, this event will be
		// handled after the player unpauses, but before the unpause event is
		// handled.
		private void OnNewCube (Cube c)
		{
			Log.Debug ("New Cube {0}", c.UniqueId);
			
			CubeWrapper wrapper = (CubeWrapper)c.userData;
			if (wrapper == null) {
				wrapper = new CubeWrapper (this, c, lastIndex);
				lastIndex += 1;
				mWrappers.Add (wrapper);
				Log.Debug ("{0}", mWrappers);
			}
			
			mNeedCheck = true;
    
		}
	
		// ### Lost Cube ###
		// When a cube falls offline while the game is running, we need to delete
		// its wrapper.
		//
		// If Siftrunner forced the to pause due to a cube going offline, this
		// event will be handled before the pause event is handled.
		private void OnLostCube (Cube c)
		{
			Log.Debug ("Lost Cube {0}", c.UniqueId);
		
			CubeWrapper wrapper = (CubeWrapper)c.userData;
			if (wrapper != null) {
				c.userData = null;
				mWrappers.Remove (wrapper);
			}
		
			mNeedCheck = true;
    
		}

		// Don't do any neighbor checking logic in these event handlers. Set a
		// flag on each wrapper, but wait until the next tick.
    
		private void OnNeighborAdd (Cube cube1, Cube.Side side1, Cube cube2, Cube.Side side2)
		{

			mNeedCheck = true;
			CubeWrapper wrapper1 = (CubeWrapper)cube1.userData;
			CubeWrapper wrapper2 = (CubeWrapper)cube2.userData;
			//If the neighbor added was the Hint Cube, only update thate one and ignore the rest)
			if (wrapper1.mCubeStateMachine.Current == Constants.HintState) {
				wrapper1.mNeedDraw = true;
			} else if (wrapper2.mCubeStateMachine.Current == Constants.HintState) {
				wrapper2.mNeedDraw = true;	
			} else {
				//Cycle through each cube and note that it needs to be repainted. 
				Cube[] connected = CubeHelper.FindConnected (cube1);
				
				foreach (Cube cube in connected) {
					CubeWrapper wrapper = (CubeWrapper)cube.userData;
					//If this cube if a cube is added/ Redraw the result cube
				
					if (wrapper.mCubeStateMachine.Current == Constants.ResultState) {
					
						wrapper.mNeedDraw = true;	
					}
				
				}
				
			}
			
		}
    
		private void OnNeighborRemove (Cube cube1, Cube.Side side1, Cube cube2, Cube.Side side2)
		{
			mNeedCheck = true;
			CubeWrapper wrapper1 = (CubeWrapper)cube1.userData;
			CubeWrapper wrapper2 = (CubeWrapper)cube2.userData;
			//If the neighbor added was the Hint Cube, only update thate one and ignore the rest)
			if (wrapper1.mCubeStateMachine.Current == Constants.HintState) {
				wrapper1.mNeedDraw = true;
			} else if (wrapper2.mCubeStateMachine.Current == Constants.HintState) {
				wrapper2.mNeedDraw = true;	
			} else {
				//Cycle through each cube and note that it needs to be repainted. 
				Cube[] connected = CubeHelper.FindConnected (cube1);
				
				foreach (Cube cube in connected) {
					CubeWrapper wrapper = (CubeWrapper)cube.userData;
					//If this cube if a cube is added/ Redraw the result cube
				
					if (wrapper.mCubeStateMachine.Current == Constants.ResultState) {
					
						wrapper.mNeedDraw = true;	
					}
				
				}
				
			}
			
		}
		
		override public void Tick ()
		{
			//Log.Debug ("Tick()");
			
			if (mNeedCheck) {
				
				mNeedCheck = false;
				foreach (CubeWrapper cube in mWrappers) {
				
					if (cube.mNeedDraw) {
						cube.Tick ();
					
						cube.mNeedDraw = false;
						//Tick each cube to be updated
					
					
						//bool t = CheckNeighbors ();
						//CheckSound (t);
					
					
					}
				
				}	
			}
			
			
		}
		
		// This method scans the cubes and checks to see if they are neighbored in
		// a left-to-right row or a bottom-to-top column. If they are neighbored
		// correctly, it also checks to see if they are sorted correctly.
		private bool CheckNeighbors ()
		{
			bool found = false;
			int totalCubes = CubeSet.Count;
	
			// ### CubeHelper.FindRow ###
			// FindRow returns the first row found in the given cube set. It can be
			// used to check whether your cubes are all lined up.
			//
			// A row is a series of cubes neighbored **left to right**.  Cubes can only
			// form a row if they are all oriented the same way.
			Cube[] row = CubeHelper.FindRow (CubeSet);

	
			// ### CubeHelper.FindColumn ###
			// FindColumn returns the first column found in the given cube set. It
			// can be used to check whether your cubes are all lined up.
			//
			// A column is a series of cubes neighbored **bottom to top**.  Cubes can
			// only form a column if they are all oriented the same way.
			Cube[] column = CubeHelper.FindColumn (CubeSet);
			// If we have a full column, check to see if it is sorted by index.
			/*
			if (column.Length == totalCubes) {
				found = true;
				int lastId = -1;
				foreach (Cube cube in column) {
					CubeWrapper wrapper = (CubeWrapper)cube.userData;
					if (wrapper.mIndex < lastId)
						found = false;
					lastId = wrapper.mIndex;
				}
			}*/
			
			//LOG Row Length
			Log.Debug ("row: " + row.Length);
			Log.Debug ("column " + column.Length);
			if (row.Length >= 1 || column.Length >= 1) {
				
					

			}

			// Here we go through each wrapper and update its state depending on the
			// results of our search.
			
			foreach (CubeWrapper wrapper in mWrappers) {
				wrapper.CheckNeighbors (true);
				//wrapper.mNeedDraw = true;
				wrapper.Tick ();
			}
			

			return found;

		}
		// ### Sound ###
		// This method starts or stops the appropriate sounds depending on the
		// results of the search in CheckNeighbors.
		private void CheckSound (bool isCorrectlyNeighbored)
		{

			// If the cubes are lined up correctly, stop the music and play a sound
			// effect.
			if (isCorrectlyNeighbored) {

				if (mMusic != null) {
					// Stop the music if it is currently playing.
					if (mMusic.IsPlaying) {
						mMusic.Stop ();
					}
					mMusic = null;

					// To play a one-shot sound effect, just create a sound object and
					// call its Play method.
					//
					// The audio system will clean up the sound object after it is done,
					// so we don't have to hold on to a handle.
					Sound s = Sounds.CreateSound ("gliss");
					s.Play (20);
				}

			} else {
				if (mMusic == null) {

					// To play a looping sound effect or music track, create the sound
					// object and call its Play method with the extra argument to tell it
					// to loop.
					//
					// We hold on to the sound object after we play it so that we can
					// stop it later.
					
					mMusic = Sounds.CreateSound ("music");
          
					//mMusic.Play (1, 1);
				
				}
			}
		}
		
		
		/*
		 * StateMachine Initialization
		 */
		private StateMachine SetupStateMachine ()
		{
			//Init the State Machine
			StateMachine sm = new StateMachine ();
			
			//Init the Controllers
			titleController = new TitleController ();
			menuController = new MenuController ();
			gameController = new GameController ();
			gameBeginController = new GameBeginController ();
			gameEndController = new GameEndController ();
			
			sm.State (Constants.TitleState, titleController);
			sm.State (Constants.MenuState, menuController);
			sm.State (Constants.GameState, gameController);
			sm.State (Constants.GameBegin, gameBeginController);
			sm.State (Constants.GameEndState, gameEndController);
	
			sm.Transition ("Title", "TitleToMenu", "Menu");
			sm.Transition (Constants.MenuState, Constants.tMenuToGameBegin, Constants.GameBegin);
			sm.Transition (Constants.GameBegin, Constants.tGameBeginToMenu, Constants.MenuState);  
			sm.Transition (Constants.GameBegin, Constants.tGameBeginToGame, Constants.GameState);
			sm.Transition (Constants.GameState, Constants.tGameToGameEnd, Constants.GameEndState);
			sm.Transition (Constants.GameEndState, Constants.tGameEndToMenu, Constants.MenuState);
			//Set the Current State
			sm.SetState (Constants.MenuState, Constants.tMenuToGameBegin);
			
			return sm;
			
		}
		
	}
	
	public class CubeWrapper
	{

		public MathScramble mApp;
		public Cube mCube;
		public int mIndex;
		private int mSpriteIndex;
		private int mRotation;
		public StateMachine mCubeStateMachine;
		private OperandController 	operandController;
		private OperatorController 	operatorController;
		private HintController 		hintController;
		private ResultController 	resultController;
		public int mValue;
		public System.Drawing.Color mColor;
		public float mSize;

		// This flag tells the wrapper to redraw the current image on the cube. (See Tick, below).
		public bool mCubeSelected = false; //Was selected by the user on this tick
		public bool mIsSelected = false; //Is noted as the selected cube
		public bool mNeedDraw = true;
		public bool mHintCheck = false;
		private CubeRole mRole; 	//The Role the physical Cube will serve in the game 
		
		public CubeWrapper (MathScramble app, Cube cube, int seq)
		{
			mApp = app;
			mCube = cube;
			mCube.userData = this;
			mSpriteIndex = 0;
			//mRectColor = new Color (36, 182, 255);
			//mSelectColor = new Color (255, 0, 0);
			mRotation = 0;
			mCube.TiltEvent += OnTilt;
			mIndex = seq;
			
			mCube.ButtonEvent += HandleCubeButtonEvent;
			mCube.ShakeStartedEvent += HandleMCubeShakeStartedEvent;
			
			
			
			
			//Init the State Machine
			mRole = mApp.cubeStates [seq - 1];			
			InitStateMachine ();		
			
			//
			mValue = mRole.GenerateValue ();
			mColor = mRole.mColor;
			mSize = mRole.mSize;
			
			/*
			if (mCubeStateMachine.Current == Constants.HintState) {
				mNeedDraw = true;	
				Tick ();
			}*/
			
			//
			
			
		}
		
	
		//Setup Operand Cube
		private void InitStateMachine ()
		{
			mCubeStateMachine = new StateMachine ();
			
			operandController = new OperandController (mCube);
			operatorController = new OperatorController (mCube);
			hintController = new HintController (mCube);
			resultController = new ResultController (mCube);
			
			mCubeStateMachine.State (Constants.HintState, hintController);
			mCubeStateMachine.State (Constants.OperandState, operandController);
			mCubeStateMachine.State (Constants.OperatorState, operatorController);
			mCubeStateMachine.State (Constants.ResultState, resultController);
			
			mCubeStateMachine.Transition (Constants.OperandState, Constants.tOperandToOperatorState, Constants.OperatorState);  
			mCubeStateMachine.Transition (Constants.OperatorState, Constants.tOperatorToOperandState, Constants.OperandState);  			
			
			
			mCubeStateMachine.SetState (mRole.stateName, mRole.stateTransition);
			
			/*
			if (mIndex == 6) { //Must have 6 cubes to play
				//Set hint cube as last cube in set
				mCubeStateMachine.SetState (Constants.HintState, Constants.tTitleToHintState);
			
				//Value is negative for Hint Cube
				mValue = -1;
			} else if (mIndex % 2 == 0) {
				//Number is Even - Make that cube an Operator Cube
				SetupOperandCube ();
			}
			
			mCubeStateMachine.SetState (Constants.OperandState, Constants.tTitleToOperandState);
			
			//Randome for OperandCube
			mValue = RandomNumber (0, 9);
			*/
		}
		
		//
		//Handle Button Event
		void HandleCubeButtonEvent (Cube c, bool pressed)
		{
			
			//Check if the button was pressed
			if (pressed) {
				
				if (mCubeStateMachine.Current == Constants.OperandState) {
					
					//Generate a random number 0 through 9
					int random = RandomNumber (0, 9);
					
					//If the number is the same, Regenerate until we find a different one
					while (random == mValue) {
						random = RandomNumber (0, 9);
					}
					
					mValue = random;
					//Refresh the screen by setting this flag
					mNeedDraw = true;
					flagResultcube ();
					//
					
					//Refresh that cube
					Tick ();
					
				} else if (mCubeStateMachine.Current == Constants.OperatorState) {
					
					//DO Nothing - Do not change Operator
					/*
					mValue = RandomNumber (1, 4);
					//Refresh the screen by setting this flag
					mNeedDraw = true;
					//Refresh that cube
					Tick ();
					*/
				} else if (mCubeStateMachine.Current == Constants.HintState || mCubeStateMachine.Current == Constants.ResultState) {
					mNeedDraw = true;
					
				} 

			}
			
		}
		
		/*
		 * Start Shaking Event Handler
		 */ 
		void HandleMCubeShakeStartedEvent (Cube c)
		{
			HandleShakeCubeEvent (c, true);
		}
		
		//Cube is shaking!!!
		void HandleShakeCubeEvent (Cube c, bool shaking)
		{
			if (shaking) {
				
				if (mCubeStateMachine.Current == Constants.OperandState) {
					
					
					//Generate a random number 0 through 9
					int random = RandomNumber (0, 9);
					
					//If the number is the same, Regenerate until we find a different one
					while (random == mValue) {
						random = RandomNumber (0, 9);
					}
					mValue = random;
					//Refresh the screen by setting this flag
					mNeedDraw = true;
					flagResultcube (); //Flag the result cube to be refreshed
					//Refresh that cube
					Tick ();
					
				} else if (mCubeStateMachine.Current == Constants.OperatorState) {
					
					//Do nothing for now. 
					/*
					mValue = RandomNumber (1, 4);
					//Refresh the screen by setting this flag
					mNeedDraw = true;
					//Refresh that cube
					Tick ();
					*/
				}

			}
			
			
		}

		private void OnTilt (Cube cube, int tiltX, int tiltY, int tiltZ)
		{
      
			int oldRotation = mRotation;
			// If the cube is tilted to a standing position, set the sprite's
			// rotation so that its head is pointing towards that side.
			if (tiltZ == 1) {
				if (tiltY == 2) {
					mRotation = 0;
				} else if (tiltY == 0) {
					mRotation = 2;
				} else if (tiltX == 0) {
					mRotation = 1;
				} else if (tiltX == 2) {
					mRotation = 3;
				}
			} else {
				mRotation = 0;
			}
	
			// If the rotation has changed, raise the flag to force a repaint.
      
			if (mRotation != oldRotation) {
				mNeedDraw = true;
			}
		}

		// This method changes the background color depending on the game state.
		public void CheckNeighbors (bool rowFound)
		{
			if (mCube != null) {
				//mSpriteIndex = 0;
				//mRectColor = new Color (36, 182, 255);

				// ### CubeHelper.FindConnected ###
				// CubeHelper.FindConnected returns an array of all cubes that are
				// neighbors of the given cube, or neighbors of those neighbors, etc.
				// The result includes the given cube, so there should always be at
				// least one element in the array.
				//
				// Here we check to see if the cube is connected to any other cubes,
				// and if it is, we draw the orange background.
				Cube[] connected = CubeHelper.FindConnected (mCube);
				if (connected.Length > 1) {

				} else {

				}
				
				if (mCubeStateMachine.Current == Constants.HintState) {
					//hintController.	
				
					mNeedDraw = true;	
				}
      
			}
		}

		// This method is called every frame by the Tick in S
		public void Tick ()
		{
			
			// If anyone has raised the mNeedDraw flag, redraw the image on the cube.
			if (mNeedDraw) {
				
				Log.Debug ("Cube Wrapper mNeedDraw {0}", this.mCube.UniqueId);
				Log.Debug ("Cube Wrapper - Draw: " + mCubeStateMachine.Current);
				Paint ();
				
				mNeedDraw = false;
						
			}

		}

		public void Paint ()
		{
			
			if (mCube != null) {
				
				Log.Debug ("Paint - Draw: " + mCubeStateMachine.Current);
				
				
				mCubeStateMachine.Tick (1);
				//mCubeStateMachine.Paint (true);	
				
				//mCube.Paint ();	

			
			}		
		}
		
		private int RandomNumber (int min, int max)
		{
			Random random = new Random ();
			return random.Next (min, max); 
		}
		
		/**
		 * flags the result cube to be refreshed
		 */
		private void flagResultcube ()
		{
			Cube[] connected = CubeHelper.FindConnected (mCube);
				
			foreach (Cube cube in connected) {
				CubeWrapper wrapper = (CubeWrapper)cube.userData;
				
				//flag the result cube to be refreshed if it is connected. 
				//Flag the Hint cube also
				if (wrapper.mCubeStateMachine.Current == Constants.ResultState || wrapper.mCubeStateMachine.Current == Constants.HintState) {
					
					wrapper.mNeedDraw = true;	
					
				}
				
			}
			//Flag the main loop to check all the cubes again. 
			mApp.mNeedCheck = true;
			
		}
		
	}
}

