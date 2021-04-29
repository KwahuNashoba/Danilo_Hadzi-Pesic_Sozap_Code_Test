[System.Serializable]
public class LevelScoreData
{
    public string LevelId;
    public int TotalAttempts = 0; //<- This should not be here!!!
    public float CompletionTime = -1;
    public bool Completed = false;
}
