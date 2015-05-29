using UnityEngine;
using System.Collections;

public abstract class Rawr : MonoBehaviour
{
    public float size = 1.0f;

    public virtual void Start()
    {
        SetSize();
        SetColor();
    }

    protected void SetSize()
    {
        transform.localScale = Vector3.one*size;
    }

    public virtual void SetColor()
    {
        GetComponent<Renderer>().material.color = Color.red;
    }

    public abstract void Move();

    public void DestroyMe()
    {
        Destroy(gameObject);
    }
}
