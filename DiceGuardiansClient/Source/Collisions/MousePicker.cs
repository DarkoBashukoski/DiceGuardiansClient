using DiceGuardiansClient.Source.Entities;
using DiceGuardiansClient.Source.RenderEngine;
using DiceGuardiansClient.Source.Toolbox;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace DiceGuardiansClient.Source.Collisions; 

public class MousePicker {
    private Vector3 _currentRay;
    private readonly Matrix _projectionMatrix;
    private Matrix _viewMatrix;

    private readonly Camera _camera;

    public MousePicker(Camera cam, Matrix projectionMatrix) {
        _camera = cam;
        _projectionMatrix = projectionMatrix;
        _viewMatrix = Maths.CreateViewMatrix(_camera);
    }

    public Vector3 GetRay() {
        return _currentRay;
    }

    public void Update() {
        _viewMatrix = Maths.CreateViewMatrix(_camera);
        _currentRay = CalculateRay();
    }

    private Vector3 CalculateRay() {
        float mouseX = Mouse.GetState().X;
        float mouseY = Mouse.GetState().Y;
        Vector2 normalized = CalculateNormalizedCoords(mouseX, mouseY);
        Vector4 clipCoords = new Vector4(normalized.X, normalized.Y, -1f, 1f);
        Vector4 eyeCoords = ToEyeCoords(clipCoords);
        Vector3 mouseRay = ToWorldCoords(eyeCoords);
        return mouseRay;
    }

    private Vector3 ToWorldCoords(Vector4 eyeCoords) {
        Matrix inverted = Matrix.Invert(_viewMatrix);
        Vector4 worldCoords = Vector4.Transform(eyeCoords, inverted);
        Vector3 mouseRay = new Vector3(worldCoords.X, worldCoords.Y, worldCoords.Z);
        mouseRay.Normalize();
        return mouseRay;
    }

    private Vector4 ToEyeCoords(Vector4 clipCoords) {
        Matrix inverted = Matrix.Invert(_projectionMatrix);
        Vector4 eyeCoords = Vector4.Transform(clipCoords, inverted);
        return new Vector4(eyeCoords.X, eyeCoords.Y, -1, 0);
    }

    private Vector2 CalculateNormalizedCoords(float mouseX, float mouseY) {
        float x = (2f * mouseX) / DisplayManager.GetWidth() - 1;
        float y = (2f * mouseY) / DisplayManager.GetHeight() - 1;
        return new Vector2(x, -y);
    }
}