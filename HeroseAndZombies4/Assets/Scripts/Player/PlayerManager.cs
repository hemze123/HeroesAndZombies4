using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [System.Serializable]
    public class Outfit
    {
        public GameObject modelPrefab;
        public AnimatorOverrideController animatorController;
        public float speedModifier = 1f;
    }

    [SerializeField] private Transform modelParent;
    [SerializeField] public Outfit[] outfits;

    private GameObject currentModel;
    private Animator animator;

    public void ChangeOutfit(int outfitIndex)
    {
        if(outfitIndex < 0 || outfitIndex >= outfits.Length) return;

        DestroyCurrentModel();
        CreateNewModel(outfits[outfitIndex]);
        
        GameEvents.OnOutfitChanged?.Invoke(outfitIndex);
    }

    void DestroyCurrentModel()
    {
        if(currentModel != null)
        {
            Destroy(currentModel);
        }
    }

    void CreateNewModel(Outfit outfit)
    {
        currentModel = Instantiate(outfit.modelPrefab, modelParent);
        animator = currentModel.GetComponent<Animator>();
        animator.runtimeAnimatorController = outfit.animatorController;
        
        GetComponent<PlayerController>().SetSpeedModifier(outfit.speedModifier);
    }
}