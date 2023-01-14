using DiceGuardiansClient.Source.Entities;
using Microsoft.Xna.Framework;

namespace DiceGuardiansClient.Source.Toolbox;

public class Maths {
    public static Matrix CreateWorldMatrix(Vector3 translation, Vector3 rotation, float scale) {
        Matrix matrix = Matrix.Identity;
        matrix.Translation = translation;
        matrix = Matrix.Multiply(Matrix.CreateRotationX(MathHelper.ToRadians(rotation.X)), matrix);
        matrix = Matrix.Multiply(Matrix.CreateRotationY(MathHelper.ToRadians(rotation.Y)), matrix);
        matrix = Matrix.Multiply(Matrix.CreateRotationZ(MathHelper.ToRadians(rotation.Z)), matrix);
        matrix = Matrix.Multiply(Matrix.CreateScale(scale), matrix);
        return matrix;
    }

    public static Matrix CreateViewMatrix(Camera camera) {
        Matrix matrix = Matrix.Identity;
        matrix = Matrix.Multiply(Matrix.CreateRotationX(MathHelper.ToRadians(camera.GetRotation().X)), matrix);
        matrix = Matrix.Multiply(Matrix.CreateRotationY(MathHelper.ToRadians(camera.GetRotation().Y)), matrix);
        matrix = Matrix.Multiply(Matrix.CreateRotationZ(MathHelper.ToRadians(camera.GetRotation().Z)), matrix);
        Vector3 negativeCameraPos = camera.GetPosition() * -1;
        matrix = Matrix.Multiply(Matrix.CreateTranslation(negativeCameraPos), matrix);
        return matrix;
    }
}