// CubeInitialState.cs
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

namespace MathScramble
{
	public class CubeRole
	{
		public String stateName;
		public String stateTransition;
		public int maxVal;
		public int minVal;
		
		public CubeRole (String name, String transition, int min, int max)
		{
			stateName = name;
			stateTransition = transition;
			maxVal = max;
			minVal = min;
		}
		
		/*
		 *  Generate the Operand Value
		 */
		public int GenerateValue ()
		{
			
			if (maxVal != minVal) {
				//Generate new Random Number
				return RandomNumber (minVal, maxVal);	
			} else {
				//Return the min number since they are equal
				return minVal;
			}
		
		}
		
		private int RandomNumber (int min, int max)
		{
			Random random = new Random ();
			return random.Next (min, max); 
		}
	}
}

