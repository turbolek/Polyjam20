using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "My Assets/Dialogue")]
public class Dialogue : ScriptableObject
{
    public string Sentence;
    public string Answer;
}
