using UnityEngine;

namespace Game.Scripts.Cubes
{
    [CreateAssetMenu(fileName = "CubeType", menuName = "Game/CubeType", order = 0)]
    public class CubeType : ScriptableObject
    {
        [SerializeField] private Sprite cubeSprite;

        public Sprite CubeSprite => cubeSprite;
    }
}
