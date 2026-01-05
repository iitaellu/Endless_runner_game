using Godot;
using System;
using System.Collections.Generic;

public partial class GlobalScore : Node
{

	public int Currentcore { get; private set; } = 0;

	public List<int> TopScores { get; private set; } = new List<int>() { 0, 0, 0 };

	public int LastScoreRank { get; private set; } = -1;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void AddScore(int amount)
	{
		Currentcore += amount;
	}

	public void ResetScore()
	{
		Currentcore = 0;
	}
	
	public void SaveScore()
    {
		TopScores.Add(Currentcore);
		TopScores.Sort((a, b) => b.CompareTo(a));
		LastScoreRank = TopScores.IndexOf(Currentcore);

		if (TopScores.Count > 3)
        {
			TopScores.Remove(3);
        }
		 // Jos ei päässyt top 3:een
		if (LastScoreRank > 2)
			LastScoreRank = -1;
    }
}
