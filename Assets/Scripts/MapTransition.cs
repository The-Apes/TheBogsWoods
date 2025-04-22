
using Unity.Cinemachine;
using UnityEngine;

public class MapTransition : MonoBehaviour
{
 [SerializeField] Direction direction;
 private enum Direction { Up, Down, Left, Right }
 [SerializeField] Collider2D mapBoundry;
 CinemachineConfiner2D _confiner;
 [SerializeField] float addetiveOffset = 2f;
 
 [Header("Sounds")]
 [SerializeField] AudioClip mapChangeSound;

 private void Awake()
 {
  _confiner = FindFirstObjectByType<CinemachineConfiner2D>();
 }
 private void OnTriggerEnter2D(Collider2D collision)
 {
  
  if (collision.gameObject.CompareTag("Player"))
  {
   UpdatePlayerPosition(collision.gameObject); 
   _confiner.BoundingShape2D = mapBoundry; 
   
   MapController_Dynamic.instance?.UpdateCurrentArea(mapBoundry.name);
   GameEvents.AreaChanged(gameObject.name);
  }
 } private void UpdatePlayerPosition(GameObject player){
  Vector3 newPos = player.transform.position; 
  // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
  switch (direction)
  {
   case Direction.Up:
    newPos.y += addetiveOffset;
    break;
   case Direction.Down:
    newPos.y -= addetiveOffset;
    break;
   case Direction.Left:
    newPos.x -= addetiveOffset;
    break;
   case Direction.Right:
    newPos.x += addetiveOffset;
    break;
  }
  player.transform.position = newPos;
 }
}
