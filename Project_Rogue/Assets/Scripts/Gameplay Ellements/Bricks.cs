using UnityEngine;

public class Bricks : MonoBehaviour, ITriggerable
{
    protected virtual void _NotifyTriggered()
    {
        _NotifyDestroy();
    }

    protected void _NotifyDestroy()
    {
        Destroy(gameObject);
    }


    public void OnTriggered()
    {
        _NotifyTriggered();
    }

}