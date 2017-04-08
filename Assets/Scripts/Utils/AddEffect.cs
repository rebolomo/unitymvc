using UnityEngine;
using System.Collections;

public class AddEffect : MonoBehaviour {

	[SerializeField]
	private GameObject mEffect;

	// Use this for initialization
	void Start () {
		GameObject go = GameObject.Instantiate (mEffect)as GameObject;
		go.transform.parent = transform;
		go.transform.localPosition = mEffect.transform.localPosition;
		go.transform.localRotation = mEffect.transform.localRotation;
		go.transform.localScale = mEffect.transform.localScale;
	}

}
