using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Traffic.Dijkstra
{
    public class Node {

        #region [ Properties ]

        public int Id { get; private set; }

        public Dictionary<Node, int> Edges { get; private set; }

        public bool Visited { get; private set; }

        public int MinorCost { get; private set; }

        #endregion

        #region [ Constructor ]

        public Node(int id) {
            this.Id = id;
            this.Edges = new Dictionary<Node, int>();
            this.MinorCost = 0;
        }

        #endregion

        public void AddEdge(Node node, int ponderosity) {
            this.Edges.Add(node, ponderosity);
        }

        public void NodeVisited() {
            this.Visited = true;
        }

        public void SetMinorCost(int cost) {
            this.MinorCost = cost;
        }
    }

    public class Graph {

        #region [ Properties ]

        public Dictionary<int, Node> Nodes { get; private set; }

        #endregion

        #region [ Constructor ]

        public Graph() {
            this.Nodes = new Dictionary<int, Node>();
        }

        #endregion

        public void AddNode(int id) {
            this.Nodes.Add(id, new Node(id));
        }

        public void AddRelationship(int idNode1, int idNode2, int ponderosity) {
            Node node1;
            this.Nodes.TryGetValue(idNode1, out node1);

            Node node2;
            this.Nodes.TryGetValue(idNode2, out node2);

            node1.AddEdge(node2, ponderosity);
        }

        public int ExecuteDijkstra(int source, int destiny) {
            int minorCost = 0;

            Stopwatch watch = Stopwatch.StartNew();

            this.Dijkstra(source, destiny, out minorCost);

            watch.Stop();

            return minorCost;
        }

        private void Dijkstra(int source, int destiny, out int minorCost) {
            Queue<Node> queue = new Queue<Node>();
            Node sourceNode, nodeDestiny, currenteNode;
            bool arrived = false;
            int edgeCost;

            this.Nodes.TryGetValue(source, out sourceNode);
            sourceNode.NodeVisited();
            queue.Enqueue(sourceNode);

            while (queue.Count > 0) {
                currenteNode = queue.Dequeue();

                if (currenteNode.Id == destiny) {
                    arrived = true;
                }

                foreach (Node node in currenteNode.Edges.Keys) {
                    edgeCost = currenteNode.Edges[node];
                    if (!node.Visited || node.MinorCost > currenteNode.MinorCost + edgeCost) {
                        node.SetMinorCost(currenteNode.MinorCost + edgeCost);
                        node.NodeVisited();
                        queue.Enqueue(node);
                    }
                }
            }

            if (arrived) {
                this.Nodes.TryGetValue(destiny, out nodeDestiny);
                minorCost = nodeDestiny.MinorCost;
            } else {
                minorCost = -1;
            }
        }
    }

    public class Program {
        static void Main(string[] args) {
            int nNodes = 0, nRelations = 0;
            int source = 0, destiny = 0;
            List<int> solutions = new List<int>();
            Graph graph;
            do {
                string length = Console.ReadLine();
                nNodes = Convert.ToInt32(length.Split(' ')[0]);
                nRelations = Convert.ToInt32(length.Split(' ')[1]);

                if (nNodes != 0 && nRelations != 0) {
                    graph = new Graph();

                    for (int i = 0; i < nNodes; i++) {
                        graph.AddNode(i + 1);
                    }

                    for (int i = 0; i < nRelations; i++) {
                        string relation = Console.ReadLine();
                        int nodeId1 = Convert.ToInt32(relation.Split(' ')[0]);
                        int nodeId2 = Convert.ToInt32(relation.Split(' ')[1]);
                        int ponderosity = Convert.ToInt32(relation.Split(' ')[2]);
                        graph.AddRelationship(nodeId1, nodeId2, ponderosity);
                    }

                    string path = Console.ReadLine();
                    source = Convert.ToInt32(path.Split(' ')[0]);
                    destiny = Convert.ToInt32(path.Split(' ')[1]);
                    solutions.Add(graph.ExecuteDijkstra(source, destiny));
                }
            } while (nNodes != 0 && nRelations != 0);

            foreach (int solution in solutions)
            {
                Console.WriteLine("{0}", solution);
            }
        }
    }
}
