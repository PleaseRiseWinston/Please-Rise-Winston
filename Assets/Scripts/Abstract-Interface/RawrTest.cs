using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;

public class RawrTest : MonoBehaviour
{
    public Rawr[] rawrs;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            int index = Random.Range(0, 2);
            rawrs[index].Move();
        }
    }
    /*
    public void Sort(ISortable obj)
    {
        if (obj.GetFirst() == obj.GetLast())
        {
            
        }
    }*/
}
