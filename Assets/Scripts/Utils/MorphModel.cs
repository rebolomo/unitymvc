using UnityEngine;
using System.Collections;

public class MorphModel : MonoBehaviour {
	[SerializeField]
	public Transform[]
	bones;

	[SerializeField]
	protected SkinnedMeshRenderer
		mrender;
	
	public SkinnedMeshRenderer mRender {
		get {
			return mrender;
		}
		set {
			mrender = value;
		}
	}
	
	[SerializeField]
	protected Renderer[]
	mrenders;
	
	public Renderer[] mRenders {
		get {
			return mrenders;
		}
		set {
			mrenders = value;
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
