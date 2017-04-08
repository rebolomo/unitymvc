using UnityEngine;
using System.Collections;
using UnityMVC.Core.View;

public class TestUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
		TopView.Instance.OpenView();
		FirstView.Instance.OpenView();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
