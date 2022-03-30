using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;

namespace HelloWorld
{
    public class Player : NetworkBehaviour
    {
        
        Renderer rend;
        private bool primeraVez;
        
        public static List<Color> coloresNet = new List<Color>();
        public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
        public NetworkVariable<Color> ColorVariable = new NetworkVariable<Color>();


        private void Start() {

            Position.OnValueChanged += OnPositionChange;
            ColorVariable.OnValueChanged += OnColorChange;
            rend = GetComponent<Renderer>();

            if(IsServer && IsOwner){
                coloresNet.Add(Color.black);
                coloresNet.Add(Color.blue);
                coloresNet.Add(Color.cyan);
                coloresNet.Add(Color.magenta);
                coloresNet.Add(Color.red);
                coloresNet.Add(Color.yellow);
                coloresNet.Add(new Color(0,0,46,32));
                coloresNet.Add(new Color(78,1,1,76));
            }
            
            if(IsOwner){
                SubmitColorRequestServerRpc(true);
            }
        }

        public void OnPositionChange(Vector3 previousValue, Vector3 newValue){
            transform.position = Position.Value;
        }

        public void OnColorChange(Color oldColor, Color newColor){
            rend.material.color = newColor;
        }

        public override void OnNetworkSpawn()
        {
             if (IsOwner)
            {
                Move();
            }
        }

        //Metodo que genera color aleatorio
        public void GetRandomColor()
        {
            SubmitColorRequestServerRpc(false);      
        }
        

        public void Move()
        {    
            SubmitPositionRequestServerRpc();
        }

        [ServerRpc]
        void SubmitPositionRequestServerRpc(ServerRpcParams rpcParams = default)
        {
            Position.Value = GetRandomPositionOnPlane();
        }

        [ServerRpc]
        public void SubmitColorRequestServerRpc(bool primeraVez = false, ServerRpcParams rpcParams = default)
        {
            
            Color oldColor = ColorVariable.Value; // Color actual del objeto
            Debug.Log(coloresNet.Count);
            Color newColor = coloresNet[Random.Range(0, coloresNet.Count)]; // Guarda el siguiente color en la variable 
            coloresNet.Remove(newColor); // Quita el color de la lista para que no se pueda volver a escoger.Queda guardada en la variable newColor
            if(! primeraVez){
                coloresNet.Add(oldColor); // Añade el color que se quitó en la lista
            }
            ColorVariable.Value = newColor; // Se asigna el nuevo color
        }
        

        static Vector3 GetRandomPositionOnPlane()
        {
           return new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-3f, 3f));
        }


        void Update()
        {
            
        }
    }
}