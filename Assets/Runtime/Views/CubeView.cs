using UnityEngine;


namespace Assets.Runtime.Views
{

    
    public class CubeView : MonoBehaviour
    {

        float rotationSpeed = 1f;

        private void OnMouseDrag()
        {
            float xAxisRotation = Input.GetAxis("Mouse X") * rotationSpeed;
            float yAxisRotation = Input.GetAxis("Mouse Y") * rotationSpeed;

            transform.Rotate(Vector3.down, xAxisRotation);
            transform.Rotate(Vector3.right, yAxisRotation);
        }
    }
}