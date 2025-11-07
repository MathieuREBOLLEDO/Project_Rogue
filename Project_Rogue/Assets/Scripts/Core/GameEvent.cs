using UnityEngine;

public class GameEvent
{
    public GameEventType eventType;

    // Payload optionnel pour transmettre des infos à l’effet
    public GameObject source; // ex: l’ennemi qui a été tué
    public GameObject target; // ex: le joueur ou l’objet touché
    public float value;       // ex: dégâts infligés, heal appliqué
    public object extraData;  // pour des données spécifiques si besoin

    public GameEvent(GameEventType type)
    {
        eventType = type;
    }

    public GameEvent(GameEventType type, GameObject source = null, GameObject target = null, object extraData = null)
    {
        this.eventType = type;
        this.source = source;
        this.target = target;
        this.extraData = extraData;
    }
}
