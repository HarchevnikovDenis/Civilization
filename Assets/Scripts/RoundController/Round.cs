using System;

public static class Round
{
    private static RoundController roundController;
    private static bool isOperationInProcess;
    private static bool isGameProcessing;

    public static void InitRoundController(RoundController controller)
    {
        roundController = controller;
    }

    public static void StartGame()
    {
        isGameProcessing = true;

        CheckController();
        roundController.StartGame();
    }

    public static void SkipTurn()
    {
        if (isOperationInProcess || !isGameProcessing) return;

        CheckController();

        isOperationInProcess = true;
        roundController.SkipTurn();
        isOperationInProcess = false;
    }

    public static void KillNextEnemy()
    {
        if (isOperationInProcess || !isGameProcessing) return;

        CheckController();

        isOperationInProcess = true;
        roundController.KillEnemy();
        isOperationInProcess = false;
    }

    public static void CompleteGame(EnemySide lostSide)
    {
        isGameProcessing = false;
        CheckController();
        roundController.CompleteGame(lostSide);
    }

    private static void CheckController()
    {
        if (!roundController) throw new NullReferenceException("RoundController is null");
    }
}
