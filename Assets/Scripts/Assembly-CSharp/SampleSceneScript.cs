using System.Linq;
using Unity.Linq;
using UnityEngine;

public class SampleSceneScript : MonoBehaviour
{
	private void OnGUI()
	{
		GameObject gameObject = GameObject.Find("Origin");
		if (GUILayout.Button("Parent"))
		{
			Debug.Log("------Parent");
			Debug.Log(gameObject.Parent().name);
		}
		if (GUILayout.Button("Child"))
		{
			Debug.Log("------Child");
			Debug.Log(gameObject.Child("Sphere_B").name);
		}
		if (GUILayout.Button("Descendants"))
		{
			Debug.Log("------Descendants");
			foreach (GameObject item in gameObject.Descendants())
			{
				Debug.Log(item.name);
			}
		}
		if (GUILayout.Button("name filter overload"))
		{
			Debug.Log("name filter overload");
			foreach (GameObject item2 in gameObject.Descendants("Group"))
			{
				Debug.Log(item2.name);
			}
		}
		if (GUILayout.Button("OfComponent"))
		{
			Debug.Log("------OfComponent");
			foreach (SphereCollider item3 in gameObject.Descendants().OfComponent<SphereCollider>())
			{
				Debug.Log("Sphere:" + item3.name + " Radius:" + item3.radius);
			}
			(from x in gameObject.Descendants()
				where x.CompareTag("foobar")
				select x).OfComponent<BoxCollider2D>();
		}
		if (GUILayout.Button("LINQ"))
		{
			Debug.Log("------LINQ");
			foreach (GameObject item4 in from x in gameObject.Children()
				where x.name.EndsWith("B")
				select x)
			{
				Debug.Log(item4.name);
			}
		}
		if (GUILayout.Button("Add"))
		{
			gameObject.Add(new GameObject[3]
			{
				new GameObject("lastChild1"),
				new GameObject("lastChild2"),
				new GameObject("lastChild3")
			});
			gameObject.AddFirst(new GameObject[3]
			{
				new GameObject("firstChild1"),
				new GameObject("firstChild2"),
				new GameObject("firstChild3")
			});
			gameObject.AddBeforeSelf(new GameObject[3]
			{
				new GameObject("beforeSelf1"),
				new GameObject("beforeSelf2"),
				new GameObject("beforeSelf3")
			});
			gameObject.AddAfterSelf(new GameObject[3]
			{
				new GameObject("afterSelf1"),
				new GameObject("afterSelf2"),
				new GameObject("afterSelf3")
			});
			(from GameObject x in Resources.FindObjectsOfTypeAll<GameObject>()
				where x.Parent() == null && !x.name.Contains("Camera") && x.name != "Root" && x.name != "" && x.name != "HandlesGO" && !x.name.StartsWith("Scene") && !x.name.Contains("Light") && !x.name.Contains("Materials")
				select x).Select(delegate(GameObject x)
			{
				Debug.Log(x.name);
				return x;
			}).Destroy();
		}
		if (GUILayout.Button("MoveTo"))
		{
			gameObject.MoveToLast(new GameObject[3]
			{
				new GameObject("lastChild1(Original)"),
				new GameObject("lastChild2(Original)"),
				new GameObject("lastChild3(Original)")
			});
			gameObject.MoveToFirst(new GameObject[3]
			{
				new GameObject("firstChild1(Original)"),
				new GameObject("firstChild2(Original)"),
				new GameObject("firstChild3(Original)")
			});
			gameObject.MoveToBeforeSelf(new GameObject[3]
			{
				new GameObject("beforeSelf1(Original)"),
				new GameObject("beforeSelf2(Original)"),
				new GameObject("beforeSelf3(Original)")
			});
			gameObject.MoveToAfterSelf(new GameObject[3]
			{
				new GameObject("afterSelf1(Original)"),
				new GameObject("afterSelf2(Original)"),
				new GameObject("afterSelf3(Original)")
			});
		}
		if (GUILayout.Button("Destroy"))
		{
			(from x in gameObject.transform.root.gameObject.Descendants()
				where x.name.EndsWith("(Clone)") || x.name.EndsWith("(Original)")
				select x).Select(delegate(GameObject x)
			{
				Debug.Log(x.name);
				return x;
			}).Destroy();
		}
	}
}
