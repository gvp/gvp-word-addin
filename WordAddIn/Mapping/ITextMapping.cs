namespace GaudiaVedantaPublications
{
    public interface ITextMapping
    {
        string Apply(string text);

        ITextMapping Inverted
        {
            get;
        }
    }
}
