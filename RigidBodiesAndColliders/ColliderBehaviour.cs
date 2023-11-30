using UnityEngine;
using UnityEngine.Events;

public class ColliderBehaviour : MonoBehaviour
{
    private Collider colliderObj;
    public UnityEvent startEvent, triggerEnterEvent, RaiseEndGameEvent;
    public GameManager gameManagerObj;
    protected virtual void Start()
    {
        colliderObj = GetComponent<Collider>();
        colliderObj.isTrigger = true;
        startEvent.Invoke();
        gameManagerObj = FindObjectOfType<GameManager>();

    }
    
    protected virtual void OnTriggerEnter(Collider other)
    {
        triggerEnterEvent.Invoke();
        gameManagerObj.LoseLife();
    }
    
    public void EndGame()
    {
        RaiseEndGameEvent.Invoke();
    }
    
    
}
