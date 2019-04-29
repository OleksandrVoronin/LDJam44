using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroAnimationMaster : MonoBehaviour
{
    [SerializeField]
    private Image _fader;

    [SerializeField]
    private GameObject[] _scenes;

    [SerializeField]
    private float _faderInTime = 0.3f;

    [SerializeField]
    private float _faderOutTime = 0.3f;

    [SerializeField]
    private float _faderDarkTime = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlayIntroAnimation());
    }

    private IEnumerator PlayIntroAnimation() {
        yield return new WaitForSeconds(2f);

        _scenes[0].SetActive(true);
        _fader.DOFade(0, _faderOutTime);

        yield return new WaitForSeconds(4f);

        _fader.DOFade(1, _faderInTime);
        yield return new WaitForSeconds(_faderInTime + _faderDarkTime);
        _scenes[1].SetActive(true);
        _fader.DOFade(0, _faderOutTime);

        yield return new WaitForSeconds(4f);

        _fader.DOFade(1, _faderInTime);
        yield return new WaitForSeconds(_faderInTime + _faderDarkTime);
        _scenes[2].SetActive(true);
        _fader.DOFade(0, _faderOutTime);

        yield return new WaitForSeconds(4f);

        _fader.DOFade(1, _faderInTime);
        yield return new WaitForSeconds(_faderInTime + _faderDarkTime);
        _scenes[3].SetActive(true);
        _fader.DOFade(0, _faderOutTime);

        yield return new WaitForSeconds(4f);

        Skip();
    }

    // Update is called once per frame
    public void Skip()
    {
        StopAllCoroutines();
        StartCoroutine(SkipRoutine());
    }

    private IEnumerator SkipRoutine() {
        _fader.DOFade(1, _faderInTime);

        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene(1);
    }
}
