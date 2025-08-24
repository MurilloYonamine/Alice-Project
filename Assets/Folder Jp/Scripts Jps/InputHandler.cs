using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour{
    //Variables
    #region
    //Movement Variables
    private float speed = 5f;
    private float HorizontalMoviment;
    //Jump Variables
    private float jumpPower = 10f;
    //Ground Check Variables
    [SerializeField] private Transform groundCheckPos;
    [SerializeField] private Vector2 groundCheckSize = new Vector2(1f, 0.05f);
    [SerializeField] private LayerMask groundLayer;
    //Other Variables
    private Rigidbody2D rb;
    #endregion

    //Unity Methods
    #region
    public void Awake(){
        rb = this.GetComponent<Rigidbody2D>();
    }

    public void Update(){
        rb.linearVelocity = new Vector2(HorizontalMoviment * speed, rb.linearVelocity.y);
    }
    #endregion

    //Player Input Methods
    #region
    public void Move(InputAction.CallbackContext context){
        HorizontalMoviment = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context){
        if (isGrounded()){ 
            if (context.performed){
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
            }else if (context.canceled){
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0.5f);
            }
        }else if(context.performed){
            Debug.Log("Usou Item");
        }
    }
    #endregion

    //Ground Check Methods
    #region
    private void OnDrawGizmosSelected(){
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
    }

    private bool isGrounded(){
        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer)){
            return true;
        }
        return false;
    }

    #endregion
}
