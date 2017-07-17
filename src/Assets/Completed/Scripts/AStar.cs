using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Completed
{

    public class AStar
    {
        private class Node
        {
            public Vector3 position;
            public Node parent;
            public float G;
            public float H;
            public float cost;

            //sets G and H (the two AStar vars)
            public Node(Vector3 position, Vector3 goal, Node parent)
            {
                this.position = position;
                this.parent = parent;
                this.cost = Locations.cost[(int)position.x,(int)position.y];

                if (this.parent == null)
                    this.G = 0;
                else {
                    //cost function
                    this.G = this.parent.G + this.cost * (Math.Abs(this.parent.position.x - position.x) + Math.Abs(this.parent.position.y - position.y));
                }

                H = Math.Abs(goal.x - position.x) + Math.Abs(goal.y - position.y);
            }

            public float GetF()
            {
                return G + H;
            }
        };

        private List<Node> open;
        private List<Node> closed;

        public AStar()
        {
            open = new List<Node>();
            closed = new List<Node>();
        }

        public List<Vector3> GetPath(Vector3 start, Vector3 goal)
        {
            Node first = new Node(start, goal, null);

            open.Clear();
            closed.Clear();

            open.Add(first);
            while (open.Count > 0)
            {
                Node current = null;
                float minF = float.MaxValue;
                for (int i = 0; i < open.Count; i++)
                {
                    float F = ((Node)open[i]).GetF(); //finds F for next node in list
                    if (F < minF) //if this F is better:
                    {
                        current = (Node)open[i]; 
                        minF = F; //update respective vars
                    }
                }

                closed.Add(current);
                open.Remove(current);

                // if target reached:
                if (current.position == goal)
                    return GetPathPositions(current);
                //else if (closed.Count > 500)
                //    break;

                // Check all four adjacent nodes
                Vector3 adjacent;

                adjacent = new Vector3(current.position.x - 1, current.position.y);
                if (IsInsideGrid(adjacent)) CheckAdjacentNode(new Node(adjacent, goal, current));     

                adjacent = new Vector3(current.position.x + 1, current.position.y);
                if (IsInsideGrid(adjacent)) CheckAdjacentNode(new Node(adjacent, goal, current));
                

                adjacent = new Vector3(current.position.x, current.position.y - 1);
                if (IsInsideGrid(adjacent)) CheckAdjacentNode(new Node(adjacent, goal, current));
                

                adjacent = new Vector3(current.position.x, current.position.y + 1);
                if (IsInsideGrid(adjacent)) CheckAdjacentNode(new Node(adjacent, goal, current));
                
            }

            return GetPathPositions(closed[closed.Count - 1]);
        }

        void CheckAdjacentNode(Node adjNode)
        {
            bool addNode = true;

            for (int i = 0; i < open.Count; i++)
            {
                if (open[i].position == adjNode.position)
                {
                    if (adjNode.G < open[i].G)
                    {
                        open[i] = adjNode;
                    }

                    addNode = false;
                    break;
                }
            }
            for(int i=0; i<closed.Count; i++){
                Node node = closed[i];
                if (adjNode.position == node.position)
                {
                    addNode = false;
                    break;
                }
            }

            if (addNode) open.Add(adjNode);

        }

        bool IsInsideGrid(Vector3 position)
        {
            int x = (int)position.x;
            int y = (int)position.y;
            if(x >= 0 && x < GetDimension() && y >= 0 && y < GetDimension()) return true;
            else return false;
        }

        List<Vector3> GetPathPositions(Node node)
        {
            List<Vector3> positions = new List<Vector3>();

            while (node.parent != null)
            {
                positions.Insert(0, node.position);
                node = node.parent;
            }

            return positions;
        }

        int GetDimension(){
            return BoardManager.dimension;
        }
    }
}
