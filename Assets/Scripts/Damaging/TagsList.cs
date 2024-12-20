using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "NewTagsList", menuName = "TagsList")]
public class TagsList : ScriptableObject
{
    [HideInInspector]
    public List<string> tags = new();
}
