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
		"Hello! You are cat who has freedom to go outside",
		"You have traveled a far from your home and it is time to go back",
		"However, the forest is full of danger!",
		"For now, ahead there is just pointy bushes where you can get stuck and die",
		"Avoid bushes jumping over them pressing 'W' from your keyboard"
		 }
		 },{ "level2", new string[] {
		"It is getting late, and the home is still far away.",
		"Hawks have notised you and rised on the sky trying chatch you",
		"Dodge hawks by chrunching, pressing 'S' from your keyboard"
		}
		},{ "level3", new string[] {
			"Home is close, but now also foxes are after you",
			"You can jump over them pressing 'W' but you can also hit them pressing 'space' button"
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

	private Action _onFinishedCallback;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_levelContainer = GetNode<MarginContainer>("%LevelContainer");
		_infoContainer = GetNode<MarginContainer>("%InfoContainer");

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

		_infoSentenceIDx = 0;
		_infoID = "level" + _levelnumber.ToString();

		_levelLabel.Text = "Level: " + _levelnumber.ToString();
		getDialogy(); //getDialogy("level" + _levelnumber.ToString()); }
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
			//GetTree().ChangeSceneToFile("res://Scenes/Menu.tscn");
			_onFinishedCallback?.Invoke();
		}
		else
			ShowLevelInfo(onFinished);
		//_levelLabel.Text = "Level: " + _levelnumber.ToString();

	}

	/*public void ShowNextLevelInfo(Action onFinished)
	{
		_levelContainer.Visible = true;
		_infoContainer.Visible = true;

		_levelnumber++;
		_infoSentenceIDx = 0;
		_infoID = "level" + _levelnumber.ToString();

		_levelLabel.Text = "Level: " + _levelnumber;
		string[] sentences = _LEVEL_INFOS[_infoID];

		_infoLabel.Text = sentences[_infoSentenceIDx];
		_infoSentenceIDx++;

		// Remove previous connections to avoid duplicates

		// Handle next button manually for this new level
		_nextButton.Pressed += () =>
		{
			if (_infoSentenceIDx < sentences.Length)
			{
				_infoLabel.Text = sentences[_infoSentenceIDx];
				_infoSentenceIDx++;
			}
			else
			{
				// Hide info UI
				_levelContainer.Visible = false;
				_infoContainer.Visible = false;

				// Disconnect the handler so it doesn’t repeat
				_nextButton.Pressed -= getDialogy;

				// ✅ Now call the callback — after the player finishes reading
				onFinished?.Invoke();
			}
		};
	}*/
}

