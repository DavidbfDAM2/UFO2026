using UnityEngine;

public class PickupRotation : MonoBehaviour
{
    // mostra la velocitat a lInspector
    [SerializeField]
    float rotationSpeed = 100.0f;
    
    void Start()
    {
        
    }

    // comprovem a cada frame si el PickUp ha de girar
    void Update()
    {        
        if(Time.timeScale > 0.0f)       // un valor de temps positiu indica que la partida esta en marxa
        {            
            transform.Rotate(0, 0, 1);  // gira un grau al voltant de l'eix Z del PickUp
        }        
    }
}