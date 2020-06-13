namespace GaudiaVedantaPublications
{
    /// <summary>
    /// Interface that represents a mapping for text transformation.
    /// Used in <see cref="MappingTextTransform"/>.
    /// </summary>
    public interface ITextMapping
    {
        string Apply(string text);

        ITextMapping Inverted
        {
            get;
        }
    }
}
