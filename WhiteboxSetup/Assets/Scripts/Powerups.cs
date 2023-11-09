using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class Powerups : MonoBehaviour
{
    public float normalSpeed;
    public float sneakSpeed;

    [SerializeField]
    private GameObject playerObject;

    [SerializeField]
    private GameObject playerBody;

    
    public magicTank magicTank;

    [SerializeField]
    public EnemyStun stunGrenade;

    public ParticleSystem invisParticle;

    private playerMovement movement;

    public AudioSource grenadeThrow;

    private Animator animator;
    private AnimatorController animatorController;

    // Start is called before the first frame update

    void Start()
    {
        movement = playerObject.GetComponent<playerMovement>();
        animator = GetComponent<Animator>();
        animatorController = GetComponent<AnimatorController>();

    }

    // Update is called once per frame
    void Update()
    {
        if (magicTank == null)
        {
            magicTank = new magicTank();
            Debug.Log("new Tank");
        }

        //Activation of Invisible with the key space, works only when mana is more than 1 and not on cooldown
        if (Input.GetKey(KeyCode.Space) && magicTank.magicMeter > 1 && !magicTank.cooldown)
        {
            invisible();
            movement.speed = sneakSpeed;
            invisParticle.Play(true);
            animator.SetBool("invisible", true);
            magicTank.deplete = true;
        }
        else
        {
            magicTank.deplete = false;
            int visLayer = LayerMask.NameToLayer("target");
            playerObject.layer = visLayer;
            invisParticle.Stop(true);
            movement.speed = normalSpeed;
            playerObject.tag = "Player";
            animator.SetBool("invisible", false);

        }

        if (Input.GetMouseButton(1) && magicTank.magicMeter > 40 && !magicTank.cooldown)
        {
            stunGrenade.holdProjectile();
            stunGrenade.drawProjection();
            stunGrenade.available = false;
        }
        if (Input.GetMouseButtonUp(1) && magicTank.magicMeter > 40 && !magicTank.cooldown)
        {
            stunGrenade.LineRenderer.enabled = false;
            stunGrenade.throwProjectile();
            magicTank.magicMeter -= 40;
            magicTank.deplete = true;
            grenadeThrow.Play();
            
        }

    }
        private void invisible()
    {
        if (magicTank.magicMeter > 0)
        playerObject.tag = "Invisible";
        int invislayer = LayerMask.NameToLayer("Ignore Raycast");
        playerObject.layer = invislayer;
    }
}
