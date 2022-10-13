using System.Numerics;
using ImGuiNET;

namespace App.Core;

public static class ImGuiExtensions
{
	public class FireOnce
	{
		private bool Active;

		public FireOnce(bool active)
		{
			this.Active = active;
		}

		public void MakeActive()
		{
			this.Active = true;
		}

		public bool Consume()
		{
			var active = this.Active;
			this.Active = false;

			return active;
		}
	}

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

	public static void Spacing(int amount)
	{
		for (int i = 0; i < amount; i++)
			ImGui.Spacing();
	}

	public static bool BeginDialogModal(string id)
	{
		bool open = true;

		ImGui.SetNextWindowSize(new Vector2(400, 0));
		ImGui.SetNextWindowPos(ImGui.GetWindowViewport().GetCenter(), ImGuiCond.Appearing, new Vector2(0.5f, 0.5f));
		bool success = ImGui.BeginPopupModal(id, ref open, ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoTitleBar);

		if (success)
		{
			ImGui.Text(id);
			ImGui.Separator();
			ImGuiExtensions.Spacing(5);
		}

		return success;
	}

	public static void EndDialogModal(string cancelName, string confirmName, Action OnYes)
	{
		EndDialogModal(cancelName, confirmName, OnYes, () => { });
	}

	public static void EndDialogModal(string cancelName, string confirmName, Action OnYes, Action OnNo)
	{
		var buttonSize = 120;

		ImGuiExtensions.Spacing(5);

		if (ImGuiExtensions.Button(cancelName, new Vector2(buttonSize, 0)) || ImGui.IsKeyPressed(ImGuiKey.Escape))
		{
			OnNo();
			ImGui.CloseCurrentPopup();
		}

		ImGui.SameLine(ImGui.GetWindowSize().X - buttonSize);

		if (ImGui.IsWindowAppearing())
			ImGui.SetKeyboardFocusHere();

		if (ImGuiExtensions.Button(confirmName, new Vector2(buttonSize, 0)))
		{
			OnYes();
			ImGui.CloseCurrentPopup();
		}
		ImGui.EndPopup();
	}

	public static void CenterText(string text)
	{
		var windowWidth = ImGui.GetWindowSize().X;
		var textWidth = ImGui.CalcTextSize(text).X;

		ImGui.SetCursorPosX((windowWidth - textWidth) * 0.5f);
		ImGui.Text(text);
	}

	public static void RightAlignText(string text, float width)
	{
		var textWidth = ImGui.CalcTextSize(text).X;
		var currentPos = ImGui.GetCursorPos();

		ImGui.SetCursorPosX(currentPos.X + width - textWidth);
		ImGui.Text(text);
	}

	public static uint Color(byte r, byte g, byte b, byte a)
	{
		uint ret = a; ret <<= 8; ret += b; ret <<= 8; ret += g; ret <<= 8; ret += r; return ret;
	}
}