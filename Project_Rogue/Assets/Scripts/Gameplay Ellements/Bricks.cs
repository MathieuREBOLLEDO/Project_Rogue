using UnityEngine;
using UnityEngine.Events;

public class Bricks : MonoBehaviour, ITriggerable
{
    private int type;
    private int numberOfLifePoint;
    [SerializeField] private int numberOfPoint_GainOnHit = 1;
    [SerializeField] private int numberOfPoint_GainOnDestroy = 20;

    [Header ("Int Events")]
    public IntEvent NotifyInit;
    public IntEvent NotifyPointGains;
    public IntEvent NotifyLifeLost;

    [Header ("Unity Events")]
    public UnityEvent NotifyDestroy; 

    [SerializeField] SpriteRenderer spriteRenderer;

    public void Initialize(int type)
    {
        this.type = type;
        //ApplyType();
        int hits = type switch
        {
            1 => 20,
            2 => 10,
            3 => 25,
            4 => 50,
            5 => 100,

            _ => 0,
        };
        numberOfLifePoint = hits;

        NotifyInit?.Invoke(numberOfLifePoint); // TO DO => change numberOfLifePoint by Type
        name = gameObject.name + type.ToString();

    }

    public void OnTriggered() => UpdateOnTriggered();
    protected virtual void UpdateOnTriggered()
    {
        UpdatelifePoint(1); /// TO DO Valeur change en fonction des dégats du joueur

        GameEventType eventType = GameEventType.EnemyTouched;
        int points = numberOfPoint_GainOnHit;

        if (CheckForDestroy())
        {
            eventType = GameEventType.EnemyKilled;
            points = numberOfPoint_GainOnDestroy;
            DestroyEvent();
        }

        // Déclenche l'événement
        var gameEvent = new GameEvent(
            eventType,
            source: gameObject,           // l'ennemi tué
            target: GameManager.Instance.gameObject, // si tu veux référencer le joueur
            extraData: points         // données additionnelles (ex: points gagnés)
        );
        GameEventManager.Instance.TriggerEvent( gameEvent );

    }

    protected void DestroyEvent()
    {
        NotifyDestroy?.Invoke();
        Destroy(gameObject);
    }

    private void UpdatelifePoint(int damage)
    {
        numberOfLifePoint -= damage;
        NotifyLifeLost?.Invoke(numberOfLifePoint);
    }

    private bool CheckForDestroy()
    {
        if (numberOfLifePoint <= 0)
            return true;
        else return false;
    }
}