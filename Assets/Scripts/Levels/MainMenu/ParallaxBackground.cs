using UnityEngine;

//Watched a bunch of videos and YouTube channels on how to make a parallax background in Unity:
//Author: Game Code Library
//Availability: https://youtu.be/AoRBZh6HvIk?si=7Z_0nBZOtvlJhfPm

namespace Levels.MainMenu
{
    public class ParallaxBackground : MonoBehaviour
    {
        private Vector2 startPos;
        [SerializeField] private float moveModifier;
        private Camera cam;

        private void Start()
        {
            startPos = transform.position;
            cam = Camera.main;
        }

        private void Update()
        {
            Vector2 pz = cam.ScreenToViewportPoint(Input.mousePosition);

            float posX = Mathf.Lerp(transform.position.x, startPos.x + (pz.x * moveModifier), 2f * Time.deltaTime);
            float posY = Mathf.Lerp(transform.position.y, startPos.y + (pz.y * moveModifier), 2f * Time.deltaTime);

            transform.position = new Vector3(posX, posY, 0);
        }
    }
}
