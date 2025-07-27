using UnityEngine;
using UnityEngine.Events;

public class Bricks : MonoBehaviour, ITriggerable
{
    private int type;
    private int numberOfHit;


    UnityEvent NotifyTriggered;
    UnityEvent NotifyDestroy; 

    [SerializeField] SpriteRenderer spriteRenderer;

    public void OnTriggered() => _NotifyTriggered();
    protected virtual void _NotifyTriggered()
    {
        _NotifyDestroy();
    }
    protected void _NotifyDestroy()
    {
        Destroy(gameObject);
    }

    public void Initialize(int type)
    {
        this.type = type;
        ApplyType();
        name = gameObject.name + type.ToString();

    }

    private void ApplyType()
    {
        Color color = type switch
        {
            1 => Color.green,
            2 => Color.yellow,
            3 => Color.blue,
            4 => Color.magenta,
            5 => Color.red,
            _ => Color.white
        };
        spriteRenderer.color = color;

        int hits = type switch
        {
            1 => 1,
            2 => 10,
            3 => 25,
            4 => 50,
            5 => 100,

            _ => 0,
        };
        numberOfHit = hits;
    }
}