public enum GameState
{
	Menu = 0,
	Praia = 1,
	Playing = Praia | MiniGame,
	MiniGame = 2,
	Vila = 3
}