using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using Priority_Queue;
using System.Drawing;
namespace RetroEngine {
    class PathFinder {
        private Bitmap map;
        const int D1 = 10;
        const int D2 = 14;

        public PathFinder(Bitmap m) {
            map = m;
        }
        /*
        static void Main(string[] args) {
            // Display the number of command line arguments:
            PathFinder p = new PathFinder(new Bitmap("output-0.png"));
            List<Vector2> way = p.aStar(new Vector2(16, 16), new Vector2(34, 154));
            Vector2 curr = way.First();
            using (Graphics g = Graphics.FromImage(p.map)) {
                Pen pen = new Pen(System.Drawing.Color.Red);
                foreach (Vector2 v in way) {
                    g.DrawLine(pen, curr.X, curr.Y, v.X, v.Y);
                    curr = v;
                    Console.WriteLine(curr.X + " " + curr.Y);
                }
            }
            p.map.Save("mapwithway.png");
        }
        */
        class Node : IEquatable<Node> {

            public long H;
            public long G;
            public int x;
            public int y;
            public int dx;
            public int dy;
            public Node parent = null;


            public Node(Vector2 pos, long H, long G) {
                x = (int)Math.Round(pos.X);
                y = (int)Math.Round(pos.Y);
                this.H = H;
                this.G = G;
            }


            public Node(int x, int y, long G) {
                this.x = x;
                this.y = y;
                this.G = G;
            }

            public long octileDist(Node other) {// recommended for 8-Way-Grids
                int dx = Math.Abs(this.x - other.x);
                int dy = Math.Abs(this.y - other.y);
                return D1 * (dx + dy) + (D2 - 2 * D1) * Math.Min(dx, dy);
            }

            public long chebychevDist(Node other) {
                int dx = Math.Abs(this.x - other.x);
                int dy = Math.Abs(this.y - other.y);
                return D1 * (dx + dy) + (D1 - 2 * D1) * Math.Min(dx, dy);
            }

            public int manhattanDist(Node other) { // recommended for 4-Way-Grids
                return (Math.Abs(this.x - other.x) + Math.Abs(this.y - other.y)) * 10;
            }

            public int euclidianDist(Node other) { //for Any-Angle Grids
                return (int)Math.Round(Math.Sqrt((this.x - other.x) * (this.x - other.x) + (this.y - other.y) * (this.y - other.y)) * 10);
            }

            public bool Equals(Node other) {
                return (this.x == other.x && this.y == other.y);
            }
        }

        private static Vector3[] sides = {
            new Vector3(1, 1, D2),
            new Vector3(1, 0, D1),
            new Vector3(1, -1, D2),
            new Vector3(0, 1, D1),
            new Vector3(0, -1, D1),
            new Vector3(-1, 1, D2),
            new Vector3(-1, 0, D1),
            new Vector3(-1, -1, D2)

    };
        private bool getWalkable(int x, int y) {
            return !(x < 0 || x >= map.Width || y < 0 || y >= map.Height) && map.GetPixel(x, y).B > 0;
        }

        private List<Node> getNeighbours(Node current) {
            List<Node> result = new List<Node>();
            int x, y;
            foreach (Vector3 v in sides) {
                x = current.x + (int)v.X;
                y = current.y + (int)v.Y;
                if (getWalkable(x, y)) {
                    result.Add(new Node(x, y, current.G + (int)v.Z));
                }
            }
            return result;
        }
        private List<Vector2> aStar(Vector2 p1, Vector2 p2) {
            SimplePriorityQueue<Node> openList = new SimplePriorityQueue<Node>();
            List<Node> closedList = new List<Node>();
            Node startNode = new Node(p1, 0, 0);
            Node goalNode = new Node(p2, 0, 0);
            startNode.H = startNode.octileDist(goalNode);
            openList.Enqueue(startNode, startNode.G + startNode.H);
            while (openList.Count > 0) {
                Node current = openList.Dequeue();
                //pw.setColor(current.x, current.y, Color.GRAY);
                map.SetPixel(current.x, current.y, System.Drawing.Color.Gray);
                closedList.Add(current);
                if (current.Equals(goalNode)) break;
                List<Node> neighbours = getNeighbours(current);
                foreach (Node neighbour in neighbours) {
                    if (closedList.Contains(neighbour)) continue;

                    IEnumerable<Node> en = openList.Where(open => open.Equals(neighbour));
                  
                    if (en.Count() > 0) {
                        Node n = en.First();
                        if (n.G > neighbour.G) {
                            n.parent = current;
                            n.G = neighbour.G;
                            openList.UpdatePriority(n, n.G + n.H);
                        }
                    } else {
                        neighbour.H = neighbour.octileDist(goalNode);
                        neighbour.parent = current;
                        openList.Enqueue(neighbour, neighbour.H + neighbour.G);
                    }
                }

            }
            List<Vector2> result = new List<Vector2>();
            Node node = closedList.Last();
            while (node != null) {
                result.Add(new Vector2(node.x, node.y));
                node = node.parent;
            }
            return result;
        }
    }
}
