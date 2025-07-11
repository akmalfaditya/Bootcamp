namespace MyGame;

public interface ICard
{
	int GetValue();
	string GetDescription();
	bool Equals(object? card);
	int GetHashCode();
	CardStatus GetStatus();
	void SetStatus(CardStatus status);
}
