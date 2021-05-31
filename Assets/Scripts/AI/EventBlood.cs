using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventBlood : MonoBehaviour
{
    public void EventStopBlood()
    {
        GetComponent<Animator>().SetInteger("Effect", 0);
    }
}
