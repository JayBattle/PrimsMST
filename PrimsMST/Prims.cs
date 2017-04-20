using System;
using System.Collections.Generic;

namespace PrimsMST {
    class Prims {

        public class Node {
            public string Name;
            public string Parent;
            public int DistanceToParent;
            public Dictionary<string, int> Vertices;
            public bool Mapped;

            public Node(string name) {
                DistanceToParent = System.Int32.MaxValue;
                Parent = "";
                Name = name;
                Vertices = new Dictionary<string, int>();
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

        public static List<Edge> edgeList = new List<Edge>();
        public static List<string> nodeList = new List<string>();
        public static Dictionary<string, Node> nodes = new Dictionary<string, Node>();

        static void Main(string[] args) {

            loadEdges("PrototypeDists.txt");
            defineNodes();
            int mappedNodeCount = 0;
            nodes.TryGetValue("1", out Node currNode);
            nodes[currNode.Name].DistanceToParent = 0;

            while (nodes.Count != mappedNodeCount) {
                string nextNodeName = getMinNodeName();
                if (mappedNodeCount != 0) Console.WriteLine(nodes[nextNodeName].Parent + " to " + nextNodeName + " w/ Distance of " + nodes[nextNodeName].DistanceToParent + " Miles");
                mappedNodeCount++;
                nodes[nextNodeName].Mapped = true;
                updateVertcies(nextNodeName);
            }
            Console.WriteLine("Press Any Key To Exit");
            Console.ReadKey();

        }

        public static void loadEdges(string edgeFileName) {
            string[] edgeArray = System.IO.File.ReadAllLines(edgeFileName);
            foreach (string edgeString in edgeArray) {
                string[] edgeInfo = edgeString.Split(',');
                Edge currEdge = new Edge(edgeInfo[0], edgeInfo[1], Convert.ToInt32(edgeInfo[2]));
                edgeList.Add(currEdge);
                if (!nodeList.Contains(currEdge.NodeA)) nodeList.Add(currEdge.NodeA);
                if (!nodeList.Contains(currEdge.NodeB)) nodeList.Add(currEdge.NodeB);
            }
        }

        public static void defineNodes() {
            foreach (string thisNode in nodeList) {
                Node newNode = new Node(thisNode);
                foreach (Edge edge in edgeList) {
                    if (newNode.Name == edge.NodeA) newNode.Vertices.Add(edge.NodeB, edge.Distance);
                    if (newNode.Name == edge.NodeB) newNode.Vertices.Add(edge.NodeA, edge.Distance);
                }
                nodes.Add(thisNode, newNode);
            }
        }

        public static string getMinNodeName() {
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

        public static void updateVertcies(string nextNodeName) {
            foreach (KeyValuePair<string, int> vertex in nodes[nextNodeName].Vertices) {
                if (!nodes[vertex.Key].Mapped && nodes[vertex.Key].DistanceToParent > vertex.Value) {
                    nodes[vertex.Key].Parent = nextNodeName;
                    nodes[vertex.Key].DistanceToParent = vertex.Value;
                }
            }
        }

    }
}