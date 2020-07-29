
public class Rootobject
{
    public string type { get; set; }
    public string context { get; set; }
    public string themeColor { get; set; }
    public string summary { get; set; }
    public Section[] sections { get; set; }
    public Potentialaction[] potentialAction { get; set; }
}

public class Section
{
    public string activityTitle { get; set; }
    public string activitySubtitle { get; set; }
    public string activityImage { get; set; }
    public Fact[] facts { get; set; }
    public bool markdown { get; set; }
}

public class Fact
{
    public string name { get; set; }
    public string value { get; set; }
}

public class Potentialaction
{
    public string type { get; set; }
    public string name { get; set; }
    public Input[] inputs { get; set; }
    public Action[] actions { get; set; }
}

public class Input
{
    public string type { get; set; }
    public string id { get; set; }
    public bool isMultiline { get; set; }
    public string title { get; set; }
    public string isMultiSelect { get; set; }
    public Choice[] choices { get; set; }
}

public class Choice
{
    public string display { get; set; }
    public string value { get; set; }
}

public class Action
{
    public string type { get; set; }
    public string name { get; set; }
    public string target { get; set; } //we do not have a target URI for the actions
}
