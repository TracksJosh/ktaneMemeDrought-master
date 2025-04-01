using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rnd = UnityEngine.Random;

public class MemeDroughtScript : MonoBehaviour {

    public KMAudio audio;
    public KMBombModule bombModule;
    private bool isSolved;

    private static int moduleIdCounter = 1;
    private int moduleId;

    public KMSelectable Button1;
    public KMSelectable Button2;
    public KMSelectable Button3;
    public GameObject GO1;
    public GameObject GO2;
    public GameObject GO3;
    public TextMesh Display;
    public GameObject DisplayBar;
    public GameObject SpriteFighter1;
    public GameObject SpriteFighter2;
    public GameObject KABOOM;

    public Sprite[] CreatureSpr;
    public AudioClip[] CreatureNames;

    private Creature creature1;
    private Creature creature2;
    private Creature creature3;

    private List<int> round1 = new List<int>();
    private List<int> round2 = new List<int>();

    private int fighter1 = -1;
    private int fighter2 = -1;

    private int round = 0;


    // Use this for initialization
    void Start () {
        isSolved = false;
        moduleId = moduleIdCounter++;
        Button1.OnInteract += delegate {PressLeft(); return false; };
        Button2.OnInteract += delegate { PressMiddle(); return false; };
        Button3.OnInteract += delegate { PressRight(); return false; };
        SpriteFighter1.gameObject.SetActive(false);
        SpriteFighter2.gameObject.SetActive(false);
        KABOOM.gameObject.SetActive(false);
        beginModule();
    }

    void beginModule()
    {
        round = 1;

        int[] ind = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };
        ind = ind.Shuffle();
        for (int i = 0; i < 3; i++)
        {
            round1.Add(ind[i]);
            round2.Add(ind[i + 3]);
        }
        RoundOne();
    }

    void RoundOne()
    {
        creature1 = new Creature(round1[0]);
        creature2 = new Creature(round1[1]);
        creature3 = new Creature(round1[2]);

        GO1.transform.localScale = new Vector3(creature1.GetSize()[0], creature1.GetSize()[1], 0.1f);
        GO2.transform.localScale = new Vector3(creature2.GetSize()[0], creature2.GetSize()[1], 0.1f);
        GO3.transform.localScale = new Vector3(creature3.GetSize()[0], creature3.GetSize()[1], 0.1f);

        GO1.GetComponent<SpriteRenderer>().sprite = CreatureSpr[creature1.GetID()];
        GO2.GetComponent<SpriteRenderer>().sprite = CreatureSpr[creature2.GetID()];
        GO3.GetComponent<SpriteRenderer>().sprite = CreatureSpr[creature3.GetID()];

        switch (Rnd.Range(0, 3))
        {
            case 0:
                Display.text = creature1.GetName();
                Debug.LogFormat("[Meme Drought #{0}] Round 1, Identify {1}", moduleId, creature1.GetName());
                break;
            case 1:
                Display.text = creature2.GetName();
                Debug.LogFormat("[Meme Drought #{0}] Round 1, Identify {1}", moduleId, creature2.GetName());
                break;
            case 2:
                Display.text = creature3.GetName();
                Debug.LogFormat("[Meme Drought #{0}] Round 1, Identify {1}", moduleId, creature3.GetName());
                break;
        }
    }

    void RoundTwo()
    {
        creature1 = new Creature(round2[0]);
        creature2 = new Creature(round2[1]);
        creature3 = new Creature(round2[2]);

        GO1.transform.localScale = new Vector3(creature1.GetSize()[0], creature1.GetSize()[1], 0.1f);
        GO2.transform.localScale = new Vector3(creature2.GetSize()[0], creature2.GetSize()[1], 0.1f);
        GO3.transform.localScale = new Vector3(creature3.GetSize()[0], creature3.GetSize()[1], 0.1f);

        GO1.GetComponent<SpriteRenderer>().sprite = CreatureSpr[creature1.GetID()];
        GO2.GetComponent<SpriteRenderer>().sprite = CreatureSpr[creature2.GetID()];
        GO3.GetComponent<SpriteRenderer>().sprite = CreatureSpr[creature3.GetID()];

        switch (Rnd.Range(0, 3))
        {
            case 0:
                Debug.LogFormat("[Meme Drought #{0}] Round 2, Identify {1}", moduleId, creature1.GetName());
                Display.text = creature1.GetName();
                break;
            case 1:
                Debug.LogFormat("[Meme Drought #{0}] Round 2, Identify {1}", moduleId, creature2.GetName());
                Display.text = creature2.GetName();
                break;
            case 2:
                Debug.LogFormat("[Meme Drought #{0}] Round 2, Identify {1}", moduleId, creature3.GetName());
                Display.text = creature3.GetName();
                break;
        }
    }

    void RoundThree()
    {
        Button2.gameObject.SetActive(false);
        creature1 = new Creature(fighter1);
        creature3 = new Creature(fighter2);

        GO1.transform.localScale = new Vector3(creature1.GetSize()[0], creature1.GetSize()[1], 0.1f);
        
        GO3.transform.localScale = new Vector3(creature3.GetSize()[0], creature3.GetSize()[1], 0.1f);

        GO1.GetComponent<SpriteRenderer>().sprite = CreatureSpr[creature1.GetID()];
        
        GO3.GetComponent<SpriteRenderer>().sprite = CreatureSpr[creature3.GetID()];

        Display.text = "Who Wins?";
        Debug.LogFormat("[Meme Drought #{0}] Round 3, {1} Wins", moduleId, creature1.GetID() > creature3.GetID() ? creature1.GetName() : creature3.GetName());
    }

    void PressLeft ()
    {
        Debug.LogFormat("[Meme Drought #{0}] Pressed {1}", moduleId, creature1.GetName());
        if (round == 1 && creature1.GetName().Equals(Display.text))
        {
            audio.PlaySoundAtTransform(CreatureNames[creature1.GetID()].name, transform);
            fighter1 = creature1.GetID();
            round = 2;
            RoundTwo();
        }
        else if (round == 2 && creature1.GetName().Equals(Display.text))
        {
            audio.PlaySoundAtTransform(CreatureNames[creature1.GetID()].name, transform);
            fighter2 = creature1.GetID();
            round = 3;
            RoundThree();
        }
        else if (round == 3 && fighter1 > fighter2)
        {
            if (!isSolved)
            {
                StartCoroutine(FightSequence());
                isSolved = true;
                bombModule.HandlePass();
            }
        }
        else
        {
            if (!isSolved)
            {
                bombModule.HandleStrike();
                beginModule();
            }
        }
    }

    void PressMiddle()
    {
        Debug.LogFormat("[Meme Drought #{0}] Pressed {1}", moduleId, creature2.GetName());
        if (round == 1 && creature2.GetName().Equals(Display.text))
        {
            audio.PlaySoundAtTransform(CreatureNames[creature2.GetID()].name, transform);
            fighter1 = creature2.GetID();
            round = 2;
            RoundTwo();
        }
        else if (round == 2 && creature2.GetName().Equals(Display.text))
        {
            audio.PlaySoundAtTransform(CreatureNames[creature2.GetID()].name, transform);
            fighter2 = creature2.GetID();
            round = 3;
            RoundThree();
        }
        else
        {
            if (!isSolved)
            {
                bombModule.HandleStrike();
                beginModule();
            }
        }
    }

    void PressRight()
    {
        Debug.LogFormat("[Meme Drought #{0}] Pressed {1}", moduleId, creature3.GetName());
        if (round == 1 && creature3.GetName().Equals(Display.text))
        {
            audio.PlaySoundAtTransform(CreatureNames[creature3.GetID()].name, transform);
            fighter1 = creature3.GetID();
            round = 2;
            RoundTwo();
        }
        else if (round == 2 && creature3.GetName().Equals(Display.text))
        {
            audio.PlaySoundAtTransform(CreatureNames[creature3.GetID()].name, transform);
            fighter2 = creature3.GetID();
            round = 3;
            RoundThree();
        }
        else if (round == 3 && fighter2 > fighter1)
        {
            if (!isSolved)
            {
                StartCoroutine(FightSequence());
                isSolved = true;
                bombModule.HandlePass();
            }
        }
        else
        {
            if (!isSolved)
            {
                bombModule.HandleStrike();
                beginModule();
            }
        }
    }

    IEnumerator FightSequence()
    {
        Button1.gameObject.SetActive(false);
        Button2.gameObject.SetActive(false);
        Button3.gameObject.SetActive(false);
        DisplayBar.gameObject.SetActive(false);
        SpriteFighter1.gameObject.SetActive(true);
        SpriteFighter2.gameObject.SetActive(true);
        SpriteFighter1.GetComponent<SpriteRenderer>().sprite = CreatureSpr[fighter1];
        SpriteFighter2.GetComponent<SpriteRenderer>().sprite = CreatureSpr[fighter2];

        Creature a = new Creature(fighter1);
        Creature b = new Creature(fighter2);

        SpriteFighter1.transform.localScale = new Vector3(a.GetSize()[0] / 25, a.GetSize()[1] / 25, 0.1f);
        SpriteFighter2.transform.localScale = new Vector3(b.GetSize()[0] / 25, b.GetSize()[1] / 25, 0.1f);
        audio.PlaySoundAtTransform(CreatureNames[fighter1].name, transform);
        yield return new WaitForSecondsRealtime(CreatureNames[fighter1].length);
        audio.PlaySoundAtTransform(CreatureNames[15].name, transform);
        yield return new WaitForSecondsRealtime(CreatureNames[15].length);
        audio.PlaySoundAtTransform(CreatureNames[fighter2].name, transform);
        yield return new WaitForSecondsRealtime(CreatureNames[fighter2].length);
        audio.PlaySoundAtTransform(CreatureNames[16].name, transform);
        yield return new WaitForSecondsRealtime(CreatureNames[16].length);
        for(int i = 0; i < 60; i++)
        {
            if(i < 50)
            {
                SpriteFighter1.transform.localPosition = new Vector3(SpriteFighter1.transform.localPosition.x + 0.00121f, SpriteFighter1.transform.localPosition.y, SpriteFighter1.transform.localPosition.z);
                SpriteFighter2.transform.localPosition = new Vector3(SpriteFighter2.transform.localPosition.x - 0.00121f, SpriteFighter2.transform.localPosition.y, SpriteFighter2.transform.localPosition.z);
            }
            if(i == 50)
            {
                SpriteFighter1.gameObject.SetActive(false);
                SpriteFighter2.gameObject.SetActive(false);
                KABOOM.gameObject.SetActive(true);
            }
            if(i >= 50 && i < 55)
            {
                KABOOM.transform.localScale = new Vector3(KABOOM.transform.localScale.x *1.2f, KABOOM.transform.localScale.y * 1.2f, KABOOM.transform.localScale.z * 1.2f);
            }
            else if (i >= 55)
            {
                KABOOM.transform.localScale = new Vector3(KABOOM.transform.localScale.x / 1.2f, KABOOM.transform.localScale.y/ 1.2f, KABOOM.transform.localScale.z / 1.2f);
            }
            yield return new WaitForSecondsRealtime(0.01f);
        }
        KABOOM.gameObject.SetActive(false);
        if(fighter1 > fighter2) SpriteFighter1.gameObject.SetActive(true);
        else SpriteFighter2.gameObject.SetActive(true);
        for(int i = 0; i < 15; i++)
        {
            if(fighter1 > fighter2) SpriteFighter1.transform.localScale = new Vector3(SpriteFighter1.transform.localScale.x * 1.05f, SpriteFighter1.transform.localScale.y * 1.05f, SpriteFighter1.transform.localScale.z * 1.05f);
            else SpriteFighter2.transform.localScale = new Vector3(SpriteFighter2.transform.localScale.x * 1.05f, SpriteFighter2.transform.localScale.y * 1.05f, SpriteFighter2.transform.localScale.z * 1.05f);
            yield return new WaitForSecondsRealtime(0.01f);
        }
    }
}

public class Creature
{
	private int name;
    //[trippi, burbaloni, frulli, chimpanzini, frigo, brr brr, tung tung, tralalelo, trulimero, bobritto, bombombini, bombardino, lirili, giraffa, la vaca]
    public Creature(int i)
    {
        SetName(i);
    }

    public float[] GetSize()
	{
		float[] floats = new float[2];
		switch(name)
		{
			case 0:
				floats[0] = 0.1f; floats[1] = 0.1f; break;
            case 1:
                floats[0] = 0.3f; floats[1] = 0.3f; break;
            case 2:
                floats[0] = 0.3f; floats[1] = 0.3f; break;
            case 3:
                floats[0] = 0.3f; floats[1] = 0.26f; break;
            case 4:
                floats[0] = 0.037f; floats[1] = 0.035f; break;
            case 5:
                floats[0] = 0.3f; floats[1] = 0.25f; break;
            case 6:
                floats[0] = 0.15f; floats[1] = 0.15f; break;
            case 7:
                floats[0] = 0.25f; floats[1] = 0.25f; break;
            case 8:
                floats[0] = 0.3f; floats[1] = 0.22f; break;
            case 9:
                floats[0] = 0.08f; floats[1] = 0.08f; break;
            case 10:
                floats[0] = 0.1f; floats[1] = 0.1f; break;
            case 11:
                floats[0] = 0.1f; floats[1] = 0.1f; break;
            case 12:
                floats[0] = 0.25f; floats[1] = 0.25f; break;
            case 13:
                floats[0] = 0.3f; floats[1] = 0.25f; break;
            case 14:
                floats[0] = 0.07f; floats[1] = 0.07f; break;
        }
		return floats;
	}
    private void SetName(int animal)
	{
		name = animal;
	}

    public int GetID()
    {
        return name;
    }
    public string GetName()
    {
        string n = "";
        switch (name)
        {
            case 11:
                n = "Bombardino Crocadilo"; break;
            case 10:
                n = "Bombombini Gusini"; break;
            case 5:
                n = "Brr Brr Patapim"; break;
            case 12:
                n = "Lirili Larila"; break;
            case 7:
                n = "Tralalelo Tralala"; break;
            case 0:
                n = "Trippi Troppi"; break;
            case 1:
                n = "Burbaloni Luliloli"; break;
            case 8:
                n = "Trulimero Trulicina"; break;
            case 4:
                n = "Frigo Camello"; break;
            case 2:
                n = "Frulli Frulla"; break;
            case 14:
                n = "La Vaca Saturno Saturnita"; break;
            case 13:
                n = "Giraffa Celeste"; break;
            case 6:
                n = "Tung Tung Tung Sahur"; break;
            case 3:
                n = "Chimpanzini Bananini"; break;
            case 9:
                n = "Bobritto Bandito"; break;
        }
        return n;
    }

    
}