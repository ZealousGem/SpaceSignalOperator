using UnityEngine;

public class DeliveredPlanet : BasePlanet
{

    public MeshRenderer mesh;
    private Material outlineMaterial;
    private const float outlineSize = 1.02f;
    private const float planetradius = 7.79f;
    private float currentPlanet = 0f;
    private bool interactablePlanet = false;
    private SphereCollider sphereCollider;

    protected override void Awake()
    {

      base.Awake();
      sphereCollider = gameObject.GetComponent<SphereCollider>(); 
       
    } 

    private void Start()=> outlineMaterial = mesh.materials[1];

    protected override void ToggleVisibility(bool state)
    {
        base.ToggleVisibility(state);
        if (state) ChangeOutlineSize(currentPlanet);       
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;

        if (interactablePlanet)
        {
           EventBus.Act(new EndGameEvent(GameState.Success, false));
           ChangeOutlineSize(0f);
           sphereCollider.radius = planetradius;
           interactablePlanet = false;    
        }

        else
        {
             EventBus.Act(new DamageShip(Damagedby.OringalPlanet, 100f));
        }
        

    }

    public void setTargetPlanet()
    {
      interactablePlanet = true;  
      currentPlanet = outlineSize;

        if (mesh.gameObject.activeSelf)
        {
            ChangeOutlineSize(currentPlanet);
        } 
    } 

    private void ChangeOutlineSize(float amount)
    {
        if(outlineMaterial == null) return; 
        
        outlineMaterial.SetFloat("_OutlineSize", amount);

    }
}
