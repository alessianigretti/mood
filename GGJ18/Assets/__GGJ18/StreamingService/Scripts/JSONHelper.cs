using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class JSONHelper {

	// Usage:
	// YourObject[] objects = JsonHelper.GetJsonArray<YourObject> (jsonString);
	public static T[] GetJsonArray<T> (string json) {
		string newJson = "{ \"array\": " + json + "}";
		Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>> (newJson);
		return wrapper.array;
	}

	// Usage:
	// string jsonString = JsonHelper.ArrayToJson<YouObject>(objects);
	public static string ArrayToJson<T> (T[] array) {
		Wrapper<T> wrapper = new Wrapper<T> ();
		wrapper.array = array;
		return JsonUtility.ToJson (wrapper);
	}

	[Serializable]
	private class Wrapper<T> {
		public T[] array;
	}


}
