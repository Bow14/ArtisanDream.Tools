using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteRendererBehaviour : MonoBehaviour
{
    private SpriteRenderer spriteRendererObj;
    public GameAction gameActionObj;
    public UnityEvent startEvent, raiseEvent;
    
    void Awake()
    {
        spriteRendererObj = GetComponent<SpriteRenderer>();
        startEvent.Invoke();
        gameActionObj.raiseNoArgs += Raise;
    }

    private void Raise()
    {
        raiseEvent.Invoke();
    }

    public void ChangeSpriteColor(ColorData colorDataObj)
    {
        spriteRendererObj.color = colorDataObj.Value;
    }

    public void MatchCameraSize(Camera cam)
    {
        var dimensions = cam.rect;
        spriteRendererObj.size = dimensions.size;
    }

    public void ChangeRenderSprite()
    {
        spriteRendererObj.sprite = gameActionObj.spriteObj.sprite;
    }
}