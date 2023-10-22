using UnityEngine;

public class CircleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject circlePrefab;

    private float minX, maxX, minY, maxY;
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;

        maxX = GetScreenResInWorldPoint().x - 1;
        minX = -GetScreenResInWorldPoint().x + 1;

        maxY = GetScreenResInWorldPoint().y - 1;
        minY = -GetScreenResInWorldPoint().y + 1;

        SpawnRandomCircles();
    }
    public void SpawnRandomCircles()
    {
        for (int i = 0; i < Random.Range(5, 11); i++)
        {
            Vector2 randomPos = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
            Instantiate(circlePrefab, randomPos, Quaternion.identity);
        }
    }
    public Vector3 GetScreenResInWorldPoint()
    {
        return cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
    }
}