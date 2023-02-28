namespace WhatsForDinner.Services;

public class PromptService
{
    private List<string> _userConstraints = new List<string>();
    private static string promptHeader = "Suggest a single cooking recipe that conforms to the user-defined constraints provided.";

    public void addNewUserConstraint(string newUserConstraint)
    {
        if (!_userConstraints.Contains(newUserConstraint))
        {
            _userConstraints.Add(newUserConstraint);
        }
    }

    public string compilePrompt() =>
        @$"{promptHeader + Environment.NewLine}
            ###
            {joinUserConstraints()}
            ###";

    public string joinUserConstraints() =>
        string.Join(Environment.NewLine, _userConstraints);

    public void clearUserConstraints() =>
        _userConstraints = new List<string>();

}

