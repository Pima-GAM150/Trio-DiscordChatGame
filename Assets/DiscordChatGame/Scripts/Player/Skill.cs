using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SkillTree/Skill")]
public class Skill : ScriptableObject {

    public string name;
    public bool melee;
    public MyDictionary[] modifier;
    public string description;
    public List<Skill> nextSkill;

    //This is my hack solution since Dictionaries do not show up in the inspector.  It is stupid, and I don't like it.
    //
    [Serializable]
    public class MyDictionary
    {
        public string key;
        public float value;
    }

}


