using UnityEngine;
using System.Collections.Generic;

public class BrickSpawner : MonoBehaviour
{
    [SerializeField] private GameObject brickPrefab;
    [SerializeField] private BrickDataBase database;

   // private void OnEnable()
   // {
   //     BrickEvents.OnSpawnBricks += SpawnBricks;
   // }
   //
   // private void OnDisable()
   // {
   //     BrickEvents.OnSpawnBricks -= SpawnBricks;
   // }

    public void SpawnBricks(BrickData data)
    {
        Vector2 cellSize = GridService.Instance.CellSize;

        float margin = 0.9f;
        Vector3 scale = new Vector3(
            cellSize.x * margin,
            cellSize.y * margin,
            1f
        );


        //Vector3 position = GridService.Instance.GetCellCenter(data.Column, data.Row);
        data.SetBrickType(GetRandomType());

        print("Data type : " + data.Type);

        GameObject go = Instantiate(brickPrefab, data.Position, data.Rotation, data.Parent);
        go.transform.localScale = data.Scale;

        Brick brick = go.GetComponent<Brick>();
        brick.Initialize(data, database);
        
    }

    private BrickType GetRandomType()
    {
        int rand = Random.Range(0, 100);

        if (rand < 70) return BrickType.Basic;
        if (rand < 90) return BrickType.Strong;
        return BrickType.Unbreakable;
    }
}