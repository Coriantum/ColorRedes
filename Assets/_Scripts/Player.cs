using Unity.Netcode;
using UnityEngine;
using System.Collections.Generic;

namespace HelloWorld
{
    public class Player : NetworkBehaviour
    {
       
        private void colors(){
            
            coloresNet.Add(Color.black);
            coloresNet.Add(Color.blue); 
            coloresNet.Add(Color.cyan); 
            coloresNet.Add(Color.green); 
            coloresNet.Add(Color.magenta); 
            coloresNet.Add(Color.red); 
            coloresNet.Add(Color.yellow); 
            coloresNet.Add(Color.gray); 
            coloresNet.Add(new Color(0,0,46,32)); 
            coloresNet.Add(new Color(78,1,1,76));
        }
        
        public NetworkList<Color> coloresNet = new NetworkList<Color>();


        public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();

        public override void OnNetworkSpawn()
        {
             if (IsOwner)
            {
                Move();
                ColorAsig();
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
            Color randomColor = coloresNet[Random.Range(0, coloresNet.Count)];
            GetComponent<Renderer>().material.color = randomColor;
            
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
        void SubmitColorRequestServerRpc(ServerRpcParams rpcParams = default){
            GetRandomColor();
            
        }

        

        static Vector3 GetRandomPositionOnPlane()
        {
           return new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-3f, 3f));
        }


        private void Start() {
            GetRandomColor();
        }


        void Update()
        {
            transform.position = Position.Value;
            
        }
    }
}