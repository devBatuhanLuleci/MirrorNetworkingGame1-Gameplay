public static class IntExtentions
{
   
    public static string ConvertThisSecondToMinute(this int seconds)
    {
        int minutes = seconds / 60 ;
        int leftoverSeconds = seconds % 60;
        string formatString = leftoverSeconds < 10 ? "{0}:0{1}" : "{0}:{1}";
        return string.Format(formatString, minutes, leftoverSeconds);



    }
}