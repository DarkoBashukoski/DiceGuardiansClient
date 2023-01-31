using Microsoft.Xna.Framework.Input;

namespace DiceGuardiansClient.Source.UserInput; 

public static class MouseInput {
    private static MouseState _oldMouse = Mouse.GetState();
    private static MouseState _mouse = Mouse.GetState();

    public static bool CheckSingleClick() {
        return _oldMouse.LeftButton == ButtonState.Released && _mouse.LeftButton == ButtonState.Pressed;
    }

    public static void Update() {
        _oldMouse = _mouse;
        _mouse = Mouse.GetState();
    }
}