using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOProjectBasedLeaning
{
    public abstract class LocalizeException : Exception
    {
        private Dictionary<string, string> localizedMessages = new Dictionary<string, string>();

        public LocalizeException()
        {

        }

        public LocalizeException(string? message) : base(message)
        {

        }

        public LocalizeException(string? message, Exception? innerException) : base(message, innerException)
        {

        }

        public override string Message { get { return AcquireMessage(); } }

        protected void AddLocalizedMessage(string locale, string message)
        {

            localizedMessages.Add(locale.ToString(), message);

        }

        private string AcquireMessage()
        {

            string message = localizedMessages[Thread.CurrentThread.CurrentCulture.Name];
            if (message is null)
            {

                return string.Empty;

            }

            return message;

        }
    }

    [Serializable]
    public class IsNotVacanciesException : LocalizeException
    {

        private static string MESSAGE_DEFAULT = "No rooms available.";
        private static string MESSAGE_JAPANESE = "空き部屋がありません。";

        public IsNotVacanciesException()
        {

            Initialize();

        }

        public IsNotVacanciesException(string? message) : base(message)
        {

            Initialize();

        }

        public IsNotVacanciesException(string? message, Exception? innerException) : base(message, innerException)
        {

            Initialize();

        }

        private void Initialize()
        {

            AddLocalizedMessage(Locale.DEFAULT, MESSAGE_DEFAULT);
            AddLocalizedMessage(Locale.JAPAN, MESSAGE_JAPANESE);

        }

    }

    [Serializable]
    public class OnlyMembersCanStayInSuiteRoomsException : LocalizeException
    {

        private static string MESSAGE_DEFAULT = "Only members can stay in suite rooms.";
        private static string MESSAGE_JAPANESE = "スイートルームには会員のみ宿泊できます。";

        public OnlyMembersCanStayInSuiteRoomsException()
        {

            Initialize();

        }

        public OnlyMembersCanStayInSuiteRoomsException(string? message) : base(message)
        {

            Initialize();

        }

        public OnlyMembersCanStayInSuiteRoomsException(string? message, Exception? innerException) : base(message, innerException)
        {

            Initialize();

        }

        private void Initialize()
        {

            AddLocalizedMessage(Locale.DEFAULT, MESSAGE_DEFAULT);
            AddLocalizedMessage(Locale.JAPAN, MESSAGE_JAPANESE);

        }

    }

    [Serializable]
    public class AlreadyCheckedInException : LocalizeException
    {
        private static string MESSAGE_DEFAULT = "Guest is already checked in.";
        private static string MESSAGE_JAPANESE = "ゲストはすでにチェックイン済みです。";

        public AlreadyCheckedInException(Guest guest)
            : base($"このゲスト {guest.Name} はすでにチェックイン済みです。")
        {
            Initialize();
        }

        public AlreadyCheckedInException(List<Guest> guests)
            : base("一部のゲストはすでにチェックイン済みです。")
        {
            Initialize();
        }

        private void Initialize()
        {
            AddLocalizedMessage(Locale.DEFAULT, MESSAGE_DEFAULT);
            AddLocalizedMessage(Locale.JAPAN, MESSAGE_JAPANESE);
        }
    }
}
