using UnityEngine;
using System.Collections;

public class LittleRawr : Rawr
{
    public override void Move()
    {
        SetSize();
        StartCoroutine(MoveLeft());
    }

    public override void Start()
    {
        base.Start();

        DoStuff();
    }

    public void DoStuff()
    {
        
    }

    IEnumerator MoveLeft()
    {
        while (true)
        {
            transform.Translate(new Vector3(-5f * Time.deltaTime, 0, 0));
            yield return null;
        }
    }
}
