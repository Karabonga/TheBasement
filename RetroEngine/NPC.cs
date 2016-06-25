using SharpDX;
using SharpDX.Direct2D1;
using System.Collections.Generic;

namespace RetroEngine {
    class NPC : Sprite {
        private PathFinder pathfinder;
        private int height;
        private List<Vector2> wanderPoints;
        private int wanderIndex = 0;
        private LinkedList<Vector2> catchPoints;
        private LinkedList<Vector2> returnPoints;
        private float movespeed;
        private Sprite sprite;
        private NPCMode mode = NPCMode.Wander;
        enum NPCMode {
            Wander,
            Catch,
            ReturnToWander
        }
        public NPC(Vector3 pos, Animation anim, HRMap map, int npcHeight, List<Vector2> wanderPoints) : base(anim, pos) {
            this.height = npcHeight;
            this.pathfinder = new PathFinder(PathFinder.fromHRMap(map, this.height));
            this.wanderPoints = wanderPoints;
        }

        private bool seeingPlayer() {
            /*
            if (somethingmagicalhappens) {
                catchPoints = pathfinder.jumpPointSearch(new Vector2(Position.X, Position.Z), playerPosition);
                catchIndex = 0;
                return true;
            }
            */
            return false;
        }

        public Vector3 think() {
            if (mode.Equals(NPCMode.Wander)) {
                if (Vector2.Distance(new Vector2(Position.X, Position.Z), wanderPoints[wanderIndex]) < 1) {
                    wanderIndex = (wanderIndex + 1) % wanderPoints.Count;
                }
                if (seeingPlayer()) {
                    mode = NPCMode.Catch;
                } else return Vector3.Subtract(new Vector3(wanderPoints[wanderIndex].X, Position.Y, wanderPoints[wanderIndex].Y), Position);

            } else if (mode.Equals(NPCMode.Catch)) {
                seeingPlayer();
                if (Vector2.Distance(new Vector2(Position.X, Position.Z), catchPoints.First.Value) < 1) {
                    catchPoints.RemoveFirst();
                }
                if (catchPoints.Count < 1) {
                    mode = NPCMode.ReturnToWander;
                    int min = 0;
                    float minDist = float.MaxValue;
                    for (int i = 0; i < wanderPoints.Count; i++) {
                        float dist = Vector2.Distance(wanderPoints[i], new Vector2(Position.X, Position.Z));
                        if (dist < minDist) {
                            min = i;
                            minDist = dist;
                        }
                    }
                    returnPoints = pathfinder.jumpPointSearch(new Vector2(Position.X, Position.Z), wanderPoints[min]);
                } else return Vector3.Subtract(new Vector3(catchPoints.First.Value.X, Position.Y, catchPoints.First.Value.Y), Position);
            } else if (mode.Equals(NPCMode.ReturnToWander)) {
                if (seeingPlayer()) {
                    mode = NPCMode.Catch;
                } else {
                    if (Vector2.Distance(new Vector2(Position.X, Position.Z), returnPoints.First.Value) < 1) {
                        returnPoints.RemoveFirst();
                    }
                    if (returnPoints.Count > 0) {
                        return Vector3.Subtract(new Vector3(returnPoints.First.Value.X, Position.Y, returnPoints.First.Value.Y), Position);
                    }
                    mode = NPCMode.Wander;
                    return think();
                }

            }
            Debug.Log("Something went terribly wrong...");
            return Vector3.Zero;
        }

    }
}
