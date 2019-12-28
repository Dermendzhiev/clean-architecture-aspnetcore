namespace CleanArchitecture.FunctionalTests.Helpers
{
    using System;
    using System.Collections.Generic;
    using CleanArchitecture.Domain.Entities;

    internal static class PredefinedData
    {
        internal static List<Poll> Polls = new List<Poll>();

        internal static void InitializePolls()
        {
            Polls.Clear();

            var socialMediaPoll = new Poll("Which of the following social media websites do you currently have an account with?", null, false, DateTime.Now.AddYears(1));
            socialMediaPoll.AddOption("Facebook");
            socialMediaPoll.AddOption("Instagram");
            socialMediaPoll.AddOption("Twitter");
            socialMediaPoll.AddOption("LinkedIn");
            socialMediaPoll.AddOption("Snapchat");
            socialMediaPoll.AddOption("Google+");
            socialMediaPoll.AddOption("Myspace");

            Polls.Add(socialMediaPoll);

            var onlineShoppingPoll = new Poll("How often do you buy products online?", "test note", true, DateTime.Now.AddDays(7));
            onlineShoppingPoll.AddOption("Extremely often");
            onlineShoppingPoll.AddOption("Quite often");
            onlineShoppingPoll.AddOption("Moderately often");
            onlineShoppingPoll.AddOption("Slightly often");
            onlineShoppingPoll.AddOption("SnapchatNot at all often");
            onlineShoppingPoll.AddOption("Never");
            Polls.Add(onlineShoppingPoll);
        }
    }
}
