using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Bug enchainement courir <-> marche arriere (bloque sur l'animation courir)



public class ClaireController : MonoBehaviour {
    Animator claireAnimator;
    AudioSource claireAudioSource;
    CapsuleCollider claireCapsule;

    float axisH, axisV, p1;

    [SerializeField]
    float walkSpeed = 2f, runSpeed = 8f, rotSpeed = 80f, jumpForce = 350f;

    Rigidbody rb;

    const float timeOut = 15.0f;
    [SerializeField] float countdown = timeOut;



    [SerializeField] AudioClip sndJump, sndImpact, sndLeftFoot, sndRightFoot;
    bool switchFoot = false;
    [SerializeField]
    bool isjumping = false;



    private void Awake()
    {
        claireAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        claireAudioSource = GetComponent<AudioSource>();
        claireCapsule = GetComponent<CapsuleCollider>();
    }




    void Update () {
        axisH = Input.GetAxis("Horizontal");
        axisV = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.Space) && !isjumping)
        {
            isjumping = true;
            countdown = timeOut;
            claireAudioSource.pitch = 1f;
            claireAnimator.SetTrigger("jump");
            rb.AddForce(Vector3.up * jumpForce);
            claireAudioSource.PlayOneShot(sndJump);
            claireAnimator.SetBool("dance", false);
            transform.Find("AudioDance").GetComponent<AudioSource>().enabled = false;

        }


        else if (axisV > 0)
        {
            if (Input.GetKey(KeyCode.RightControl))
            {
                transform.Translate(Vector3.forward * runSpeed * axisV * Time.deltaTime);
                transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime * axisH);
                claireAnimator.SetFloat("run", axisV);
            }
            else
            {
                claireAnimator.SetBool("walk", true);
                transform.Translate(Vector3.forward * walkSpeed * axisV * Time.deltaTime);
                transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime * axisH);
                claireAnimator.SetFloat("run", 0);
            }

        }
        else
        {
            claireAnimator.SetBool("walk", false);
        }


        if(axisH != 0 && axisV == 0)
        {
            claireAnimator.SetFloat("h", axisH);
            float p = claireAnimator.GetFloat("h");
            transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime * axisH);
            claireAnimator.SetBool("dance", false);

        }
        else
        {

            claireAnimator.SetBool("dance", false);

        }



        if(axisV < 0)
        {
            claireAnimator.SetBool("walkBack", true);
            claireAnimator.SetFloat("run", 0);
            transform.Translate(Vector3.forward * walkSpeed * axisV * Time.deltaTime);
            transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime * axisH);
        }
        else
        {
            claireAnimator.SetBool("walkBack", false);
            transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime * axisH);
        }

        //Idle Dance Twerk

        if(axisH==0 && axisV == 0)
        {
            countdown -= Time.deltaTime;

            if (countdown <= 0)
            {
                claireAnimator.SetBool("dance", true);
                transform.Find("AudioDance").GetComponent<AudioSource>().enabled = true;
            }
        }
        else
        {
            countdown = timeOut;
            claireAnimator.SetBool("dance", false);
            transform.Find("AudioDance").GetComponent<AudioSource>().enabled = false;
        }

        //debug dead

        if (Input.GetKeyDown(KeyCode.AltGr))
        {
            ClaireDead();
        }

        //curve de saut
        if (isjumping)
        {
            claireCapsule.height = claireAnimator.GetFloat("colidheight");
        }
	}

    private void FixedUpdate()
    {

    }

    public void ClaireDead()
    {
        claireAnimator.SetTrigger("dead");
        claireAnimator.SetBool("dance", false);
        transform.Find("AudioDance").GetComponent<AudioSource>().enabled = false;
        GetComponent<ClaireController>().enabled = false;

    }

    public void PlaySoundImpact()
    {
        claireAudioSource.pitch = 1f;
        claireAudioSource.PlayOneShot(sndImpact);
    }

    public void PlayFootStep()
    {
         if (!claireAudioSource.isPlaying)
        {
            switchFoot = !switchFoot;

            if (switchFoot)
            {
                if (Input.GetKey(KeyCode.RightControl))
                {
                    claireAudioSource.pitch = 4f;
                    claireAudioSource.PlayOneShot(sndLeftFoot);
                }
                else
                {
                    claireAudioSource.pitch = 3f;
                    claireAudioSource.PlayOneShot(sndLeftFoot);
                }
                
            }
            else
            {
                if (Input.GetKey(KeyCode.RightControl))
                {
                    claireAudioSource.pitch = 4f;
                    claireAudioSource.PlayOneShot(sndRightFoot);
                }
                else
                {
                    claireAudioSource.pitch = 3f;
                    claireAudioSource.PlayOneShot(sndRightFoot);
                }

            }
        }
    }

    public void IsClaireJumping()
    {
        isjumping = false;
    }
}
