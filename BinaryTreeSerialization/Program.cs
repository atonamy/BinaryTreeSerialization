using System;
using System.Collections.Generic;
using System.Threading;
using System.Text;

namespace BinaryTreeSerialization
{
	class MainClass
	{
		private static readonly Random mRandom = new Random();

		public static void PopulateNode(Node node, int total_levels, int current_level) {
			//populate binary tree
			if (node == null || current_level > total_levels)
				return;
			
			current_level++;

			if (Convert.ToBoolean (mRandom.Next (0, 2))) {
				node.left = new Node ();
				node.right = new Node ();
				PopulateNode (node.left, total_levels, current_level);
				PopulateNode (node.right, total_levels, current_level);
			} else if (!Convert.ToBoolean (mRandom.Next (0, 2))) {
				node.left = new Node ();
				PopulateNode (node.left, total_levels, current_level);
			} else if (Convert.ToBoolean (mRandom.Next (0, 2))) {
				node.right = new Node ();
				PopulateNode (node.right, total_levels, current_level);
			}
		}

		public static string Serialize(Node node) {
			string result = string.Empty;
			if (node == null)
				return result;

			if(node.left != null)
				result += "\\l";
			if (node.right != null)
				result += "\\r";

			result += node.c.ToString ();

			if (node.left != null) {
				result += "\\<";
				result += Serialize (node.left);
			}
			if (node.right != null) {
				result += "\\>";
				result += Serialize (node.right);
			}
				
			return result;
		}

		public static Node Deserialize(string data) {

			if (string.IsNullOrEmpty (data))
				return null;

			Node result = new Node((char)0);
			Stack<Node> right_parent = new Stack<Node>();
			Node current_node = result;
			int current_index = 0;

			Func<int, string> shift_position = (index) => { return (index < data.Length-1) ? data.Substring (index, 2) : String.Empty; };

			while (current_index < data.Length) {
				string current_position = shift_position(current_index);
				while (current_position.Equals ("\\l") || current_position.Equals ("\\r")) {
				
					if (current_position.Equals ("\\l")) {
						current_index += 2;
						current_node.left = new Node ((char)0);
						current_position = shift_position(current_index);
					}
					if (current_position.Equals ("\\r")) {
						current_index += 2;
						current_node.right = new Node ((char)0);
						right_parent.Push (current_node);
					}

					current_node.c = data[current_index++];
					current_position = shift_position(current_index);

					while (current_position.Equals ("\\<") || current_position.Equals ("\\>")) {
						if (current_position.Equals ("\\<")) {
							current_index += 2;
							current_position = shift_position(current_index);
							if (current_index < data.Length-1 && (current_position.Equals ("\\l") || current_position.Equals ("\\r"))) {
								current_node = current_node.left;
								continue;
							}
							current_node.left.c = data[current_index++];
							current_position = shift_position(current_index);
						}
						if (current_position.Equals ("\\>")) {
							current_index += 2;
							current_position = shift_position(current_index);
							if (current_position.Equals ("\\l") || current_position.Equals ("\\r")) {
								current_node = right_parent.Pop ().right;
								continue;
							}
							right_parent.Pop ().right.c = data[current_index++];
							current_position = shift_position(current_index);
						}
					}
				}

				if (data.Length - current_index == 1) 
					result.c =  data[current_index++];
			}



			return result;
				
		}


		public static bool TestNodes(Node node1, Node node2) {

			if (node1 == null && node2 == null)
				return true;
			else if (node1 == null || node2 == null)
				return false;

			if (node1.c == node2.c)
				Console.WriteLine (string.Format ("Test passed {0} == {1}", node1.c, node2.c));
			else {
				Console.WriteLine (string.Format ("Test not passed {0} != {1}", node1.c, node2.c));
				return false;
			}

			bool result = TestNodes (node1.left, node2.left);
			if (!result)
				return false;
			return TestNodes (node1.right, node2.right);

		}

		public static void Main (string[] args)
		{
			for (int i = 1; i <= 10; i++) {
				Console.WriteLine ("Start testing iteration " + i);
				Thread.Sleep (2000);
				Node serialized_node = new Node ();
				PopulateNode (serialized_node, mRandom.Next (1, 31), 0);
				String result = Serialize (serialized_node);
				Node deserialize_node = Deserialize (result);
				if (TestNodes (serialized_node, deserialize_node))
					Console.WriteLine ("Success!");
				else {
					Console.WriteLine (result);
					Console.WriteLine ("Failed!");
					break;
				}
				Console.WriteLine ("Finished testing iteration " + i + "\n===========================================================================================\n");
				Thread.Sleep (3000);
			}

		}
	}
}
