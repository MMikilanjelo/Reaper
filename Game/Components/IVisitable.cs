using Godot;

public interface IVisitable 
{
	void Accept(IVisitor visitor);
}
