using System.Collections;
using UnityEngine;

public interface IBallMovement
{
    void Move();

    void SetDirection(Vector2 direction);

    void ResetPosition();
    void InitPosition();
}
