using Godot;
using System;

public partial class Credits : Node
{

	[Export]

	private TextureButton _backButton;

	private AudioStreamPlayer2D _soundEffect;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_backButton = GetNode<TextureButton>("%Back");
		_soundEffect = GetNode<AudioStreamPlayer2D>("BackContainer/AudioStreamPlayer2D");
		//_backButton.Pressed += () => GetTree().ChangeSceneToFile("res://Scenes/Menu.tscn");
		_backButton.Pressed += OnBackPressed;

	}
	
	 private async void OnBackPressed()
    {
		_soundEffect.Play();
		await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
        GetTree().ChangeSceneToFile("res://Scenes/Menu.tscn");
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
