namespace PaGG.Core.Utilities
{
    public static class GeneralUtils
    {
        public const char MaskCharacter = '*';

        public static string MaskStringPrefix(int suffixLength, string input)
        {
            string output = new(MaskCharacter, input.Length - suffixLength);
            output += input.Substring(input.Length - suffixLength, suffixLength);
            return output;
        }
    }
}
