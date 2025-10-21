using Godot;
using System;

public partial class Menu : Node
{

	[Export]

	private TextureButton _startButton;
	private TextureButton _exitButton;
	private TextureButton _creditsButton;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
    {
		_startButton = GetNode<TextureButton>("%Start");
		_exitButton = GetNode<TextureButton>("%Exit");
		_creditsButton = GetNode<TextureButton>("%Credits");


		_startButton.Pressed += () => GetTree().ChangeSceneToFile("res://Scenes/main.tscn");
		_exitButton.Pressed += () => GetTree().Quit();
		_creditsButton.Pressed += () => GetTree().ChangeSceneToFile("res://Scenes/credits.tscn");
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}
}
