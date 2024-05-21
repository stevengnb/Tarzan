using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Grappling : MonoBehaviour
{
    [Header("Grappling")]
    private PlayerMovement movement;
    [SerializeField] private Transform cam;
    [SerializeField] private Transform point;
    [SerializeField] private LayerMask grappable;
    [SerializeField] private float maxDistance;
    [SerializeField] private float grappleDelay;
    [SerializeField] private float overshootYAxis;
    [SerializeField] private float grappleCooldown;
    private Vector3 grapplePos;
    private float grappleCooldownTimer;
    private bool grappling;

    [SerializeField] private CinemachineVirtualCamera camm;

    private void Start()
    {
        movement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            GrappleStarts();
        }

        if (grappleCooldownTimer > 0)
        {
            grappleCooldownTimer -= Time.deltaTime;
        }

        if (grappling) {
            camm.m_Lens.FieldOfView = Mathf.Lerp(camm.m_Lens.FieldOfView, 80, 0.35f * Time.deltaTime);
        } else
        {
            camm.m_Lens.FieldOfView = Mathf.Lerp(camm.m_Lens.FieldOfView, 60, 0.35f * Time.deltaTime);
        }
    }

    public bool isGrappling
    {
        get { return grappling; }
    }

    public Vector3 GrapplePos
    {
        get { return grapplePos; }
    }

    public Transform Point {
        get { return point; }
    }

    private void GrappleStarts()
    {
        if(grappleCooldownTimer <= 0)
        {
            grappling = true;
            movement.isFrozen = true;
            RaycastHit hit;
            if (Physics.Raycast(cam.position, cam.forward, out hit, maxDistance, grappable))
            {
                grapplePos = hit.point;
                Invoke("DoGrapple", grappleDelay);
                AudioManagerGame.instance.GrapplingSfx();
            }
            else
            {
                grapplePos = cam.position + cam.forward * maxDistance;
                Invoke("GrappleEnds", grappleDelay);
            }
        } else
        {
            return;
        }
    }

    private void DoGrapple()
    {
        movement.isFrozen = false;

        // menghitung lowest point dibawah character
        Vector3 lowest = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);

        // mencari height difference antara titik grapple dengan lowest point
        float grappleRelativeY = grapplePos.y - lowest.y;

        // highest point ditambah overshootYAxis untuk
        // menentukan seberapa tinggi swingnya di atas grapple position
        // semakin tinggi overshootYAxis, swing dari position sekarang ke grapple position akan semakin tinggi
        float highest = grappleRelativeY + overshootYAxis;

        // kalau ternyata grapple positionnya dibawah character kita
        // highestnya diganti overshootYAxis agar swingnya lebih realistis dan tidak swing kebawah
        if(grappleRelativeY < 0)
        {
            highest = overshootYAxis;
        }

        movement.GrappleTo(grapplePos, highest);
        Invoke("GrappleEnds", 1f);
    }

    public void GrappleEnds()
    {
        grappling = false;
        movement.isFrozen = false;
        grappleCooldownTimer = grappleCooldown;
        AudioManagerGame.instance.StopGrapplingSfx();
    }
}
