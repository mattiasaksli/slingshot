using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface State
{
    void Update(MonoBehaviour controller);
    void FixedUpdate(MonoBehaviour controller);
}
