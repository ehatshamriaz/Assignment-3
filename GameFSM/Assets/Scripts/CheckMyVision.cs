using UnityEngine;

public class CheckMyVision : MonoBehaviour
{
    
    public enum enmSensitivity { HIGH, LOW}; 
    public enmSensitivity sensitivity = enmSensitivity.HIGH; 
    public bool targetInSight = false; 
    public float fieldOfVision = 45f; 
    private Transform target = null; 
    public Transform myEyes = null; 
    private Transform npcTransform = null; 
    private SphereCollider sphereCollider = null; 
    public Vector3 lastKnownSighting = Vector3.zero;

    private void Awake()
    {
        npcTransform = GetComponent<Transform>();
        sphereCollider = GetComponent<SphereCollider>();
        lastKnownSighting = npcTransform.position;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>(); //Okay we shall tag this later

    }

    bool InMyFieldOfVision()
    {
        Vector3 dirToTarget = target.position - myEyes.position; 
        float angle = Vector3.Angle(myEyes.forward, dirToTarget); 
        if (angle <= fieldOfVision)
        {
           // Debug.Log("Player in my field of vision");
            return true;
        }
        else
            return false;
    } 
    bool ClearLineofSight()
    {
        RaycastHit hit;

        if (Physics.Raycast(myEyes.position, 
            (target.position - myEyes.position).normalized,
            out hit, sphereCollider.radius))
        {
            if (hit.transform.CompareTag("Player"))
            {
                //Debug.Log("Player Clear Line of Sight");
                return true;
            }
            
        }
        return false;


    }

    void UpdateSight()
    {
        switch (sensitivity)
        {
            case enmSensitivity.HIGH:
                targetInSight = InMyFieldOfVision() && ClearLineofSight();
                break;
            case enmSensitivity.LOW:
                targetInSight = InMyFieldOfVision() || ClearLineofSight();
                break;

        }
    }

    private void OnTriggerStay(Collider other)
    {
        UpdateSight();
        if (targetInSight)
        { 
            lastKnownSighting = target.position;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
          return;
      targetInSight = false;
    }

}
