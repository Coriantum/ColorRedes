using Unity.Netcode;
using UnityEngine;

namespace HelloWorld
{
    public class Player : NetworkBehaviour
    {

        public Color[] colores = {
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


        public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();

        public override void OnNetworkSpawn()
        {
             if (IsOwner)
            {
                Move();
            }
        }

        //Metodo que asigna color 
        public void ColorAsig(){
            if(NetworkManager.Singleton.IsServer){
                int randomColor = Random.Range(0, colores.Length);

                //Vincular color al gameobject
                colores.SetValue(gameObject,randomColor);
            }
            else
            {
                SubmitColorRequestServerRpc();
            }
        }

        //Metodo que genera color aleatorio
        public void GetRandomColor()
        {
            int randomColor = Random.Range(0, colores.Length);
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
            //colores.Value = GetRandomColor();
        }

        static Vector3 GetRandomPositionOnPlane()
        {
           return new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-3f, 3f));
        }

        void Update()
        {
            transform.position = Position.Value;
        }
    }
}