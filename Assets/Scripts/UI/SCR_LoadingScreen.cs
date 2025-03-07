using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.TextCore.Text;

public class LoadingScreen : MonoBehaviour { 
    public TextMeshProUGUI displayText;
    public TextMeshProUGUI note;
    public TextMeshProUGUI note2;
    public TextMeshProUGUI note3;
    public TextMeshProUGUI openEye;
    public TextMeshProUGUI closeEye;
    public Button startButton;
    public bool eyeVisible = false;

    public float delayBetweenLines = 0.5f;


    #region BootUp
    string[] bootSequence = {
        "C:\\> BIOS SYSTEM INITIATED...",
        "C:\\> DETECTING HARDWARE...",
        "",
        "[ SYSTEM CHECK ]",
        "> CPU: 486DX2 @ 66MHz",
        "> MEMORY: 640KB BASE, 8192KB EXTENDED",
        "> FLOPPY DRIVE: 3.5\" 1.44MB",
        "> HARD DISK: PRIMARY MASTER - CONNER CP3544 (540MB)",
        "> CD-ROM DRIVE: MITSUMI 4X",
        "",
        "C:\\> INITIALIZING DEVICE DRIVERS...",
        "> LOADING HIMEM.SYS... OK",
        "> LOADING EMM386.EXE... OK",
        "",
        "C:\\> DETECTING PERIPHERALS...",
        "> PS/2 MOUSE DETECTED",
        "> SERIAL PORT 1: ACTIVE",
        "> PARALLEL PORT: LPT1 - READY",
        "",
        "C:\\> SYSTEM RESOURCES... CHECK",
        "> INTERRUPT VECTOR TABLE: STABLE",
        "> DMA CHANNELS: ALL CLEAR",
        "",
        "C:\\> STARTING NETWORK CONNECTION...",
        "> ETHERNET ADAPTER: NE2000 - LINK ESTABLISHED",
        "",
        "C:\\> SHADOW RAM ENABLED",
        "C:\\> CACHE: WRITE-BACK ENABLED",
        "",
        "C:\\> READY TO BOOT FROM DISK...",
        "",
        "[ SYSTEM POST COMPLETE ]",
        "",
        "C:\\> BOOT SECTOR FOUND. LAUNCHING OPERATING SYSTEM...",
        "",
        "_Starting MS-DOS..._",
        "",
        "C:\\>"
    };



    #endregion

    #region ASCII OpenEye

    private string asciiOpenEye =
        "@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@\n@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@\n@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@\n@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@%%@@@@%%@@@@@@@@@@@@@@@@@@@@@@@@@@\n@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@#%%*%@@++%@%%%%**#%@@@@@@@@@@@@@@@@@@@@@\n@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@%%#@##%*%@%=*@%#%@+*%#*%%@%**%@@@@@@@@@@@@@@@@@\n@@@@@@@@@@@@@@@@@@@@@@@@@@%%%###@#%#*@@=+@*+%%=+#=+%%=-+%@#*+*#@@@@@@@@@@@@@@@\n@@@@@@@@@@@@@@@@@@@@@@%%%#@*%+@#%#*@*=#*=##==#==#*-=##==*%%*++++*#@@@@@@@@@@@@\n@@@@@@@@@@@@@@@@@@@#%#+%*@**#####%=+%=+#=+#+=***%@##%@@#%%%#*===++#@@@@@@@@@@@\n@@@@@@@@@@@@@@@@@@*%**@*@*%%+%##-**-*++@@%@@@@@@@@@@@@@@@@@@%#****#%@@@@@@@@@@\n@@@@@@@@@@@@@@@%%=%-+*-%=@**%+-*=*@@@@@@@@@@@@@@@@@@@@@@@@@@@@@%*+==%@@@@@@@@@\n@@@@@@@@@@@@@@%=#==+-**+@=%=*@%@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@%*#@@@@@@@@@\n@@@@@@@@@@@@@##++*-#+=+**#@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@%#@@@@@@@@\n@@@@@@@@@@@@@@=*+@*%%@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@\n@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@%%%####*#+#%#@%@@@@@@@@@@@@@@@@@@@@@@@@@@@@\n@@@@@@@@@@@@@@@@@@@@@@@@@@@@@%*+-=+=#*#=*@#*+*=%##-=%@@@@@@@@@@@@@@@@@@@@@@@@@\n@@@@@@@@@@@@@@@@@@@@@@@@@@#+@@%=@*+-::.......:=+=+%%%#-#@@%@@@@@@@@@@@@@@@@@@@\n@@@@@@@@@@@@@@@@@@@@@@@@-%@%*##............::.....:-==%@+*@@@@@@@@@@@@@@@@@@@@\n@@@@@@@@@*%@@@@@@@@@@%-##%==:.......@#......*@@@@=...-+=+@@%%@@@@@@@@@@@@@@@@@\n@@@@@@@@#+@@@@@@@@@%-%@#+::%@@::-::.....-:..-@@@@@@@+...-+*#%@@@@@@@@@@@@@@@@@\n@@@@@@@%:@@@@@@@@@#%%#=:#@@@@@-++*==--+*++*-+@@@@@@#-@@*+#*#%@@@@@@@@@@@@@@@@@\n@@@@@@@#-@@@@@@@@@@@++@=%@@@@@#:%#++%#*#%-=:%@@@@@=#@%*%#*%%@@@@@@@@@@@@@@@@@@\n@@@@@@@#+@@@@@@@@@@%@@@#=@@@@@@@-=-#%==#*-*@@@@%+%@@*=*%##%@@@@@@@@@@@@@@@@@@@\n@@@@@@@#*@@@@@@@@@@@%####***++*%@@%#**#%@@@@@%@@@%%#-=#%#@@@@@@@@@@@@@@@@@@@@@\n@@@@@@@@@@@@@@@@@@@@@#%%%%%@%@@@@@@@@@@@@@@#%*%**+*#@@*@@%@@@@@@@@@@@@@@@@@@@@\n@@@@@@@@@@@@@@@@@@@@@@@@@#*%#%%#*%%#*##**+**####@%##@@@@@@@@@@@@@@@@@@@@@@@@@@\n@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@%@@%%*%%*%#%%%#@%@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@\n@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@%@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@\n@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@\n@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@\n@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@\n@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@\n@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@\n@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@\n@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@\n@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@\n";

    #endregion

    #region asciiClosedEye

    private string asciiClosedEye = "@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@\n@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@\n@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@\n@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@%@@@@@@@@@@@@@@@@@@@@@@@@@@@@@\n@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@%%%#%@@**#@%%%##%%@@@@@@@@@@@@@@@@@@@@@@@@\n@@@@@@@@@@@@@@@@@@@@@@@@@@@@@%@#%%#@*%@%+*@##%#+#%*#%@@#%@@@@@@@@@@@@@@@@@@@@@\n@@@@@@@@@@@@@@@@@@@@@@@@@#@#%*@%%#*@@=*@+*@*-#+=%%+-+%@%**#%@@@@@@@@@@@@@@@@@@\n@@@@@@@@@@@@@@@@@@@@@@*%%%%*@%%##@*-@+=%*:#+:#*:=%#-+%@#=--==*@@@@@@@@@@@@@@@@\n@@@@@@@@@@@@@@@@@@#%%*#@*%+%*%#@:*#:%+:%*:%+*@@@@@@@@@#*===+*##@@@@@@@@@@@@@@@\n@@@@@@@@@@@@@@@@@***=#%###:%*%.#-=*:%@@@@@@@@@@@@@@@@@@@@@%#%##%@@@@@@@@@@@@@@\n@@@@@@@@@@@@@@%@=%++%+#@#=@=-%=@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@%%#%@@@@@@@@@@@@@\n@@@@@@@@@@@@@%=#=:*=:*+=*+%@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@%#@@@@@@@@@@@@@\n@@@@@@@@@@@@@*=+#-+#=*#%@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@%@@@@@@@@@@@@\n@@@@@@@@@@@@@+*#@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@\n@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@\n@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@\n@@@@@@@@@@@@@@@@@@@@@@@@@@@%=-+#%%%@@@%%%*==#@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@\n@@@@@@@@@%@@@@@@@@@@@@@@==@@@@@@@@@@@@@@@@@@@@@*=@@@@@@@@@@@@@@@@@@@@@@@@@@@@@\n@@@@@@@@=#@@@@@@@@@@@%:#@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@\n@@@@@@@%+@@@@@@@@@@@-*@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@\n@@@@@@@=%@@@@@@@@@@-@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@\n@@@@@@@:%@@@@@@@@@@@@@@%%@%##****###%%@@@@@@@@@%%#*=:-%@@@@@@@@@@@@@@@@@@@@@@@\n@@@@@@@-%@@@@@@@@@@@*#+:..........................-%@@@@@%@@@@@@@@@@@@@@@@@@@@\n@@@@@@@@@@@@@@@@@@#*:=+==::-::....:..:..:-::...:=:=--+##%%##%@@@@@@@@@@@@@@@@@\n@@@@@@@@@@@@@@@@@@@%@@%#=*+=+--+=*--=++.=-=+*.=:-=##=*#%@@@@@@@@@@@@@@@@@@@@@@\n@@@@@@@@@@@@@@@@@@@@@@@@@@%##-%**=%*#+##=#+*==##**#%@@@@@@@@@@@@@@@@@@@@@@@@@@\n@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@%%*@@@#%@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@\n@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@\n@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@\n@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@\n@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@\n@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@\n@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@\n@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@\n@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@\n@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@\n";

    #endregion

    #region Message

    private string message =
        "Daniel...\r\n\r\n    Help us. Daniel believes in you. Find him. He has the truth. It is up to you.\r\n\r\nWe need you Daniel\r\n";
    #endregion

    #region Instruction

    private string firstText =
        "I hope you are better than the last one. Here is some advice";

    private string secondText = "ACCESS THE TERMINAL. INSTALL YOUR RAM. \n\n PICK YOUR WEAPON. CHOOSE WISELY.";
    #endregion





    int lineBreak = 177;
    public string[] imageMessage;
    public string[] openEyeMessage;
    public string[] closedEyeMessage;




    private void Start() {
        AudioManager.Instance.Stop("Music_Main");
        AudioManager.Instance.Play("CRT_Static");
        startButton.gameObject.SetActive(false);
        openEyeMessage = asciiOpenEye.Split('\n');
        closedEyeMessage = asciiClosedEye.Split('\n');
        StartCoroutine(DisplayBootMessages());
        

    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.M))
        {
            StartCoroutine(DisplayBootMessages());

        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            //displayText3.gameObject.SetActive(true);
            displayText.text = "";
            //StartCoroutine(DisplayClosedEye());
            DisplayClosedEye();
            closeEye.gameObject.SetActive(false);
            StartCoroutine(DisplayOpenEye());
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            StartCoroutine(ToggleOpenEye());
        }

        if ((Random.Range(0f, 1.0f) <= 0.05) && eyeVisible)
        {
            StartCoroutine(ToggleOpenEye());
        }
    }

    private float MakeRandom() {
        float randomDelay = UnityEngine.Random.Range(0.1f, 0.5f);
        return randomDelay;
    }

    private IEnumerator ToggleOpenEye() {
        openEye.gameObject.SetActive(false);
        closeEye.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        //AudioManager.Instance.PlayOne("Typing_Sound"); 
        openEye.gameObject.SetActive(true);
        closeEye.gameObject.SetActive(false);
    }

    private IEnumerator DisplayLoadingMessages() {
        openEye.text = string.Join("\n", new string[imageMessage.Length]); 

        int[] indices = GenerateShuffledIndices(imageMessage.Length);
        string[] currentDisplay = new string[imageMessage.Length]; 

        foreach (int index in indices)
        {
            currentDisplay[index] = imageMessage[index]; 
            openEye.text = string.Join("\n", currentDisplay); 
            yield return new WaitForSeconds(0.02f);
        }
    }
    private IEnumerator DisplayOpenEye() {
        DisplayClosedEye();
        openEye.text = string.Join("\n", new string[openEyeMessage.Length]);

        int[] indices = GenerateShuffledIndices(openEyeMessage.Length);
        string[] currentDisplay = new string[openEyeMessage.Length];

        foreach (int index in indices)
        {
            AudioManager.Instance.PlayOne("Typing_Sound"); 
            currentDisplay[index] = openEyeMessage[index];
            openEye.text = string.Join("\n", currentDisplay);
            yield return new WaitForSeconds(0.02f);
        }

        eyeVisible = true;
        StartCoroutine(DisplayMessageSequence());
    }

    private void DisplayClosedEye() {
        //AudioManager.Instance.PlayOne("Typing_Sound");
        string[] currentDisplay = new string[closedEyeMessage.Length];
        for (int i = 0; i < closedEyeMessage.Length; i++)
        {
            currentDisplay[i] = closedEyeMessage[i]; 
            closeEye.text = string.Join("\n", currentDisplay); 
        }
        closeEye.gameObject.SetActive(false);
    }


    private IEnumerator DisplayBootMessages() {
        displayText.text = ""; 
        int maxVisibleLines = 27; 

        List<string> displayedLines = new List<string>();

        for (int i = 0; i < bootSequence.Length; i++)
        {
            displayedLines.Add(bootSequence[i]);
            AudioManager.Instance.PlayOne("Typing_Sound");

            if (displayedLines.Count > maxVisibleLines)
            {
                displayedLines.RemoveAt(0);
            }

            displayText.text = string.Join("\n", displayedLines);

            yield return new WaitForSeconds(MakeRandom()); 
        }

        yield return new WaitForSeconds(2f);
        StartCoroutine(DisplayLetterSequence());
    }

    private IEnumerator DisplayLetterSequence() {
        yield return new WaitForSeconds(1f);
        displayText.gameObject.SetActive(false);
        note.gameObject.SetActive(true);
        string[] words = message.Split(' ');
        note.text = "";
        foreach (string word in words)
        {
            AudioManager.Instance.PlayOne("Typing_Sound");
            note.text += word + " ";
            yield return new WaitForSeconds(MakeRandom());
        }

        yield return new WaitForSeconds(5f);

        note.gameObject.SetActive(false);
        StartCoroutine(DisplayOpenEye());
    }

    private IEnumerator DisplayMessageSequence() {
        yield return new WaitForSeconds(1f);
        displayText.gameObject.SetActive(false);
        note2.gameObject.SetActive(true);
        string[] wordsFirstText = firstText.Split(' ');
        note2.text = "";
        foreach (string word in wordsFirstText)
        {
            note2.text += word + " ";
            yield return new WaitForSeconds(MakeRandom());
        }
        yield return new WaitForSeconds(5f);
        note2.gameObject.SetActive(false);
        //NEXT MESSAGE
        note3.gameObject.SetActive(true);
        string[] wordsSecondText = secondText.Split(' ');
        note3.text = "";
        foreach (string word in wordsSecondText)
        {
            note3.text += word + " ";
            yield return new WaitForSeconds(MakeRandom());
        }
        startButton.gameObject.SetActive(true);
        yield return null;
    }

    int[] GenerateShuffledIndices(int length) {
        List<int> indices = new List<int>();
        for (int i = 0; i < length; i++) indices.Add(i);

        System.Random rng = new System.Random();
        for (int i = indices.Count - 1; i > 0; i--)
        {
            int j = rng.Next(0, i + 1);
            (indices[i], indices[j]) = (indices[j], indices[i]); // Swap
        }

        return indices.ToArray();
    }

    string InsertNewLines(string input, int interval) {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < input.Length; i++)
        {
            sb.Append(input[i]);
            if ((i + 1) % interval == 0)
            {
                sb.Append('\n');
            }
        }
        return sb.ToString();
    }
}