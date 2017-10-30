using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkills : MonoBehaviour {

    public List<Skill> currentSkills;
    public Canvas canvas;
    public HorizontalLayoutGroup horizontalLayoutGroup;
    public HorizontalLayoutGroup prefabHLG;
    public List<HorizontalLayoutGroup> listHLG;
    public Text skillText;

    private Vector2 lastSkillTree;
    private Skill levelskill;
    private Skill s;


    void Awake()
    {
        levelskill = new Skill();
    }

    // Use this for initialization
    void Start () {
        //Set the position where the skill tree will show up
        lastSkillTree = new Vector2(0f, Camera.main.orthographicSize * 2.0f);
        //Add the first Horizontal Layout Group
        instaniateHorizontalLayoutGroup(prefabHLG, lastSkillTree);
        BFS();

    }

    // Update is called once per frame
    void Update () {
		
	}

    //Create a new skill
    public void instaniateSkillTree(Text prefab, Vector2 position, string name, HorizontalLayoutGroup horzGroup)
    {
        Text newSkill = (Text)Instantiate(prefab);
        newSkill.name = name;
        newSkill.text = name;

        newSkill.rectTransform.localScale = new Vector3(1, 1, 1);

        newSkill.transform.SetParent(horzGroup.transform, true);
        Vector2 localPoint = transform.InverseTransformVector(position);

        newSkill.rectTransform.anchoredPosition = localPoint;
    }
    
    //Creat a new Horizontal Layout Group
    public void instaniateHorizontalLayoutGroup(HorizontalLayoutGroup prefab, Vector2 position)
    {
        HorizontalLayoutGroup newHLG = (HorizontalLayoutGroup)Instantiate(prefab);
        newHLG.transform.SetParent(canvas.transform, true);
        newHLG.transform.localPosition = position;
        
        listHLG.Add(newHLG);
    }

    public void BFS()
    {
        int depth = 0;
        Queue<Skill> queue = new Queue<Skill>();
        Skill rootSkill = currentSkills[0];
        
        //starting at level 0
        levelskill.name = "%";
        queue.Enqueue(rootSkill);
        queue.Enqueue(levelskill);

        //Adding in a safety measure for now to jump out of a loop in case it becomes an infinite.
        int count = 0;
        while (queue.Count != 1 && count < 100)
        {
            s = queue.Dequeue();
            Debug.Log(s.name + ", " + depth + ", " + queue.Count);
            if (s.name == "%")
            {
                queue.Enqueue(levelskill);
                depth++;
                lastSkillTree.y -= 25;
                instaniateHorizontalLayoutGroup(prefabHLG, lastSkillTree);
                //Need to instaniate horizontalLayoutGroup
                
            } else
            {
                instaniateSkillTree(skillText, lastSkillTree, s.name, listHLG[depth]);
                foreach (Skill skill in s.nextSkill)
                {
                    queue.Enqueue(skill);
                }
            }
            count++;
        }
    }
}
