using SharpDX;
using SharpDX.Direct2D1;
using System.Collections.Generic;

namespace RetroEngine {
    class NPC : GameObject {
        private PathFinder pathfinder;
        private int height;
        private List<Vector2> wanderPoints;
        private int wanderIndex = 0;
        private List<Vector2> catchPoints;
        private int catchIndex = 0;
        private float movespeed;
        private NPCMode mode = NPCMode.Wander;
        enum NPCMode {
            Wander,
            Catch
        }
        public NPC(Vector3 pos, Bitmap texture, HRMap map, int npcHeight) : base(pos, texture) {
            this.height = npcHeight;
            this.pathfinder = new PathFinder(PathFinder.fromHRMap(map, this.height));
        }

        private bool seeingPlayer() {
            /*
            if (condition) {
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
                if (Vector2.Distance(new Vector2(Position.X, Position.Z), catchPoints[catchIndex]) < 1) {
                    catchIndex++;
                }
                if (catchIndex < catchPoints.Count) {
                    mode = NPCMode.Wander;
                    return think();
                } else return Vector3.Subtract(new Vector3(catchPoints[catchIndex].X, Position.Y, wanderPoints[wanderIndex].Y), Position);
            }
            Debug.Log("Something went terribly wrong...");
            return Vector3.Zero;
        }

    }
}
