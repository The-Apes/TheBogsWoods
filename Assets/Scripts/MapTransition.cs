
using Managers;
using Unity.Cinemachine;
using UnityEngine;

public class MapTransition : MonoBehaviour
{
 [SerializeField] private Direction direction;
 private enum Direction { Up, Down, Left, Right }
 [SerializeField] Collider2D mapBoundary;
 private CinemachineConfiner2D _confiner;
 [SerializeField] private float additiveOffset = 2f;
 
 [Header("Sounds")]
 [SerializeField] AudioClip mapChangeSound;

 private void Awake()
 {
  _confiner = FindFirstObjectByType<CinemachineConfiner2D>();
 }
 
 private void OnTriggerEnter2D(Collider2D collision)
 {
  if (!collision.gameObject.CompareTag("Player")) return;
  UpdatePlayerPosition(collision.gameObject); 
  _confiner.BoundingShape2D = mapBoundary; 
   
  MapControllerDynamic.Instance?.UpdateCurrentArea(mapBoundary.name);
  AudioManager.instance.PlaySound(mapChangeSound);
  GameEvents.AreaChanged(gameObject.name);
 } 
 
 private void UpdatePlayerPosition(GameObject player){
  Vector3 newPos = player.transform.position; 
  
  // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
  switch (direction)
  {
   case Direction.Up:
    newPos.y += additiveOffset;
    break;
   case Direction.Down:
    newPos.y -= additiveOffset;
    break;
   case Direction.Left:
    newPos.x -= additiveOffset;
    break;
   case Direction.Right:
    newPos.x += additiveOffset;
    break;
  }
  player.transform.position = newPos;
 }
}
