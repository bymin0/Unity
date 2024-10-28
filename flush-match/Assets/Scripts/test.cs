using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public int[,] board = new int[,]
    {
        { 0, 0, 0, 0 },
        { 1, 1, 0, 1 },
        { 0, 0, 0, 0 },
        { 0, 1, 1, 0 }
    };

    public Vector2 start = new Vector2(0, 0);
    public Vector2 end = new Vector2(2, 3);
    public GameObject dummyObject;
    public float moveSpeed = 1.0f;

    private List<Vector2> path;

    private void Start()
    {
        path = BFSPath(board, start, end);
        if (path != null)
        {
            foreach (var step in path)
            {
                Debug.Log("Step: " + step);
            }
            StartCoroutine(MoveAlongPath());
        }
        else
        {
            Debug.Log("No path found");
        }
    }

    IEnumerator MoveAlongPath()
    {
        foreach (Vector2 step in path)
        {
            Vector3 targetWorldPosition = new Vector3(step.y, 0, step.x);

            while (Vector3.Distance(dummyObject.transform.position, targetWorldPosition) > 0.01f)
            {
                dummyObject.transform.position = Vector3.MoveTowards(dummyObject.transform.position, targetWorldPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }
        }
    }

    public List<Vector2> BFSPath(int[,] board, Vector2 start, Vector2 end)
    {
        int rows = board.GetLength(0);
        int cols = board.GetLength(1);
        Vector2[] directions = new Vector2[]
        {
            new Vector2(0, -1),
            new Vector2(0, 1),
            new Vector2(-1, 0),
            new Vector2(1, 0),
        };

        Queue<(Vector2 position, List<Vector2> path)> queue = new Queue<(Vector2, List<Vector2>)> ();
        queue.Enqueue((start, new List<Vector2> { start }));
        HashSet<Vector2> visited = new HashSet<Vector2> { start };

        while (queue.Count > 0)
        {
            var (current, path) = queue.Dequeue();

            if (current == end)
                return path;

            foreach (var direction in directions)
            {
                Vector2 newPos = current + direction;

                if (newPos.x >= 0 && newPos.x < rows && newPos.y >= 0 && newPos.y < cols && !visited.Contains(newPos))
                {
                    if (board[(int) newPos.x, (int) newPos.y] == 0 || newPos == end)
                    {
                        List<Vector2> newPath = new List<Vector2>(path) { newPos };

                        queue.Enqueue((newPos, newPath));
                        visited.Add(newPos);
                    }
                }
            }
        }

        return null;
    }
}