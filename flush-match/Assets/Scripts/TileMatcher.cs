using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class TileMatcher : MonoBehaviour
{
    static public bool AreTilesMatching(Vector2 start, Vector2 end, int[,] board)
    {
        // setting BFS direction
        Vector2[] directions = {Vector2.left, Vector2.right, Vector2.up, Vector2.down};

        /* queue's save data type : tuple
        * position : now position(x, y)
        * bends : how many time bands?
        * lastDirection : last use directions(¡Ö previous direction)
        */
        Queue<(Vector2 position, int bends, Vector2 lastDirection)> queue = new Queue<(Vector2, int, Vector2)>();
        // save visited note
        HashSet<Vector2> visited = new HashSet<Vector2>();

        // initialize start point
        queue.Enqueue((start, 0, Vector2.zero));
        visited.Add(start);

        // BFS logic
        while (queue.Count > 0)
        {
            var (currentPosition, bends, lastDirection) = queue.Dequeue();

            // reached to end position -> can remove tile set
            if (currentPosition == end)
                return true;

            // set limit bends time
            if (bends > 3)
                return false;

            // search 4 direction
            foreach (var direction in directions)
            {
                Vector2 nextPosition = currentPosition + direction;

                // check position is out of bounds of th board
                if (nextPosition.x < 0 || nextPosition.x >= board.GetLength(1) ||
                    nextPosition.y < 0 || nextPosition.y >= board.GetLength(0))
                    continue;

                // skip if already visited node or reached other tile
                if (visited.Contains(nextPosition) || board[(int)nextPosition.y, (int)nextPosition.x] != 0)
                    continue;
                
                // turn in a diffrent direction
                int newBends = bends + (direction != lastDirection && lastDirection != Vector2.zero ? 1 : 0);

                queue.Enqueue((nextPosition, newBends, direction));
                visited.Add(nextPosition);
            }
        }

        // faild search channel
        return false;
    }
}
