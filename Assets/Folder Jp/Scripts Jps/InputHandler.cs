using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour{
    private bool isMoving, isJumping;
    private float speed = 5f;
    private float jumpForce = 10f;
    public Rigidbody2D rb;
    private Vector2 direction;
    AudioHandler audioHandler;
    public void Move(InputAction.CallbackContext context){
        if (context.phase == InputActionPhase.Started){
        }else if (context.phase == InputActionPhase.Performed){
            isMoving = true;
            direction = context.ReadValue<Vector2>();
            audioHandler.PlaySFX(audioHandler.jump);
        }else if (context.phase == InputActionPhase.Canceled){
            isMoving = false;
        }
    }

    /*public void Jump(InputAction.CallbackContext context){
        if (context.phase == InputActionPhase.Started){
        }else if (context.phase == InputActionPhase.Performed){
            isJumping = true;
            direction = context.ReadValue<Vector2>();
        }else if (context.phase == InputActionPhase.Canceled){
            isJumping = false;
        }
    }*/

    public void Awake(){
        audioHandler = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioHandler>();
    }

    public void Update() {
        if (isMoving){
            transform.position += new Vector3(direction.x, direction.y, 0) * Time.deltaTime * speed;
        }
        if (isJumping){
            //transform.position += new Vector3(0, jumpForce, 0) * Time.deltaTime * speed;
            //rb.linearVelocity = new Vector2 (direction.x, direction.y * jumpForce);
        }
    }
}
