using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputSystemRebindBall : MonoBehaviour
{


    public InputActionReference jumpAction;
    public Text rebindText;


    private PlayerInput playerInput;
    private Rigidbody rigi;
    // Start is called before the first frame update
    void Awake(){
         playerInput=GetComponent<PlayerInput>();
        rigi=GetComponent<Rigidbody>();
    }
    void Start()
    {

        //获取读取动作的json值
       string json=PlayerPrefs.GetString("InputActions",null);  
        if(json!=null){
            playerInput.actions.LoadBindingOverridesFromJson(json);
        } 
       
        //显示获取的
      rebindText.text=InputControlPath.ToHumanReadableString(jumpAction.action.bindings[0].effectivePath,InputControlPath.HumanReadableStringOptions.OmitDevice);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnJump(InputAction.CallbackContext context)
    {
       //started performed canceld
       if(context.phase==InputActionPhase.Performed){
           Debug.Log($"跳起:{context.phase}");
        rigi.AddForce(Vector3.up*50f*Time.deltaTime,ForceMode.Impulse);
       }
       
    }


    public void StartRebind(){
        playerInput.SwitchCurrentActionMap("UI");//转换到Ui输入使得无法输入
        rebindText.text="请输入....";//显示输入提示
        jumpAction.action.PerformInteractiveRebinding()
        .WithControlsExcluding("Mouse")//出来鼠标的输入
        .OnComplete(operation=>{
            rebindText.text=//重新显示
            InputControlPath.ToHumanReadableString(jumpAction.action.bindings[0].effectivePath,InputControlPath.HumanReadableStringOptions.OmitDevice);
            operation.Dispose();
            playerInput.SwitchCurrentActionMap("GamePlay");//切换回来
            //保存 
            string json=  playerInput.actions.SaveBindingOverridesAsJson();
             PlayerPrefs. SetString("InputActions",json);
        })
        .Start();
    }

}
