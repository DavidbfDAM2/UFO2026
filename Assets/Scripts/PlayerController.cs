using Unity.VisualScripting;
using UnityEngine;
using TMPro;                        // importa TextMesh Pro
using UnityEngine.SceneManagement;  // SceneManager serveix per consultar i carregar escenes

public class PlayerController : MonoBehaviour
{
    // força del moviment. 'SerializeField' ens permet configurar aquests camps a lInspector
    [SerializeField]
    float speed = 5;
    
    // text de TextMesh Pro on es mostra la puntuacio
    [SerializeField]
    TextMeshProUGUI punts;

    // panell de pausa que apareix en apretar 'Esc' o en guanyar la partida
    [SerializeField]
    GameObject menuPausa;
    
    // missatge que apareix quan es recullen tots els PickUps
    [SerializeField]
    GameObject textVictoria;

    // panell que es mostra abans de tancar el joc
    [SerializeField]
    GameObject pantallaSortida;

    
    int points = 0;         // comptador per als PickUps que ha recollit el jugador    
    int totalPickups = 0;   // nombre de PickUps que hi ha en lescena quan comença la partida    
    int frameSortida = -1;  // frame de sortida. amb el valor -1 indiquem que encara no sha programat

    private Rigidbody2D rb2d;    // guarda el component fisic del jugador per poderhi aplicar força


    // preparem el component fisic i comptem els PickUps al començar la partida
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();                                 // obtenim el 'Rigidbody2D' del mateix objecte on hi ha aquest script
        totalPickups = GameObject.FindGameObjectsWithTag("PickUp").Length;  // comptem els objectes actius que tenen letiqueta 'PickUp'
    }
    
    // comprovem la sortida programada i la tecla 'Esc' a cada frame
    private void Update()
    {        
        if(Time.frameCount == frameSortida)                                 // comparem el fotograma actual amb el que hem reservat per sortir
        {
            Application.Quit();  
        }
        if (Input.GetKeyDown(KeyCode.Escape) && (points < totalPickups) )   // detectem pulsacio de la tecla 'Esc' mentre quedin PickUps per recollir
        {
            bool obrirMenu = !menuPausa.activeSelf;                         // canviem estat del panell per obrir o tancarlo
            PausarPartida(obrirMenu);                                       // apliquem el nou estat al menu i al temps de la partida
        }
    }

    // llegim els controls i movem el jugador a cada pas de fisica
    void FixedUpdate()
    {        
        float moveHorizontal    = Input.GetAxis("Horizontal");          // llegim les entrades de moviment
        float moveVertical      = Input.GetAxis("Vertical");        
                
        Vector2 movement = new Vector2(moveHorizontal, moveVertical);   // combinem els dos eixos en un vector de moviment 2D      
        rb2d.AddForce(movement * speed);                                // apliquem al cos fisic el vector de moviment multiplicat per la speed
    }
    
    // gestio del contacte del jugador amb un trigger 2D
    private void OnTriggerEnter2D(Collider2D collision)
    {        
        if (collision.gameObject.CompareTag("PickUp")) // comprovem si lobjecte amb que ha colisionat te letiqueta PickUp
        {
            //Debug.Log("Xoca amb un PickUp");  <---> recomanable per comprovar si la colisio funciona
            
            points++;                               // sumem un punt per lobjecte recollit            
            punts.text = "Puntuació: " + points;    // actualitzem el text del marcador            
            Destroy(collision.gameObject);          // eliminem el PickUp recollit de lescena            
            
            if(points == totalPickups)              // comprovem si el jugador ha recollit tots els PickUps
            {
                textVictoria.SetActive(true);       // mostrem missatge de victoria                
                PausarPartida(true);                // obrim el menu de pausa i aturem el temps de la partida
            }
        }        
    }
    
    // boto 'Reiniciar' per tornar a carregar lescena inicial
    public void Reiniciar() 
    {
        Time.timeScale = 1.0f;                                          // restablim lescala de temps normal abans de reiniciar
        int indexEscena = SceneManager.GetActiveScene().buildIndex;     // obtenim lindex de lescena activa dins la llista descenes de la build
        SceneManager.LoadScene(indexEscena);                            // carreguem lescena a partir del seu index
    }
    
    // boto 'Sortir' per tancar el joc
    public void Sortir() 
    {        
        pantallaSortida.SetActive(true);    // activem el panell de sortida xk Unity el pinti        
        frameSortida = Time.frameCount + 1; // reservem el frame seguent per tancar el joc des de Update()
    }
    
    
    // ajustem el temps de partida quan obrim o tanquem el menu
    private void PausarPartida(bool obrirMenu) 
    {
        menuPausa.SetActive(obrirMenu);         // activem o desactivem el panell segons el valor de obrirMenu

        if (obrirMenu)                          // escollim lescala de temps segons si el menu es obert o tancat
        {
            Time.timeScale = 0.0f;              // menu obert, escala 0.0 per aturar el joc
        }
        else
        {
            Time.timeScale = 1.0f;              // menu tancat, valor 1.0 continua el joc en velocitat normal
        }
    }
}