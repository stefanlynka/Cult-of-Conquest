using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tools {

    public static GameObject GetChildNamed(GameObject parent, string childName) {
        for (int i = 0; i < parent.transform.childCount; i++) {
            if (parent.transform.GetChild(i).name == childName) return parent.transform.GetChild(i).gameObject;
        }
        return null;
    }

    public static GameObject MakeText(GameObject parent, string name, Vector3 offset, int fontSize, int sortingOrder) {
        GameObject text = new GameObject();
        text.name = "Text";
        text.transform.parent = parent.transform;
        text.transform.localPosition = offset;
        TextMesh textMesh = text.AddComponent<TextMesh>();
        textMesh.characterSize = 0.1f;
        textMesh.fontSize = fontSize;
        textMesh.color = new Color(0.1f, 0.1f, 0.1f);
        textMesh.fontStyle = FontStyle.Bold;
        textMesh.anchor = TextAnchor.MiddleCenter;
        textMesh.alignment = TextAlignment.Center;
        textMesh.text = name;
        MeshRenderer render = textMesh.GetComponent<MeshRenderer>();
        render.sortingOrder = sortingOrder;
        return text;
    }
    public static int SortByTime(Cooldown c1, Cooldown c2) {
        return c1.timeToAct.CompareTo(c2.timeToAct);
    }
}
