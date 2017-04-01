using System;
using System.Collections.Generic;

namespace PrimsMST {
    class Program {

        public class Node {
            public int Parent;
            public int Distance;
            public int Name;
            public Node(int name) {
                Distance = System.Int32.MaxValue; //2. u.key = inf
                Parent = 0; //3. u.pi = nil
                Name = name;
            }
        }
        static void Main(string[] args) {

            //setup
            Dictionary<string, int> Edges = new Dictionary<string, int>();
            Dictionary<string, Node> Nodes = new Dictionary<string, Node>();
            Dictionary<string, Node> UnusedNodes = new Dictionary<string, Node>();
            //define edges
            Edges.Add("12", 930);
            Edges.Add("34", 1135);
            Edges.Add("45", 2064);
            Edges.Add("51", 789);
            Edges.Add("26", 548);
            Edges.Add("63", 1895);
            Edges.Add("56", 926);
            Edges.Add("37", 270);
            Edges.Add("47", 1117);
            Edges.Add("78", 1218);
            Edges.Add("85", 927);
            Edges.Add("68", 506);

            //MST-Prim
            //define nodes
            for (int i = 1; i <= 8; i++) {
                Node newNode = new Node(i);
                Nodes.Add(Convert.ToString(i), newNode); //1: Foreach u in G.V
                UnusedNodes.Add(Convert.ToString(i), newNode); //5. Q = G.V
            }

            //select starting node
            Nodes.TryGetValue("1", out Node currNode);
            Nodes[Convert.ToString(currNode.Name)].Distance = 0; //4. r.key = 0
            while (UnusedNodes.Count != 0) { //6. while Q != null
                UnusedNodes.Remove(Convert.ToString(currNode.Name));
                Dictionary<string, int> currVertices = new Dictionary<string, int>();
                string nodeName = Convert.ToString(currNode.Name);
                foreach (KeyValuePair<string, int> edge in Edges) {
                    if (edge.Key.Contains(nodeName)) currVertices.Add(edge.Key, edge.Value);
                }
                bool minNotFound = true;
                string nextNodeName = "";
                while (minNotFound && currVertices.Count != 0) {
                    int minDist = System.Int32.MaxValue;
                    string minNodeName = "";
                    foreach (KeyValuePair<string, int> vertix in currVertices) { //7. u = Extract-Min(q)
                        if (minDist > vertix.Value) {
                            minDist = vertix.Value;
                            minNodeName = vertix.Key;
                        }
                    }
                    currVertices.Remove(minNodeName);
                    nextNodeName = minNodeName.Replace(nodeName, "");
                    if (minNodeName != "") {
                        if (UnusedNodes.ContainsKey(nextNodeName) || UnusedNodes.ContainsKey(nodeName) || UnusedNodes.Count == 0) {
                            minNotFound = false;
                            Console.Write(minNodeName.Replace(nextNodeName, "") + " to ");
                            Console.Write(nextNodeName + " w/ Distance of " + Edges[minNodeName] + " Miles");
                            Console.WriteLine("");
                            Nodes[nextNodeName].Parent = currNode.Name;
                            Nodes[nextNodeName].Distance = Edges[minNodeName];
                            Edges.Remove(minNodeName);
                            Dictionary<string, int> adjVertices = new Dictionary<string, int>();
                            foreach (KeyValuePair<string, int> edge in Edges) { //8. foreach v in G.Adj[u]
                                if (edge.Key.Contains(nextNodeName)) {
                                    string vertixKey = edge.Key.Replace(nextNodeName, "");
                                    int vertixValue = edge.Value;
                                    if (Nodes.ContainsKey(vertixKey)) { //9. if v in Q and w(u,v) < v.key
                                        if (Nodes[vertixKey].Parent == 0 && Nodes[vertixKey].Distance > Nodes[nextNodeName].Distance) {
                                            Nodes[vertixKey].Parent = Nodes[nextNodeName].Parent; //v.pi = u
                                            Nodes[vertixKey].Distance = vertixValue; //11. v.key = w(u,v)
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (minNotFound) {
                    foreach (KeyValuePair<string, Node> node in UnusedNodes) nextNodeName = node.Key;
                }
                currNode = Nodes[nextNodeName];
            }
            Console.WriteLine("Press Any Key To Exit");
            Console.ReadKey();
        } //end of main
    } //end of class
}// end of namespace