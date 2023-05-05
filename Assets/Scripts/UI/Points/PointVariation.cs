using UnityEngine;
using UnityEngine.UI;

public class PointVariation : MonoBehaviour
{
    [SerializeField] private string prefix;
    [SerializeField] private Text text;
    [SerializeField] private Animator animator;

    public void UpdateText(string val)
    {
        text.text = prefix + val;
    }
    
    void Start () {
        Destroy (gameObject, animator.GetCurrentAnimatorStateInfo(0).length + 5); 
    }
}