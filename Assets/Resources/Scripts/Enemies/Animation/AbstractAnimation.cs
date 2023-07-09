using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractAnimation : MonoBehaviour
{

    protected bool _inDefaultPosition = true;
    
    protected virtual void FixedUpdate()
    {
        if(!_inDefaultPosition)
        {
            ReturnToDefaultPosition();
        }
    }
    


    protected abstract void ReturnToDefaultPosition();

    public abstract void AimAtPoint(Vector3 point);


    public virtual void ReturnToDefault()
    {
        _inDefaultPosition = false;
    }


    public abstract bool IsAimedAtPoint(Vector3 point);
}
