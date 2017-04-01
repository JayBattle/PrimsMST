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
            nodes.TryGetValue("1", out Node currNode);
            nodes[currNode.Name].DistanceToParent = 0; //4. r.key = 0


            //Main Loop
            while (nodes.Count != mappedNodeCount) { //6. while Q != null
                nodes[currNode.Name].Mapped = true;
                mappedNodeCount++;
                bool minNotFound = true;
                string nextNodeName = "";
                while (minNotFound && currNode.Vertices.Count != 0) {
                    int minDist = System.Int32.MaxValue;
                    foreach (KeyValuePair<string, int> vertex in currNode.Vertices) { //7. u = Extract-Min(q)
                        if (minDist > vertex.Value) {
                            minDist = vertex.Value;
                            nextNodeName = vertex.Key;
                        }
                    }
                    currNode.Vertices.Remove(nextNodeName);
                    if (nextNodeName != "") {
                        if (!nodes[nextNodeName].Mapped || mappedNodeCount == nodes.Count) {
                            minNotFound = false;
                            Console.Write(currNode.Name + " to ");
                            Console.Write(nextNodeName + " w/ Distance of " + minDist + " Miles");
                            Console.WriteLine("");
                            nodes[nextNodeName].Parent = currNode.Name;
                            nodes[nextNodeName].DistanceToParent = minDist;
                            Dictionary<string, int> adjVertices = new Dictionary<string, int>();
                            foreach (KeyValuePair<string, int> vertex in nodes[currNode.Name].Vertices) { //8. foreach v in G.Adj[u]
                                if (nodes.ContainsKey(vertex.Key)) { //9. if v in Q and w(u,v) < v.key
                                    if (nodes[vertex.Key].Parent == "" && nodes[vertex.Key].DistanceToParent > nodes[nextNodeName].DistanceToParent) {
                                        nodes[vertex.Key].Parent = vertex.Key; //v.pi = u //was currNode?
                                        nodes[vertex.Key].DistanceToParent = vertex.Value; //11. v.key = w(u,v)
                                    }
                                }
                            }
                        }
                    }
                }
                if (minNotFound) {
                    foreach (KeyValuePair<string, Node> node in nodes) if (!node.Value.Mapped) nextNodeName = node.Key;
                }
                currNode = nodes[nextNodeName];
            }
            Console.WriteLine("Press Any Key To Exit");
            Console.ReadKey();

        } //end of main
    } //end of class
}//end of namespace