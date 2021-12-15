using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Red_Black_Tree_Blake
{
    class Program
    {
        static void Main(string[] args)
        {
            RedBlackTree tree = new RedBlackTree();

            List<int> numbersToAdd = new List<int>();

            //Gets 100 random numbers to put into the tree.
            numbersToAdd = GetRandomData(100, 1, 50);

            foreach (int number in numbersToAdd)
            {
                tree.AddNewNode(number);
            }

            List<int> nodeValues = new List<int>();

            nodeValues = tree.DisplayAllNodes(tree.root);

            foreach (int number in nodeValues)
            {
                Console.WriteLine(number);
            }
        }

        //This is the method used to get the random numbers. It returns a list that contains all of the numbers in it.
        public static List<int> GetRandomData(int numberOfRandomItems, int min, int max)
        {
            List<int> intList = new List<int>();
            Random randomNumber = new Random();

            for (int i = 0; i < numberOfRandomItems; i++)
            {
                intList.Add(randomNumber.Next(min, max));
            }

            return intList;
        }
    }

    //Node class we will be using for the tree structure.
    public class Node
    {
        public int data;
        public string color;

        public Node parent;
        public Node left;
        public Node right;

        public Node(int Data)
        {
            data = Data;
        }

    }
    class RedBlackTree
    {
        //This will help keep track of what the uncle node for the current node is. Will change depending on the location of the parent node.
        public Node uncle;

        //The very top node will be this root node. It will have no parent node attached to it and will always be the color black.
        public Node root;
        public RedBlackTree()
        {

        }

        List<int> allNodeValues = new List<int>();
        public List<int> DisplayAllNodes(Node current)
        {
            //I really wish I would have done this same exact method on my group project. This is much, much easier, can't believe I forgot that I already had to figure this out.
            if (current != null)
            {
                DisplayAllNodes(current.left);
                allNodeValues.Add(current.data);
                DisplayAllNodes(current.right);

            }
            return allNodeValues;
        }
        public void AddNewNode(int data)
        {

            Node newNode = new Node(data);

            //This is the first case for insertion, here we will set the root of the tree as long as no other nodes have been inserted into the tree. This node will also be given the black color here.
            if (root == null)
            {
                root = newNode;
                root.color = "Black";
                return;
            }

            //This will make sure the root will always stay black.
            if (root.color != "Black")
            {
                root.color = "Black";
            }

            #region Regular Insert
            Node tempNode1 = null;
            Node tempNode2 = root;

            while (true)
            {
                //once temp node is null we break the loop.
                if (tempNode2 == null)
                {
                    break;
                }
                tempNode1 = tempNode2;
                if (newNode.data < tempNode2.data)
                {
                    tempNode2 = tempNode2.left;
                }
                else
                {
                    tempNode2 = tempNode2.right;
                }
            }
            newNode.parent = tempNode1;
            if (tempNode1 == null)
            {
                root = newNode;
            }
            else if (newNode.data < tempNode1.data)
            {
                tempNode1.left = newNode;
            }
            else
            {
                tempNode1.right = newNode;
            }
            newNode.left = null;
            newNode.right = null;
            #endregion

            //All new nodes are colored red to start except the root node.
            newNode.color = "Red";

            //If the parent is black there will not be any issues, but if it is red we have to check it and change if needed.
            if (newNode.parent != null && newNode.parent.color == "Red")
            {
                //If the parent is the left node of the grandparent then the uncle is on the right.
                //Which side the uncle is located is dependant on the location of the parent.
                //If the parent is a left node, the uncle is on the right, and if the parent is on the right, the uncle is on the left.
                if (newNode.parent.parent != null)
                {
                    //If the new node is on the left of the parent the uncle is the right node of the grandparent.
                    if (newNode.parent.left == newNode)
                    {
                        uncle = newNode.parent.parent.right;
                    }
                    else
                    {
                        uncle = newNode.parent.parent.left;
                    }
                }

                //As long as the newNode is not the root and the parent color is red we will continue.
                while (true)
                {
                    //as soo as the new node is the root, or the parent is black, we stop.
                    if(newNode == root || newNode.parent.color == "Black")
                    {
                        break;
                    }

                    if (newNode.parent == newNode.parent.parent.left)
                    {
                        if (uncle != null && uncle.color == "Red")
                        {
                            newNode.parent.color = "Black";
                            uncle.color = "Black";
                            newNode.parent.parent.color = "Red";
                            newNode = newNode.parent.parent;
                        }
                        else
                        {
                            if (newNode == newNode.parent.right)
                            {
                                newNode = newNode.parent;
                                RotateNodeLeft(newNode);
                            }
                            newNode.parent.color = "Black";
                            newNode.parent.parent.color = "Red";

                            //We rotate the nodes to the right around the grandparent
                            RotateNodeRight(newNode.parent.parent);
                        }

                    }
                    else
                    {
                        //All of this code is the same as the code above but the uncle is on the opposite side.
                        if (uncle != null && uncle.color == "Black")
                        {
                            newNode.parent.color = "Red";
                            uncle.color = "Red";
                            newNode.parent.parent.color = "Black";
                            newNode = newNode.parent.parent;
                        }
                        else
                        {
                            if (newNode == newNode.parent.left)
                            {
                                newNode = newNode.parent;
                                RotateNodeRight(newNode);
                            }

                            newNode.parent.color = "Black";
                            newNode.parent.parent.color = "Red";
                            RotateNodeLeft(newNode.parent.parent);
                        }
                    }
                }
            }
        }

        #region Rotate
        //If it needs to be rotated, this method will rotate the given node and will switch the node places by rotating to the left.
        private void RotateNodeLeft(Node nodeToRotate)
        {
            //We have to keep this data so that it is not lost when we rotate the nodes. Otherwise, when you would rotate, you would overrite this data and it would be gone.
            Node tempNode = nodeToRotate.right;

            nodeToRotate.right = tempNode.left;
            if (tempNode.left != null)
            {
                tempNode.left.parent = nodeToRotate;
            }

            if (tempNode != null)
            {
                tempNode.parent = nodeToRotate.parent;
            }

            if (nodeToRotate.parent == null)
            {
                root = tempNode;
            }

            //threw an error here, hopefully this works and does not mess up the node structure at all. I just went ahead and made sure that the parent was not null.
            if (nodeToRotate.parent != null)
            {
                if (nodeToRotate == nodeToRotate.parent.left)
                {
                    nodeToRotate.parent.left = tempNode;
                }
                else
                {
                    nodeToRotate.parent.right = tempNode;
                }
            }
            tempNode.left = nodeToRotate;

            if (nodeToRotate != null)
            {
                nodeToRotate.parent = tempNode;
            }

        }

        //This is very similar to rotate node left, but reversed.
        private void RotateNodeRight(Node nodeToRotateAround)
        {
            Node leftNodeTemp = nodeToRotateAround.left;

            nodeToRotateAround.left = leftNodeTemp.right;
            if (leftNodeTemp.right != null)
            {
                leftNodeTemp.right.parent = nodeToRotateAround;
            }
            if (leftNodeTemp != null)
            {
                leftNodeTemp.parent = nodeToRotateAround.parent;
            }
            if (nodeToRotateAround.parent == null)
            {
                root = leftNodeTemp;
            }


            if (nodeToRotateAround.parent != null)
            {
                if (nodeToRotateAround == nodeToRotateAround.parent.right)
                {
                    nodeToRotateAround.parent.right = leftNodeTemp;
                }

                if (nodeToRotateAround == nodeToRotateAround.parent.left)
                {
                    nodeToRotateAround.parent.left = leftNodeTemp;
                }
            }

            leftNodeTemp.right = nodeToRotateAround;
            if (nodeToRotateAround != null)
            {
                nodeToRotateAround.parent = leftNodeTemp;
            }
        }
        #endregion
    }

}