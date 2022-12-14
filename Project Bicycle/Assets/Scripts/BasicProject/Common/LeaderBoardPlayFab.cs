using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LeaderBoardPlayFab : MonoBehaviour 
{
    [DllImport("__Internal")]
    private static extern bool IsMobile();

    public bool isMobile()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
             return IsMobile();
#endif
        return false;
    }

    public static LeaderBoardPlayFab leaderBoard;

    //public GameObject ForMobile;
    //public GameObject ForPC;


    public Text userEmailText;
    public Text DisplayIDText;
    public Text userPasswordText;
    public Text userNameText;
    public Text errorMessgaeText;
    public GameObject loginScreen;
    public GameObject SuccessfulyLoginScreen;
    public GameObject LeaderBoardScreen;
    public GameObject leaderBoardInstance;
    public GameObject DailyLeaderContent;
    public GameObject WeeklyLeaderContent;
    private string userEmail;
    private string userPassword;
    private string userName;
    private int playerLogin;
    private void OnEnable() {
        if (LeaderBoardPlayFab.leaderBoard == null) {
            LeaderBoardPlayFab.leaderBoard = this;
        } else {
            if (LeaderBoardPlayFab.leaderBoard != this) {
                Destroy(this.gameObject);
            }
        }
      //  DontDestroyOnLoad(this.gameObject);
    }
    public void CheckPlayerDetail() {
        int nameLength= userNameText.text.Length;
        if ((!string.IsNullOrEmpty(userNameText.text))&&(!string.IsNullOrEmpty(userEmailText.text))&&nameLength>=3 && (!string.IsNullOrEmpty(userPasswordText.text))) {
            SetUserNameAndEmail();
        } else {
            DisplayError("Fill all required fields properly");
        }
    }
    #region Login
    public void Start() {
        //Note: Setting title Id here can be skipped if you have set the value in Editor Extensions already.
        if (string.IsNullOrEmpty(PlayFabSettings.TitleId)) {
            PlayFabSettings.TitleId = "6E4FE"; // Please change this value to your own titleId from PlayFab Game Manager
        }
        if (PlayerPrefs.HasKey("EMAIL")) {
             userName = PlayerPrefs.GetString("NAME");
             userEmail = PlayerPrefs.GetString("EMAIL");
             userPassword = PlayerPrefs.GetString("PASSWORD");
             var request = new LoginWithEmailAddressRequest { Email = userEmail, Password = userPassword };
             PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
         }
    }
    public void LoginWithCustomID()
    {
        var request = new LoginWithCustomIDRequest { CustomId = PlayerPrefs.GetString("Account"), CreateAccount = true };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
    }
    private void OnLoginSuccess(LoginResult result) {
        Debug.Log("Congratulations, you made your first successful API call!");
        PlayerPrefs.SetString("NAME", userName);
        PlayerPrefs.SetString("EMAIL", userEmail);
        GetUserData(result.PlayFabId);
        GetSats(); GetSatsWeekly();
        loginScreen.SetActive(false);
        SuccessfulyLoginScreen.SetActive(true);
    }
    void SetUserData(string s) {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest() {
            Data = new Dictionary<string, string>() {
            {"playerLogin",s},
            {"MetaID",PlayerPrefs.GetString("Account") }
        }
        },
        result => Debug.Log("Successfully updated player login data"),
        error => {
            Debug.Log("Got error setting user data Ancestor to Arthur");
            Debug.Log(error.GenerateErrorReport());
        }) ;
    }
    void GetUserData(string myPlayFabeId) {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest() {
            PlayFabId = myPlayFabeId,
            Keys = null
        }, result => {
            Debug.Log("Got user data:");
            if (result.Data == null || result.Data.ContainsKey("playerLogin")) {
                if (!((result.Data["playerLogin"].Value) == "b")) {
                  loginScreen.SetActive(true);
                    SetUserData("b");
                } else {
                    //WaitingScreen.SetActive(false);
                    SuccessfulyLoginScreen.SetActive(true);
                }
                Debug.Log(" player login"+ result.Data["playerLogin"].Value); } 
            else { Debug.Log("No Values: " ); loginScreen.SetActive(true); SetUserData("b");
            }
        }, (error) => {
            Debug.Log("Got error retrieving user data:");
            Debug.Log(error.GenerateErrorReport());
        });
    }
    private void SetUserNameAndEmail() {
        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest { DisplayName = userNameText.text }, OnDisplayName, OnLoginFailure);
        PlayFabClientAPI.AddOrUpdateContactEmail(new AddOrUpdateContactEmailRequest { EmailAddress = userEmailText.text }, OnDisplayEmail, OnLoginFailure);
        GetSats();
    }
    private IEnumerator DisplayError(string error) {
        errorMessgaeText.gameObject.SetActive(true);
        errorMessgaeText.text = error;
        yield return new WaitForSeconds(2);
        errorMessgaeText.gameObject.SetActive(false);
    }
    private void OnRegisterSuccess(RegisterPlayFabUserResult result) {
        Debug.Log("Congratulations, you made your first successful API call!");
        PlayerPrefs.SetString("EMAIL", userEmail);
        PlayerPrefs.SetString("NAME", userName);
        PlayerPrefs.SetString("PASSWORD", userPassword);
        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest { DisplayName = userName }, OnDisplayName, OnLoginFailure);
        GetSats(); GetSatsWeekly();
        loginScreen.SetActive(false);
        SuccessfulyLoginScreen.SetActive(true);
    }
    private void OnRegisterfailure(PlayFabError error) {
        StartCoroutine(DisplayError(error.GenerateErrorReport()));
    }
    private void OnLoginFailure(PlayFabError error) {
        var registerRequest = new RegisterPlayFabUserRequest { Email = userEmailText.text, Password = userPassword, Username = userNameText.text };
        PlayFabClientAPI.RegisterPlayFabUser(registerRequest, OnRegisterSuccess, OnRegisterfailure);
    }
    private void OnDisplayName(UpdateUserTitleDisplayNameResult result) {
        Debug.Log(result.DisplayName + "is your new display name");
        SuccessfulyLoginScreen.SetActive(true);
    }
    private void OnDisplayEmail(AddOrUpdateContactEmailResult results) {
        Debug.Log(results.CustomData+"is your email");
    }
    public void GetUserEmail() {
        userEmail = userEmailText.text; 
    }
    public void GetUsername() {
        userName = userNameText.text;
    }
    public void GetUserPasword() {
        userPassword = userPasswordText.text;
    }
    public void OnClickLogin() {
        GetUserEmail(); GetUsername(); GetUserPasword();
        var request = new LoginWithEmailAddressRequest { Email = userEmailText.text, Password = userPassword };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
    }
    #endregion Login
    public void LoadNextScene() {
        SceneManager.LoadScene(2);
    }
    public int playerScore;
    #region PlayerStatistics
    public void SetStats() {
        //Debug.Log("scrggggggggggggggg"+PlayerPrefs.GetInt("BestScore"));
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest {
            Statistics = new List<StatisticUpdate> {
             new StatisticUpdate{StatisticName="PlayerScores",Value=PlayerPrefs.GetInt("BestScore")}
            }
        },
        result => { Debug.Log("User Statistics update"); },
        error => { Debug.Log(error.GenerateErrorReport()); });
    }
    private void GetSats() {
        PlayFabClientAPI.GetPlayerStatistics(
            new GetPlayerStatisticsRequest(),
            OnGetStats,
            error => Debug.Log(error.GenerateErrorReport())
            );
    }
    private void OnGetStats(GetPlayerStatisticsResult result) {
        Debug.Log("Recieved following statistics");
        foreach (var eachStat in result.Statistics) {
            Debug.Log("Statictis( " + eachStat.StatisticName + "):" + eachStat.Value);
            playerScore = eachStat.Value;
        }
    }
 
    public void SetStatsWeekly() {
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest {
            Statistics = new List<StatisticUpdate> {
             new StatisticUpdate{StatisticName="UpdateWeekly"  ,Value=PlayerPrefs.GetInt("BestScore")}
            }
        },
        result => { Debug.Log("User Statistics update"); },
        error => { Debug.Log(error.GenerateErrorReport()); });
    }
    private void GetSatsWeekly() {
        PlayFabClientAPI.GetPlayerStatistics(
            new GetPlayerStatisticsRequest(),
            OnGetStatsWeekly,
            error => Debug.Log(error.GenerateErrorReport())
            );
    }
    private void OnGetStatsWeekly(GetPlayerStatisticsResult result) {
        Debug.Log("Recieved following statistics");
        foreach (var eachStat in result.Statistics) {
            //Debug.Log("Statictis( " + eachStat.StatisticName + "):" + eachStat.Value);
            playerScore = eachStat.Value;
        }
    }
    #endregion
    #region LeaderBoard
    public void GetDailyLeaderBoard() {
        var requestLeaderBoard = new GetLeaderboardRequest { StartPosition = 0, StatisticName = "PlayerScores", MaxResultsCount = 30 };
        PlayFabClientAPI.GetLeaderboard(requestLeaderBoard, OnGetDailyLeaderBoard, OnErrorLeaderBoard);
    }
    private void OnGetDailyLeaderBoard(GetLeaderboardResult result) {
       // LeaderBoardScreen.SetActive(true);
        for (int i = 0; i < DailyLeaderContent.transform.childCount; i++) {
            Destroy(DailyLeaderContent.transform.GetChild(i).gameObject);
        }int j = 0;
        foreach (PlayerLeaderboardEntry player in result.Leaderboard) {
            j += 1;
            GameObject playerProperties = Instantiate(leaderBoardInstance, DailyLeaderContent.transform);
            playerProperties.transform.GetChild(0).GetComponent<Text>().text = player.DisplayName;
            playerProperties.transform.GetChild(1).GetComponent<Text>().text = player.StatValue.ToString();
            playerProperties.transform.GetChild(3).GetComponent<Text>().text = j + " .";
            Debug.Log(player.DisplayName + ":" + player.StatValue);
        }
    }
    public void GetWeeklyLeaderBoard() {
        var requestLeaderBoard = new GetLeaderboardRequest { StartPosition = 0, StatisticName = "UpdateWeekly", MaxResultsCount = 30 };
        PlayFabClientAPI.GetLeaderboard(requestLeaderBoard, OnGetWeeklyLeaderBoard, OnErrorLeaderBoard);
    }
    private void OnGetWeeklyLeaderBoard(GetLeaderboardResult result) {
     //   LeaderBoardScreen.SetActive(true);
        for (int i = 0; i < WeeklyLeaderContent.transform.childCount; i++) {
            Destroy(WeeklyLeaderContent.transform.GetChild(i).gameObject);
        }
        int j = 0;
        foreach (PlayerLeaderboardEntry player in result.Leaderboard) {
            Debug.Log("weeklyleaderboard");
            j += 1;
            GameObject playerProperties = Instantiate(leaderBoardInstance, WeeklyLeaderContent.transform);
            playerProperties.transform.GetChild(0).GetComponent<Text>().text = player.DisplayName;
            playerProperties.transform.GetChild(1).GetComponent<Text>().text = player.StatValue.ToString();
            playerProperties.transform.GetChild(3).GetComponent<Text>().text = j + " .";
            Debug.Log(player.DisplayName + ":" + player.StatValue);
        }
    }
    public void ShowLeaderBoard() {

        SetStats();
        GetSats();
        SetStatsWeekly(); 
        LeaderBoardScreen.SetActive(true);
        GetWeeklyLeaderBoard();
        GetDailyLeaderBoard();

    }
    public void UpdateLeaderBoard() {

        SetStats();
        GetSats();
        SetStatsWeekly();
        GetWeeklyLeaderBoard();
        GetDailyLeaderBoard();

    }
    private void OnErrorLeaderBoard(PlayFabError error) {
        //Debug.log(error.generateerrorreport());
    }
    #endregion LeaderBoard
}















