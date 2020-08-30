namespace RPG.Saving
{
    interface ISaveable
    {
        object CaptureState();
        void RestoreState(object state);
    }
}