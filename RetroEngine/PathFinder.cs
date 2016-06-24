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
            LinkedList<Vector2> way = p.jumpPointSearch(new Vector2(16, 16), new Vector2(34, 154));
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
        public LinkedList<Vector2> aStar(Vector2 p1, Vector2 p2) {
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
            LinkedList<Vector2> result = new LinkedList<Vector2>();
            Node node = closedList.Last();
            while (node != null) {
                result.AddFirst(new Vector2(node.x, node.y));
                node = node.parent;
            }
            return result;
        }
        public LinkedList<Vector2> jumpPointSearch(Vector2 p1, Vector2 p2) {
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
                List<Node> neighbours = identifySuccessors(current, startNode, goalNode);
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
            LinkedList<Vector2> result = new LinkedList<Vector2>();
            Node node = closedList.Last();
            while (node != null) {
                result.AddFirst(new Vector2(node.x, node.y));
                node = node.parent;
            }
            return result;
        }
        private List<Node> getPrunedNeighbours(Node current, Node startNode) {
            if (current.Equals(startNode)) {
                return getNeighbours(current);
            }
            int dx = current.dx;
            int dy = current.dy;
            List<Node> result = new List<Node>();
            if (dx == 0) { //vertically
                           //get natural neighbour
                if (getWalkable(current.x, current.y + dy)) result.Add(new Node(current.x, current.y + dy, current.G + D1));
                //get forced neighbours
                if (!getWalkable(current.x + 1, current.y) && getWalkable(current.x + 1, current.y + dy))
                    result.Add(new Node(current.x + 1, current.y + dy, current.G + D2));
                if (!getWalkable(current.x - 1, current.y) && getWalkable(current.x - 1, current.y + dy))
                    result.Add(new Node(current.x - 1, current.y + dy, current.G + D2));
            } else if (dy == 0) { //horizontally
                                  //get natural neighbour
                if (getWalkable(current.x + dx, current.y)) result.Add(new Node(current.x + dx, current.y, current.G + D1));
                //get forced neighbours
                if (!getWalkable(current.x, current.y + 1) && getWalkable(current.x + dx, current.y + 1))
                    result.Add(new Node(current.x + dx, current.y + 1, current.G + D2));
                if (!getWalkable(current.x, current.y - 1) && getWalkable(current.x + dx, current.y - 1))
                    result.Add(new Node(current.x + dx, current.y - 1, current.G + D2));
            } else { //diagonal
                     //get natural neighbours
                if (getWalkable(current.x + dx, current.y)) result.Add(new Node(current.x + dx, current.y, current.G + D1));
                if (getWalkable(current.x, current.y + dy)) result.Add(new Node(current.x, current.y + dy, current.G + D1));
                if (getWalkable(current.x + dx, current.y + dy))
                    result.Add(new Node(current.x + dx, current.y + dy, current.G + D2));
                //get forced neighbours
                if (!getWalkable(current.x, current.y - dy) && getWalkable(current.x + dx, current.y - dy))
                    result.Add(new Node(current.x + dx, current.y - dy, current.G + D2));
                if (!getWalkable(current.x - dx, current.y) && getWalkable(current.x - dx, current.y + dy))
                    result.Add(new Node(current.x - dx, current.y + dy, current.G + D2));
            }
            return result;
        }
        private bool hasForcedNeighbour(Node current) {
            int dx = current.dx;
            int dy = current.dy;
            if (dx == 0) {
                if (!getWalkable(current.x + 1, current.y) && getWalkable(current.x + 1, current.y + dy)) return true;
                if (!getWalkable(current.x - 1, current.y) && getWalkable(current.x - 1, current.y + dy)) return true;
            } else if (dy == 0) {
                if (!getWalkable(current.x, current.y + 1) && getWalkable(current.x + dx, current.y + 1)) return true;
                if (!getWalkable(current.x, current.y - 1) && getWalkable(current.x + dx, current.y - 1)) return true;
            } else {
                if (!getWalkable(current.x, current.y - dy) && getWalkable(current.x + dx, current.y - dy))
                    return true;
                if (!getWalkable(current.x - dx, current.y) && getWalkable(current.x - dx, current.y + dy))
                    return true;
            }
            return false;
        }
        private Node jump(Node initial, int dx, int dy, Node goal) {
            map.SetPixel(initial.x, initial.y, System.Drawing.Color.LightGray);
            Node next = new Node(initial.x + dx, initial.y + dy, (dx == 0 || dy == 0) ? initial.G + D1 : initial.G + D2);
            next.dx = dx;
            next.dy = dy;

            if (!getWalkable(next.x, next.y)) return null;
            if (next.Equals(goal) || hasForcedNeighbour(next)) return next;
            if (dx != 0 && dy != 0) {
                if (jump(next, dx, 0, goal) != null) return next;
                if (jump(next, 0, dy, goal) != null) return next;
            }
            return jump(next, dx, dy, goal);

            /*
            while (getWalkable(next.x, next.y)) {
                if (next.samePos(goal) || hasForcedNeighbour(next)) return next;
                if (dx != 0 && dy != 0) {
                    if (jump(next, dx, 0, goal) != null) return next;
                    if (jump(next, 0, dy, goal) != null) return next;
                }
                next = new Node(next.x + dx, next.y + dy, (dx == 0 || dy == 0) ? next.G + D1 : next.G + D2);
            }
            return null;*/
        }
        private List<Node> identifySuccessors(Node current, Node start, Node goal) {
            List<Node> neighbours = getPrunedNeighbours(current, start);
            for (int i = 0; i < neighbours.Count; i++) {
                neighbours[i] = jump(current, neighbours[i].x - current.x, neighbours[i].y - current.y, goal);
                if (neighbours[i] != null) neighbours[i].parent = current;

            }
            neighbours.RemoveAll(elem => elem == null);
            return neighbours;
        }
    }
}
