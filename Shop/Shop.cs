using Godot;
using System;

public partial class Shop : Node2D
{
    DialogeManager _dialogeManager;
    private Godot.Collections.Array<string> _interactionMessages = new Godot.Collections.Array<string>
    {
        "I am Nazar",
        "Bro how are you ?",
        "I am fine mrgl mrgl mrgl",
        "asdaskdaskdaskdaskkasdkadskadskdaskdasksadkkasdkdasasdkaskdadsjkasd;ladsljsdaljasdjdsajdasjadsdasjlsajdl;sajdl;sajdl;asjdl;saj;lasjd;lasdj;lasdjals;djasl;djasdl;jasdl;jasd;ljdas;ldasjd;lsaj",


    };
    public override void _Ready()
    {
        _dialogeManager = GetNode<DialogeManager>("/root/DialogeManager");

    }
    public override void _UnhandledInput(InputEvent @event)
    {
        if(@event.IsActionPressed("interect"))
        {
            _dialogeManager.StartDialog(GlobalPosition , _interactionMessages);
        }
    }


}
