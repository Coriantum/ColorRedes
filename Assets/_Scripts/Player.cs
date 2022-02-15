using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;

namespace HelloWorld
{
    public class Player : NetworkBehaviour
    {
        
        Renderer rend;
       
        List<Color> colores = new List<Color>()
        {
            Color.black,
            Color.blue, 
            Color.cyan, 
            Color.green, 
            Color.magenta, 
            Color.red, 
            Color.yellow, 
            Color.gray, 
            new Color(0,0,46,32),
            new Color(78,1,1,76)
        };
        
        public NetworkList<Color> coloresNet = new NetworkList<Color>();


        public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();

        public override void OnNetworkSpawn()
        {
             if (IsOwner)
            {
                Move();
                rend = GetComponent<Renderer>();
                GetRandomColor();
            }
        }

        //Metodo que asigna color 
        public void ColorAsig(){
            if(NetworkManager.Singleton.IsServer){
                GetRandomColor();                
            }
            else
            {
                SubmitColorRequestServerRpc();
            }
        }

        //Metodo que genera color aleatorio
        public void GetRandomColor()
        {
            Color randomColor = colores[Random.Range(0, colores.Count)];
            rend.material.color = randomColor;
            
        }
        

        public void Move()
        {
            if (NetworkManager.Singleton.IsServer)
            {
                var randomPosition = GetRandomPositionOnPlane();
                transform.position = randomPosition;
                Position.Value = randomPosition;
            }
            else
            {
                SubmitPositionRequestServerRpc();
            }
        }

        [ServerRpc]
        void SubmitPositionRequestServerRpc(ServerRpcParams rpcParams = default)
        {
            Position.Value = GetRandomPositionOnPlane();
        }

        [ServerRpc]
        void SubmitColorRequestServerRpc(ServerRpcParams rpcParams = default)
        {
            GetRandomColor();    
        }
        

        static Vector3 GetRandomPositionOnPlane()
        {
           return new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-3f, 3f));
        }


        void Update()
        {
            transform.position = Position.Value;
            //rend.material.color = coloresNet;
        }
    }
}