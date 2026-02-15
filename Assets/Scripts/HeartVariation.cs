// UMD IMDM290 
// Instructor: Myungin Lee
    // [a <-----------> b]
    // Lerp : Linearly interpolates between two points. 
    // https://docs.unity3d.com/6000.3/Documentation/ScriptReference/Vector3.Lerp.html

using UnityEngine;
using static UnityEngine.Rendering.HableCurve;

public class HeartVariation : MonoBehaviour
{
    GameObject[] spheres;
    static int numSphere = 200; 
    float time = 0f;
    Vector3[] startPosition, endPosition, midrandomPosition, wavePosition;

    // Start is called before the first frame update
    void Start()
    {
        // Assign proper types and sizes to the variables.
        spheres = new GameObject[numSphere];
        startPosition = new Vector3[numSphere]; 
        endPosition = new Vector3[numSphere];
        midrandomPosition = new Vector3[numSphere];
        wavePosition = new Vector3[numSphere];
        
        // Define target positions. Start = random, then = heart, then = random, End = wave
        for (int i =0; i < numSphere; i++){
            // Random start positions
            float r = 10f;
            startPosition[i] = new Vector3(r * Random.Range(-1f, 1f), r * Random.Range(-1f, 1f), r * Random.Range(-1f, 1f));        

            r = 3f; // radius of the circle
                    // Circular end position
            float sin = Mathf.Sin(i * 2 * Mathf.PI / numSphere);
            float cos = Mathf.Cos(i * 2 * Mathf.PI / numSphere);
            float a = 5f;
            endPosition[i] = new Vector3(Mathf.Sqrt(2f) * a * sin * sin * sin, a * (-1 * cos * cos * cos - cos * cos + 2 * cos), 10f);

            r = 10f;
            midrandomPosition[i] = new Vector3(r * Random.Range(-1f, 1f), r * Random.Range(-1f, 1f), r * Random.Range(-1f, 1f));
            
            wavePosition[i] = new Vector3((i - 100) * 0.1f, Mathf.Sin(i * 2f) * 3f, 2f);
        }
        // Let there be spheres..
        for (int i =0; i < numSphere; i++){
            // Draw primitive elements:
            // https://docs.unity3d.com/6000.0/Documentation/ScriptReference/GameObject.CreatePrimitive.html
            spheres[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere); 

            // Position
            spheres[i].transform.position = startPosition[i];

            // Color. Get the renderer of the spheres and assign colors.
            Renderer sphereRenderer = spheres[i].GetComponent<Renderer>();
            // HSV color space: https://en.wikipedia.org/wiki/HSL_and_HSV
            float hue = (float)i / numSphere; // Hue cycles through 0 to 1
            Color color = Color.HSVToRGB(hue, 1f, 1f); // Full saturation and brightness
            sphereRenderer.material.color = color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Measure Time 
        time += Time.deltaTime; // Time.deltaTime = The interval in seconds from the last frame to the current one
        // what to update over time?
        for (int i =0; i < numSphere; i++){
            // Lerp : Linearly interpolates between two points.
            // https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Vector3.Lerp.html
            // Vector3.Lerp(startPosition, endPosition, lerpFraction)
            
            // lerpFraction variable defines the point between startPosition and endPosition (0~1)
            // let it oscillate over time using sin function
            float lerpFraction = Mathf.Sin(time * 0.7f) * 0.5f + 0.5f;
            float lerpFrac2 = lerpFraction * 3f;
            int segment = Mathf.FloorToInt(lerpFrac2);
            float local = lerpFrac2 - segment;


            // Lerp logic. Update position
            Vector3 changingPosition;
            if (segment == 0)
            {
                changingPosition = Vector3.Lerp(startPosition[i], endPosition[i], local);
            }
            else if (segment == 1)
            {
                changingPosition = Vector3.Lerp(endPosition[i], midrandomPosition[i], local);
            }
            else
            {
                changingPosition = Vector3.Lerp(midrandomPosition[i], wavePosition[i], local);
            }
            spheres[i].transform.position = changingPosition;
            // For now, start positions and end positions are fixed. But what if you change it over time?
            // startPosition[i]; endPosition[i];

            // Color Update over time
            Renderer sphereRenderer = spheres[i].GetComponent<Renderer>();
            float hue = (float)i / numSphere; // Hue cycles through 0 to 1
            Color color = Color.HSVToRGB(Mathf.Abs(hue * Mathf.Sin(time)), Mathf.Cos(time), 2f + Mathf.Cos(time)); // Full saturation and brightness
            sphereRenderer.material.color = color;
        }
    }
}
