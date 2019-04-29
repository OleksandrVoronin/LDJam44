using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class GameIntroSequence : MonoBehaviour
{

    [SerializeField]
    private CanvasGroup _responseCanvasGroup;

    [SerializeField]
    private TextMeshProUGUI _alienText;
    [SerializeField]
    private TextMeshProUGUI _responseText;

    [SerializeField]
    private GameObject _shipPurchaseScreen;

    protected string[] _alienPhrases = {
        "You woke up, at last. Welcome to our world, looks like it was your ship burning at orbit yesterday. Flashy.",
        "It's a remote world but a friendly one, you're safe here. Alas, our doctors fought for your life, but it was too late, I'm sorry.",
        "Well, technically, you died.",
        "Have you seriously not heard tales of cats having nine lives? That's not just a legend, but still a rare thing. You're lucky, you are the one of who we call Sanctus Cattus. The members of Sanctus Cattus order are rare guests these days but are always welcome in our sector of the galaxy.",
        "The first thing you should do is visit Sanctus Cattus temple on the other side of our sector, they will explain more. I'll mark it on your map.",
        "The local government is ready to provide you a ship and some supplies. Going to be a long journey but keep your head up.",
        "Ha-ha-ha of course not, nothing comes for free. Unless you have money to buy a ship, you can give up some of your lives.",
        "Sanctus Cattus have invented a peculiar technology allowing them to turn life source into very strong sources of energy. We could use some, hence our offer to you. This accident did cost you a life, a new ship will cost a few more. But given your situation, it's probably an offer you should take.",
        "Good. Let's go choose you a ship, many dangerous things are hidden in our sector and your journey will be a tough one, you better have a vessel you can rely on."


    };

    protected string[] _responsePhrases = {
        "What happened? Where am I? ",
        "What do you mean, have I died? ",
        "I sure don't feel like I'm dead. Not that I'd know that feeling..",
        "Are you implying I'm some kind of a special one? I'm not so sure about that.. nor what to do now.",
        "But how am I supposed to get off this rock without my ship? Can you help me?",
        "Wow, thanks, are you really giving me a starship for free?",
        "Lives? But how?",
        "Alright, not that I have a choice, really.",
        "Continue..."
    };

    private int _dialogueProgress = 0;

    public void Respond()
    {
        _responseCanvasGroup.DOFade(0, 0f);

        if (_dialogueProgress == _responsePhrases.Length - 1)
        {
            Skip();
        }
        else
        {
            DialogueContinue();
        }
    }

    public void Skip()
    {
        GameManager.Instance.Fader.DOFade(1, 0.3f).OnComplete(() =>
        {
            gameObject.SetActive(false);
            _shipPurchaseScreen.SetActive(true);

            GameManager.Instance.Fader.DOFade(0, 0.3f).SetDelay(0.5f);
        });
    }

    // Start is called before the first frame update
    protected void Start()
    {
        DialogueContinue();
    }

    private void DialogueContinue()
    {
        _alienText.DOText(_alienPhrases[_dialogueProgress], _alienPhrases[_dialogueProgress].Length / 80f, scrambleMode: ScrambleMode.Custom, scrambleChars: " ").SetEase(Ease.Linear).OnComplete(() =>
        {
            _responseCanvasGroup.DOFade(1, 0.2f);
            _responseText.text = _responsePhrases[_dialogueProgress];

            _dialogueProgress++;
        });
    }


    // Update is called once per frame
    void Update()
    {

    }
}
