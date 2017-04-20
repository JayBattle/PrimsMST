using System;
using System.Collections.Generic;

namespace PrimsMST {
    class Prims {
        //comments added via request!

        public class Node {
            public string Name;
            public string Parent;
            public int DistanceToParent;
            public Dictionary<string, int> Vertices;
            public bool Mapped;

            public Node(string name) {
                DistanceToParent = System.Int32.MaxValue; //2. u.key = inf
                Parent = ""; //3. u.pi = nil
                Name = name;
                Vertices = new Dictionary<string, int>();
                Mapped = false; //5. Q = G.V
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

        static void Main(string[] args) {

            //setup
            string edgeFileName = "PrototypeDists.txt";
            int mappedNodeCount = 0;

            //used in setup only
            List<Edge> edgeList = new List<Edge>();
            List<string> nodeList = new List<string>();

            Dictionary<string, Node> nodes = new Dictionary<string, Node>();

            //define edges
            string[] edgeArray = System.IO.File.ReadAllLines(edgeFileName);
            foreach (string edgeString in edgeArray) {
                string[] edgeInfo = edgeString.Split(','); //replace with regex
                Edge currEdge = new Edge(edgeInfo[0], edgeInfo[1], Convert.ToInt32(edgeInfo[2]));
                edgeList.Add(currEdge);
                if (!nodeList.Contains(currEdge.NodeA)) nodeList.Add(currEdge.NodeA);
                if (!nodeList.Contains(currEdge.NodeB)) nodeList.Add(currEdge.NodeB);
            }

            //MST-Prim
            //define nodes
            foreach (string thisNode in nodeList) {
                Node newNode = new Node(thisNode);
                foreach (Edge edge in edgeList) {
                    if (newNode.Name == edge.NodeA) newNode.Vertices.Add(edge.NodeB, edge.Distance);
                    if (newNode.Name == edge.NodeB) newNode.Vertices.Add(edge.NodeA, edge.Distance);
                }
                nodes.Add(thisNode, newNode); //1: Foreach u in G.V
            }

            //select starting node
            nodes.TryGetValue("8", out Node currNode);
            nodes[currNode.Name].DistanceToParent = 0; //4. r.key = 0

            //Main Loop
            while (nodes.Count != mappedNodeCount) { //6. while Q != null
                string nextNodeName = "";
                int minDist = System.Int32.MaxValue;
                foreach (KeyValuePair<string, Node> entry in nodes) { //7. u = Extract-Min(q)     
                    if (minDist > entry.Value.DistanceToParent && entry.Value.Mapped == false) {
                        minDist = entry.Value.DistanceToParent;
                        nextNodeName = entry.Key;
                    }
                }
                if (mappedNodeCount != 0) Console.WriteLine(nodes[nextNodeName].Parent + " to " + nextNodeName + " w/ Distance of " + minDist + " Miles");
                mappedNodeCount++;
                nodes[nextNodeName].Mapped = true;
                foreach (KeyValuePair<string, int> vertex in nodes[nextNodeName].Vertices) { //8. foreach v in G.Adj[u]
                    if (!nodes[vertex.Key].Mapped && nodes[vertex.Key].DistanceToParent > vertex.Value) { //9. if v in Q and w(u,v) < v.key
                        nodes[vertex.Key].Parent = nextNodeName; //v.pi = u //was currNode?
                        nodes[vertex.Key].DistanceToParent = vertex.Value; //11. v.key = w(u,v)
                    }
                }
            }
            Console.WriteLine("Press Any Key To Exit");
            Console.ReadKey();

        } //end of main
    } //end of class
}//end of namespace