using Godot;
using System;
using System.Collections.Generic;

public partial class Levels : Node2D
{

	[Signal] public delegate void LevelInfoFinishedEventHandler();

	private AudioStreamPlayer2D _soundEffect;

	private static Dictionary<string, string[]> _LEVEL_INFOS
	= new Dictionary<string, string[]>() {
		{"level1", new string[] {
		"I am a cat who has freedom to go outside. I love to case butterflies!",
		"I never go far from garden, but this time I think I went bit too far from home.",
		"It is getting dark, and in the forest there is many things what can end my life.",
		"I must hurry back home...",
		"For now, ahead there is just pointy bushes where I can get stuck and die",
		"I have to avoid these bushes jumping over them (pressing 'W' from keyboard)",
		"Let's go home! (HINT! Collect stars to get points!)"
		 }
		 },{ "level2", new string[] {
		"It is getting late, and the home is still far away.",
		"I see that hawks have risen to the sky. I wish they do not notice me!",
		"(Dodge hawks by crawling, pressing 'S' from keyboard)"
		}
		},{ "level3", new string[] {
			"Home is close!",
			"Oh... Wait. There is foxes ahead! I have two options to survive from them.",
			"I need to either protect my self (pressing ´space´), or jump over them (pressing 'w')",
			"I wish I can make it back home. (HINT!: Hittig foxes give you extra points!)"
		}
	}
};
	private MarginContainer _infoContainer;
	private MarginContainer _levelContainer;
	private Label _levelLabel;
	private Label _infoLabel;
	private TextureButton _nextButton;
	private int _infoSentenceIDx;
	private int _levelnumber = 0;
	private string _infoID;

	private Panel _panel;

	private Action _onFinishedCallback;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_levelContainer = GetNode<MarginContainer>("%LevelContainer");
		_infoContainer = GetNode<MarginContainer>("%InfoContainer");
		_panel = GetNode<Panel>("Panel");

		_soundEffect = GetNode<AudioStreamPlayer2D>("InfoContainer/AudioStreamPlayer2D");

		_levelnumber++;
		_infoID = "level" + _levelnumber.ToString();

		_levelLabel = GetNode<Label>("%Level");
		_infoLabel = GetNode<Label>("%Info");

		_nextButton = GetNode<TextureButton>("%Next");
		_nextButton.Pressed += () => getDialogy();

		ShowLevelInfo();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	public void ShowLevelInfo(Action onFinished = null)
	{
		_onFinishedCallback = onFinished;

		_levelContainer.Visible = true;
		_infoContainer.Visible = true;
		_panel.Visible = true;

		_infoSentenceIDx = 0;
		_infoID = "level" + _levelnumber.ToString();

		_levelLabel.Text = "Level: " + _levelnumber.ToString();
		getDialogy();
	}

	public async void getDialogy()
	{
		_soundEffect.Play();
		await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
		string[] sentences = _LEVEL_INFOS[_infoID];

		if (_infoSentenceIDx < sentences.Length)
		{
			_infoLabel.Text = (sentences[_infoSentenceIDx]);
			_infoSentenceIDx++;
		}
		else
		{
			_levelContainer.Visible = false;
			_infoContainer.Visible = false;
			_panel.Visible = false;

			if (_infoID == "level1")
				EmitSignal(SignalName.LevelInfoFinished);
			else
				_onFinishedCallback?.Invoke();

		}
	}

	public void ShowNextLevelInfo(Action onFinished)
	{
		_levelnumber++;
		if (_levelnumber == 4)
		{
			GD.Print("Game finished! Returning to Menu.");
			_onFinishedCallback?.Invoke();
		}
		else
			ShowLevelInfo(onFinished);

	}
}

