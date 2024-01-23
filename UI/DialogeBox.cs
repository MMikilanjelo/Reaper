using Godot;
using System;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

public partial class DialogeBox : Node2D
{
	[Signal] public delegate void FinishedDisplayingEventHandler();
	private Label _messageLable;
	private Timer _displayTimer;
	private MarginContainer _marginContainer;
	private string text = "";
	private int _letterIndex = 0;
	
	private double _letterTime = 0.03f;
	private double _spaceTime = 0.06f;
	private float _punctuationTime = 0.2f;
    public override void _Ready()
    {

		_displayTimer = GetNode<Timer>("MarginContainer/Timer");
		_messageLable = GetNode<Label>("MarginContainer/MarginContainer/Label");
		_marginContainer = GetNode<MarginContainer>("MarginContainer/MarginContainer");
        _displayTimer.Timeout += () => DisplayLetter();
		_marginContainer.Resized += () => OnMessageBoxResized();
    }
	private void OnMessageBoxResized()
	{
		
	}
    public void DisplayMessage(string _messageToDisplay)
	{
		text = _messageToDisplay;
		ResetLabel();
		DisplayLetter();
	}
	private void ResetLabel()
	{
		_messageLable.Text = "";
	}

	public void SetDialogBoxSize (Vector2 _newSize)
	{
		_marginContainer.CustomMinimumSize = _newSize;
	}
	private void DisplayLetter()
	{
		_messageLable.Text += text[_letterIndex];
		_letterIndex += 1 ;
		if(_letterIndex >= text.Length)
		{
			EmitSignal(SignalName.FinishedDisplaying);
			return;
		}
		char currentChar = text[_letterIndex];
		switch (currentChar)
		{
			case '!':
			case '.':
			case ',':
			case '?':
				_displayTimer.Start(_punctuationTime);
				break;
			case ' ':
				_displayTimer.Start(_spaceTime);
				break;
			default :
				_displayTimer.Start(_letterTime);
				break;
		}
	}
}


