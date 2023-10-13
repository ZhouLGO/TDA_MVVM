using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEvents
{
    public event Action OnInventoryBagOperating;
    public void NotifyOperationBagPanel()
    {
        OnInventoryBagOperating?.Invoke();
    }
}
