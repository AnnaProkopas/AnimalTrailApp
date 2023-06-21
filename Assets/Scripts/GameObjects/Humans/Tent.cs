using PlayerModule;
using UnityEngine;

namespace GameObjects.Humans
{
    public class Tent : MonoBehaviour, IPlayerTriggered, ISavable
    {
        private readonly TriggeredObjectType type = TriggeredObjectType.Default;

        public TriggeredObjectType Type { get => type; }

        public Vector3 GetPosition()
        {
            return transform.position;
        }
    
        public GameObject GetGameObject() {
            return gameObject;
        }
    
        public GameObject human;
    
        void Start()
        {

        }

        void Update()
        {

        }

        public void OnPlayerTriggerEnter(Player player, PlayerState playerState)
        {
            Spawn(player.GetPosition() + (new Vector2(1, 0)));
            // switch (state)
            // {
            //     case PlayerState.Attack:
            //         break;
            //     default:
            //         // player.onAttack += OnAttack;
            //         break;
            // }
        }

        public void OnPlayerTriggerExit(Player player, PlayerState playerState)
        {
            // player.onAttack -= OnAttack;
        }

        // private CollisionResult OnAttack()
        // {
        //     CollisionResult res = new CollisionResult();
        //     return res;
        // }
    
        private void Spawn(Vector2 position) 
        {
            Instantiate(human, position, Quaternion.identity);
        }
    }
}
