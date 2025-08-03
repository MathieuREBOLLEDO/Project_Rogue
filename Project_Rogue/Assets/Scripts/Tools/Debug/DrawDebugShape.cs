using UnityEngine;

public class DrawDebugShape : MonoBehaviour
{
    [SerializeField] GameObject Shape;

    public void SpawnShape(Vector2 mousePos)
    {
        GameObject obj = GameObject.Instantiate(Shape, (Vector3)mousePos, Quaternion.identity);

        Destroy(obj, 2f);
    }
}
