using Plus.Messages;
using System.Collections.Generic;

namespace Plus.HabboHotel.Polls
{
    /// <summary>
    /// Class Poll.
    /// </summary>
    internal class Poll
    {
        /// <summary>
        /// The identifier
        /// </summary>
        internal uint Id;

        /// <summary>
        /// The room identifier
        /// </summary>
        internal uint RoomId;

        /// <summary>
        /// The poll name
        /// </summary>
        internal string PollName;

        /// <summary>
        /// The poll invitation
        /// </summary>
        internal string PollInvitation;

        /// <summary>
        /// The thanks
        /// </summary>
        internal string Thanks;

        /// <summary>
        /// The prize
        /// </summary>
        internal string Prize;

        /// <summary>
        /// The type
        /// </summary>
        internal PollType Type;

        /// <summary>
        /// The questions
        /// </summary>
        internal List<PollQuestion> Questions;

        /// <summary>
        /// The answers positive
        /// </summary>
        internal int answersPositive;

        /// <summary>
        /// The answers negative
        /// </summary>
        internal int answersNegative;

        /// <summary>
        /// Initializes a new instance of the <see cref="Poll"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="roomId">The room identifier.</param>
        /// <param name="pollName">Name of the poll.</param>
        /// <param name="pollInvitation">The poll invitation.</param>
        /// <param name="thanks">The thanks.</param>
        /// <param name="prize">The prize.</param>
        /// <param name="type">The type.</param>
        /// <param name="questions">The questions.</param>
        internal Poll(uint id, uint roomId, string pollName, string pollInvitation, string thanks, string prize, int type, List<PollQuestion> questions)
        {
            this.Id = id;
            this.RoomId = roomId;
            this.PollName = pollName;
            this.PollInvitation = pollInvitation;
            this.Thanks = thanks;
            this.Type = (PollType)type;
            this.Prize = prize;
            this.Questions = questions;
            this.answersPositive = 0;
            this.answersNegative = 0;
        }

        /// <summary>
        /// Enum PollType
        /// </summary>
        internal enum PollType
        {
            /// <summary>
            /// The opinion
            /// </summary>
            Opinion,

            /// <summary>
            /// The prize_ badge
            /// </summary>
            Prize_Badge,

            /// <summary>
            /// The prize_ furni
            /// </summary>
            Prize_Furni,

            /// <summary>
            /// The matching
            /// </summary>
            Matching
        }

        /// <summary>
        /// Serializes the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        internal void Serialize(ServerMessage message)
        {
            message.AppendInteger(this.Id);
            message.AppendString("");//?
            message.AppendString(this.PollInvitation);
            message.AppendString("Test"); // whats this??
        }
    }
}