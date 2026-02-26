using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.Events;

public class Bricks : MonoBehaviour, ITriggerable
{
    [Header ("Feedbacks")]
    public MMFeedbacks HitFeedbck;

    [Header("Bricks Datas")]
    [SerializeField] private int numberOfPoint_GainOnHit = 1;
    [SerializeField] private int numberOfPoint_GainOnDestroy = 20;
    private int type;
    private int numberOfLifePoint;

    [Header ("Int Events")]
    public IntEvent NotifyInit;
    public IntEvent NotifyPointGains;
    public IntEvent NotifyLifeLost;

    [Header("Unity Events")]
    public UnityEvent NotifyDestroy; 

    [SerializeField] SpriteRenderer spriteRenderer;

    [Header("Bonus")]
    [SerializeField] private GameObject bonusPrefab;
    [SerializeField] private float bonusChance = 0.25f;

    void Start ()
    {
        Initialize(1);
    }

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
        HitFeedbck?.PlayFeedbacks();

        UpdatelifePoint(1); /// TO DO Valeur change en fonction des dégats du joueur

        /// A Definir
       

        /*

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

        */


        // Toujours envoyer Touched
        GameEventManager.Instance.TriggerEvent(new GameEvent(
            GameEventType.EnemyTouched,
            source: gameObject,
            target: GameManager.Instance.gameObject,
            extraData: numberOfPoint_GainOnHit
        ));


        /// TO DO Invoke global event for gestion of Bonus 

        if (CheckForDestroy())
        {
            GameEventManager.Instance.TriggerEvent(new GameEvent(
                GameEventType.EnemyKilled,
                source: gameObject,
                target: GameManager.Instance.gameObject,
                extraData: numberOfPoint_GainOnDestroy
            ));
            DestroyEvent();
        }

    }



    private void TrySpawnBonus()
    {
        if (Random.value <= bonusChance)
        {
            Instantiate(bonusPrefab, transform.position, Quaternion.identity);
        }
    }


    protected void DestroyEvent()
    {
        TrySpawnBonus();
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