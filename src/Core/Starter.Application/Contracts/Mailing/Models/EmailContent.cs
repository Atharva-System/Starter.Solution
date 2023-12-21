namespace Starter.Application.Contracts.Mailing.Models;
public class EmailContent
{
    #region Constant
    public const string LogoURL = "https://mearto.com/assets/no_logo-1e90e24424146337cc897d37f459cb257e22d6284d1467ff0bf17d1f123d3831.png";
    #endregion

    #region Readonly Property
    public string DomainLogo { get; }

    public string ButtonAnchorUrl { get; set; }

    public bool HasButton { get; } = false;
    #endregion

    public string Subject { get; set; }

    public string HeyUserName { get; set; }

    public string YourDomain { get; set; } = string.Empty;

    public List<string> RowData { get; set; }

    public string ButtonText { get; set; } = string.Empty;

    public EmailContent(string origion, string buttonAnchorUrl = "")
    {
        RowData = new List<string>();

        DomainLogo = $"{LogoURL}";

        if (!string.IsNullOrEmpty(origion) && !string.IsNullOrEmpty(buttonAnchorUrl))
        {
            ButtonAnchorUrl = new Uri(string.Concat($"{origion}/", buttonAnchorUrl)).ToString();
        }

        HasButton = !string.IsNullOrEmpty(buttonAnchorUrl);
    }
}

