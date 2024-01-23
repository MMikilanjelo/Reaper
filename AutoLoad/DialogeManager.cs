using Godot;
using System;

public partial class DialogeManager : Node
{
	[Signal] public delegate void DialogFinishedEventHandler();
	ResourcePreloader _resourcePreloader;
	PackedScene _dialogeBoxScene;
	private Godot.Collections.Array<string> _dialogMessages = new();
	private int _currentLineIndex = 0;
	private bool _isDialogActive = false;
	private bool _canAdvanceLine = false;
	private Vector2 _dialogBoxPosition;
	DialogeBox _dialogBoxInstance;

    public override void _Ready()
    {
        _resourcePreloader = GetNode("ResourcePreloader") as ResourcePreloader;
		_dialogeBoxScene = _resourcePreloader.GetResource("DialogeBox") as PackedScene;
		GD.Print(_dialogeBoxScene);
    }
	public void StartDialog(Vector2 _dialogBoxPosition , Godot.Collections.Array<string> _messages)
	{
		if(_isDialogActive) return;
		_dialogMessages = _messages;
		this._dialogBoxPosition = _dialogBoxPosition;
		ShowTextBox();
		_isDialogActive = true;
	}
	public void ShowTextBox()
	{
		_dialogBoxInstance = _dialogeBoxScene.Instantiate() as DialogeBox;
		AddChild(_dialogBoxInstance);
		
		_dialogBoxInstance.FinishedDisplaying += () => OnDialogBoxFinishedDisplaying();
		_dialogBoxInstance.GlobalPosition = _dialogBoxPosition;
		_dialogBoxInstance.DisplayMessage(_dialogMessages[_currentLineIndex]);
		_canAdvanceLine = false;
	}
	private void OnDialogBoxFinishedDisplaying()
	{
		_canAdvanceLine = true;
	}
    public override void _UnhandledInput(InputEvent @event)
    {
    	if(@event.IsActionPressed("advence_dialog") && _isDialogActive && _canAdvanceLine)
    	{
    		
    		_dialogBoxInstance.QueueFree();
    		_currentLineIndex ++;
    		if(_currentLineIndex >= _dialogMessages.Count)
    		{
    			_isDialogActive = false;
    			_currentLineIndex = 0;
				EmitSignal(SignalName.DialogFinished);
    			return;
    		}
    		ShowTextBox();
    	}
    }



}
