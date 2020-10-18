namespace CSE.BL.Interfaces
{
    public interface IDatabaseConnection
    {
        void SetConnection(object dataPath);
        void Dispose();
    }
}
