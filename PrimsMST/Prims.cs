using System;
using System.Collections.Generic;
using System.IO;

namespace PrimsMST {
    class Prims {

        public class Node {
            public string Name;
            public string Parent;
            public int DistanceToParent;
            public Dictionary<string, int> AdjNodes;
            public bool Mapped;

            public Node(string name) {
                DistanceToParent = System.Int32.MaxValue;
                Parent = "";
                Name = name;
                AdjNodes = new Dictionary<string, int>();
                Mapped = false;
            }

        }

        public class Edge {
            public string NodeA;
            public string NodeB;
            public int Distance;

            public Edge(string nodeA, string nodeB, int dist) {
                Distance = dist;
                NodeA = nodeA;
                NodeB = nodeB;
            }
        }

        public static bool DebugMode = true;

        static void Main(string[] args) {

            string inputFileName;
            Dictionary<string, Node> nodes = new Dictionary<string, Node>();

            Console.WriteLine("Please enter the name of input file (ex: PrototypeDists.txt): ");
            if (DebugMode) inputFileName = "PrototypeDists.txt";
            else inputFileName = Console.ReadLine();
            if (!File.Exists(inputFileName)) printMessageAndExit("The file " + inputFileName + " does not exist!");
            else nodes = loadEdgesFromFile(inputFileName);

            string startNodeName = "";
            while (startNodeName != "exit") {
                Console.WriteLine("Please enter the name of the desired starting node or exit to quit: ");
                startNodeName = Console.ReadLine();
                if (startNodeName == "exit") printMessageAndExit("Exit Prims");
                else if (verifyNode(nodes, startNodeName)) mapNodes(nodes, startNodeName);
                else Console.WriteLine("The node " + startNodeName + " does not exist!");
                Console.WriteLine("All Nodes Napped");
                foreach (KeyValuePair<string, Node> entry in nodes) entry.Value.Mapped = false;
            }
        }

        private static void printMessageAndExit(string error) {
            Console.WriteLine(error);
            Console.WriteLine("Press Any Key To Exit");
            Console.ReadKey();
            Environment.Exit(0);
        }

        private static Dictionary<string, Node> loadEdgesFromFile(string edgeFileName) {
            List<Edge> edges = new List<Edge>();
            List<string> nodeList = new List<string>();
            string[] edgeArray = System.IO.File.ReadAllLines(edgeFileName);
            foreach (string edgeString in edgeArray) {
                string[] edgeInfo = edgeString.Split(',');
                Edge currEdge = new Edge(edgeInfo[0], edgeInfo[1], Convert.ToInt32(edgeInfo[2]));
                edges.Add(currEdge);
                if (!nodeList.Contains(currEdge.NodeA)) nodeList.Add(currEdge.NodeA);
                if (!nodeList.Contains(currEdge.NodeB)) nodeList.Add(currEdge.NodeB);
            }
            return defineNodes(nodeList, edges);
        }

        private static Dictionary<string, Node> defineNodes(List<string> nodeList, List<Edge> edges) {
            Dictionary<string, Node> nodes = new Dictionary<string, Node>();
            foreach (string thisNode in nodeList) {
                Node newNode = new Node(thisNode);
                foreach (Edge edge in edges) {
                    if (newNode.Name == edge.NodeA) newNode.AdjNodes.Add(edge.NodeB, edge.Distance);
                    if (newNode.Name == edge.NodeB) newNode.AdjNodes.Add(edge.NodeA, edge.Distance);
                }
                nodes.Add(thisNode, newNode);
            }
            return nodes;
        }

        private static bool verifyNode(Dictionary<string, Node> nodes, string node) {
            if (nodes.ContainsKey(node)) return true;
            else return false;
        }

        private static void mapNodes(Dictionary<string, Node> nodes, string startNodeName) {
            int mappedNodeCount = 0;
            nodes[startNodeName].DistanceToParent = 0;
            while (nodes.Count != mappedNodeCount) {
                string nextNodeName = getMinNodeName(nodes);
                if (mappedNodeCount != 0) Console.WriteLine(nodes[nextNodeName].Parent + " to " + nextNodeName + " w/ Distance of " + nodes[nextNodeName].DistanceToParent + " Miles");
                mappedNodeCount++;
                nodes[nextNodeName].Mapped = true;
                updateAdjNodes(nodes, nextNodeName);
            }
        }

        private static string getMinNodeName(Dictionary<string, Node> nodes) {
            string nextNodeName = "";
            int minDist = System.Int32.MaxValue;
            foreach (KeyValuePair<string, Node> entry in nodes) {
                if (minDist > entry.Value.DistanceToParent && entry.Value.Mapped == false) {
                    minDist = entry.Value.DistanceToParent;
                    nextNodeName = entry.Key;
                }
            }
            return nextNodeName;
        }

        private static void updateAdjNodes(Dictionary<string, Node> nodes, string nextNodeName) {
            foreach (KeyValuePair<string, int> currNode in nodes[nextNodeName].AdjNodes) {
                if (!nodes[currNode.Key].Mapped && nodes[currNode.Key].DistanceToParent > currNode.Value) {
                    nodes[currNode.Key].Parent = nextNodeName;
                    nodes[currNode.Key].DistanceToParent = currNode.Value;
                }
            }
        }

    }
}