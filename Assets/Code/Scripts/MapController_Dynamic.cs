using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapController_Dynamic : MonoBehaviour
{
   [Header("UI References")]
   public RectTransform mapParent;
   public GameObject areaPrefab;
   public RectTransform playerIcon;

   [Header("Coulours")] public Color defultColor = Color.gray; //Area on our map that we are not in
   public Color currentAreaColor = Color.green; //Area on our map that we are in
   
   [Header("Map Settings")]
   public GameObject mapBounds; //Parent of area colliders
   public PolygonCollider2D[] initialAreas; //Inital starting areas
   public float mapScale = 10f; //Adjust map size on UI
   
   private PolygonCollider2D[] mapAreas; //Children of MapBounds
   private Dictionary<string, RectTransform> uiAreas = new Dictionary<string, RectTransform>(); //Map each PolygonCollider2D to corrisponding recTransform

   public static MapController_Dynamic instance { get; set; }
   
   private void Awake()
   {
      if (instance == null)
      {
         instance = this;
      }
      else
      {
         Destroy(gameObject);
      }
      
      mapAreas = mapBounds.GetComponentsInChildren<PolygonCollider2D>();
   }
   
   //GenerateMap
   public void GenerateMap(PolygonCollider2D newCurrentArea = null)
   {
      if (initialAreas != null)
      {
         PolygonCollider2D currentArea = newCurrentArea != null ? newCurrentArea :initialAreas[0];

         ClearMap();

         foreach (PolygonCollider2D area in mapAreas)
         {
            CreateAreaUI(area, area == currentArea);
         }
      
         MovePlayerIcon(currentArea.name);
      }
   }
   
   //ClearMap
   private void ClearMap()
   {
      foreach(Transform child in mapParent)
      {
         Destroy(child.gameObject);
      }
      uiAreas.Clear();
   }

   private void CreateAreaUI(PolygonCollider2D area, bool isCurrent)
   {
      //Instantiate prefab  for image
      GameObject areaImage = Instantiate(areaPrefab, mapParent);
      RectTransform rectTransform = areaImage.GetComponent<RectTransform>();
      
      //Get bounds
      Bounds bounds = area.bounds;
      
      //Sale UI image to fit map and bounds 
      rectTransform.sizeDelta = new Vector2(bounds.size.x * mapScale, bounds.size.y * mapScale);
      rectTransform.anchoredPosition = bounds.center * mapScale;
      
      //Set colour based on if current area or not
      areaImage.GetComponent<Image>().color = isCurrent ? currentAreaColor : defultColor;
      
      //Add to dictionary
      uiAreas[area.name] = rectTransform;
      
      
   }

   public void UpdateCurrentArea(string newCurrentArea)
   {
      //Update Colour
      foreach(KeyValuePair<string, RectTransform> area in uiAreas)
      {
         area.Value.GetComponent<Image>().color = area.Key == newCurrentArea ? currentAreaColor : defultColor;
      }
      
      //Move player icon
      MovePlayerIcon(newCurrentArea);
   }

   private void MovePlayerIcon(string newCurrentArea)
   {
      if (uiAreas.TryGetValue(newCurrentArea, out RectTransform areaUI))
      {
         //If current area was found set the icon position to center of area
         playerIcon.anchoredPosition = areaUI.anchoredPosition;
      }
   }
}
