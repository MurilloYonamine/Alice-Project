using UnityEngine;

public class AliceEyes : MonoBehaviour {
    private Camera _mainCamera => Camera.main;
    [SerializeField] private RectTransform _eyes;
    [SerializeField] private RectTransform _leftEye;
    [SerializeField] private RectTransform _rightEye;
    [SerializeField] private float _adjustPosition = 3f;
    [SerializeField] private float _maxEyeMovement = 50f; 
    void Update() {
        HandlePlayerEyes();
    }

    private void HandlePlayerEyes() {
        Vector2 mousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _eyes.parent as RectTransform, 
            Input.mousePosition, 
            _mainCamera, 
            out mousePosition
        );

        Vector2 adjustedPosition = mousePosition * _adjustPosition;

        // Limita o movimento dos olhos dentro de um círculo
        if (adjustedPosition.magnitude > _maxEyeMovement) {
            adjustedPosition = adjustedPosition.normalized * _maxEyeMovement;
        }

        _eyes.localPosition = adjustedPosition;
    }
}
