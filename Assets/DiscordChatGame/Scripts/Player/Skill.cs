using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour {

    public string name;
    public bool melee;
    public Dictionary<string, float> modifier;
    public List<string> nextSkill;

}
