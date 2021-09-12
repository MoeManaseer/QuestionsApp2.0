using System;
using LoggerUtils;

namespace QuestionEntities
{
    public static class QuestionsFactory
    {
        public static Question GetInstance(string type = "")
        {
            Question tQuestion = null;

            try
            {
                switch (type)
                {
                    case "Smiley":
                        tQuestion = new SmileyQuestion();
                        break;
                    case "Star":
                        tQuestion = new StarQuestion();
                        break;
                    case "Slider":
                        tQuestion = new SliderQuestion();
                        break;
                    default:
                        tQuestion = new Question();
                        break;
                }
            }
            catch (Exception tException)
            {
                Logger.WriteExceptionMessage(tException);
            }

            return tQuestion;
        }

    }
}
