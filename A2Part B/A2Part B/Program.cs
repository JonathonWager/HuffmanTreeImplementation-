using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A2Part_B
{
    internal class Program
    {
        public class PriorityClass : IComparable
        {
            private int priorityValue;       // Ordered on priorityValue
            private String name;

            // Constructor

            public PriorityClass(int priority, String name)
            {
                this.name = name;
                priorityValue = priority;
            }

            // CompareTo (inherited from IComparable)
            // Returns >0 if the current item is greater than obj (null or otherwise)
            // Returns 0  if the current item is equal to obj (of PriorityClass)
            // Returns <0 if the current item is less than obj (of PriorityClass)

            public int CompareTo(Object obj)
            {
                if (obj != null)
                {
                    PriorityClass other = (PriorityClass)obj;   // Explicit cast
                    if (other != null)
                        return priorityValue - other.priorityValue;
                    else
                        return 1;
                }
                else
                    return 1;
            }

            // ToString (overridden from Object class)
            // Returns a string represent of an object of PriorityClass

            public override string ToString()
            {
                return name + " with priority " + priorityValue;
            }
        }

        public interface IContainer<T>
        {
            void MakeEmpty();  // Reset an instance to empty
            bool Empty();      // Test if an instance is empty
            int Size();        // Return the number of items in an instance
        }

        //-----------------------------------------------------------------------------

        public interface IPriorityQueue<T> : IContainer<T> where T : IComparable
        {
            void Insert(T item);  // Insert an item to a priority queue
            T Remove();        // Remove the item with the highest priority
            T Front();            // Return the item with the highest priority
        }
        // Priority Queue
        // Implementation:  Binary heap
         public class Node : IComparable
        {
            public char Character { get; set; }
            public int Frequency { get; set; }
            public Node Left { get; set; }
            public Node Right { get; set; }
            public Node(char character, int frequency, Node left, Node right)
            {
                Character = character;
                Frequency = frequency;
                Left = left;
                Right = right;
            }
            // 3 marks
            public int CompareTo(Object obj)
            {
                if(obj != null)
                {
                    Node other = (Node)obj;
                    if(this.Frequency < other.Frequency)
                    {
                        return 1;
                    }
                    else
                    {
                        return -1;
                    }
                }
                else
                {
                    return 0;
                }
            }
        }
        public class PriorityQueue<T> : IPriorityQueue<T> where T : IComparable
        {
            private T[] A;         // Linear array of items (Generic)
            private int capacity;  // Maximum number of items in a priority queue
            private int count;     // Number of items in the priority queue

            // Constructor 1
            // Create an empty priority queue
            // Time complexity:  O(1)

            public PriorityQueue()
            {
                capacity = 3;
                A = new T[capacity + 1];  // Indexing begins at 1
                MakeEmpty();
            }

            // Constructor 2
            // Create a priority queue from an array of items
            // Time complexity:  O(n)

            public PriorityQueue(T[] inputArray)
            {
                int i;

                count = capacity = inputArray.Length;
                A = new T[capacity + 1];

                for (i = 0; i < capacity; i++)
                {
                    A[i + 1] = inputArray[i];
                }

                BuildHeap();
            }

            // MakeEmpty
            // Reset a priority queue to empty
            // Time complexity:  O(1)

            public void MakeEmpty()
            {
                count = 0;
            }

            // Empty
            // Return true if the priority is empty; false otherwise
            // Time complexity:  O(1)

            public bool Empty()
            {
                return count == 0;
            }

            // Size
            // Return the number of items in the priority queue
            // Time complexity:  O(1)

            public int Size()
            {
                return count;
            }

            // DoubleCapacity
            // Doubles the capacity of the priority queue
            // Time complexity:  O(n)

            private void DoubleCapacity()
            {
                T[] oldA = A;

                capacity = 2 * capacity;
                A = new T[capacity + 1];
                for (int i = 1; i <= count; i++)
                {
                    A[i] = oldA[i];
                }
            }

            // PercolateUp
            // Percolate up an item from position i in a priority queue
            // Time complexity:  O(log n)

            private void PercolateUp(int i)
            {
                int child = i, parent;

                // As long as child is not the root (i.e. has a parent)
                while (child > 1)
                {
                    parent = child / 2;

                    if (A[child].CompareTo(A[parent]) > 0)
                    // If child has a higher priority than parent
                    {
                        // Swap parent and child
                        T item = A[child];
                        A[child] = A[parent];
                        A[parent] = item;
                        child = parent;  // Move up child index to parent index
                    }
                    else
                        // Item is in its proper position
                        return;
                }
            }

            // Insert
            // Insert an item into the priority queue
            // Time complexity:  O(log n)

            public void Insert(T item)
            {
                if (count == capacity)
                {
                    DoubleCapacity();
                }
                A[++count] = item;      // Place item at the next available position
                PercolateUp(count);
            }

            // PercolateDown
            // Percolate down an item from position i in a priority queue
            // Time complexity:  O(log n)

            private void PercolateDown(int i)
            {
                int parent = i, child;

                // while parent has at least one child
                while (2 * parent <= count)
                {             
                    // Select the child with the highest priority
                    child = 2 * parent;    // Left child index
                    if (child < count)  // Right child also exists
                        if (A[child + 1].CompareTo(A[child]) > 0)
                            // Right child has a higher priority than left child
                            child++;

                    // If child has a higher priority than parent
                    if (A[child].CompareTo(A[parent]) > 0)
                    {
                        // Swap parent and child
                        T item = A[child];
                        A[child] = A[parent];
                        A[parent] = item;
                        parent = child;  // Move down parent index to child index
                    }
                    else
                        // Item is in its proper place
                        return;
                }
            }

            // Remove
            // Remove (if possible) the item with the highest priority
            // Otherwise throw an exception
            // Time complexity:  O(log n)

            public T Remove()
            {
                if (Empty())
                    throw new InvalidOperationException("Priority queue is empty");
                else
                {
                    // Remove item with highest priority (root) and
                    // replace it with the last item

                    T rItem = A[1];
                    A[1] = A[count--];

                    // Percolate down the new root item
                    PercolateDown(1);

                    return rItem;
                }
            }

            // Front
            // Return (if possible) the item with the highest priority
            // Otherwise throw an exception
            // Time complexity:  O(1)

            public T Front()
            {
                if (Empty())
                    throw new InvalidOperationException("Priority queue is empty");
                else
                    return A[1];  // Return the root item (highest priority)
            }

            // BuildHeap
            // Create a binary heap from an arbitrary array
            // Time complexity:  O(n)

            private void BuildHeap()
            {
                int i;

                // Percolate down from the last parent to the root (first parent)
                for (i = count / 2; i >= 1; i--)
                {
                    PercolateDown(i);
                }
            }

            // HeapSort
            // Sort and return inputArray
            // Time complexity:  O(n log n)

            public void HeapSort(T[] inputArray)
            {
                int i;

                capacity = count = inputArray.Length;

                // Copy input array to A (indexed from 1)
                for (i = capacity - 1; i >= 0; i--)
                {
                    A[i + 1] = inputArray[i];
                }

                // Create a binary heap
                BuildHeap();

                // Remove the next item and place it into the input (output) array
                for (i = 0; i < capacity; i++)
                {
                    inputArray[i] = Front();
                    Remove();
                }
            }
        }
       
        class Huffman
        {
            private Node HT; // Huffman tree to create codes and decode text
            private Dictionary<char, string> D = new Dictionary<char, string>();// Dictionary to encode text
                                                // Constructor
            public Huffman(string S)
            {
                Build(AnalyzeText(S));
            }
            // 8 marks
            // Return the frequency of each character in the given text (invoked by Huffman)
            public int[] AnalyzeText(string S)
            {
                int[] freqStorage = new int[53];
                for (int i = 0; i < S.Length; i++)
                {
                    if ((int)S[i] == 32)
                    {
                        freqStorage[52]++;
                    }
                    else
                    {
                        if ((int)S[i] >= 97)
                        {
                            freqStorage[(int)S[i] - 71]++;
                        }
                        else
                        {
                            freqStorage[(int)S[i] - 65]++;
                        }
                    }
                }
                return freqStorage;
            }
            // 16 marks
            // Build a Huffman tree based on the character frequencies greater than 0 (invoked by Huffman)
            private void Build(int[] F)
            {
                PriorityQueue<Node> PQ = new PriorityQueue<Node>();
                for (int i = 0; i < F.Length; i++)
                {
                    if (F[i] != 0)
                    {
                        if (i == 52)
                        {
                            PQ.Insert(new Node((char)(32), F[i], null, null));
                        }
                        else
                        {
                            if (i < 26)
                            {
                                PQ.Insert(new Node((char)(i + 65), F[i], null, null));
                            }
                            else
                            {
                                PQ.Insert(new Node((char)(i + 71), F[i], null, null));
                            }
                        }
                    }
                }
                Node lowest1;
                Node lowest2;
                while (PQ.Size() > 1)
                {

                   lowest1 = PQ.Remove();
                   lowest2 = PQ.Remove();
                   PQ.Insert(new Node((char)0, lowest1.Frequency + lowest2.Frequency, lowest1, lowest2));
                }
                HT = PQ.Remove();
            }
            // 12 marks
            // Create the code of 0s and 1s for each character by traversing the Huffman tree (invoked by Huffman)
            // Store the codes in Dictionary D using the char as the key
            public void CreateCodes()
            {
                CreateCodes(HT, "");
            }
            private void CreateCodes(Node HuffTree, string bits)
            {
                Node curr = HuffTree;
                //traversing through the Binary Tree
                if (curr.Right != null && curr.Left != null)
                {
                    CreateCodes(curr.Right, bits + "0");
                    CreateCodes(curr.Left, bits + "1");
                }
                else
                {                 
                    D.Add(curr.Character, bits);
                    Console.WriteLine(curr.Character + " Code " + bits);
                }
            }
            // 8 marks
            // Encode the given text and return a string of 0s and 1s
            public string Encode(string S)
            {
                return "";
            }
            // 8 marks
            // Decode the given string of 0s and 1s and return the original text
            public string Decode(string S)
            {
                return "";
            }
            public void PrintOrder()
            {
                PrintOrder(HT, 0);
              
            }
            private void PrintOrder(Node root, int indent)
            {

                if (root != null)
                {
                    PrintOrder(root.Right, indent + 5);
                    if (root.Character != (char)0)
                    {
                        Console.WriteLine(new String(' ', indent) + root.Frequency + root.Character);
                    }
                    else
                    {
                        Console.WriteLine(new String(' ', indent) + root.Frequency);
                    }

                    PrintOrder(root.Left, indent + 5);
                }
            }
        }
        static void Main(string[] args)
        {
            Huffman test = new Huffman("for each character by traversing the Huffman tree and return the original text  Decode the given string of Return the frequency of each character in the given text");
            test.PrintOrder();
            test.CreateCodes();
            //Console.WriteLine((int)'A');
            Console.ReadLine();
        }
    }
}
