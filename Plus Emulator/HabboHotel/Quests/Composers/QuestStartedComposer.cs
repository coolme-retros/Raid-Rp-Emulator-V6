using Plus.HabboHotel.GameClients;
using Plus.Messages;
using Plus.Messages.Parsers;

namespace Plus.HabboHotel.Quests.Composer
{
    /// <summary>
    /// Class QuestStartedComposer.
    /// </summary>
    internal class QuestStartedComposer
    {
        /// <summary>
        /// Composes the specified session.
        /// </summary>
        /// <param name="Session">The session.</param>
        /// <param name="Quest">The quest.</param>
        /// <returns>ServerMessage.</returns>
        internal static ServerMessage Compose(GameClient Session, Quest Quest)
        {
            var serverMessage = new ServerMessage(LibraryParser.OutgoingRequest("QuestStartedMessageComposer"));
            QuestListComposer.SerializeQuest(serverMessage, Session, Quest, Quest.Category);
            return serverMessage;
        }
    }
}