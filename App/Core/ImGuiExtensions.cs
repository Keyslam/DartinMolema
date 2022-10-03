using System.Numerics;
using ImGuiNET;

namespace App.Core;

public static class ImGuiExtensions
{
    public static bool Button(string label)
    {
        return ImGui.Button(label) || (ImGui.IsItemFocused() && ImGui.IsKeyPressed(ImGuiKey.Enter));
    }

    public static bool Button(string label, Vector2 size)
    {
        return ImGui.Button(label, size) || (ImGui.IsItemFocused() && ImGui.IsKeyPressed(ImGuiKey.Enter));
    }

    public static bool KeyboardSelected()
    {
        return ImGui.IsItemFocused() && ImGui.IsKeyPressed(ImGuiKey.Enter);
    }
}