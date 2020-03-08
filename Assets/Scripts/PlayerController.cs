using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Only public for debug purposes
    public float verticalVelovity = 0.0f;

    public float baseForwardSpeed = 5.0f;
    public float baseLateralSpeed = 3.5f;
    public float baseJumpSpeed = 5.0f;
    public int jumpAmount = 2;

    public float sensitivity = 5.0f;
    public bool inverted = false;
    public float maxRotation = 15.0f;

    public GameObject shot;
    public Transform shotTransform;
    public float nextFire = 0.25f;
    public float fireRate = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;
        if (Input.GetKeyDown("escape"))
            Cursor.lockState = CursorLockMode.None;

        // Rotating player object around Y axis
        float rotation = Input.GetAxis("Mouse X");
        transform.Rotate(0, rotation * sensitivity, 0);

        // Rotating camera up and down
        float upDownRotaion = Input.GetAxis("Mouse Y");

        if(inverted)
            Camera.main.transform.Rotate(Mathf.Clamp (upDownRotaion, -maxRotation, maxRotation) * sensitivity, 0, 0);
        else
            Camera.main.transform.Rotate(Mathf.Clamp (-upDownRotaion, -maxRotation, maxRotation) * sensitivity, 0, 0);

        float forwardSpeed = Input.GetAxis("Vertical");
        float lateralSpeed = Input.GetAxis("Horizontal");

        CharacterController controller = GetComponent<CharacterController>();

        // Applying gravity, stupid error, only apply while not grounded, then reset when grounded
        if(!controller.isGrounded)
            verticalVelovity += Physics.gravity.y * Time.deltaTime;
        else
            verticalVelovity = 0.0f;

        if(Input.GetButton("Jump") && controller.isGrounded)
            verticalVelovity = baseJumpSpeed;

        if(Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Instantiate(shot, shotTransform.position, Camera.main.transform.rotation);
        }
        
        Vector3 speed = new Vector3(lateralSpeed * baseLateralSpeed, verticalVelovity, forwardSpeed * baseForwardSpeed);
        // Move in look direction
        speed = transform.rotation * speed;
        // Scale to delta time, to account for variable refresh rates. Ensure movement speed is not tied to FPS
        controller.Move(speed * Time.deltaTime);
    }
}
