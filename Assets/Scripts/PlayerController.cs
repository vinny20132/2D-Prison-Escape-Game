using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public LayerMask foregroundElementsLayer;
    public LayerMask interactableLayer;
    public LayerMask portalLayer;
    private bool isMoving;
    private Vector2 input;
    private Animator animator;
    

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    
    public void HandleUpdate()
    {
        if (!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            if (input != Vector2.zero)
            {
                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);
                var targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;
                if (isWalkable(targetPos))
                {
                    StartCoroutine(Move(targetPos));
                    if (Physics2D.OverlapCircle(targetPos, 0.2f, portalLayer) != null)
                    {
                        OnMoveOver(targetPos);
                    }
                }
            }
        }
        animator.SetBool("isMoving", isMoving);
        if(Input.GetKeyDown(KeyCode.Space))
        {
            interact();
        }
        
    }

    void interact()
    {
        var facingDir = new Vector3(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
        var interactPos = transform.position + facingDir;
        var collidedObject = Physics2D.OverlapCircle(interactPos, 0.3f, interactableLayer);
        if(collidedObject != null)
        {
            collidedObject.GetComponent<Interactable>()?.interact();
        }
    }

    void OnMoveOver(Vector3 targetPos)
    {
        var encounteredPortal = Physics2D.OverlapCircle(targetPos, 0.2f, portalLayer);
        var triggerable = encounteredPortal.GetComponent<IPlayerTriggerable>();
        if (triggerable != null)
        {
            triggerable.OnPlayerTriggered(this);
        }
    }



    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;
        while((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;
        isMoving = false;
    }
    private bool isWalkable(Vector3 targetPos)
    {
        if(Physics2D.OverlapCircle(targetPos, 0.2f, foregroundElementsLayer | interactableLayer) != null)
        {
            return false;
        }
        return true;
    }
}
