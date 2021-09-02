using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class rocket : MonoBehaviour
{
    enum State { Transcending, Dying, Alive };
    State state = State.Alive;
    public static int Score, HighScore;
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip finish;

    [SerializeField] ParticleSystem mainEngineParticle;
    [SerializeField] ParticleSystem deathParticle;
    [SerializeField] ParticleSystem finishParticle;
    public bool collisionsDisabled = false;


    Rigidbody rigidbody_;
    AudioSource audiosource;
    void Start()
    {
        Score = SceneManager.GetActiveScene().buildIndex;
        HighScore = PlayerPrefs.GetInt("HighScore", HighScore);
        rigidbody_ = GetComponent<Rigidbody>();
        audiosource = GetComponent<AudioSource>();
        Debug.Log("Score" + Score);
        Debug.Log("High Score" + HighScore);
    }
    void Update()
    {
        setHighScore();
        if (state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }
        if (Debug.isDebugBuild)
        {
            RespondDebugKey();
        }       
    }
    public void setHighScore()
    {

        if (Score >HighScore) 
        {
            HighScore = Score;
            PlayerPrefs.SetInt("HighScore", HighScore);
        }
    }

    void RespondDebugKey()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
        else if(Input.GetKeyDown(KeyCode.C))
        {
            collisionsDisabled = !collisionsDisabled;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(state != State.Alive || collisionsDisabled) {return;}
        switch (collision.gameObject.tag)
        {
            case "Friendly" :

                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }
    void StartSuccessSequence()
    {   
        setHighScore();
        state = State.Transcending;
        audiosource.Stop();
        audiosource.PlayOneShot(finish);
        finishParticle.Play();
        Invoke("LoadNextLevel", 2f);
    }
    void StartDeathSequence()
    {
        state = State.Dying;
        audiosource.Stop();
        audiosource.PlayOneShot(death);
        deathParticle.Play();
        Invoke("LoadFirstLevel", 1f);
    }
    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if(nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0 ;
        }
        SceneManager.LoadScene(nextSceneIndex);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void LoadFirstLevel()
    {
        SceneManager.LoadScene(1);
    }

    void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            audiosource.Stop();
            mainEngineParticle.Stop();
        }
    }

    public void ApplyThrust()
    {
        rigidbody_.AddRelativeForce(Vector3.up * mainThrust);
        if (!audiosource.isPlaying)
        {
            audiosource.PlayOneShot(mainEngine);
        }
        mainEngineParticle.Play();
    }
    

    void RespondToRotateInput()
    {
        
        float rotationThisFrame = rcsThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
        {
            RotateManually(rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            RotateManually(-rotationThisFrame);
        }
       
    }

    void RotateManually(float rotationThisFrame)
    {
        rigidbody_.freezeRotation = true;
        transform.Rotate(Vector3.forward * rotationThisFrame);
        rigidbody_.freezeRotation = false;
    }


}
