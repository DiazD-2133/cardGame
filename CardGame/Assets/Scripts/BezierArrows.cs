using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
namespace Arrow.Bezier
{
    public class BezierArrows : MonoBehaviour
    {
        public GameObject headPrefab; // Prefab of Arrow head
        public GameObject segmentPrefab; // Prefab of Arrow nodes
        public GameObject arrowHeadInstance; // New Arrow head reference
        public int segmentsNum = 10; // Total Arrow's Segments
        public float scaleFactor = 1f; // Arrow scale factor

        private RectTransform origin; // Arrow origin position
        private List<RectTransform> arrowNodes = new List<RectTransform>(); // List of arrow nodes
        private List<Vector2> controlPoints = new List<Vector2>(); // List of Bezier Curve Control Points
        private readonly List<Vector2> controlPointFactors = new List<Vector2> { new Vector2(-0.3f, 0.8f), new Vector2(0.1f, 1.4f) }; // Factors to Calculate Control Points


        public void Awake()
        {
            //Get origin from GameObject RectTransform
            origin = GetComponent<RectTransform>();

            arrowHeadInstance =Instantiate(headPrefab, transform);

            for (int i = 0; i < segmentsNum; i++)
            {
                GameObject neWNode = Instantiate(segmentPrefab, transform);
                arrowNodes.Add(neWNode.GetComponent<RectTransform>());

                // Fill ArrowCollisions nodeList
                arrowHeadInstance.GetComponent<ArrowCollisions>().nodesList.Add(neWNode);
            }

            arrowNodes.Add(arrowHeadInstance.GetComponent<RectTransform>());

            arrowNodes.ForEach(a => a.GetComponent<RectTransform>().position = new Vector2(-1000, -1000));

            for (int i = 0; i < 4; i++)
            {
                controlPoints.Add(Vector2.zero);
            }

            
        }

        public void Update()
        {
            // Set the origin point as the position of the object this script is on
            controlPoints[0] = origin.position;

            // Set target point as mouse position
            controlPoints[3] = Input.mousePosition;

            // Calculate intermediate control points using defined factors
            controlPoints[1] = controlPoints[0] + (controlPoints[3] - controlPoints[0]) * controlPointFactors[0];
            controlPoints[2] = controlPoints[0] + (controlPoints[3] - controlPoints[0]) * controlPointFactors[1];

            // Calculate the position of each node of the arrow on the Bezier curve
            for (int i = 1; i < arrowNodes.Count; i++)
            {
                var t = Mathf.Log(1f * i / (arrowNodes.Count - 1) + 1f, 2f);

                Vector2 pos = Mathf.Pow(1 - t, 3) * controlPoints[0] +
                        3 * Mathf.Pow(1 - t, 2) * t * controlPoints[1] +
                        3 * (1 - t) * Mathf.Pow(t, 2) * controlPoints[2] +
                        Mathf.Pow(t, 3) * controlPoints[3];

                // Set the arrow node position
                arrowNodes[i].position = pos;

                if (i > 0)
                {
                    // Calculate the rotation of the arrow node based on the direction vector between the current node and the previous one
                    var euler = new Vector3(0, 0, Vector2.SignedAngle(Vector2.up, arrowNodes[i].position - arrowNodes[i - 1].position));
                    arrowNodes[i].rotation = Quaternion.Euler(euler);
                }

                // Adjust the scale of the arrow node to create the perspective effect
                var scale = scaleFactor * (1f - 0.03f * (arrowNodes.Count - 1 - i));
                arrowNodes[i].localScale = new Vector3(scale, scale, 1f);
            }

            // Set the rotation of the first node of the arrow equal to that of the second node
            arrowNodes[0].rotation = arrowNodes[1].rotation;
        }
    }
}
