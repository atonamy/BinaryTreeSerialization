using System;

namespace BinaryTreeSerialization
{
	public class Node
	{
		private static readonly Random mRandom = new Random ();
		public char c;
		public Node left;
		public Node right;

		public Node ()
		{
			int index = mRandom.Next(0, 26); 
			c = /*mRandom.Next(0, 2) == 0 ? (char)('a' + index) :*/ (char)('A' + index);
			left = null;
			right = null;
		}

		public Node (char c)
		{
			this.c = c;
			left = null;
			right = null;
		}
	}
}

