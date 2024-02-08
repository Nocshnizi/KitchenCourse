using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event EventHandler OnStaeChange;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;

    private enum State {
        WatingToStart,
        CountdownToStart,
        GamePlaying,
        GameOver
    }

    private State state;
    private float countingToStart = 3f;
    private float gamePlaying;
    private float gamePlayingMax = 100f;

    private bool isGamePaused = false;

    private void Awake() {
        Instance = this;
        state = State.WatingToStart;
    }

    private void Start() {
        GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
        GameInput.Instance.OnIneractAction += GameInput_OnIneractAction; 
    }

    private void GameInput_OnIneractAction(object sender, EventArgs e) {
        if(state == State.WatingToStart) {
            state = State.CountdownToStart;
            OnStaeChange?.Invoke(this, EventArgs.Empty);
        }
    }

    private void GameInput_OnPauseAction(object sender, EventArgs e) {
            TogglePauseGame();
        
    }


    private void Update() {
        switch (state) {
            case State.WatingToStart:
                
                break;
            case State.CountdownToStart:
                countingToStart -= Time.deltaTime;
                if (countingToStart < 0f) {
                    gamePlaying = gamePlayingMax;
                    state = State.GamePlaying;
                    OnStaeChange?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GamePlaying:
                gamePlaying -= Time.deltaTime;
                if (gamePlaying < 0f) {
                    state = State.GameOver;
                    OnStaeChange?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GameOver:
                break;
        }
        Debug.Log(state);
    }

    public bool IsGamePlaying() {
        return state == State.GamePlaying;
    }

    public bool IsCountdownStartActive() {
        return state == State.CountdownToStart;
    }


    public float GetCountdownToStartTimer() {
        return countingToStart;
    }

    public bool IsGameOver() {
        return state == State.GameOver;
    }

    public float GetGamePlayingTimerNormilized() {
        return 1 - (gamePlaying / gamePlayingMax);
    }

    public void TogglePauseGame() {
        isGamePaused = !isGamePaused;
        if(isGamePaused) {
            Time.timeScale = 0f;
            OnGamePaused?.Invoke(this, EventArgs.Empty);    
        }
        else {
            Time.timeScale = 1f;
            OnGameUnpaused?.Invoke(this, EventArgs.Empty);
        }
        
    }
}
