using UnityEngine;
using SIP.Common;

public class SimpleMoveTest : MonoBehaviour {

    public GameObject ojb;
	// Use this for initialization
	void Start () 
    {
        iTweenPath path = ojb.GetComponent<iTweenPath>();
//		Vector3[] path_array = new Vector3[]() ;
//		int index = 0 ;
//		for(Vector3 v in path)
//		{
//			path_array[index] = v ;
//			++index ;
//		}
//		Hashtable myhash= iTween.Hash("path", path);
//		var path = path.GetPath("myPath");
//         iTween.MoveTo(gameObject, iTween.Hash(//"position", Vector3(0, 0, 0), 
//                                          "path", path,
//                                          "time", 20,
//                                          "easetype", "linear"));
		
//        iTween.MoveTo(gameObject, iTweenPath.GetPath("New Path 1"));
		
		//iTweenPath.GetPath(path.pathName);
         iTween.MoveTo(gameObject, iTween.Hash(//"position", Vector3(0, 0, 0), 
                                          "path", path.nodes.ToArray(),//iTweenPath.GetPath(path.pathName)
                                          "time", 20,
                                          "easetype", "linear"));
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
