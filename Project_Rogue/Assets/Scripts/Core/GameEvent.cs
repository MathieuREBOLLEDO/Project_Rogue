using UnityEngine;

public class GameEvent
{
    public GameEventType type;
    public GameObject source;
    public GameObject target;
    public object extraData;

    public GameEvent(GameEventType type, GameObject source = null, GameObject target = null, object extraData = null)
    {
        this.type = type;
        this.source = source;
        this.target = target;
        this.extraData = extraData;
    }
}
