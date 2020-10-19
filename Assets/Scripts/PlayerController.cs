using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[SelectionBase]
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement properties")]
    [SerializeField] float jumpPower = 5f;
    [SerializeField] float movementSpeed = 3f;
    [SerializeField] float lookSensitivity = 1f;

    [Tooltip("Minimal rotation angle.")] [SerializeField] float maximumY = 80f;
    [Tooltip("Maximal rotation angle.")] [SerializeField] float minimumY = -50f;

    [Header("Building")]
    [SerializeField] GameObject[] buildBlocks = default;
    [SerializeField] float buildingRange = 10;


    [Header("Body objects")]
    [SerializeField] GameObject head;


    private Rigidbody rb;
    private float rotationX, rotationY;
    private Vector2 moveAxis;
    private bool isSecondaryHitting;
    private bool isPrimaryHitting;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Move();
        ResolveAttack();
    }

    private void ResolveAttack()
    {
        RaycastHit target;
        var hit = Physics.Raycast(head.transform.position, head.transform.forward, out target, buildingRange);
        if (hit)
        {
            var targetTile = target.collider.gameObject.GetComponent<Tile>();
            Debug.Log(target.collider.gameObject.name);
            if (targetTile != null)
            {
                if (isPrimaryHitting)
                {
                    //todo building
                }
                else if (isSecondaryHitting)
                {
                    targetTile.Hit();
                }
            }
        }
    }

    private void Move()
    {
        var delta = rb.rotation * new Vector3(moveAxis.x, 0, moveAxis.y) * movementSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + delta);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        var change = context.ReadValue<Vector2>();
        moveAxis = change;
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        var change = context.ReadValue<Vector2>();
        rotationX += change.x * lookSensitivity * Time.deltaTime;
        rotationY = Mathf.Clamp(rotationY + change.y * lookSensitivity * Time.deltaTime, minimumY, maximumY);
        Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
        Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, -Vector3.right);
        transform.localRotation = Quaternion.identity * xQuaternion;
        head.transform.localRotation = Quaternion.identity * yQuaternion;
    }


    public void OnJump(InputAction.CallbackContext context)
    {
        rb.AddForce(Vector3.up * jumpPower * 1000);
    }

    public void OnPrimaryAttack(InputAction.CallbackContext context)
    {
        isPrimaryHitting = context.ReadValueAsButton();
        Debug.Log("PrimaryAttack");
    }

    public void OnSecondaryAttack(InputAction.CallbackContext context)
    {
        isSecondaryHitting = context.ReadValueAsButton();
        Debug.Log("SecondaryAttack");
    }
}
