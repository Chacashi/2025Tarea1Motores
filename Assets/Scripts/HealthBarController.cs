using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public  class HealthBarController : MonoBehaviour
{
    [SerializeField] private int maxValue;
    [Header("Health Bar Visual Components")] 
    [SerializeField] private RectTransform healthBar;
    [SerializeField] private RectTransform modifiedBar;
    [SerializeField] private float changeSpeed;

    [SerializeField] private int currentValue;
    public int CurrentValue => currentValue;
    private float _fullWidth;
    private float TargetWidth => currentValue * _fullWidth / maxValue;
    private Coroutine updateHealthBarCoroutine;
    public static event Action OnPlayerDeath;
    public static event Action<int> OnDestroyEnemie1;
    public static event Action<int> OnDestroyEnemie2;
    private void Start() {
        currentValue = maxValue;
        _fullWidth = healthBar.rect.width;
    }

    /// <summary>
    /// Metodo <c>UpdateHealth</c> actualiza la vida del personaje de manera visual. Recibe una cantidad de vida modificada.
    /// </summary>
    /// <param name="amount">El valor de vida modificada.</param>
    public void UpdateHealth(int amount){
        currentValue = Mathf.Clamp(currentValue + amount, 0, maxValue);

        if(updateHealthBarCoroutine != null){
            StopCoroutine(updateHealthBarCoroutine);
        }
        updateHealthBarCoroutine = StartCoroutine(AdjustWidthBar(amount));
    }

    IEnumerator AdjustWidthBar(int amount){
        RectTransform targetBar = amount >= 0 ? modifiedBar : healthBar;
        RectTransform animatedBar = amount >= 0 ? healthBar : modifiedBar;

        targetBar.sizeDelta = SetWidth(targetBar,TargetWidth);

        while(Mathf.Abs(targetBar.rect.width - animatedBar.rect.width) > 1f){
            animatedBar.sizeDelta = SetWidth(animatedBar,Mathf.Lerp(animatedBar.rect.width, TargetWidth, Time.deltaTime * changeSpeed));
            yield return null;
        }

        animatedBar.sizeDelta = SetWidth(animatedBar,TargetWidth);
    }

    private Vector2 SetWidth(RectTransform t, float width){
        return new Vector2(width, t.rect.height);
    }



    private void OnEnable()
    {
        WizardBodyController.OnEnemyStaticTakeBullet += Injurious;
        PlayerController.OnPlayerTakeBullet += Injurious;
        PatrolMovementController.OnEnemyPatrolTakeBullet += Injurious;
    }

    private void OnDisable()
    {
        WizardBodyController.OnEnemyStaticTakeBullet -= Injurious;
        PlayerController.OnPlayerTakeBullet -= Injurious;
        PatrolMovementController.OnEnemyPatrolTakeBullet -= Injurious;
    }
    void AddLife(int value)
    {
       currentValue = Math.Clamp(currentValue + value, 0, maxValue);
    }

    void Injurious(string tag)
    {
        if ( this.gameObject.CompareTag(tag))
        {

            
            UpdateHealth(-20);
           
            if (this.CurrentValue <= 0)
            {
                if (tag == "BarEnemyPatrol")
                {
                    OnDestroyEnemie1?.Invoke(2);
                }
                if (tag == "BarEnemyStatic")
                {
                    OnDestroyEnemie2?.Invoke(4);
                }
                if (tag == "BarPlayer")
                {
                    OnPlayerDeath?.Invoke();
                }
                
                Destroy(transform.root.gameObject);
               
            }

        }

    }

    void Health(string tag)
    {

        if (this.gameObject.CompareTag(tag))
        {
            UpdateHealth(20);
           
        }
     
        
    }

}
