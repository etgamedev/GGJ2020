using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IThrowable
{
	void Throw(Vector3 direction);
}

public class GenericItem : MonoBehaviour, IInteractable, IThrowable
{
    public virtual void Interact()
	{
	}
	
	public virtual void Throw(Vector3 throwDirection)
	{}
}
