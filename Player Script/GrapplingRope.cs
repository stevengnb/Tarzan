using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingRope : MonoBehaviour
{
    private Vector3 currentGrapplePos;
    private Spring spring;
    private LineRenderer line;
    private Grappling grapplingGun;
    [SerializeField] private int quality;
    [SerializeField] private float damper;
    [SerializeField] private float strength;
    [SerializeField] private float velocity;
    [SerializeField] private float waveCount;
    [SerializeField] private float waveHeight;
    [SerializeField] private AnimationCurve curve;

    private void Awake()
    {
        grapplingGun = GetComponentInParent<Grappling>();
        line = GetComponent<LineRenderer>();
        spring = new Spring();
        spring.SetTarget(0);
    }

    private void LateUpdate()
    {
        DrawRope();
    }


    public void DrawRope()
    {
        if(!grapplingGun.isGrappling)
        {
            currentGrapplePos = grapplingGun.Point.position;
            spring.Reset();

            if(line.positionCount > 0)
            {
                line.positionCount = 0;
            }

            return;
        }

        if(line.positionCount == 0)
        {
            spring.SetVelocity(velocity);
            line.positionCount = quality + 1;
        }

        spring.SetDamper(damper);
        spring.SetStrength(strength);
        spring.Update(Time.deltaTime);

        var grapplePosition = grapplingGun.GrapplePos;
        var gunPosition = grapplingGun.Point.position;
        var up = Quaternion.LookRotation(grapplePosition - gunPosition).normalized * Vector3.up;

        currentGrapplePos = Vector3.Lerp(currentGrapplePos, grapplePosition, Time.deltaTime * 12f);
        
        for(var i = 0; i < quality + 1; i++)
        {
            var delta = i / (float) quality;
            var offset = up * waveHeight * Mathf.Sin(delta * waveHeight * Mathf.PI * spring.Value * curve.Evaluate(delta));

            line.SetPosition(i, Vector3.Lerp(gunPosition, currentGrapplePos, delta) + offset);
        }
    }
}
