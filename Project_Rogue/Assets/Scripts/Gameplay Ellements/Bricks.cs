using UnityEngine;

public class Bricks : MonoBehaviour, ITriggerable
{
    public void OnTriggered()
    {
        Destroy(gameObject);
        //throw new System.NotImplementedException();
    }

}