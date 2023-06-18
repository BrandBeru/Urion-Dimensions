using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MyEventSystem : EventSystem
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.OnEnable();
    }

    // Update is called once per frame
    protected override void Update()
    {
        EventSystem originalCurrent = EventSystem.current;
        current = this;
        base.Update();
        current = originalCurrent;
    }
}
