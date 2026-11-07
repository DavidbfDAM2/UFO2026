using UnityEngine;

public class CameraController : MonoBehaviour

{
    // permet assignar el jugador que ha de seguir la camera des de lInspector de Unity
    [SerializeField]
    GameObject player;
    
    private Vector3 offset;                                     // calcula el desplaçament quan comença la partida

    void Start()
    {        
        offset = transform.position - player.transform.position;// resta la posicio del jugador a la de la camera per obtenir el desplaçament
    }

    // actualitzem la posicio de la camera a cada frame
    void Update()
    {        
        transform.position = player.transform.position + offset;// sumem la posicio actual del jugador al desplaçament inicial
    }
}